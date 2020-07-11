using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http;
using ICSharpCode.SharpZipLib.BZip2;
using System.IO;
using System.Security.Policy;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Data.Common;

namespace MVDPassportLoader
{
    public class PassportsLoader
    {
        public readonly string Url;
        /// <summary>
        /// Default path is root application path
        /// </summary>
        public string FilePath = "./mvd.csv";
        private Task progress;
        public readonly string Connection;

        public PassportsLoader(string Url, string DbConnect)
        {
            this.Url = Url;
            this.Connection = DbConnect;
        }

        public PassportsLoader GetArchive()
        {
            var request = WebRequest.Create(Url);
            var resp = request.GetResponse();
            Stream data = resp.GetResponseStream();
            Console.WriteLine($"Content length = {resp.ContentLength.AsMegabytes()} Mb");
            Stream decom = new FileStream(FilePath, FileMode.Create);
         
            progress = new Task(() =>
            {
                BZip2.Decompress(data, decom, true);
            });

            progress.Start();
            progress.Wait();
            Console.WriteLine("Download and decompression complete");
            return this;
        }

        public PassportsLoader LoadIntoDB()
        {
            if(progress != null && progress?.Status != TaskStatus.Canceled)
            {
                progress.Wait();
            }
            Console.WriteLine("Uploading into DB");
           
            using(SqlConnection _cn = new SqlConnection(Connection))
            {
                _cn.Open();
                
                using (SqlCommand _cmd = _cn.CreateCommand())
                {
                    _cmd.CommandText = "truncate table BadPassports";
                    _cmd.ExecuteNonQuery();
                    
                        using (FxCsvReader<Passport> _CSV = new FxCsvReader<Passport>(FilePath))
                        {
                            using(SqlBulkCopy _copier = new SqlBulkCopy(_cn))
                            {
                                _copier.DestinationTableName = "BadPassports";
                                _copier.NotifyAfter = 1000000;
                                _copier.SqlRowsCopied += new SqlRowsCopiedEventHandler(ShowProgress);
                                _copier.BulkCopyTimeout = 0;
                                _copier.ColumnMappings.Add(0, 0);
                                _copier.ColumnMappings.Add(1, 1);
                                _copier.WriteToServer(_CSV);
                            

                            }
                        }
                
                }
                
            }
            Console.WriteLine("Uploading complete");
            return this;
        }

        private void ShowProgress(object sender, SqlRowsCopiedEventArgs args)
        {
            SingleRowPrint($"Uploaded = {args.RowsCopied}");
        }

        private void SingleRowPrint( string message )
        {
            Console.WriteLine(message);
            Console.CursorTop = Console.CursorTop - 1;
            Console.CursorLeft = 0;
        }
        
    }
}

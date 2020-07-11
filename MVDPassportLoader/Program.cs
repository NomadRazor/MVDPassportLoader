using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MVDPassportLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = @"{your_server}";
            _builder.InitialCatalog = "{your_database}";
            _builder.UserID = "{your_login}";
            _builder.Password = "{your_password}";
            _builder.ApplicationName = "MVDLOADER";

            string fileLink = "http://guvm.mvd.ru/upload/expired-passports/list_of_expired_passports.csv.bz2";

            var loader = new PassportsLoader(fileLink,_builder.ToString());

            Stopwatch watchdog = new Stopwatch();
            watchdog.Start();
            loader = loader.GetArchive();
            watchdog.Stop();
            Console.WriteLine($"GetArchive - Elapsed milliseconds = {watchdog.ElapsedMilliseconds}");
            watchdog.Restart();
            loader.LoadIntoDB();
            watchdog.Stop();
            Console.WriteLine($"LoadIntoDB - Elapsed milliseconds = {watchdog.ElapsedMilliseconds}");

            Console.ReadKey();
        }
    }
}

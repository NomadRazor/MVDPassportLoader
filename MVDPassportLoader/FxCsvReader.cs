using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MVDPassportLoader
{
    public class FxCsvReader<T> : IDataReader
    {
        public object this[int i] => properties[i].GetValue(current);

        public object this[string name] => ByName(name);

        public int Depth => 0; 

        public bool IsClosed => _currentLine == null;

        public int RecordsAffected => ProcessedRows;

        public int FieldCount => properties.Length;

        private int ProcessedRows { get; set; }

        private T current { get; set; }
        private string _currentLine { get; set; }
        private PropertyInfo[] properties { get; set; }

        private StreamReader _dataStream { get; set; }

        public FxCsvReader(string FileName)
        {
            ProcessedRows = 0;
            _dataStream = new StreamReader(FileName);
            _currentLine = _dataStream.ReadLine();
            properties = typeof(T).GetProperties();
           
        }

        public void Close()
        {
            _dataStream.Close();
        }

        public void Dispose()
        {
            _dataStream.Dispose();
        }

        public bool GetBoolean( int i )
        {
            return (bool)properties[i].GetValue(current);
        }

        public byte GetByte( int i )
        {
            return (byte)properties[i].GetValue(current);
        }

        public long GetBytes( int i, long fieldOffset, byte[] buffer, int bufferoffset, int length )
        {
            throw new NotImplementedException();
        }

        public char GetChar( int i )
        {
            throw new NotImplementedException();
        }

        public long GetChars( int i, long fieldoffset, char[] buffer, int bufferoffset, int length )
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData( int i )
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName( int i )
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime( int i )
        {
            return (DateTime)properties[i].GetValue(current);
        }

        public decimal GetDecimal( int i )
        {
            return (decimal)properties[i].GetValue(current);
        }

        public double GetDouble( int i )
        {   
            return (double)properties[i].GetValue(current);
        }

        public Type GetFieldType( int i )
        {
            return properties[i].PropertyType;
        }

        public float GetFloat( int i )
        {
            return (float)properties[i].GetValue(current);
        }

        public Guid GetGuid( int i )
        {
            return Guid.Parse(properties[i].GetValue(current).ToString());
        }

        public short GetInt16( int i )
        {
            return (short)properties[i].GetValue(current);
        }

        public int GetInt32( int i )
        {
            return (int)properties[i].GetValue(current);
        }

        public long GetInt64( int i )
        {
            return (long)properties[i].GetValue(current);
        }

        public string GetName( int i )
        {
            return properties[i].Name;
        }

        public int GetOrdinal( string name )
        {
            for(int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.ToLower().Equals(name.ToLower()))
                {
                    return i;
                }
            }
            return -1;
            
        }

        private object ByName(string name )
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.ToLower().Equals(name.ToLower()))
                {
                    return properties[i].GetValue(current);
                }
            }
            return null;
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString( int i )
        {
            return (string)properties[i].GetValue(current);
        }

        public object GetValue( int i )
        {
            
           return properties[i].GetValue(current);
         
        }

        public int GetValues( object[] values )
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull( int i )
        {
          return properties[i].GetValue(current) == null;
        }

        public bool NextResult()
        {
           
            return false;
        }

        public bool Read()
        {
            _currentLine = _dataStream.ReadLine();
           
            if (_currentLine == null)
            {
                return false;
            }

            ProcessedRows++;

            current = Activator.CreateInstance<T>();
            string[] values = _currentLine.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                properties[i].SetValue(current, values[i]);
            }
          
            return true;
        }
    }
}

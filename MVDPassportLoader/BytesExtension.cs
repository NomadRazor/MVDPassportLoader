using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVDPassportLoader
{
    public static class BytesExtension
    {
        public static long AsKilobytes(this long bytes)
        {
            if (bytes <= 0)
            {
                throw new Exception("Object size cannot be less than zero");
            }
            return bytes.GetDegree();
        }

        public static long AsMegabytes( this long bytes )
        {
            if (bytes <= 0)
            {
                throw new Exception("Object size cannot be less than zero");
            }
            return bytes.GetDegree().GetDegree();
        }

        public static long AsGigabytes( this long bytes )
        {
            if (bytes <= 0)
            {
                throw new Exception("Object size cannot be less than zero");
            }
            return bytes.GetDegree().GetDegree().GetDegree();
        }

        private static long GetDegree( this long _bytes )
        {
            return _bytes / 1024;
        }
    }
}

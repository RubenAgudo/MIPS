using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIPS.src.exceptions
{
    public class FileNotSelectedException : Exception
    {
        public FileNotSelectedException() : base() { }

        public FileNotSelectedException(string message) : base(message) { }

        public FileNotSelectedException(string message, Exception innerException) : base(message, innerException) { }
    
    }
}

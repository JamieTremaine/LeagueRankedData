using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueTierLevels
{
    #region FileNotFoundException
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException()
        {
        }

        public FileNotFoundException(string message)
            : base(message)
        {
        }

        public FileNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion

    #region MultipleFilesException
    public class MultipleFilesException : Exception
    {
        public MultipleFilesException()
        {
        }

        public MultipleFilesException(string message)
            : base(message)
        {
        }

        public MultipleFilesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion

}

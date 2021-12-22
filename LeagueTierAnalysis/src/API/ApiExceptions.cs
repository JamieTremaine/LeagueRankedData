using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueTierLevels
{
    #region ApiFailedExeception
    public class ApiFailedExeception : Exception
    {
        public ApiFailedExeception()
        {
        }

        public ApiFailedExeception(string message)
            : base(message)
        {
        }

        public ApiFailedExeception(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion
}

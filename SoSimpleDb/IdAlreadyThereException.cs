using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoSimpleDb
{
    public class IdAlreadyThereException : Exception
    {
        public IdAlreadyThereException(string message) : base(message)
        {
        }
    }
}

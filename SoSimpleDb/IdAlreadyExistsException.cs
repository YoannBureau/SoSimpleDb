using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoSimpleDb
{
    public class IdAlreadyExistsException : Exception
    {
        public IdAlreadyExistsException(string message) : base(message)
        {
        }
    }
}

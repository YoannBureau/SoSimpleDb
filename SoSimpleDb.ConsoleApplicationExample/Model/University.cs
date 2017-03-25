using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoSimpleDb.ConsoleApplicationExample.Model
{
    public class University : SoSimpleDbModelBase
    {
        public string Name { get; set; }
        List<Degree> Degrees { get; set; } = new List<Degree>();
    }
}

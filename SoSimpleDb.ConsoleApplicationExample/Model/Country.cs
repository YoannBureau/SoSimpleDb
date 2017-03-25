using SoSimpleDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoSimpleDb.ConsoleApplicationExample.Model
{
    public class Country : SoSimpleDbModelBase
    {
        public string Name { get; set; }
        public List<University> Universities { get; set; } = new List<University>();
    }
}

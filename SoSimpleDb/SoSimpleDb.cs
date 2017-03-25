using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoSimpleDb
{
    public sealed class SoSimpleDb<T> where T : SoSimpleDbModelBase
    {
        private static readonly Lazy<SoSimpleDb<T>> lazy =
            new Lazy<SoSimpleDb<T>>(() => new SoSimpleDb<T>());

        public static SoSimpleDb<T> Instance { get { return lazy.Value; } }

        private SoSimpleDb()
        {
        }
        
        private static List<T> data = new List<T>();

        public void Add(T obj)
        {
            data.Add(obj);
        }

        public void Add(IEnumerable<T> objs)
        {
            data.AddRange(objs);
        }

        public T Get(int id)
        {
            return data.Single(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return data;
        }
    }
}

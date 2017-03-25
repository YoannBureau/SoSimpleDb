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
            if(data.Any(x => x.Id == obj.Id))
            {
                throw new IdAlreadyThereException($"An object of type ${typeof(T).FullName} with Id ${obj.Id} is already in Db.");
            }

            data.Add(obj);
        }

        public void Add(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
            {
                Add(obj);
            }
        }

        public T Get(int id)
        {
            return data.Single(x => x.Id == id);
        }

        public IEnumerable<T> Get(Func<T, bool> function)
        {
            return data.Where(function);
        }

        public IEnumerable<T> GetAll()
        {
            return data;
        }

        public int Count()
        {
            return data.Count;
        }

        public void Clear()
        {
            data.Clear();
        }
    }
}

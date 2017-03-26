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

        /// <summary>
        /// Represents the instance of the SoSimpleDb database
        /// </summary>
        public static SoSimpleDb<T> Instance { get { return lazy.Value; } }

        private SoSimpleDb()
        {
        }
        
        private static List<T> data = new List<T>();

        /// <summary>
        /// Inserts a new item in the database
        /// </summary>
        /// <param name="obj"></param>
        public void Insert(T obj)
        {
            if(data.Any(x => x.Id == obj.Id))
            {
                throw new IdAlreadyExistsException($"An object of type ${typeof(T).FullName} with Id ${obj.Id} is already in Db.");
            }

            data.Add(obj);
        }

        /// <summary>
        /// Inserts a new collection of items in the database
        /// </summary>
        /// <param name="objs"></param>
        public void Insert(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
            {
                Insert(obj);
            }
        }

        /// <summary>
        /// Selects one item from the database that corresponds to the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Select(int id)
        {
            ThrowExceptionIfNotExists(id);

            return data.Single(x => x.Id == id);
        }

        /// <summary>
        /// Selects a collection of items from the database that matches with the pattern passed as parameter
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public IEnumerable<T> Select(Func<T, bool> function)
        {
            return data.Where(function);
        }

        /// <summary>
        /// Selects all items from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> SelectAll()
        {
            return data;
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="obj"></param>
        public void Update(T obj)
        {
            ThrowExceptionIfNotExists(obj.Id);

            Delete(obj.Id);
            Insert(obj);
        }

        /// <summary>
        /// Deletes all the items in the database
        /// </summary>
        public void DeleteAll()
        {
            data.Clear();
        }

        /// <summary>
        /// Deletes one item in the database
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ThrowExceptionIfNotExists(id);

            data.Remove(Select(id));
        }

        /// <summary>
        /// Returns the number of items in the database
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return data.Count;
        }

        private void ThrowExceptionIfNotExists(int id)
        {
            if (!data.Any(x => x.Id == id))
            {
                throw new IdNotFoundException($"Object of type ${typeof(T).FullName} with Id ${id} has not been found in Db.");
            }
        }
    }
}

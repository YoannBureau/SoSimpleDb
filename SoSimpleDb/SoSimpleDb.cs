using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            ReadDataFromFileStorage();
        }

        private string configCustomPathName = "SoSimpleDb.CustomFile";
        private string defaultPath = "Data.ssdb";
        private static List<T> data = new List<T>();

        public string FileStoragePath
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Any(x => x == configCustomPathName))
                {
                    return ConfigurationManager.AppSettings[configCustomPathName];
                }
                else
                {
                    return defaultPath;
                }
            }
        }

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

            WriteDataToFileStorage();
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

            WriteDataToFileStorage();
        }

        /// <summary>
        /// Deletes one item in the database
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ThrowExceptionIfNotExists(id);

            data.Remove(Select(id));

            WriteDataToFileStorage();
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

        private void ReadDataFromFileStorage()
        {
            if (!File.Exists(FileStoragePath))
            {
                CreateEmptyStorageFile();
            }

            dynamic fileStorageJsonObject = JsonConvert.DeserializeObject(File.ReadAllText(FileStoragePath));

            if (fileStorageJsonObject[GetCollectionNameInFileStorage()] != null)
            {
                data = JsonConvert.DeserializeObject<List<T>>(fileStorageJsonObject[GetCollectionNameInFileStorage()].ToString());
            }
        }

        private void WriteDataToFileStorage()
        {
            if (!File.Exists(FileStoragePath))
            {
                CreateEmptyStorageFile();
            }

            var dataJToken = JToken.FromObject(data);

            dynamic fileStorageJsonObject = JsonConvert.DeserializeObject(File.ReadAllText(FileStoragePath));
            if(fileStorageJsonObject[GetCollectionNameInFileStorage()] == null)
            {
                //Add collection to file storage
                fileStorageJsonObject.Add(GetCollectionNameInFileStorage(), dataJToken);
            }
            else
            {
                //Update collection in file storage
                fileStorageJsonObject[GetCollectionNameInFileStorage()] = dataJToken;
            }

            var modifiedJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fileStorageJsonObject, Formatting.Indented);
            File.WriteAllText(FileStoragePath, modifiedJsonString);
        }

        private void CreateEmptyStorageFile()
        {
            File.WriteAllText(FileStoragePath, JsonConvert.SerializeObject(new { }));
        }

        private string GetCollectionNameInFileStorage()
        {
            return $"{typeof(T).FullName.Replace(".", "_")}_Collection";
        }
    }
}

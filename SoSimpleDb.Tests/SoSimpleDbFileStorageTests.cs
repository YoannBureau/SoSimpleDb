using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoSimpleDb.ConsoleApplicationExample.Model;
using System.Configuration;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SoSimpleDb.Tests
{
    [TestClass]
    public class SoSimpleDbFileStorageTests
    {
        [TestInitialize]
        public void ClearBeforeTest()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();

            SoSimpleDb<Country>.Instance.DeleteAll();
            SoSimpleDb<University>.Instance.DeleteAll();
        }

        [TestMethod]
        public void FileStoragePathIsDefaultIfNoCustomPathIsSpecifiedInConfigurationFile()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();

            Assert.IsTrue(SoSimpleDb<Country>.Instance.FileStoragePath == "Data.ssdb");
        }

        [TestMethod]
        public void FileStoragePathIsCustomPathIfCustomPathIsSpecifiedInConfigurationFile()
        {
            string customPath = "C:/file.ssdb";
            AddCustomPathToConfigurationFile(customPath);

            Assert.IsTrue(SoSimpleDb<Country>.Instance.FileStoragePath == customPath);

            EnsureThatThereIsNoCustomPathInApplicationFile();
        }

        [TestMethod]
        public void FileStorageIsCreatedIsNotExists()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Assert.IsFalse(File.Exists(fileStoragePath));

            SoSimpleDb<Country>.Instance.Insert(new Country() { Id = 1, Name = "Country #1" });

            Assert.IsTrue(File.Exists(fileStoragePath));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenInsertOne()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(newCountry1);
            SoSimpleDb<Country>.Instance.Insert(newCountry2);

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry1));
            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry2));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenInsertMany()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(new List<Country>() { newCountry1, newCountry2 });

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry1));
            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry2));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenUpdate()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(new List<Country>() { newCountry1, newCountry2 });

            Country newCountry2Bis = new Country { Id = 2, Name = "Country #2 Bis" };
            SoSimpleDb<Country>.Instance.Update(newCountry2Bis);

            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newCountry2));
            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry2Bis));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenDeleteOne()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(new List<Country>() { newCountry1, newCountry2 });

            SoSimpleDb<Country>.Instance.Delete(1);

            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newCountry1));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenDeleteAll()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(new List<Country>() { newCountry1, newCountry2 });

            SoSimpleDb<Country>.Instance.DeleteAll();

            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newCountry1));
            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newCountry2));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenInsertingInSeveralCollections()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry = new Country() { Id = 1, Name = "Country #1" };
            SoSimpleDb<Country>.Instance.Insert(newCountry);

            University newUniversity = new University() { Id = 1, Name = "University #1", Degrees = new List<Degree>() { new Degree() { Level = 13 } } };
            SoSimpleDb<University>.Instance.Insert(newUniversity);

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry));
            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newUniversity));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenUpdatingInSeveralCollections()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(new List<Country>() { newCountry1, newCountry2 });

            University newUniversity1 = new University() { Id = 1, Name = "Country #1" };
            University newUniversity2 = new University() { Id = 2, Name = "Country #2" };
            SoSimpleDb<University>.Instance.Insert(new List<University>() { newUniversity1, newUniversity2 });

            Country newCountry2Bis = new Country() { Id = 2, Name = "Country #2 Bis" };
            SoSimpleDb<Country>.Instance.Update(newCountry2Bis);

            University newUniversity2Bis = new University() { Id = 2, Name = "Unversity #2 Bis" };
            SoSimpleDb<University>.Instance.Update(newUniversity2Bis);

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry1));
            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newCountry2));
            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry2Bis));

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newUniversity1));
            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newUniversity2));
            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newUniversity2Bis));
        }

        [TestMethod]
        public void FileStorageIsFilledWhenDeletingInSeveralCollections()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Country newCountry1 = new Country() { Id = 1, Name = "Country #1" };
            Country newCountry2 = new Country() { Id = 2, Name = "Country #2" };
            SoSimpleDb<Country>.Instance.Insert(new List<Country>() { newCountry1, newCountry2 });

            University newUniversity1 = new University() { Id = 1, Name = "Country #1" };
            University newUniversity2 = new University() { Id = 2, Name = "Country #2" };
            SoSimpleDb<University>.Instance.Insert(new List<University>() { newUniversity1, newUniversity2 });
            
            SoSimpleDb<Country>.Instance.Delete(2);
            SoSimpleDb<University>.Instance.Delete(2);

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newCountry1));
            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newCountry2));

            Assert.IsTrue(JsonFileContainsObject(fileStoragePath, newUniversity1));
            Assert.IsFalse(JsonFileContainsObject(fileStoragePath, newUniversity2));
        }


        public bool JsonFileContainsObject(string filePath, object obj)
        {
            string fileStorageContent = File.ReadAllText(filePath).Replace(" ", "");
            string objectJson = JsonConvert.SerializeObject(obj, Formatting.Indented).Replace(" ", "");
            return fileStorageContent.Contains(objectJson);
        }


        private void AddCustomPathToConfigurationFile(string customPath)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add("SoSimpleDb.CustomFile", customPath);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void EnsureThatThereIsNoCustomPathInApplicationFile()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Any(x => x == "SoSimpleDb.CustomFile"))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("SoSimpleDb.CustomFile");
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private void DeleteFileStorage()
        {
            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;
            File.Delete(fileStoragePath);
        }
    }
}

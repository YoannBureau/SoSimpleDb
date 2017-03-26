using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoSimpleDb.ConsoleApplicationExample.Model;
using System.Configuration;
using System.Linq;
using System.IO;

namespace SoSimpleDb.Tests
{
    [TestClass]
    public class SoSimpleDbFileStorageTests
    {
        [TestInitialize]
        public void ClearBeforeTest()
        {
            SoSimpleDb<Country>.Instance.DeleteAll();
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
        }

        [TestMethod]
        public void FileStorageIsCreatedIsNotExists()
        {
            EnsureThatThereIsNoCustomPathInApplicationFile();
            DeleteFileStorage();

            string fileStoragePath = SoSimpleDb<Country>.Instance.FileStoragePath;

            Assert.IsTrue(!File.Exists(fileStoragePath));

            SoSimpleDb<Country>.Instance.Insert(new Country() { Id = 1, Name = "Country #1" });

            Assert.IsTrue(File.Exists(fileStoragePath));
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

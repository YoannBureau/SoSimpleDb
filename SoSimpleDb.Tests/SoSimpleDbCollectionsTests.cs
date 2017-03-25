using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoSimpleDb.ConsoleApplicationExample.Model;
using System.Collections.Generic;
using System.Linq;

namespace SoSimpleDb.Tests
{
    [TestClass]
    public class SoSimpleDbCollectionsTests
    {
        [TestInitialize]
        public void ClearBeforeTest()
        {
            SoSimpleDb<Country>.Instance.Clear();
        }

        [TestMethod]
        public void AddSingleAndGetSingleById()
        {
            Country country = new Country()
            {
                Id = 1,
                Name = "France"
            };

            SoSimpleDb<Country>.Instance.Add(country);

            var result = SoSimpleDb<Country>.Instance.Get(1);

            Assert.IsTrue(result == country);
        }

        [TestMethod]
        public void AddMultipleAndGetAll()
        {
            List<Country> countries = new List<Country>();
            for (int i = 0; i < 10; i++)
            {
                countries.Add(new Country() { Id = i, Name = $"Country #{i}" });
            }

            SoSimpleDb<Country>.Instance.Add(countries);

            var result = SoSimpleDb<Country>.Instance.GetAll();


            foreach (var country in countries)
            {
                Assert.IsTrue(result.ToList().Contains(country));
            };
        }

        [TestMethod]
        public void AddMultipleAndGetSingleById()
        {
            List<Country> countries = new List<Country>();
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            Country country3 = new Country() { Id = 3, Name = $"Country #3" };
            countries.Add(country1);
            countries.Add(country2);
            countries.Add(country3);

            SoSimpleDb<Country>.Instance.Add(countries);

            var result = SoSimpleDb<Country>.Instance.Get(1);

            Assert.IsTrue(result == country1);
        }

        [TestMethod]
        public void GetCount()
        {
            int numberOfItems = 10;

            List<Country> countries = new List<Country>();
            for (int i = 0; i < numberOfItems; i++)
            {
                countries.Add(new Country() { Id = i, Name = $"Country #{i}" });
            }

            SoSimpleDb<Country>.Instance.Add(countries);

            var result = SoSimpleDb<Country>.Instance.Count();

            Assert.IsTrue(result == numberOfItems);
        }

        [TestMethod]
        public void Clear()
        {
            int numberOfItems = 10;

            List<Country> countries = new List<Country>();
            for (int i = 0; i < numberOfItems; i++)
            {
                countries.Add(new Country() { Id = i, Name = $"Country #{i}" });
            }

            SoSimpleDb<Country>.Instance.Add(countries);

            SoSimpleDb<Country>.Instance.Clear();
            var result = SoSimpleDb<Country>.Instance.Count();

            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(IdAlreadyThereException))]
        public void CantAddAnItemWithTheSameId()
        {
            var country1 = new Country() { Id = 1 };
            var country2 = new Country() { Id = 1 };

            SoSimpleDb<Country>.Instance.Add(country1);
            SoSimpleDb<Country>.Instance.Add(country2);
        }

        [TestMethod]
        public void GetOneByExpression()
        {
            List<Country> countries = new List<Country>();
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            Country country3 = new Country() { Id = 3, Name = $"Country #3" };
            countries.Add(country1);
            countries.Add(country2);
            countries.Add(country3);

            SoSimpleDb<Country>.Instance.Add(countries);

            var result = SoSimpleDb<Country>.Instance.Get(x => x.Name == country1.Name);

            Assert.IsTrue(result.Contains(country1));
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void GetMultipleByExpression()
        {
            List<Country> countries = new List<Country>();
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            Country country3 = new Country() { Id = 3, Name = $"Country #3" };
            countries.Add(country1);
            countries.Add(country2);
            countries.Add(country3);

            SoSimpleDb<Country>.Instance.Add(countries);

            var result = SoSimpleDb<Country>.Instance.Get(x => x.Name.Contains("Country"));

            Assert.IsTrue(result.Contains(country1));
            Assert.IsTrue(result.Contains(country2));
            Assert.IsTrue(result.Contains(country3));
            Assert.IsTrue(result.Count() == 3);
        }
    }
}

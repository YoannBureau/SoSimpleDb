﻿using System;
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
            SoSimpleDb<Country>.Instance.DeleteAll();
        }

        [TestMethod]
        public void InsertSingleAndSelectSingleById()
        {
            Country country = new Country()
            {
                Id = 1,
                Name = "France"
            };

            SoSimpleDb<Country>.Instance.Insert(country);

            var result = SoSimpleDb<Country>.Instance.Select(1);

            Assert.IsTrue(result == country);
        }

        [TestMethod]
        public void InsertMultipleAndSelectAll()
        {
            List<Country> countries = new List<Country>();
            for (int i = 0; i < 10; i++)
            {
                countries.Add(new Country() { Id = i, Name = $"Country #{i}" });
            }

            SoSimpleDb<Country>.Instance.Insert(countries);

            var result = SoSimpleDb<Country>.Instance.SelectAll();


            foreach (var country in countries)
            {
                Assert.IsTrue(result.ToList().Contains(country));
            };
        }

        [TestMethod]
        public void InsertMultipleAndSelectSingleById()
        {
            List<Country> countries = new List<Country>();
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            Country country3 = new Country() { Id = 3, Name = $"Country #3" };
            countries.Add(country1);
            countries.Add(country2);
            countries.Add(country3);

            SoSimpleDb<Country>.Instance.Insert(countries);

            var result = SoSimpleDb<Country>.Instance.Select(1);

            Assert.IsTrue(result == country1);
        }


        //Select Id that dont exists throzs an exception
        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void CantSelectAnItemWithAnIdThatDontExists()
        {
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            SoSimpleDb<Country>.Instance.Insert(country1);

            SoSimpleDb<Country>.Instance.Select(2);
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

            SoSimpleDb<Country>.Instance.Insert(countries);

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

            SoSimpleDb<Country>.Instance.Insert(countries);

            SoSimpleDb<Country>.Instance.DeleteAll();
            var result = SoSimpleDb<Country>.Instance.Count();

            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(IdAlreadyExistsException))]
        public void CantAddAnItemWithTheSameId()
        {
            var country1 = new Country() { Id = 1 };
            var country2 = new Country() { Id = 1 };

            SoSimpleDb<Country>.Instance.Insert(country1);
            SoSimpleDb<Country>.Instance.Insert(country2);
        }

        [TestMethod]
        public void SelectOneByExpression()
        {
            List<Country> countries = new List<Country>();
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            Country country3 = new Country() { Id = 3, Name = $"Country #3" };
            countries.Add(country1);
            countries.Add(country2);
            countries.Add(country3);

            SoSimpleDb<Country>.Instance.Insert(countries);

            var result = SoSimpleDb<Country>.Instance.Select(x => x.Name == country1.Name);

            Assert.IsTrue(result.Contains(country1));
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void SelectMultipleByExpression()
        {
            List<Country> countries = new List<Country>();
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            Country country3 = new Country() { Id = 3, Name = $"Country #3" };
            countries.Add(country1);
            countries.Add(country2);
            countries.Add(country3);

            SoSimpleDb<Country>.Instance.Insert(countries);

            var result = SoSimpleDb<Country>.Instance.Select(x => x.Name.Contains("Country"));

            Assert.IsTrue(result.Contains(country1));
            Assert.IsTrue(result.Contains(country2));
            Assert.IsTrue(result.Contains(country3));
            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public void UpdateItem()
        {
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            SoSimpleDb<Country>.Instance.Insert(country1);

            Country country2 = new Country() { Id = 1, Name = $"Country #2" };
            SoSimpleDb<Country>.Instance.Update(country2);

            var result = SoSimpleDb<Country>.Instance.Select(1);

            Assert.IsTrue(result == country2);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void CantUpdateAnItemThatDontExists()
        {
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            SoSimpleDb<Country>.Instance.Insert(country1);

            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            SoSimpleDb<Country>.Instance.Update(country2);
        }

        [TestMethod]
        public void DeleteItem()
        {
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            SoSimpleDb<Country>.Instance.Insert(country1);

            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            SoSimpleDb<Country>.Instance.Insert(country2);

            SoSimpleDb<Country>.Instance.Delete(1);

            var result = SoSimpleDb<Country>.Instance.SelectAll().FirstOrDefault(x => x.Id == 1);

            Assert.IsTrue(result == null);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void CantDeleteAnItemThatDontExists()
        {
            Country country1 = new Country() { Id = 1, Name = $"Country #1" };
            SoSimpleDb<Country>.Instance.Insert(country1);

            Country country2 = new Country() { Id = 2, Name = $"Country #2" };
            SoSimpleDb<Country>.Instance.Insert(country2);

            SoSimpleDb<Country>.Instance.Delete(10);
        }
    }
}

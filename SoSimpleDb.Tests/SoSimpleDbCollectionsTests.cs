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
        [TestMethod]
        public void AddSingleAndGetSingleObject()
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
    }
}

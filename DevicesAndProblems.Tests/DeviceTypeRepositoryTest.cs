using System;
using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.DAL.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevicesAndProblems.Tests
{
    [TestClass]
    public class DeviceTypeRepositoryTest
    {
        [TestMethod]
        public void SelectListTest()
        {
            // arrange

            // act 
            var repository = new DeviceTypeRepository().SelectList();

            // assert 
            Assert.IsTrue(repository.Count > 0);
        }
    }
}

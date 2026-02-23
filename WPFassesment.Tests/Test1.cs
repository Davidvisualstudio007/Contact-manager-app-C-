using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFassesment.ViewModels;
using System;

namespace WPFassesment.Tests
{
    [TestClass]
    public class RecruitmentSystemTests
    {
        [TestMethod]
        public void AddContractor_Valid_ShouldReturnTrue()
        {
            var system = new RecruitmentSystem();
            bool result = system.AddContractor("Alex", "Kim", DateTime.Now, 30m);

            Assert.IsTrue(result);
            Assert.AreEqual(1, system.Contractors.Count);
        }

        [TestMethod]
        public void AddJob_Valid_ShouldReturnTrue()
        {
            var system = new RecruitmentSystem();
            bool result = system.AddJob("Website Fix", DateTime.Now, 200m);

            Assert.IsTrue(result);
            Assert.AreEqual(1, system.Jobs.Count);
        }

        [TestMethod]
        public void CompleteJob_ShouldMarkCompleted()
        {
            var system = new RecruitmentSystem();
            system.AddJob("Test", DateTime.Now, 100);

            bool result = system.CompleteJob(1);

            Assert.IsTrue(result);
            Assert.IsTrue(system.Jobs[0].Completed);
        }
    }
}

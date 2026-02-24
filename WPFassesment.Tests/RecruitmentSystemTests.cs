using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WPFassesment.ViewModels;

namespace WPFassesment.Tests
{
    [TestClass]
    public class RecruitmentSystemTests
    {
        // AddContractor (3 branches) 

        [TestMethod]
        public void AddContractor_InvalidName_ReturnsFalse()
        {
            var system = new RecruitmentSystem();

            bool result = system.AddContractor("", "Kim", DateTime.Today, 30m);

            Assert.IsFalse(result);
            Assert.AreEqual(0, system.Contractors.Count);
        }

        [TestMethod]
        public void AddContractor_InvalidWage_ReturnsFalse()
        {
            var system = new RecruitmentSystem();

            bool result = system.AddContractor("Alex", "Kim", DateTime.Today, 0m);

            Assert.IsFalse(result);
            Assert.AreEqual(0, system.Contractors.Count);
        }

        [TestMethod]
        public void AddContractor_ValidInputs_AddsContractorAndReturnsTrue()
        {
            var system = new RecruitmentSystem();

            bool result = system.AddContractor("Alex", "Kim", DateTime.Today, 30m);

            Assert.IsTrue(result);
            Assert.AreEqual(1, system.Contractors.Count);
            Assert.AreEqual("Alex", system.Contractors[0].FirstName);
            Assert.AreEqual("Kim", system.Contractors[0].LastName);
        }

        // AddJob (valid + invalid)

        [TestMethod]
        public void AddJob_InvalidTitle_ReturnsFalse()
        {
            var system = new RecruitmentSystem();

            bool result = system.AddJob("", DateTime.Today, 100m);

            Assert.IsFalse(result);
            Assert.AreEqual(0, system.Jobs.Count);
        }

        [TestMethod]
        public void AddJob_ValidInputs_AddsJobAndReturnsTrue()
        {
            var system = new RecruitmentSystem();

            bool result = system.AddJob("Website Fix", DateTime.Today, 200m);

            Assert.IsTrue(result);
            Assert.AreEqual(1, system.Jobs.Count);
            Assert.AreEqual("Website Fix", system.Jobs[0].Title);
            Assert.AreEqual(200m, system.Jobs[0].Cost);
        }

        // RemoveContractor

        [TestMethod]
        public void RemoveContractor_ExistingId_RemovesAndReturnsTrue()
        {
            var system = new RecruitmentSystem();
            system.AddContractor("Alex", "Kim", DateTime.Today, 30m);

            int id = system.Contractors[0].Id;
            bool result = system.RemoveContractor(id);

            Assert.IsTrue(result);
            Assert.AreEqual(0, system.Contractors.Count);
        }

        // AssignJob 

        [TestMethod]
        public void AssignJob_ValidJobAndContractor_AssignsAndReturnsTrue()
        {
            var system = new RecruitmentSystem();
            system.AddContractor("Alex", "Kim", DateTime.Today, 30m);
            system.AddJob("Website Fix", DateTime.Today, 200m);

            int contractorId = system.Contractors[0].Id;
            int jobId = system.Jobs[0].Id;

            bool result = system.AssignJob(jobId, contractorId);

            Assert.IsTrue(result);
            Assert.IsNotNull(system.Jobs[0].ContractorAssigned);
            Assert.AreEqual(contractorId, system.Jobs[0].ContractorAssigned!.Id);
        }

        [TestMethod]
        public void AssignJob_ContractorAlreadyHasActiveJob_ReturnsFalse()
        {
            var system = new RecruitmentSystem();
            system.AddContractor("Alex", "Kim", DateTime.Today, 30m);
            system.AddJob("Job 1", DateTime.Today, 100m);
            system.AddJob("Job 2", DateTime.Today, 200m);

            int contractorId = system.Contractors[0].Id;
            int job1Id = system.Jobs[0].Id;
            int job2Id = system.Jobs[1].Id;

            bool firstAssign = system.AssignJob(job1Id, contractorId);
            bool secondAssign = system.AssignJob(job2Id, contractorId);

            Assert.IsTrue(firstAssign);
            Assert.IsFalse(secondAssign);
        }

        // CompleteJob

        [TestMethod]
        public void CompleteJob_ExistingJob_MarksCompletedAndReturnsTrue()
        {
            var system = new RecruitmentSystem();
            system.AddJob("Website Fix", DateTime.Today, 200m);

            int jobId = system.Jobs[0].Id;
            bool result = system.CompleteJob(jobId);

            Assert.IsTrue(result);
            Assert.IsTrue(system.Jobs[0].Completed);
        }

        [TestMethod]
        public void CompleteJob_AlreadyCompleted_ReturnsFalse()
        {
            var system = new RecruitmentSystem();
            system.AddJob("Website Fix", DateTime.Today, 200m);

            int jobId = system.Jobs[0].Id;
            bool first = system.CompleteJob(jobId);
            bool second = system.CompleteJob(jobId);

            Assert.IsTrue(first);
            Assert.IsFalse(second);
        }

        // GetAvailableContractors

        [TestMethod]
        public void GetAvailableContractors_WhenContractorHasActiveJob_NotReturned()
        {
            var system = new RecruitmentSystem();
            system.AddContractor("Alex", "Kim", DateTime.Today, 30m);
            system.AddJob("Website Fix", DateTime.Today, 200m);

            int contractorId = system.Contractors[0].Id;
            int jobId = system.Jobs[0].Id;

            system.AssignJob(jobId, contractorId);

            var available = system.GetAvailableContractors();

            Assert.AreEqual(0, available.Count);
        }

        // ---------- GetUnassignedJobs ----------

        [TestMethod]
        public void GetUnassignedJobs_ReturnsOnlyUnassignedAndNotCompleted()
        {
            var system = new RecruitmentSystem();
            system.AddContractor("Alex", "Kim", DateTime.Today, 30m);
            system.AddJob("Job 1", DateTime.Today, 100m);
            system.AddJob("Job 2", DateTime.Today, 200m);

            int contractorId = system.Contractors[0].Id;
            int job1Id = system.Jobs[0].Id;

            system.AssignJob(job1Id, contractorId);

            var unassigned = system.GetUnassignedJobs();

            Assert.AreEqual(1, unassigned.Count);
            Assert.AreEqual("Job 2", unassigned[0].Title);
        }

        // GetJobByCost

        [TestMethod]
        public void GetJobByCost_ExactMatch_ReturnsJob()
        {
            var system = new RecruitmentSystem();
            system.AddJob("Website Fix", DateTime.Today, 200m);

            var job = system.GetJobByCost(200m);

            Assert.IsNotNull(job);
            Assert.AreEqual("Website Fix", job!.Title);
        }
    }
}
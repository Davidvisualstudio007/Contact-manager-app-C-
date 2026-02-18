 using System.Collections.Generic;
using WPFassesment.Models;
using System;
using System.Net.Http.Headers;
using System.Windows.Navigation;
using System.Security.Cryptography.X509Certificates;

namespace WPFassesment.ViewModels
{
    public class RecruitmentSystem
    {
        private int nextContractorId = 1;
        private int nextJobId = 1;

        public List<Contractor> Contractors { get; } = new List<Contractor>();
        public List<Job> Jobs { get; } = new List<Job>();

        public bool AddContractor(string firstName, string lastName, DateTime startDate, decimal hourlyWage)
        {
            if (!Contractor.IsValidName(firstName, lastName))
            {
                return false;
            }

            if (hourlyWage <= 0)
            {
                return false;
            }

            var contractor = new Contractor
            {
                Id = nextContractorId,
                FirstName = firstName,
                LastName = lastName,
                StartDate = startDate,
                HourlyWage = hourlyWage
            };

            Contractors.Add(contractor);

            nextContractorId++;
            return true;
        }
        public bool AddJob(string title, DateTime dateTime, decimal cost)
        {
            if (!Job.IsValidTitle(title))
            {
                return false;
            }
            if (!Job.IsValidCost(cost))
            {
                return false;
            }

            var job = new Job
            {
                Id = nextJobId,
                Title = title,
                Date = dateTime,
                Cost = cost
            };

            Jobs.Add(job);
            nextJobId++;
            return true;
        }
        public bool RemoveContractor(int id)
        {
            for (int i = 0; i < Contractors.Count; i++)

                if (Contractors[i].Id == id)
                {
                    Contractors.RemoveAt(i);
                    return true;
                }
            return false;

        }
        public bool AssignJob(int jobId, int contractorId)
        {
            Job? jobToAssign = null;
            Contractor? contractorToAssign = null;

            foreach (var job in Jobs)
            {
                if (job.Id == jobId)
                {
                    jobToAssign = job;
                    break;
                }
            }

            foreach (var contractor in Contractors)
            {
                if(contractor.Id == contractorId)
                {
                    contractorToAssign = contractor;
                    break;
                }
            }

            if (jobToAssign == null || contractorToAssign == null)
            {
                return false;
            }

            if (jobToAssign.Completed)
            {
                return false;
            }

            if (jobToAssign.ContractorAssigned != null)
            {
                return false;
            }

            foreach (var job in Jobs)
            {
                if 
                    (!job.Completed &&
                    job.ContractorAssigned != null &&
                    job.ContractorAssigned.Id == contractorToAssign.Id)
                {
                    return false;
                }
            }
        jobToAssign.ContractorAssigned = contractorToAssign;
        jobToAssign.Completed = false;
        return true;
        }

        public bool CompleteJob(int jobId)
        {
            foreach (var job in Jobs)
            {
                if (job.Id == jobId)
                {
                    if (job.Completed)
                    {
                        return false;
                    }
                    job.Completed = true;
                    return true;
                }
            }
            return false;
        }

        public List<Contractor> GetAvailableContractors()
        {
            var available = new List<Contractor>();

            foreach (var contractor in Contractors)
            {
                bool isBusy = false;

                foreach (var job in Jobs)
                {
                    if (!job.Completed &&
                        job.ContractorAssigned != null &&
                        job.ContractorAssigned.Id == contractor.Id)
                    {
                        isBusy = true;
                        break;
                    }
                }

                if (!isBusy)
                {
                    available.Add(contractor);
                }
            }

            return available;
        }
        public List<Job> GetUnassignedJobs()
        {
            var unassigned = new List<Job>();
            
            foreach (var job in Jobs)
            {
                if (job.ContractorAssigned == null && !job.Completed)
                {
                    unassigned.Add(job);
                }
            }
            return unassigned;
        }
        public Job? GetJobByCost(decimal cost)
        {
            foreach ( var job in Jobs)
            {
                if (job.Cost == cost)
                    return job;
            }
            return null;
        }
    }
}
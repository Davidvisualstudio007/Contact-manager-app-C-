 using System.Collections.Generic;
using WPFassesment.Models;
using System;
using System.Net.Http.Headers;
using System.Windows.Navigation;
using System.Security.Cryptography.X509Certificates;

namespace WPFassesment.ViewModels
{
    /// <summary>
    /// Provides core logic for managing contractors and jobs.
    /// Handles adding, assigning, completing, and filtering data.
    /// </summary>
    public class RecruitmentSystem
    {
        private int nextContractorId = 1;
        private int nextJobId = 1;

        /// <summary>
        /// Gets the list of contractors in the system.
        /// </summary>
        public List<Contractor> Contractors { get; } = new List<Contractor>();

        /// <summary>
        /// Gets the list of jobs in the system.
        /// </summary>
        public List<Job> Jobs { get; } = new List<Job>();

        /// <summary>
        /// Adds a contractor to the system after validating inputs.
        /// </summary>
        /// <param name="firstName">Contractor first name.</param>
        /// <param name="lastName">Contractor last name.</param>
        /// <param name="startDate">Contractor start date.</param>
        /// <param name="hourlyWage">Contractor hourly wage.</param>
        /// <returns>True if contractor added successfully; otherwise false.</returns>
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

        /// <summary>
        /// Adds a job to the system after validating inputs.
        /// </summary>
        /// <param name="title">Job title.</param>
        /// <param name="dateTime">Job date.</param>
        /// <param name="cost">Job cost.</param>
        /// <returns>True if job added successfully; otherwise false.</returns>
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

        /// <summary>
        /// Removes a contractor from the system using their ID.
        /// </summary>
        /// <param name="id">Contractor ID.</param>
        /// <returns>True if contractor removed; otherwise false.</returns>
        public bool RemoveContractor(int id)
        {
            for (int i = 0; i < Contractors.Count; i++)
            {
                if (Contractors[i].Id == id)
                {
                    Contractors.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Assigns a job to a contractor if both exist and are available.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="contractorId">Contractor ID.</param>
        /// <returns>True if assignment successful; otherwise false.</returns>
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
                if (contractor.Id == contractorId)
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
                if (!job.Completed &&
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

        /// <summary>
        /// Marks a job as completed using its ID.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <returns>True if job marked completed; otherwise false.</returns>
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

        /// <summary>
        /// Returns contractors who are not assigned to any active (not completed) jobs.
        /// </summary>
        /// <returns>List of available contractors.</returns>
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

        /// <summary>
        /// Returns jobs that are not assigned and not completed.
        /// </summary>
        /// <returns>List of unassigned jobs.</returns>
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

        /// <summary>
        /// Finds the first job with an exact matching cost.
        /// </summary>
        /// <param name="cost">Cost value to search for.</param>
        /// <returns>The matching job if found; otherwise null.</returns>
        public Job? GetJobByCost(decimal cost)
        {
            foreach (var job in Jobs)
            {
                if (job.Cost == cost)
                {
                    return job;
                }
            }
            return null;
        }
    }
}
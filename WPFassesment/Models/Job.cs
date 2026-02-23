using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFassesment.Models
{
    /// <summary>
    /// Represents a job that can be assigned to a contractor.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique job identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the scheduled date for the job.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the cost of the job.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the contractor assigned to the job.
        /// </summary>
        public Contractor? ContractorAssigned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the job is completed.
        /// </summary>
        public bool Completed { get; set; } = false;

        /// <summary>
        /// Validates that the job title is not empty.
        /// </summary>
        /// <param name="title">The title to validate.</param>
        /// <returns>True if valid; otherwise false.</returns>
        public static bool IsValidTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        /// <summary>
        /// Validates that the job cost is zero or greater.
        /// </summary>
        /// <param name="cost">Cost to validate.</param>
        /// <returns>True if valid; otherwise false.</returns>
        public static bool IsValidCost(decimal cost)
        {
            return cost >= 0;
        }
    }
}
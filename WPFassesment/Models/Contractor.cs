using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFassesment.Models
{
    /// <summary>
    /// Represents a contractor within the recruitment system.
    /// Stores personal and employment information.
    /// </summary>
    public class Contractor
    {
        /// <summary>
        /// Gets or sets the unique identifier for the contractor.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the contractor's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the contractor's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the contractor's full name.
        /// Setter exists to support WPF binding.
        /// </summary>
        public string FullName
        {
            get => $"{FirstName} {LastName}";
            set { }
        }
        /// <summary>
        /// Gets or sets the contractor's employment start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the contractor's hourly wage.
        /// </summary>
        public decimal HourlyWage { get; set; }

        /// <summary>
        /// Validates that first and last names are not null or whitespace.
        /// </summary>
        /// <param name="first">First name to validate.</param>
        /// <param name="last">Last name to validate.</param>
        /// <returns>True if both names are valid; otherwise false.</returns>
        public static bool IsValidName(string? first, string? last)
        {
            return !string.IsNullOrWhiteSpace(first)
                && !string.IsNullOrWhiteSpace(last);
        }
    }
}

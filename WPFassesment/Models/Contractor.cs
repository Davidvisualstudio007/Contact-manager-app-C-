using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFassesment.Models
{
    public class Contractor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName
        {
            get => $"{FirstName} {LastName}";
            set { }
        }
        public DateTime StartDate { get; set; }
        public decimal HourlyWage { get; set; }
        public static bool IsValidName(string? first, string? last)
        {
            return !string.IsNullOrWhiteSpace(first)
                && !string.IsNullOrWhiteSpace(last);
        }
    }
}

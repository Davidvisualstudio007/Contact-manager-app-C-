using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFassesment.Models
{
    public class Job
    {
        public string Title { get; set; } = string.Empty;
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public Contractor? ContractorAssigned { get; set; }
        public static bool IsValidTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }
        public static bool IsValidCost(decimal cost)
        {
            return cost >= 0;
        }
        public bool Completed { get; set; } = false;
    }
}

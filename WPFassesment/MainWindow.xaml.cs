using System.Windows;
using System.Windows.Controls;
using WPFassesment.Models;
using WPFassesment.ViewModels;
using System.Collections.Generic;


namespace WPFassesment
{
    public partial class MainWindow : Window
    {
        private RecruitmentSystem recruitmentSystem = new RecruitmentSystem();

        public MainWindow()
        {
            InitializeComponent();

            ContractorListBox.ItemsSource = recruitmentSystem.Contractors;
            JobListBox.ItemsSource = recruitmentSystem.Jobs;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string wageText = WageTextBox.Text;

            if (!decimal.TryParse(wageText, out decimal wage))
            {
                StatusText.Text = "Hourly wage must be a valid number.";
                return;
            }

            DateTime startDate = ContractorStartDatePicker.SelectedDate ?? DateTime.Today;

            bool success = recruitmentSystem.AddContractor(firstName, lastName, startDate, wage);

            if (!success)
            {
                StatusText.Text = "Invalid contractor details. Check names and wage > 0.";
                return;
            }

            StatusText.Text = "Contractor added.";

            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            WageTextBox.Text = string.Empty;
            ContractorStartDatePicker.SelectedDate = null;

            ContractorListBox.Items.Refresh();
        }



        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContractorListBox.SelectedItem is not Contractor selected)
            {
                StatusText.Text = "Choose which contractor to remove.";
                return;
            }

            bool removed = recruitmentSystem.RemoveContractor(selected.Id);

            if (removed)
            {
                StatusText.Text = "Contractor has been removed.";
                ContractorListBox.Items.Refresh();
            }
            else
            {
                StatusText.Text = "Could not remove this contractor.";
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = FirstNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                StatusText.Text = "Enter a name to search.";
                return;
            }

            Contractor? found = null;

            foreach (var contractor in recruitmentSystem.Contractors)
            {
                if (contractor.FirstName.Contains(keyword, System.StringComparison.OrdinalIgnoreCase) ||
                    contractor.LastName.Contains(keyword, System.StringComparison.OrdinalIgnoreCase))
                {
                    found = contractor;
                    break;
                }
            }

            if (found != null)
            {
                ContractorListBox.SelectedItem = found;
                ContractorListBox.ScrollIntoView(found);
                StatusText.Text = $"Contractor found: {found.FullName}";
            }
            else
            {
                StatusText.Text = "Contractor not found.";
            }
        }


        private void ContractorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContractorListBox.SelectedItem is not Contractor selected)
            {
                FirstNameTextBox.Text = "";
                LastNameTextBox.Text = "";
                WageTextBox.Text = "";
                ContractorStartDatePicker.SelectedDate = null;
                return;
            }

            FirstNameTextBox.Text = selected.FirstName;
            LastNameTextBox.Text = selected.LastName;
            WageTextBox.Text = selected.HourlyWage.ToString();
            ContractorStartDatePicker.SelectedDate = selected.StartDate;

            StatusText.Text = $"Selected: {selected.FullName}";
        }

        private void AddJobButton_Click(object sender, RoutedEventArgs e)
        {
            string title = JobTitleTextBox.Text;
            string costText = JobCostTextBox.Text;

            if (!decimal.TryParse(costText, out decimal cost))
            {
                StatusText.Text = "Job cost must be a valid number.";
                JobListBox.ItemsSource = recruitmentSystem.Jobs;
                JobListBox.Items.Refresh();
                return;
            }

            DateTime date = JobDatePicker.SelectedDate ?? DateTime.Today;

            bool success = recruitmentSystem.AddJob(title, date, cost);

            if (!success)
            {
                StatusText.Text = "Invalid job details. Check title and cost >= 0.";
                return;
            }

            StatusText.Text = "Job added.";

            JobTitleTextBox.Text = "";
            JobCostTextBox.Text = "";
            JobDatePicker.SelectedDate = null;

            JobListBox.Items.Refresh();
        }

        private void AssignJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (JobListBox.SelectedItem is not Job selectedJob)
            {
                StatusText.Text = "Select a job to assign.";
                return;
            }

            if (ContractorListBox.SelectedItem is not Contractor selectedContractor)
            {
                StatusText.Text = "Select a contractor to assign the job to.";
                return;
            }

            bool success = recruitmentSystem.AssignJob(selectedJob.Id, selectedContractor.Id);

            if (!success)
            {
                StatusText.Text = "Could not assign job. Check: job Completed/Contractor Busy/Already Assigned.";
                return;
            }

            StatusText.Text = $"Assigned '{selectedJob.Title}' to {selectedContractor.FullName}.";

            JobListBox.Items.Refresh();
        }
        private void CompleteJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (JobListBox.SelectedItem is not Job selectedJob)
            {
                StatusText.Text = "Select a job to complete.";
                return;
            }

            bool success = recruitmentSystem.CompleteJob(selectedJob.Id);

            if (!success)
            {
                StatusText.Text = "Job is already completed or could not be completed.";
                return;
            }

            StatusText.Text = $"Job '{selectedJob.Title}' marked as completed.";
            JobListBox.Items.Refresh();
        }

        private void ShowAllJobsButton_Click(object sender, RoutedEventArgs e)
        {
            JobListBox.ItemsSource = recruitmentSystem.Jobs;
            JobListBox.Items.Refresh();
            StatusText.Text = "Showing all jobs.";
        }

        private void ShowUnassignedJobsButton_Click(object sender, RoutedEventArgs e)
        {
            var unassigned = recruitmentSystem.GetUnassignedJobs();
            JobListBox.ItemsSource = unassigned;
            JobListBox.Items.Refresh();
            StatusText.Text = "Showing unassigned jobs.";
        }

        private void SearchJobByCostButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(JobCostMinTextBox.Text, out decimal minCost) ||
                !decimal.TryParse(JobCostMaxTextBox.Text, out decimal maxCost))
            {
                StatusText.Text = "Enter valid min and max cost.";
                return;
            }

            if (minCost > maxCost)
            {
                StatusText.Text = "Min cost cannot be greater than max cost.";
                return;
            }

            var matchingJobs = new List<Job>();

            foreach (var job in recruitmentSystem.Jobs)
            {
                if (job.Cost >= minCost && job.Cost <= maxCost)
                {
                    matchingJobs.Add(job);
                }
            }

            if (matchingJobs.Count == 0)
            {
                JobListBox.ItemsSource = new List<Job>();
                JobListBox.Items.Refresh();
                StatusText.Text = "No jobs found in that cost range.";
            }
            else
            {
                JobListBox.ItemsSource = matchingJobs;
                JobListBox.Items.Refresh();
                StatusText.Text = $"Showing jobs with cost between {minCost} and {maxCost}.";
            }
        }



    }
}

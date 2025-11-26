using System.Windows;
using System.Windows.Controls;
using WPFassesment.Models;
using WPFassesment.ViewModels;

namespace WPFassesment
{
    public partial class MainWindow : Window
    {
        private ContactViewModel viewModel = new ContactViewModel();
        private RecruitmentSystem recruitmentSystem = new RecruitmentSystem();

        public MainWindow()
        {
            InitializeComponent();

            ContactListBox.ItemsSource = recruitmentSystem.Contractors;
            JobListBox.ItemsSource = recruitmentSystem.Jobs;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string hourlyText = WageTextBox.Text;

            if (!decimal.TryParse(hourlyText, out decimal hourlyWage))
            {
                StatusText.Text = "Hourly wage must be a number.";
                return;
            }

            DateTime startDate = DateTime.Today;

            bool success = recruitmentSystem.AddContractor(firstName, lastName, startDate, hourlyWage);

            if (!success)
            {
                StatusText.Text = "Invalid contractor details. Check first/last name and wage.";
                return;
            }

            StatusText.Text = "Contractor added!";

            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            WageTextBox.Text = string.Empty;

            ContactListBox.Items.Refresh();
        }


        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactListBox.SelectedItem is not Contractor selected)
            {
                StatusText.Text = "Choose which contractor to remove.";
                return;
            }

            bool removed = recruitmentSystem.RemoveContractor(selected.Id);

            if (removed)
            {
                StatusText.Text = "Contractor has been removed.";
                ContactListBox.Items.Refresh();
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
                ContactListBox.SelectedItem = found;
                ContactListBox.ScrollIntoView(found);
                StatusText.Text = $"Contractor found: {found.FullName}";
            }
            else
            {
                StatusText.Text = "Contractor not found.";
            }
        }


        private void ContactListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContactListBox.SelectedItem is not Contractor selected)
            {
                FirstNameTextBox.Text = "";
                LastNameTextBox.Text = "";
                WageTextBox.Text = "";
                return;
            }

            FirstNameTextBox.Text = selected.FirstName;
            LastNameTextBox.Text = selected.LastName;
            WageTextBox.Text = selected.HourlyWage.ToString();

            StatusText.Text = $"Selected: {selected.FullName}";
        }
        private void AddJobButton_Click(object sender, RoutedEventArgs e)
        {
            string title = JobTitleTextBox.Text;
            string costText = JobCostTextBox.Text;

            if (!decimal.TryParse(costText, out decimal cost))
            {
                StatusText.Text = "Job cost must be a valid number.";
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

            if (ContactListBox.SelectedItem is not Contractor selectedContractor)
            {
                StatusText.Text = "Select a contractor to assign the job to.";
                return;
            }

            bool success = recruitmentSystem.AssignJob(selectedJob.Id, selectedContractor.Id);

            if (!success)
            {
                StatusText.Text = "Could not assign job. Check: job not completed/already assigned, contractor available.";
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

    }
}

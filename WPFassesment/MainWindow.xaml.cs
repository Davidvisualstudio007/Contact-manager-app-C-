using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFassesment.Models;
using WPFassesment.ViewModels;

namespace WPFassesment
{
    public partial class MainWindow : Window
    {
        private ContactViewModel viewModel = new ContactViewModel();

        public MainWindow()
        {
            InitializeComponent();
            ContactListBox.ItemsSource = viewModel.Contacts;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = viewModel.AddContact(
                FullNameTextBox.Text,
                PhoneTextBox.Text,
                EmailTextBox.Text);

            if (!success)
            {
                StatusText.Text = "Invalid details. Please check Name, Phone, or email.";
                return;
            }
            StatusText.Text = "Content added!";

            FullNameTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;

            // refresh the list contact
            ContactListBox.Items.Refresh();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactListBox.SelectedItem is not Contact selected)
            {
                StatusText.Text = "Choose which you want gone";
                return;
            }

            bool removed = viewModel.RemoveContact(selected.Id);

            if (removed)
            {
                StatusText.Text = "Contact has been removed";
                ContactListBox.Items.Refresh();
            }
            else
            {
                StatusText.Text = "Could not remove this contact";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = FullNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                StatusText.Text = "Enter a name to search";
                return;
            }

            var found = viewModel.SearchContact(keyword);

            if (found != null)
            {
                ContactListBox.SelectedItem = found;
                ContactListBox.ScrollIntoView(found);
                StatusText.Text = "Contact found.";
            }
            else
            {
                StatusText.Text = "Contact not found or doesn't exist";
            }
        }

        private void ContactListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ContactListBox.SelectedItem as Contact;

            if (selected != null)
            {
                FullNameTextBox.Text = string.Empty;
                PhoneTextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
                return;
            }

            FullNameTextBox.Text = selected.FullName;
            PhoneTextBox.Text = selected.Phone;
            EmailTextBox.Text = selected.Email;

            StatusText.Text = $"Selected: {selected.FullName}";
        }
    }
}

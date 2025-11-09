using System.Collections.Generic;
using System.Windows.Navigation;
using WPFassesment.Models;

namespace WPFassesment.ViewModels
{
    public class ContactViewModel
    {
        private int nextId = 1;
        public List<Contact> contacts { get; } = new List<Contact>();

        public bool AddContact(string fullName, string phone, string email)
        {
            if (!Contact.IsValidName(fullName))
                return false;

            if (!Contact.IsValidEmail(email))
                return false;

            if (!Contact.IsValidPhone(phone))
                return false;

            var newContact = new Contact
            {
                id = nextId,
                fullName = fullName,
                phone = phone,
                email = email
            };

            contacts.Add(newContact);
            nextId++;
            return true;
        }
        public bool RemoveContact(int id)
        {
            foreach (var contact in contacts)
            {
                if (contact.id == id)
                {
                    contacts.Remove(contact);
                    return true;
                }
            }

            return false;
        }
        public Contact? SearchContact(string fullName)
        {
            foreach (var contact in contacts)
            {
                if (contact.fullName.ToLower().Contains(fullName.ToLower()))
                {
                    return contact; 
                }
            }
            return null;
        }
    }
}

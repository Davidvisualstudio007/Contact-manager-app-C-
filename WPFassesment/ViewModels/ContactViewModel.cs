using System.Collections.Generic;
using System.Windows.Navigation;
using WPFassesment.Models;

namespace WPFassesment.ViewModels
{
    public class ContactViewModel
    {
        private int nextId = 1;
        public List<Contact> Contacts { get; } = new List<Contact>();

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
                Id = nextId,
                FullName = fullName,
                Phone = phone,
                Email = email
            };

            Contacts.Add(newContact);
            nextId++;
            return true;
        }
        public bool RemoveContact(int id)
        {
            for (int i = 0; i < Contacts.Count; i++)
            {
                if (Contacts[i].Id == id)
                {
                    Contacts.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public Contact? SearchContact(string fullName)
        {
            foreach (var contact in Contacts)
            {
                if (contact.FullName.ToLower().Contains(fullName.ToLower()))
                {
                    return contact; 
                }
            }
            return null;
        }
    }
}

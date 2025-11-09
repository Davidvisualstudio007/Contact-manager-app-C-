using System;

namespace WPFassesment.Models
{
    public class Contact
    {
        public int id {  get; set; }
        // this can take and change the id
        public string fullName { get; set; } = string.Empty;
        // this can take and change the name
        public string email { get; set; } = string.Empty;
        // this can take and change the email
        public string phone { get; set; } = string.Empty;
        // this can take and change the phone number
        // string because most number include symbols
        public static bool IsValidName(string? name)
        // checks if name is not empty or while space
        {
            return !string.IsNullOrEmpty(name);
        }
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            // ok if no email
                return true;
            return email.Contains("@") && email.Contains(".");
            // checks if it have @ and . as most email(or all) have
        }
        public static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            foreach (var character in phone)
            {
                if (!(char.IsDigit(character) ||
                    character == '+' ||
                    character == '-' ||
                    character == ' '))
                return false;
            }
            return true;
        }
    }
}
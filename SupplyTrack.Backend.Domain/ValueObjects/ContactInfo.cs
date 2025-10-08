using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.ValueObjects
{
    internal class ContactInfo
    {
        string Email { get; set; }
        string PhoneNumber { get; set; }
        string Address { get; set; }
        public ContactInfo(string email, string phoneNumber, string address)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("L'adresse email est requis");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Le numéro de téléphone est requis");
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("L'adresse est requis");
            }
            if(!email.Contains("@"))
            {
                throw new ArgumentException("L'adresse email n'est pas valide");
            }
            if(phoneNumber.Length < 10)
            {
                throw new ArgumentException("Le numéro de téléphone n'est pas valide");
            }
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }

    }
}

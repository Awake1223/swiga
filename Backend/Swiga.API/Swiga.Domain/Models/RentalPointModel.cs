using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class RentalPointModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        private RentalPointModel(Guid id, string name, string address, string city, string phoneNumber, string email) 
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public static (RentalPointModel RentalPointModel, string error) Create (Guid id, string name, string address, string city, string phoneNumber, string email)
        {
            var rentalPoint = new RentalPointModel(id, name, address, city, phoneNumber, email);

            string errorString = string.Empty;

            if (string.IsNullOrEmpty(name))
            {
                errorString = "Ошибка ввода названия";
            }
            else if (string.IsNullOrEmpty(address))
            {
                errorString = "Ошибка ввода адреса";
            }
            else if (string.IsNullOrEmpty(city))
            {
                errorString = "Ошибка ввода города";
            }
            else if (string.IsNullOrEmpty(phoneNumber))
            {
                errorString = "Ошибка ввода номера телефона";
            }
            else if (string.IsNullOrEmpty(email))
            {
                errorString = "Ошибка ввода email";
            }

            if(!string.IsNullOrEmpty(errorString))
            {
                return (null, errorString);
            }

            return (rentalPoint, errorString);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public abstract class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }


        protected UserModel(Guid id, string email, string phoneNumber, string password, DateTime createdAt, Role role) 
        {
            Id = id;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            CreatedAt = createdAt;
            Role = role;
        }

        
    }
    public enum Role {
        Client = 1,
        Admin = 2,
    }


}

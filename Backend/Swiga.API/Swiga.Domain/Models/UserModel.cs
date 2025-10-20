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

        public string FirstName { get; set; }  // ✅ ДОБАВИТЬ
        public string LastName { get; set; }   // ✅ ДОБАВИТЬ



        protected UserModel(Guid id, string firstName, string lastName, string email, string phoneNumber, string password, DateTime createdAt, Role role)
        {
            Id = id;
            FirstName = firstName;    // 2-й параметр
            LastName = lastName;      // 3-й параметр  
            Email = email;            // 4-й параметр
            PhoneNumber = phoneNumber; // 5-й параметр
            Password = password;      // 6-й параметр
            CreatedAt = createdAt;    // 7-й параметр
            Role = role;              // 8-й параметр
        }


    }
    public enum Role {
        Client = 1,
        Admin = 2,
    }


}

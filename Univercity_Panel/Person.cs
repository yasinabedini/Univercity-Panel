using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Univercity_Panel
{
    public abstract class Person
    {
        public int PersonId { get; set; }
        public string NationalCode { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }        
        public DateTime RegisterDate { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }


        public virtual Role Role { get; set; }


        public Person() { }
        public Person(string NationalCode, string Name, string Family, string Mobile, string Password,Role Role)
        {
            this.NationalCode = NationalCode;
            this.Name = Name;
            this.Family = Family;
            this.Mobile = Mobile;
            this.Password = Password;
            this.RegisterDate = DateTime.Now;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;
            this.Role = Role;
        }
        public Person(int PersonId, string NationalCode, string Name, string Family, string Mobile, string Password,Role Role)
        {
            this.PersonId = PersonId;
            this.NationalCode = NationalCode;
            this.Name = Name;
            this.Family = Family;
            this.Mobile = Mobile;
            this.Password = Password;
            this.RegisterDate = DateTime.Now;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;
            this.Role = Role;
        }
    }
}

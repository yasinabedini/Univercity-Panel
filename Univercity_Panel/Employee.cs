using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univercity_Panel
{
    public class Employee : Person
    {
        public int Salary { get; set; }
        public string Department { get; set; }



        public virtual ICollection<Score> EditScore { get; set; }



        public Employee() { }
        public Employee(string NationalCode, string Name, string Family, string Mobile, string Password, int Salary, string Department, Role Role) : base(NationalCode, Name, Family, Mobile, Password, Role)
        {
            this.Salary = Salary;
            this.Department = Department;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;
        }
        public Employee(int PersonId, string NationalCode, string Name, string Family, string Mobile, string Password, int Salary, string Department, Role Role) : base(PersonId, NationalCode, Name, Family, Mobile, Password, Role)
        {
            this.Salary = Salary;
            this.Department = Department;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;
        }
    }
}

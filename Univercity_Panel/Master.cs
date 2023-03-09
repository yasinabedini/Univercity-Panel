using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univercity_Panel
{
    public class Master : Person
    {
        public int Salary { get; set; }
        public string Degree { get; set; }



        
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Score> RegisterScore { get; set; }



        public Master() { }
        public Master(string NationalCode, string Name, string Family, string Mobile, string Password, int Salary, string Degree,Role Role) : base(NationalCode, Name, Family, Mobile, Password,Role)
        {
            this.Salary = Salary;
            this.Degree = Degree;
            this.RegisterDate = DateTime.Now;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;           
            Courses = new List<Course>();
        }
        public Master(int PersonId, string NationalCode, string Name, string Family, string Mobile, string Password, int Salary, string Degree,Role Role) : base(PersonId, NationalCode, Name, Family, Mobile, Password,Role)
        {
            this.Salary = Salary;
            this.Degree = Degree;
            this.RegisterDate = DateTime.Now;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;         
            Courses = new List<Course>();          
        }


        public override string ToString()
        {
            return string.Format($" {PersonId}    \t{NationalCode}\t{Mobile}\t{Password}\t\t{Name}\t{Family}    \t{Salary} \t{Degree}    \t{IsActive}");

        }
    }
}

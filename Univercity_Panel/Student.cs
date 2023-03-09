using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univercity_Panel
{
    public class Student : Person
    {
        public string Degree { get; set; }
        public int Code { get; set; }
        public static int AutoCode { get; set; } = 970098734;




        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Score> Scores { get; set; }



        public Student() { }
        public Student(string NationalCode, string Name, string Family, string Mobile, string Password, string Degree,Role Role) : base(NationalCode, Name, Family, Mobile, Password,Role)
        {
            this.Degree = Degree;
            using (UniverCityDbContext db = new UniverCityDbContext("UnDbContext"))
            {
                if (db.Students.Any())
                {
                    AutoCode = db.Students.Max(t => t.Code);                    
                }                         
            }
            AutoCode++;
            this.Code = AutoCode;
            this.RegisterDate = DateTime.Now;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;           
            Scores = new List<Score>();
            Courses = new List<Course>();
        }
        public Student(int PersonId, string NationalCode, string Name, string Family, string Mobile, string Password, string Degree,Role Role) : base(PersonId, NationalCode, Name, Family, Mobile, Password,Role)
        {
            this.Degree = Degree;
            using (UniverCityDbContext db = new UniverCityDbContext("UnDbContext"))
            {
                AutoCode = db.Students.Max(t => t.Code);
            }
            AutoCode++;
            this.Code = AutoCode;
            this.RegisterDate = DateTime.Now;
            this.BirthDate = DateTime.Now.AddYears(new Random().Next(23, 55));
            this.IsActive = true;          
            Scores = new List<Score>();
            Courses = new List<Course>();
        }




        public override string ToString()
        {
            return string.Format($" {PersonId}    \t{NationalCode}\t{Mobile}\t{Password}\t\t{Name}\t{Family}\t\t{Code}\t{Degree}\t{IsActive}");
            
        }
    }
}

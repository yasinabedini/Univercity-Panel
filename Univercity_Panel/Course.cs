using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univercity_Panel
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Unit { get; set; }
        public DateTime Registerdate { get; set; }
        public bool IsActive { get; set; }



        public virtual Master Master { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Score> Scores { get; set; }



        public Course() { }
        public Course(string Name, int Unit)
        {
            this.Name = Name;
            this.Unit = Unit;
            IsActive = true;
            Registerdate = DateTime.Now;            
        }
        public Course(int Id, string Name, int Unit)
        {
            this.Id = Id;
            this.Name = Name;
            this.Unit = Unit;
            IsActive = true;
            Registerdate = DateTime.Now;            
        }



        public override string ToString()
        {
            return string.Format($"Id :  |{Id}|\t\tName : |{Name}|    \t      Unit : |{Unit}|");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univercity_Panel
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public virtual ICollection<Master> Masters { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Student> Students { get; set; }



        public Role() { }
        public Role(string Name)
        {
            this.Name = Name;
        }
    }
}

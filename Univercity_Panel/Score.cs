using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univercity_Panel
{
    public class Score
    {
        public int Id { get; set; }
        public int ScoreNumber { get; set; }                
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }



        public virtual Student Student { get; set; }
        public virtual Master Registerar { get; set; }
        public virtual Course Course { get; set; }
        public virtual Employee Edit { get; set; }



        public Score() { }
        public Score(int Id,int ScoreNumber, Course Relevant_Course, Student Relevant_Student, Master MasterRegistrar, Employee EmployeeEditor)
        {
            this.Id = Id;
            this.ScoreNumber = ScoreNumber;
            Course = Relevant_Course;
            Student = Relevant_Student;
            Registerar = MasterRegistrar;
            Edit = EmployeeEditor;
            RegisterDate = DateTime.Now;
            IsActive = true;
        }
        public Score(int Id, int ScoreNumber, Course Relevant_Course, Student Relevant_Student, Master MasterRegistrar)
        {
            this.Id = Id;
            this.ScoreNumber = ScoreNumber;
            Course = Relevant_Course;
            Student = Relevant_Student;
            Registerar = MasterRegistrar;
            Edit = null;
            RegisterDate = DateTime.Now;
            IsActive = true;
        }
        public Score( int ScoreNumber, Course Relevant_Course, Student Relevant_Student, Master MasterRegistrar, Employee EmployeeEditor)
        {            
            this.ScoreNumber = ScoreNumber;
            Course = Relevant_Course;
            Student = Relevant_Student;
            Registerar = MasterRegistrar;
            Edit = EmployeeEditor;
            RegisterDate = DateTime.Now;
            IsActive = true;
        }
        public Score( int ScoreNumber, Course Relevant_Course, Student Relevant_Student, Master MasterRegistrar)
        {            
            this.ScoreNumber = ScoreNumber;
            Course = Relevant_Course;
            Student = Relevant_Student;
            Registerar = MasterRegistrar;
            Edit = null;
            RegisterDate = DateTime.Now;
            IsActive = true;
        }


        public override string ToString()
        {
            return string.Format($"Id : {Id}\tScore : {ScoreNumber}\tStudent Name : {Student.Name} {Student.Family}\tCourse Name : {Course.Name}\tRegisterar : {Registerar.Name} {Registerar.Family}");
        }
    }

}

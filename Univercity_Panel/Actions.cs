using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace Univercity_Panel
{
    public static class Actions
    {
        /// <summary>
        /// Print DateTime For Login Form
        /// </summary>
        public static Action printDatetime = delegate
          {
              Console.Clear();
              PersianCalendar pc = new PersianCalendar();
              Console.WriteLine("=======================================================================================================================");
              Console.WriteLine($"  {pc.GetYear(DateTime.Now)}/{pc.GetMonth(DateTime.Now)}/{pc.GetDayOfMonth(DateTime.Now)}\t{pc.GetHour(DateTime.Now)}:{pc.GetMinute(DateTime.Now)}");
              Console.WriteLine("=======================================================================================================================");
          };

        /// <summary>
        /// Print DateTime And Status For All Roles
        /// </summary>
        public static Action<string> printDateTimeAndStatus = delegate (string type)
         {
             type.ToLower().Trim();
             switch (type)
             {
                 case "master":

                     Console.Clear();
                     PersianCalendar pc = new PersianCalendar();
                     Console.WriteLine("=======================================================================================================================");
                     using (UniverCityDbContext dbTime = new UniverCityDbContext("undbcontext"))
                     {
                         Console.Write($"\tStudent : {dbTime.Students.Count()} | Course : {dbTime.Masters.Count()}\t\t\t\t");
                     }
                     Console.WriteLine($"  {pc.GetYear(DateTime.Now)}/{pc.GetMonth(DateTime.Now)}/{pc.GetDayOfMonth(DateTime.Now)}\t{pc.GetHour(DateTime.Now)}:{pc.GetMinute(DateTime.Now)}");
                     Console.WriteLine("=======================================================================================================================");


                     break;

                 case "employee":

                     Console.Clear();
                     PersianCalendar pcEmployee = new PersianCalendar();
                     Console.WriteLine("=======================================================================================================================");
                     using (UniverCityDbContext dbTime = new UniverCityDbContext("undbcontext"))
                     {
                         Console.Write($"\tMaster:{dbTime.Masters.Count()} | Employee : {dbTime.Employees.Count()} | Student : {dbTime.Students.Count()} | Course : {dbTime.Masters.Count()}\t\t\t\t");
                     }
                     Console.WriteLine($"  {pcEmployee.GetYear(DateTime.Now)}/{pcEmployee.GetMonth(DateTime.Now)}/{pcEmployee.GetDayOfMonth(DateTime.Now)}\t{pcEmployee.GetHour(DateTime.Now)}:{pcEmployee.GetMinute(DateTime.Now)}");
                     Console.WriteLine("=======================================================================================================================");
                     break;

                 case "student":

                     Console.Clear();
                     PersianCalendar pcStudent = new PersianCalendar();
                     Console.WriteLine("=======================================================================================================================");
                     using (UniverCityDbContext dbTime = new UniverCityDbContext("undbcontext"))
                     {
                         Console.Write($"\t Course : {dbTime.Courses.Count()}\t\t\t\t");
                     }
                     Console.WriteLine($"  {pcStudent.GetYear(DateTime.Now)}/{pcStudent.GetMonth(DateTime.Now)}/{pcStudent.GetDayOfMonth(DateTime.Now)}\t{pcStudent.GetHour(DateTime.Now)}:{pcStudent.GetMinute(DateTime.Now)}");
                     Console.WriteLine("=======================================================================================================================");
                     break;

             }
         };

        /// <summary>
        /// Come Back To Menu
        /// </summary>
        public static Action comeBackToMainMenu = delegate
        {
            Console.WriteLine("\n\n\n\tPress Any Key For ComeBack To Menu ..... ");
            Console.ReadKey();
        };

        /// <summary>
        /// If the user enters something another instead of Int
        /// </summary>
        public static Action<string> warningEnterNumber = delegate (string title)
        {
            Console.WriteLine("\n\tWarning !!!!! ");
            Thread.Sleep(1500);
            Console.WriteLine($"\n\tYou Can Just Enter Number For {title} ");
            Thread.Sleep(2000);
        };

        /// <summary>
        /// If the user enters an ID that is not in the list
        /// </summary>
        public static Action<int, string> warningNotFound = delegate (int id, string title)
         {
             Console.WriteLine($"\n\tUsre By Id[{id}] not Found In {title} .... ");
             Thread.Sleep(2000);
             Console.WriteLine("\n\tTry Again");
             Thread.Sleep(2000);
         };

        /// <summary>
        /// For Message To User If Her/His Acsess Is Blocked
        /// </summary>
        public static Action blockMessage = delegate
        {
            printDatetime();
            Console.WriteLine("\n\tYour Acsess Blocked ....");
            Thread.Sleep(1000);
            Console.Beep();
        };

        /// <summary>
        /// Show All Course With Or Without Master
        /// </summary>
        public static Action<bool> showAllCourse = delegate (bool showMaster)
        {
            using (UniverCityDbContext showCourse = new UniverCityDbContext("undbcontext"))
            {
                foreach (Course course in showCourse.Courses.ToList())
                {
                    Console.WriteLine("\n" + course);
                    if (showMaster == true)
                    {
                        if (course.Master != null)
                        {
                            Console.WriteLine("______");
                            Console.WriteLine($"Master : {course.Master.Name} {course.Master.Family}");
                        }
                    }
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                }
            }


        };

        /// <summary>
        /// Show All Student With Course Or WithOut Course
        /// </summary>
        public static Action<bool> showAllStudent = delegate (bool showCourseList)
        {
            using (UniverCityDbContext showStudent = new UniverCityDbContext("undbcontext"))
            {
                foreach (Student student in showStudent.Students.ToList())
                {
                    Console.WriteLine("\n" + student);

                    if (student.Courses.ToList().Count() != 0)
                    {
                        if (showCourseList == true)
                        {
                            Console.WriteLine("\n\tCourseList : ");
                            foreach (var course in student.Courses)
                            {
                                Console.WriteLine("\t" + course + "\n");
                            }
                        }
                    }
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                }
            }

        };

        /// <summary>
        /// Show All Master With Course Or WithOut Course
        /// </summary>
        public static Action<bool> showAllMaster = delegate (bool showCourseList)
        {
            using (UniverCityDbContext showMaster = new UniverCityDbContext("undbcontext"))
            {
                Console.WriteLine();
                foreach (Master master in showMaster.Masters.ToList())
                {
                    Console.WriteLine(master);

                    if (master.Courses.ToList().Count() != 0)
                    {
                        if (showCourseList == true)
                        {
                            Console.WriteLine("\n\tCourseList : ");
                            foreach (var course in master.Courses)
                            {
                                Console.WriteLine("\t" + course + "\n");
                            }
                        }
                    }
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                }
            }
        };

        /// <summary>
        /// Search Course By Id
        /// </summary>
        public static Action<string> searchCourseById = delegate (string panelType)
        {
            using (UniverCityDbContext dbSerachCourse = new UniverCityDbContext("undbcontext"))
            {
            searchById:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Course Id For Search : ");
                int courseId;
                while (!int.TryParse(Console.ReadLine(), out courseId))
                {
                    printDateTimeAndStatus(panelType);
                    warningEnterNumber("Course Id");
                    goto searchById;
                }
                if (!dbSerachCourse.Courses.Any(t => t.Id == courseId))
                {
                    printDateTimeAndStatus(panelType);
                    warningNotFound(courseId, "Course List");
                    goto searchById;
                }
                printDateTimeAndStatus(panelType);
                Console.WriteLine("\n\n  " + dbSerachCourse.Courses.First(t => t.Id == courseId));
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Course By Name
        /// </summary>
        public static Action<string> searchCourseByName = delegate (string panelType)
        {
            using (UniverCityDbContext dbSerachCourse = new UniverCityDbContext("undbcontext"))
            {
            searchByName:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Course Name For Search : ");
                string courseName = Console.ReadLine();
                if (!dbSerachCourse.Courses.Any(t => t.Name == courseName))
                {
                    printDateTimeAndStatus(panelType);
                    Console.WriteLine($"there Isn't Any Course By This Name({courseName})");
                    Thread.Sleep(3000);
                    goto searchByName;
                }
                printDateTimeAndStatus(panelType);
                Console.WriteLine("\n" + dbSerachCourse.Courses.First(t => t.Name == courseName));
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Score By Id
        /// </summary>
        public static Action<string> searchScoreById = delegate (string panelType)
        {
            using (UniverCityDbContext dbSerachScore = new UniverCityDbContext("undbcontext"))
            {
            searchById:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Score Id For Search : ");
                int scoreId;
                while (!int.TryParse(Console.ReadLine(), out scoreId))
                {
                    printDateTimeAndStatus(panelType);
                    warningEnterNumber("Score Id");
                    goto searchById;
                }
                //If Not Available Score By This Id
                if (!dbSerachScore.Scores.Any(t => t.Id == scoreId))
                {
                    printDateTimeAndStatus(panelType);
                    warningNotFound(scoreId, "Score List");
                    goto searchById;
                }

                printDateTimeAndStatus(panelType);
                Console.WriteLine("\n  " + dbSerachScore.Scores.FirstOrDefault(t => t.Id == scoreId));
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Score By StudentId
        /// </summary>
        public static Action<string> searchScoreByStudent = delegate (string panelType)
        {
            using (UniverCityDbContext dbSerachScore = new UniverCityDbContext("undbcontext"))
            {
            searchByStudent:
                printDateTimeAndStatus(panelType);
                Console.WriteLine("StudentList");
                showAllStudent(false);
                Console.Write("\n\tEnter Student Id For See Her/His Score : ");
                int studentId;
                while (!int.TryParse(Console.ReadLine(), out studentId))
                {
                    printDateTimeAndStatus(panelType);
                    warningEnterNumber("Student Id");
                    goto searchByStudent;
                }
                if (!dbSerachScore.Students.Any(t => t.PersonId == studentId))
                {
                    printDateTimeAndStatus(panelType);
                    warningNotFound(studentId, "Student List");
                    goto searchByStudent;
                }
                //If Student Does'nt Have Any Course
                if (dbSerachScore.Students.FirstOrDefault(t => t.PersonId == studentId).Scores.Count() == 0)
                {
                    printDateTimeAndStatus(panelType);
                    Console.WriteLine("\n\tNo Scores have been Registered for this student ");
                    goto searchByStudent;
                }

                printDateTimeAndStatus(panelType);
                foreach (Score score in dbSerachScore.Students.FirstOrDefault(t => t.PersonId == studentId).Scores.ToList())
                {
                    Console.WriteLine("\n  " + score);
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                }
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Score By Course
        /// </summary>
        public static Action<string> searchScoreByCourse = delegate (string panelType)
        {
            using (UniverCityDbContext dbSerachScore = new UniverCityDbContext("undbcontext"))
            {
            searchByCourse:
                printDateTimeAndStatus(panelType);
                Console.WriteLine("Course List");
                showAllCourse(false);
                Console.Write("\n\tEnter the course Id to see its Scores : ");
                int courseId;
                while (!int.TryParse(Console.ReadLine(), out courseId))
                {
                    printDateTimeAndStatus(panelType);
                    Actions.warningEnterNumber("Course Id");
                    goto searchByCourse;
                }
                if (!dbSerachScore.Courses.Any(t => t.Id == courseId))
                {
                    printDateTimeAndStatus(panelType);
                    warningNotFound(courseId, "Course List");
                    goto searchByCourse;
                }
                //If Student Does'nt Have Any Course
                if (dbSerachScore.Courses.FirstOrDefault(t => t.Id == courseId).Scores.Count() == 0)
                {
                    printDateTimeAndStatus(panelType);
                    Console.WriteLine("\n\tNo Scores have been Registered for this Course ");
                    goto searchByCourse;
                }
                printDateTimeAndStatus(panelType);
                foreach (Score score in dbSerachScore.Courses.First(t => t.Id == courseId).Scores.ToList())
                {
                    Console.WriteLine("\n  " + score);
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                }
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Student By Id
        /// </summary>
        public static Action<string> SearchStudentById = delegate (string panelType)
        {
            using (UniverCityDbContext dbSearchStudent = new UniverCityDbContext("undbcontext"))
            {
            searchById:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Student Id For Search : ");
                int searchStudent;
                while (!int.TryParse(Console.ReadLine(), out searchStudent))
                {
                    printDateTimeAndStatus(panelType);
                    warningEnterNumber("Student Id");
                    goto searchById;
                }
                if (!dbSearchStudent.Students.Any(t => t.PersonId == searchStudent))
                {
                    printDateTimeAndStatus(panelType);
                    warningNotFound(searchStudent, "StudentList");
                    goto searchById;
                }
                printDateTimeAndStatus(panelType);
                Console.WriteLine("\n" + dbSearchStudent.Students.First(t => t.PersonId == searchStudent));
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Student By Name
        /// </summary>
        public static Action<string> SearchStudentByName = delegate (string panelType)
        {
            using (UniverCityDbContext dbSearchStudent = new UniverCityDbContext("undbcontext"))
            {
            searchByName:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Student Name For Search : ");
                string studentName = Console.ReadLine();
                if (!dbSearchStudent.Students.Any(t => t.Name == studentName))
                {
                    printDateTimeAndStatus(panelType);
                    Console.WriteLine($"\n\n\tthere Isn't Any Student By This Name({studentName})");
                    Thread.Sleep(3000);
                    goto searchByName;
                }
                printDateTimeAndStatus(panelType);
                Console.WriteLine();
                foreach (Student student in dbSearchStudent.Students.Where(t => t.Name == studentName).ToList())
                {
                    Console.WriteLine(student);
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                }
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Student By Family
        /// </summary>
        public static Action<string> SearchStudentByFamily = delegate (string panelType)
        {
            using (UniverCityDbContext dbSearchStudent = new UniverCityDbContext("undbcontext"))
            {
            searchByFamily:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Student Family For Search : ");
                string studentFamily = Console.ReadLine();
                if (!dbSearchStudent.Students.Any(t => t.Family == studentFamily))
                {
                    printDateTimeAndStatus(panelType);
                    Console.WriteLine($"\n\n\tthere Isn't Any Student By This Family({studentFamily})");
                    Thread.Sleep(3000);
                    goto searchByFamily;
                }
                printDateTimeAndStatus(panelType);
                Console.WriteLine();
                foreach (Student student in dbSearchStudent.Students.Where(t => t.Family == studentFamily).ToList())
                {
                    Console.WriteLine(student);
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                }
                comeBackToMainMenu();
            }
        };

        /// <summary>
        /// Search Student By Mobile
        /// </summary>
        public static Action<string> SearchStudentByMobile = delegate (string panelType)
        {
            using (UniverCityDbContext dbSearchStudent = new UniverCityDbContext("undbcontext"))
            {
            searchByMobile:
                printDateTimeAndStatus(panelType);
                Console.Write("\n\tEnter Student Mobile For Search : ");
                string StudentMobile = Console.ReadLine();
                if (!dbSearchStudent.Students.Any(t => t.Mobile == StudentMobile))
                {
                    printDateTimeAndStatus(panelType);
                    Console.WriteLine($"\n\n\tthere Isn't Any Student By This mobile({StudentMobile})");
                    Thread.Sleep(3000);
                    goto searchByMobile;
                }
                printDateTimeAndStatus(panelType);
                Console.WriteLine("\n" + dbSearchStudent.Students.First(t => t.Mobile == StudentMobile));
                comeBackToMainMenu();
            }
        };
    }
}

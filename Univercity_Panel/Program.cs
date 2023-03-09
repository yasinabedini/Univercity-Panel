using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Univercity_Panel
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Defualt Data

            using (UniverCityDbContext dbDefault = new UniverCityDbContext("undbcontext"))
            {
                Console.WriteLine("\n\tCreating DataBase Please Wait .......");
                dbDefault.Database.CreateIfNotExists();
                Actions.printDatetime();
                Console.WriteLine("\n\tTrying to Login to the Database .......");

                if (!dbDefault.Roles.Any())
                {
                    dbDefault.Roles.Add(new Role("master"));
                    dbDefault.Roles.Add(new Role("employee"));
                    dbDefault.Roles.Add(new Role("student"));
                    dbDefault.SaveChanges();
                }
                if (!dbDefault.Employees.Any())
                {
                    dbDefault.Employees.Add(new Employee("0052586957", "ali", "ghorbani", "09122589647", "0000", 6500000, "Admin",dbDefault.Roles.First(t=>t.Name == "employee")));
                }
                if (!dbDefault.Masters.Any())
                {
                    dbDefault.Masters.Add(new Master("0012547698", "sara", "abdi", "09105258754", "0000", 6500000, "manage",dbDefault.Roles.First(t=>t.Name=="master")));
                }
                if (!dbDefault.Students.Any())
                {
                    dbDefault.Students.Add(new Student("0023698547", "yasin", "abedini", "09125874869", "0000", "it", dbDefault.Roles.First(t => t.Name == "student")));
                }
                if (!dbDefault.Courses.Any())
                {
                    dbDefault.Courses.Add(new Course(10, "csharp", 3));
                }
                dbDefault.SaveChanges();
            }

            #endregion

            #region Pattern

            string validNationalCode = "^[0-9]{10}$";
            string validMobile = @"^((\+98|0)9\d{9})$";

        #endregion

        #region Login form
        login:
            Actions.printDatetime();
            Console.WriteLine("\tLogin Form : ");
            Console.Write("\n\tEnter Your Mobile : ");
            string mobile = Console.ReadLine();
            //Valid Mobile Number
            if (!Regex.IsMatch(mobile, validMobile))
            {
                Actions.printDatetime();
                Console.WriteLine("\n\n\tMobile Number Is Not Correct .... ");
                Console.WriteLine("\n\tTry Again .... ");
                Thread.Sleep(3000);
                goto login;
            }
            Console.Write("\n\tEnter Your Password : ");
            string password = Console.ReadLine();

            Master masterSign;
            Employee employeeSign;
            Student studentSign;

            UniverCityDbContext dbAllUser = new UniverCityDbContext("undbcontext");

            var users = dbAllUser.Masters.Select(t => new { Id = t.PersonId, Code = t.NationalCode, Role = t.Role, Mobile = t.Mobile })
                .Concat(dbAllUser.Students.Select(t => new { Id = t.PersonId, Code = t.NationalCode, Role = t.Role, Mobile = t.Mobile })
                .Concat(dbAllUser.Employees.Select(t => new { Id = t.PersonId, Code = t.NationalCode, Role = t.Role, Mobile = t.Mobile })));

            using (UniverCityDbContext dbValidSignIn = new UniverCityDbContext("undbcontext"))
            {
                masterSign = dbValidSignIn.Masters.FirstOrDefault(t => t.Mobile == mobile && t.Password == password);
                if (masterSign != null)
                {
                    // IF IsActive Was Blocked 
                    if (masterSign.IsActive == false)
                    {
                        Actions.blockMessage();
                        goto login;
                    }
                    goto SignIn_Master;

                }
                employeeSign = dbValidSignIn.Employees.FirstOrDefault(t => t.Mobile == mobile && t.Password == password);
                if (employeeSign != null)
                {
                    if (employeeSign.IsActive == false)
                    {
                        Actions.blockMessage();
                        goto login;
                    }
                    goto SignIn_Employee;
                }
                studentSign = dbValidSignIn.Students.FirstOrDefault(t => t.Mobile == mobile && t.Password == password);
                if (studentSign != null)
                {
                    if (studentSign.IsActive == false)
                    {
                        Actions.blockMessage();
                        goto login;
                    }
                    goto SignIn_Student;
                }
                else
                {
                    Actions.printDatetime();
                    Console.WriteLine("\n\tMobile Or Password Incorrect ........");
                    Thread.Sleep(2000);
                    Console.WriteLine("\n\tTry Again .....");
                    Thread.Sleep(1000);
                    goto login;
                }
            }
        #endregion


        #region Logged Master

        #region Menu
        SignIn_Master:
        master_Menu:
            Actions.printDateTimeAndStatus("master");
            Console.WriteLine("\n\tMaster Menu : \n");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" | 1. Add Score     |    3. See All Student    |    6. Search Student   |   9. Delete My Course  |   10. Edit My      |");
            Console.WriteLine(" |                  |                          |                        |                        |       Information  |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" | 2. Add Course    |    4. See All Course     |    7. Search Course    |                        |   11. Exit         |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" |                  |    5. See My Course      |    8. Search Score     |                        |                    |");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.Write("\n\n\tEnter number of the part you want : ");
            int answer;
            while (!int.TryParse(Console.ReadLine(), out answer))
            {
                Actions.printDateTimeAndStatus("master");
                Actions.warningEnterNumber("Menu");
                goto master_Menu;
            }
            #endregion

            switch (answer)
            {
                #region Add Score
                case 1:
                AddScore:
                    Actions.printDateTimeAndStatus("master");
                    Console.WriteLine("Add Score : ");
                    Console.WriteLine();
                    using (UniverCityDbContext dbAddScoreInMasterPanel = new UniverCityDbContext("undbcontext"))
                    {
                        //show All Student For Choose Student For Add Score.
                        Actions.showAllStudent(false);

                        Console.Write("\tEnter ID of the student for you want to register a Score : ");
                        int studentId;
                        while (!int.TryParse(Console.ReadLine(), out studentId))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("Student ID");
                            goto AddScore;
                        }
                        Student studentFind = dbAddScoreInMasterPanel.Students.FirstOrDefault(t => t.PersonId == studentId);
                        //Master enter a StudentId that is available in the student list
                        if (studentFind != null)
                        {
                        askScoreInformation:
                            Actions.printDateTimeAndStatus("master");
                            Console.WriteLine($"\tEnter ID of the student for you want to register a Score : {studentId} ");

                            //If Student Does'nt Have a Course In CourseList.
                            if (studentFind.Courses.Count == 0)
                            {
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\tThere are no Course registered for {studentFind.Name} {studentFind.Family}");
                                Thread.Sleep(3000);
                                Console.WriteLine($"Please First Register A Course For {studentFind.Name} {studentFind.Family}");
                                Thread.Sleep(2000);
                                goto master_Menu;
                            }

                            // Show All Course Of Student For Choose Master Them Add Score
                            Console.WriteLine("\t______________________________________________________________");
                            Console.WriteLine($"\n\t{studentFind.Name} {studentFind.Family}'s Courses");
                            foreach (var course in studentFind.Courses)
                            {
                                Console.WriteLine("\n\t" + course);
                            }
                            Console.WriteLine("\t______________________________________________________________");

                            Console.Write($"\n\tWhich {studentFind.Name}'s  course do you want to register a Score (Enter ID)?");
                            int courseId;
                            while (!int.TryParse(Console.ReadLine(), out courseId))
                            {
                                Actions.printDateTimeAndStatus("master");
                                Actions.warningEnterNumber("Course ID");
                                goto askScoreInformation;
                            }

                            Course courseFind = dbAddScoreInMasterPanel.Courses.FirstOrDefault(t => t.Id == courseId);

                            //Master enter a CourseId that is available in the Course list
                            if (courseFind != null)
                            {
                            getScoreId:
                                Console.Write($"\n\tEnter Score's Number For {courseFind.Name} For {studentFind.Name} {studentFind.Family} : ");
                                int scoreNumber;
                                while (!int.TryParse(Console.ReadLine(), out scoreNumber))
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Actions.warningEnterNumber("Score ID");
                                    Actions.printDateTimeAndStatus("master");
                                    Console.WriteLine($"Enter ID of the student for you want to register a Score : {studentId} ");
                                    Console.WriteLine($"Which {studentFind.Name}'s  course do you want to register a Score (Enter ID)? {courseId}");
                                    goto getScoreId;
                                }
                                if (scoreNumber < 0 || scoreNumber > 20)
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Console.WriteLine("\n\tScore Number Is Flase");
                                    Thread.Sleep(2000);
                                    Console.WriteLine("\n\tTRy Again ....... ");
                                    goto askScoreInformation;
                                }

                                studentFind.Scores.Add(new Score(scoreNumber, courseFind, studentFind, masterSign));
                                dbAddScoreInMasterPanel.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\tscore of {scoreNumber} was Registered for {studentFind.Name} {studentFind.Family}'s {courseFind.Name} course");
                                Thread.Sleep(3000);
                                goto master_Menu;
                            }

                            //Master enter a CourseId that is not available in the Course list
                            else
                            {
                                Actions.printDateTimeAndStatus("master");
                                Actions.warningNotFound(courseId, "Course List");
                                goto askScoreInformation;
                            }

                        }

                        //Master enter a StudentId that is not available in the student list
                        else
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningNotFound(studentId, "StudentList");
                            goto master_Menu;
                        }
                    }
                #endregion

                #region Add Course
                case 2:

                    using (UniverCityDbContext dbAddCourseInMasterPanel = new UniverCityDbContext("undbcontext"))
                    {
                    AddCourseInMasterPanel:
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine("Add Course");
                        Console.Write("\n\tEnter a Name For Course : ");
                        string courseName = Console.ReadLine();

                        //If the user enters a name that already exists
                        if (dbAddCourseInMasterPanel.Courses.Any(t => t.Name == courseName))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Console.WriteLine("\n\tThere is a Course with this Name....\n\n\tEnter Another Name....");
                            Thread.Sleep(4000);
                            goto AddCourseInMasterPanel;
                        }

                    getUnit:
                        Console.Write($"\n\tEnter Unit For {courseName} : ");
                        int courseUnit;
                        while (!int.TryParse(Console.ReadLine(), out courseUnit))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("Course Unit");
                            Actions.printDateTimeAndStatus("master");
                            Console.Write($"\n\tEnter a Name For Course : {courseName}");
                            Console.WriteLine();
                            goto getUnit;
                        }
                        //Add A course Without a MAster                        
                        dbAddCourseInMasterPanel.Courses.Add(new Course(courseName, courseUnit));
                        dbAddCourseInMasterPanel.SaveChanges();
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\n\t{courseName} was successfully registered with {courseUnit} Unit");
                        Thread.Sleep(3000);
                    answerForAddMaster:
                        Actions.printDateTimeAndStatus("master");
                        Console.Write($"\n\tDo you want to be registered as a Master of {courseName}? (1. Yes | 2. No) : ");
                        int answerAddMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerAddMaster))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("This Question");
                            goto answerForAddMaster;
                        }
                        switch (answerAddMaster)
                        {
                            #region Yes
                            //Do You Want To Choose A MAster For Course ? *Yse*
                            case 1:
                                dbAddCourseInMasterPanel.Courses.First(t => t.Name == courseName).Master = dbAddCourseInMasterPanel.Masters.First(t => t.PersonId == masterSign.PersonId);
                                dbAddCourseInMasterPanel.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("\n\tYou Are now the Master of this Course");
                                Thread.Sleep(3000);
                                goto master_Menu;
                            #endregion

                            #region no
                            //Do You Want To Choose A MAster For Course ? *No*
                            case 2:
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("\n\n\tNo Problem ........");
                                Actions.comeBackToMainMenu();
                                goto master_Menu;
                            #endregion

                            default:
                                goto answerForAddMaster;
                        }
                    }
                #endregion

                #region See All Student
                case 3:

                    Actions.printDateTimeAndStatus("master");
                    Actions.showAllStudent(true);
                    Actions.comeBackToMainMenu();
                    goto master_Menu;
                #endregion

                #region See All Course
                case 4:
                    Actions.printDateTimeAndStatus("master");
                    Actions.showAllCourse(true);
                    Actions.comeBackToMainMenu();
                    goto master_Menu;
                #endregion

                #region See My Course
                case 5:

                    using (UniverCityDbContext dbseeMyCourses = new UniverCityDbContext("undbcontext"))
                    {
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine("\nMy Courses : ");
                        //Show Master's Course
                        foreach (Course course in dbseeMyCourses.Masters.First(t => t.PersonId == masterSign.PersonId).Courses.ToList())
                        {
                            Console.WriteLine("\n  " + course);
                        }
                        Actions.comeBackToMainMenu();
                        goto master_Menu;
                    }
                #endregion

                #region Serach Student
                case 6:
                    using (UniverCityDbContext dbSearchStudent = new UniverCityDbContext("undbcontext"))
                    {
                    searchStudent:
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine("Search Student");
                        Console.WriteLine("\n\t1.Search By Id");
                        Console.WriteLine("\n\t2.Search By Name");
                        Console.WriteLine("\n\t3.Search By Family");
                        Console.WriteLine("\n\t4.Search By Mobile");
                        Console.Write("\n\n\tWitch Part : ");
                        int searchAnswer;
                        while (!int.TryParse(Console.ReadLine(), out searchAnswer))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("Menu Operation");
                            goto searchStudent;
                        }
                        switch (searchAnswer)
                        {
                            //Search By Id
                            case 1:
                                Actions.SearchStudentById("master");
                                goto master_Menu;

                            //Search By Name
                            case 2:
                                Actions.SearchStudentByName("master");
                                goto master_Menu;

                            //Search By Family
                            case 3:
                                Actions.SearchStudentByFamily("master");
                                goto master_Menu;

                            //Search By Mobile
                            case 4:
                                Actions.SearchStudentByMobile("master");
                                goto master_Menu;

                            default:
                                goto searchStudent;
                        }
                    }
                #endregion

                #region Search Course
                case 7:
                    using (UniverCityDbContext dbSerachCourse = new UniverCityDbContext("undbcontext"))
                    {
                    SearchCourse:
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine("Search Course");
                        Console.WriteLine("\n\t1.Search By Id");
                        Console.WriteLine("\n\t2.Search By Name");
                        Console.Write("\n\n\tWitch Part : ");
                        int answerSearchCourse;
                        while (!int.TryParse(Console.ReadLine(), out answerSearchCourse))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("This Question");
                            goto SearchCourse;
                        }
                        switch (answerSearchCourse)
                        {
                            //Search By Id                             
                            case 1:
                                Actions.searchCourseById("master");
                                goto master_Menu;

                            //search By Name
                            case 2:
                                Actions.searchCourseByName("master");
                                goto master_Menu;

                            default:
                                goto SearchCourse;
                        }

                    }
                #endregion

                #region Search Score

                case 8:
                    using (UniverCityDbContext dbSerachScore = new UniverCityDbContext("undbcontext"))
                    {
                    searchScore:
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine("Search Course");
                        Console.WriteLine("\n\t1.Serach By Id");
                        Console.WriteLine("\n\t2.Serach By Student");
                        Console.WriteLine("\n\t3.Serach By Course");
                        Console.Write("\n\n\tWitch Part? : ");
                        int searchScoreAnswer;
                        while (!int.TryParse(Console.ReadLine(), out searchScoreAnswer))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("This Question");
                            goto searchScore;
                        }
                        switch (searchScoreAnswer)
                        {
                            // Search By Score Id
                            case 1:
                                Actions.searchScoreById("master");
                                goto master_Menu;

                            case 2:
                                //Search By StudentId
                                Actions.searchScoreByStudent("master");
                                goto master_Menu;

                            //Search By CourseId
                            case 3:
                                Actions.searchScoreByCourse("master");
                                goto master_Menu;

                            default:
                                goto searchScore;
                        }

                    }







                #endregion

                #region Delete My Course
                case 9:
                deleteMyCourse:
                    using (UniverCityDbContext dbDeleteMyCource = new UniverCityDbContext("undbcontext"))
                    {
                        Actions.printDateTimeAndStatus("master");

                        //print MasterSign's Courses
                        Console.WriteLine("My Course List ");
                        foreach (Course course in dbDeleteMyCource.Masters.First(t => t.PersonId == masterSign.PersonId && t.Mobile == masterSign.Mobile).Courses.ToList())
                        {
                            Console.WriteLine("\n  " + course);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                        }

                        //If MasterSign Doesn't Have Any Course
                        if (dbDeleteMyCource.Masters.First(t => t.PersonId == masterSign.PersonId).Courses.Count() == 0)
                        {
                            Actions.printDateTimeAndStatus("master");
                            Console.WriteLine("You Don't Have Any Course.....");
                            Thread.Sleep(3000);
                            goto master_Menu;
                        }
                        Console.Write("Enter Course Id For Delete : ");
                        int courseId;
                        while (!int.TryParse(Console.ReadLine(), out courseId))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("Course List");
                            goto deleteMyCourse;
                        }

                        //If the user enters a CourseId that already exists
                        if (!dbDeleteMyCource.Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningNotFound(courseId, "Course List");
                            goto deleteMyCourse;
                        }
                        dbDeleteMyCource.Masters.First(t => t.PersonId == masterSign.PersonId).Courses.Remove(dbDeleteMyCource.Courses.First(t => t.Id == courseId));
                        dbDeleteMyCource.SaveChanges();
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine($"\n\t{dbDeleteMyCource.Courses.First(t => t.Id == courseId).Name} Remove From Your Courses List");
                        Thread.Sleep(3000);
                        goto master_Menu;
                    }
                #endregion

                #region Edit My Information
                case 10:

                    using (UniverCityDbContext dbEditInformation = new UniverCityDbContext("undbcontext"))
                    {
                    editMyInformation:
                        Actions.printDateTimeAndStatus("master");
                        Console.WriteLine("Edit My Information");
                        Console.WriteLine("\n\t1. Edit My Name");
                        Console.WriteLine("\n\t2. Edit My Family");
                        Console.WriteLine("\n\t3. Edit Password");
                        Console.WriteLine("\n\t4. Add Course For Me");
                        Console.WriteLine("\n\t5. Edit My Degree");
                        Console.WriteLine("\n\n\tWhich Part ? :");
                        int answerEditMyInformation;
                        while (!int.TryParse(Console.ReadLine(), out answerEditMyInformation))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("This Question");
                            goto master_Menu;
                        }
                        switch (answerEditMyInformation)
                        {
                            case 1:
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("Edit Name");
                                Console.WriteLine($"\n\tYour Previous Name : {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Name}");
                                Console.Write("\n\tEnter Your New Name : ");
                                dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Name = Console.ReadLine();
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\tYour Name Changed To {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Name}");
                                Thread.Sleep(3000);
                                goto master_Menu;
                            case 2:
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("Edit Family");
                                Console.WriteLine($"\n\tYour Previous Family :{dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Family}");
                                Console.Write("\n\tEnter Your New Family : ");
                                dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Family = Console.ReadLine();
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\tYour Name Changed To {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Family}");
                                Thread.Sleep(3000);
                                goto master_Menu;

                            case 3:
                            editPassword:
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("Edit Password");
                                Console.WriteLine($"\n\tYour Previous Password : {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Password}");
                                Console.Write("\n\tEnter Your New Password : ");
                                string newPassword = Console.ReadLine();
                                Console.Write("\n\tEnter Your New Password Again : ");
                                string repeatNewPassword = Console.ReadLine();
                                if (newPassword != repeatNewPassword)
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Console.WriteLine("\n\tYour Repeat New Password Is Not True .. ");
                                    Thread.Sleep(3000);
                                    goto editPassword;
                                }
                                dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Password = newPassword;
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\tYour Password Changed To {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Password}\n\n\tPlease Try Again For Login .......");
                                Thread.Sleep(3000);
                                goto login;

                            case 4:
                            addCourseToMyCourseList:
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("Course List");
                                Actions.showAllCourse(true);
                                Console.Write("\n\tEnter CourseID For Add Its To Your Corses List : ");
                                int courseId;
                                while (!int.TryParse(Console.ReadLine(), out courseId))
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Actions.warningEnterNumber("Course Id");
                                    goto addCourseToMyCourseList;
                                }
                                if (!dbEditInformation.Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Actions.warningNotFound(courseId, "CourseList");
                                    goto addCourseToMyCourseList;
                                }
                                //If Course Have a Master
                                if (dbEditInformation.Courses.First(t => t.Id == courseId).Master != null)
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Console.WriteLine("\n\tFor This Course Register A Master Please Choose Another Course Or Talk To Employee For Slove This Problem");
                                    Actions.comeBackToMainMenu();
                                    goto master_Menu;
                                }
                                dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Courses.Add(dbEditInformation.Courses.First(t => t.Id == courseId));
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\t{dbEditInformation.Courses.FirstOrDefault(t => t.Id == courseId)} Was Added To Your Courses List");
                                Thread.Sleep(3000);
                                goto master_Menu;

                            case 5:
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine("Edit Degree");
                                Console.WriteLine($"\n\tYour Previous Degree : {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Degree}");
                                Console.Write("\n\tEnter Your New Degree : ");
                                dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Degree = Console.ReadLine();
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("master");
                                Console.WriteLine($"\n\tYour Degree Changed To {dbEditInformation.Masters.First(t => t.PersonId == masterSign.PersonId).Degree}");
                                Thread.Sleep(3000);
                                goto master_Menu;

                            default:
                                goto editMyInformation;
                        }
                    }

                #endregion

                #region Exit
                case 11:
                    goto login;
                #endregion

                #region default
                default:
                    goto master_Menu;
                    #endregion
            }
        #endregion

        #region Logged Employee
        SignIn_Employee:
        #region Menu
        employeeMenu:
            Actions.printDateTimeAndStatus("employee");
            Console.WriteLine("\n\tEmpoyee Menu : \n");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" | 1. Add Master    |    6. See All Student    |    9. Search Master    |    13. Delete Master   |    17. Edit Master |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" | 2. Add Student   |    7. See All Master     |    10. Search Student  |    14. Delete Student  |    18. Edit Student|");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" | 3. Add Course    |    8. See All Course     |    11. Search Course   |    15. Delete Course   |    19. Edit Course |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" | 4. Add Course To |                          |    12. Search Score    |    16. Delete Score    |    20. Edit Score  |");
            Console.WriteLine(" |    Student       |                          |                        |                        |                    |");
            Console.WriteLine(" |                  |                          |                        |                        |                    |");
            Console.WriteLine(" | 5. Add Employee  |                          |                        |                        |    21. Exit        |");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.Write("\n\n\tEnter number of the part you want : ");
            int answerEmployeeList;
            while (!int.TryParse(Console.ReadLine(), out answerEmployeeList))
            {
                Actions.printDateTimeAndStatus("master");
                Actions.warningEnterNumber("Menu");
                goto employeeMenu;
            }
            #endregion

            switch (answerEmployeeList)
            {
                #region Add Master
                case 1:
                    using (UniverCityDbContext dbAddMasterInEmployeePanel = new UniverCityDbContext("undbcontext"))
                    {

                    signUpMaster:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Register New Master : ");
                        Console.Write("\n\n\tEnter National Code   : ");
                        string signUpnationalCode = Console.ReadLine();
                        if (users.Any(t => t.Code == signUpnationalCode))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\tA User By This National Code Registered As {users.First(t => t.Code == signUpnationalCode).Role.Name}");
                            Thread.Sleep(3000);
                            goto signUpMaster;
                        }
                        if (!Regex.IsMatch(signUpnationalCode, validNationalCode))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\t\tThe national code is not entered correctly .....");
                            Thread.Sleep(4000);
                            goto signUpMaster;
                        }
                        Console.Write("\n\tEnter Name            : ");
                        string signUpName = Console.ReadLine();
                        Console.Write("\n\tEnter Family          : ");
                        string signUpFamily = Console.ReadLine();
                        Console.Write("\n\tEnter Mobile          : ");
                        string signUpMobile = Console.ReadLine();
                        if (users.Any(t => t.Mobile == signUpMobile))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\tA User Registered By This Mobile  As {users.First(t => t.Mobile == signUpMobile).Role.Name}");
                            Thread.Sleep(3000);
                            goto signUpMaster;
                        }
                        if (!Regex.IsMatch(signUpMobile, validMobile))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\t\tThe national code is not entered correctly .....");
                            Thread.Sleep(4000);
                            goto signUpMaster;
                        }
                    enterPassword:
                        Console.Write("\n\tEnter Password        : ");
                        string signUpPassword = Console.ReadLine();
                        Console.Write("\n\tEnter Repeat Password : ");
                        string repeatPassword = Console.ReadLine();
                        if (signUpPassword != repeatPassword)
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\tThe password is not the same as its repetition");
                            Thread.Sleep(4000);
                            Actions.printDateTimeAndStatus("employee");
                            Console.Write($"\n\n\tEnter National Code   : {signUpnationalCode}");
                            Console.Write($"\n\n\tEnter Name            : {signUpName}");
                            Console.Write($"\n\n\tEnter Family          : {signUpFamily}");
                            Console.Write($"\n\n\tEnter Mobile          : {signUpMobile}");
                            goto enterPassword;
                        }
                        Console.Write("\n\tEnter Degree          : ");
                        string signUpDegree = Console.ReadLine();
                    enterSalary:
                        Console.Write("\n\tEnter Salary          : ");
                        int signUpSalary;
                        while (!int.TryParse(Console.ReadLine(), out signUpSalary))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Salary");
                            Actions.printDateTimeAndStatus("employee");
                            Console.Write($"\n\n\tEnter National Code   : {signUpnationalCode}");
                            Console.Write($"\n\n\tEnter Name            : {signUpName}");
                            Console.Write($"\n\n\tEnter Family          : {signUpFamily}");
                            Console.Write($"\n\n\tEnter Mobile          : {signUpMobile}");
                            Console.Write($"\n\n\tEnter Salary          : {signUpDegree}");
                            goto enterSalary;
                        }

                        dbAddMasterInEmployeePanel.Masters.Add(new Master(signUpnationalCode, signUpName, signUpFamily, signUpMobile, signUpPassword, signUpSalary, signUpDegree,dbAddMasterInEmployeePanel.Roles.First(t=>t.Name=="master")));
                        dbAddMasterInEmployeePanel.SaveChanges();
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\t{signUpName} {signUpFamily} was successfully registered as a Master");
                        Thread.Sleep(3000);
                        goto employeeMenu;
                    }
                #endregion

                #region Add Student
                case 2:

                    using (UniverCityDbContext dbAddStudentInEmployeePanel = new UniverCityDbContext("undbcontext"))
                    {


                    signUpMaster:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Register New Stident : ");
                        Console.Write("\n\n\tEnter National Code   : ");
                        string signUpnationalCode = Console.ReadLine();
                        if (users.Any(t => t.Code == signUpnationalCode))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\tA User By This National Code Registered As {users.First(t => t.Code == signUpnationalCode).Role.Name}");
                            Thread.Sleep(3000);
                            goto employeeMenu;
                        }
                        if (!Regex.IsMatch(signUpnationalCode, validNationalCode))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\t\tThe national code is not entered correctly .....");
                            Thread.Sleep(4000);
                            goto signUpMaster;
                        }
                        Console.Write("\n\tEnter Name            : ");
                        string signUpName = Console.ReadLine();
                        Console.Write("\n\tEnter Family          : ");
                        string signUpFamily = Console.ReadLine();
                        Console.Write("\n\tEnter Mobile          : ");
                        string signUpMobile = Console.ReadLine();
                        if (users.Any(t => t.Mobile == signUpMobile))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\tA User Registered By This Mobile  As {users.First(t => t.Mobile == signUpMobile).Role.Name}");
                            Thread.Sleep(3000);
                            goto employeeMenu;
                        }
                        if (!Regex.IsMatch(signUpMobile, validMobile))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\t\tThe national code is not entered correctly .....");
                            Thread.Sleep(4000);
                            goto signUpMaster;
                        }
                    enterPassword:
                        Console.Write("\n\tEnter Password        : ");
                        string signUpPassword = Console.ReadLine();
                        Console.Write("\n\tEnter Repeat Password : ");
                        string repeatPassword = Console.ReadLine();
                        if (signUpPassword != repeatPassword)
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\tThe password is not the same as its repetition");
                            Thread.Sleep(4000);
                            Actions.printDateTimeAndStatus("employee");
                            Console.Write($"\n\n\tEnter National Code   : {signUpnationalCode}");
                            Console.Write($"\n\n\tEnter Name            : {signUpName}");
                            Console.Write($"\n\n\tEnter Family          : {signUpFamily}");
                            Console.Write($"\n\n\tEnter Mobile          : {signUpMobile}");
                            goto enterPassword;
                        }
                        Console.Write("\n\tEnter Degree          : ");
                        string signUpDegree = Console.ReadLine();
                        dbAddStudentInEmployeePanel.Students.Add(new Student(signUpnationalCode, signUpName, signUpFamily, signUpMobile, signUpPassword, signUpDegree,dbAddStudentInEmployeePanel.Roles.First(t=>t.Name=="student")));
                        dbAddStudentInEmployeePanel.SaveChanges();
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\t{signUpName} {signUpFamily} By Code ({dbAddStudentInEmployeePanel.Students.First(t => t.NationalCode == signUpnationalCode).Code}) was successfully registered as a Student");
                        Thread.Sleep(3000);
                        goto employeeMenu;
                    }
                #endregion

                #region Add Course
                case 3:
                    using (UniverCityDbContext dbAddCourseInEmployeePanel = new UniverCityDbContext("undbcontext"))
                    {
                    addMasterInEmployeePanel:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Add Course");
                        Console.Write("\n\tEnter a Name For Course : ");
                        string courseName = Console.ReadLine();

                        //If the user enters a name that already exists
                        if (dbAddCourseInEmployeePanel.Courses.Any(t => t.Name == courseName))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\tThere is a Course with this Name....\n\n\tEnter Another Name....");
                            Thread.Sleep(4000);
                            goto addMasterInEmployeePanel;
                        }

                    getUnit:
                        Console.Write($"\n\tEnter Unit For {courseName} : ");
                        int courseUnit;
                        while (!int.TryParse(Console.ReadLine(), out courseUnit))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Course Unit");
                            Actions.printDateTimeAndStatus("employee");
                            Console.Write($"\n\tEnter a Name For Course : {courseName}");
                            Console.WriteLine();
                            goto getUnit;
                        }
                        //Add A course Without a MAster                        
                        dbAddCourseInEmployeePanel.Courses.Add(new Course(courseName, courseUnit));
                        dbAddCourseInEmployeePanel.SaveChanges();
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\n\t{courseName} was successfully registered with {courseUnit} Unit");
                        Thread.Sleep(3000);
                    answerForAddMaster:
                        Actions.printDateTimeAndStatus("employee");
                        Console.Write($"\n\tDo you want to be registered as a Master of {courseName}? (1. Yes | 2. No) : ");
                        int answerAddMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerAddMaster))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto answerForAddMaster;
                        }
                        switch (answerAddMaster)
                        {
                            #region Yes                            
                            //Do You Want To Choose A MAster For Course ? *Yse*
                            case 1:
                            answerForAddMaster_yes:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n\t1.Choose from Master List ");
                                Console.WriteLine("\n\n\t2.Create New Master Then Select");
                                Console.Write("\n\n\tSelect One Part : ");
                                int answerSelectMaster;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectMaster))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("this Questions");
                                    goto answerForAddMaster_yes;
                                }

                                switch (answerSelectMaster)
                                {
                                    //Select Course's Master From MasterList 
                                    #region Select Old Master
                                    case 1:
                                    addMasterForNewCourse:
                                        Actions.printDateTimeAndStatus("employee");
                                        Actions.showAllMaster(true);
                                        Console.Write("Enter Master Id For Add Course For :");
                                        int masterId;
                                        while (!int.TryParse(Console.ReadLine(), out masterId))
                                        {
                                            Actions.printDateTimeAndStatus("employee");
                                            Actions.warningEnterNumber("masterId");
                                            goto addMasterForNewCourse;
                                        }
                                        //The user entered an ID that did not exist
                                        if (!dbAddCourseInEmployeePanel.Masters.Any(t => t.PersonId == masterId))
                                        {
                                            Actions.printDateTimeAndStatus("employee");
                                            Actions.warningNotFound(masterId, "Master List");
                                            goto addMasterForNewCourse;
                                        }
                                        //With the Id Entered by the user, We Found a Master And put it into a new object
                                        Master CourseMaster = dbAddCourseInEmployeePanel.Masters.FirstOrDefault(t => t.PersonId == masterId);
                                        //Add Master For New Course
                                        CourseMaster.Courses.Add(dbAddCourseInEmployeePanel.Courses.FirstOrDefault(t => t.Name == courseName && t.Unit == courseUnit));
                                        dbAddCourseInEmployeePanel.SaveChanges();
                                        Actions.printDateTimeAndStatus("employee");
                                        Console.WriteLine($"\n\t{CourseMaster.Name} {CourseMaster.Family} was registered for {courseName} course As Master");
                                        Thread.Sleep(4000);
                                        goto employeeMenu;
                                    #endregion

                                    //First Employee Create New Master And Add Him/Her To MasterList Then Select Course's Master 
                                    #region Create New Master Then Select New Master
                                    case 2:
                                        using (UniverCityDbContext dbAddMasterInEmployeePanel = new UniverCityDbContext("undbcontext"))
                                        {

                                        signUpMaster:
                                            Actions.printDateTimeAndStatus("employee");
                                            Console.WriteLine("Register New Master : ");
                                            Console.Write("\n\n\tEnter National Code   : ");
                                            string signUpnationalCode = Console.ReadLine();
                                            //If there was an employee with this national code in the database
                                            if (users.Any(t => t.Code == signUpnationalCode))
                                            {
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.WriteLine($"\n\n\tA User By This National Code Registered As {users.First(t => t.Code == signUpnationalCode).Role.Name}");
                                                Thread.Sleep(3000);
                                                goto signUpMaster;
                                            }
                                            //Validation National Code 
                                            if (!Regex.IsMatch(signUpnationalCode, validNationalCode))
                                            {
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.WriteLine("\n\t\tThe national code is not entered correctly .....");
                                                Thread.Sleep(4000);
                                                goto signUpMaster;
                                            }
                                            Console.Write("\n\tEnter Name            : ");
                                            string signUpName = Console.ReadLine();
                                            Console.Write("\n\tEnter Family          : ");
                                            string signUpFamily = Console.ReadLine();
                                            Console.Write("\n\tEnter Mobile          : ");
                                            string signUpMobile = Console.ReadLine();
                                            //If there was an employee with this Mobile in the database
                                            if (users.Any(t => t.Mobile == signUpMobile))
                                            {
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.WriteLine($"\n\n\tA User Registered By This Mobile  As {users.First(t => t.Mobile == signUpMobile).Role.Name}");
                                                Thread.Sleep(3000);
                                                goto signUpMaster;
                                            }
                                            //Validation Mobile Number
                                            if (!Regex.IsMatch(signUpMobile, validMobile))
                                            {
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.WriteLine("\n\t\tThe national code is not entered correctly .....");
                                                Thread.Sleep(4000);
                                                goto signUpMaster;
                                            }
                                        enterPassword:
                                            Console.Write("\n\tEnter Password        : ");
                                            string signUpPassword = Console.ReadLine();
                                            //Validation Password
                                            Console.Write("\n\tEnter Repeat Password : ");
                                            string repeatPassword = Console.ReadLine();
                                            if (signUpPassword != repeatPassword)
                                            {
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.WriteLine("\n\tThe password is not the same as its repetition");
                                                Thread.Sleep(4000);
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.Write($"\n\n\tEnter National Code   : {signUpnationalCode}");
                                                Console.Write($"\n\n\tEnter Name            : {signUpName}");
                                                Console.Write($"\n\n\tEnter Family          : {signUpFamily}");
                                                Console.Write($"\n\n\tEnter Mobile          : {signUpMobile}");
                                                goto enterPassword;
                                            }
                                            Console.Write("\n\tEnter Degree          : ");
                                            string signUpDegree = Console.ReadLine();
                                        enterSalary:
                                            Console.Write("\n\tEnter Salary          : ");
                                            int signUpSalary;
                                            while (!int.TryParse(Console.ReadLine(), out signUpSalary))
                                            {
                                                Actions.printDateTimeAndStatus("employee");
                                                Actions.warningEnterNumber("Salary");
                                                Actions.printDateTimeAndStatus("employee");
                                                Console.Write($"\n\n\n\tEnter National Code   : {signUpnationalCode}");
                                                Console.Write($"\n\n\n\tEnter Name            : {signUpName}");
                                                Console.Write($"\n\n\n\tEnter Family          : {signUpFamily}");
                                                Console.Write($"\n\n\n\tEnter Mobile          : {signUpMobile}");
                                                Console.Write($"\n\n\n\tEnter Degree          : {signUpDegree}\n\n");
                                                goto enterSalary;
                                            }

                                            dbAddMasterInEmployeePanel.Masters.Add(new Master(signUpnationalCode, signUpName, signUpFamily, signUpMobile, signUpPassword, signUpSalary, signUpDegree,dbAddMasterInEmployeePanel.Roles.First(t=>t.Name =="master")));
                                            dbAddMasterInEmployeePanel.SaveChanges();
                                            Actions.printDateTimeAndStatus("employee");
                                            Console.WriteLine($"\n\t{signUpName} {signUpFamily} was successfully registered as a Master");
                                            Thread.Sleep(2500);
                                            Actions.printDateTimeAndStatus("employee");
                                            dbAddCourseInEmployeePanel.Courses.First(t => t.Name == courseName && t.Unit == courseUnit).Master = dbAddCourseInEmployeePanel.Masters.First(t => t.NationalCode == signUpnationalCode);
                                            dbAddCourseInEmployeePanel.SaveChanges();
                                            Console.WriteLine($"\n\n\t{signUpName} {signUpFamily} was chosen as the Master of a {courseName} course");
                                            Actions.comeBackToMainMenu();
                                            goto employeeMenu;

                                        }
                                    #endregion
                                    default:
                                        goto answerForAddMaster_yes;
                                }
                            #endregion

                            #region no
                            //Do You Want To Choose A MAster For Course ? *No*
                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n\tNo Problem ........");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;
                            #endregion

                            default:
                                goto answerForAddMaster;
                        }
                    }
                #endregion

                #region Add Course To Student's CourseList
                case 4:
                addCourseToStudentCourseList:
                    using (UniverCityDbContext dbAddCourseToStudent = new UniverCityDbContext("undbcontext"))
                    {

                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\tAdd Course To Student CourseList");
                        Actions.showAllStudent(false);
                        Console.Write("\n\tEnter Student Id For Add Course : ");
                        int studentId;
                        while (!int.TryParse(Console.ReadLine(), out studentId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Student Id");
                            goto addCourseToStudentCourseList;
                        }
                        if (!dbAddCourseToStudent.Students.Any(t => t.PersonId == studentId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(studentId, "Student List");
                            goto addCourseToStudentCourseList;
                        }
                        Student studentFind = dbAddCourseToStudent.Students.First(t => t.PersonId == studentId);

                    showCourseForAddToStudentCoursesList:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\tAdd Course to {studentFind.Name} {studentFind.Family}'s CourseList");
                        Actions.showAllCourse(false);
                        Console.Write($"\n\n\tWhich Course Do You Want Add To {studentFind.Name}'s Courses List ? ");
                        int courseId;
                        while (!int.TryParse(Console.ReadLine(), out courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Course Id");
                            goto showCourseForAddToStudentCoursesList;
                        }
                        if (!dbAddCourseToStudent.Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(courseId, "Course List");
                            goto addCourseToStudentCourseList;
                        }
                        Course courseFind = dbAddCourseToStudent.Courses.First(t => t.Id == courseId);

                        if (dbAddCourseToStudent.Students.First(t => t.PersonId == studentId).Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\t{courseFind.Name} Last Registered For {studentFind.Name} {studentFind.Family}");
                            Thread.Sleep(3000);
                            goto showCourseForAddToStudentCoursesList;
                        }

                        dbAddCourseToStudent.Students.First(t => t.PersonId == studentId).Courses.Add(dbAddCourseToStudent.Courses.First(t => t.Id == courseId));
                        dbAddCourseToStudent.SaveChanges();
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\n\t{courseFind.Name} Added To {studentFind.Name} {studentFind.Family}'s CourseList \n\n");
                        Actions.comeBackToMainMenu();
                        goto employeeMenu;
                    }
                #endregion

                #region Add Employee
                case 5:
                    using (UniverCityDbContext dbAddEmployee = new UniverCityDbContext("undbcontext"))
                    {
                        //If the user is not Admin
                        Actions.printDateTimeAndStatus("employee");
                        string department = employeeSign.Department.ToLower();
                        if (department != "admin")
                        {
                            Actions.printDateTimeAndStatus("Employee");
                            Console.WriteLine("\n\n\tOnly admin can access this part");
                            Actions.comeBackToMainMenu();
                            goto employeeMenu;
                        }
                    dbAddEmployee:
                        Actions.printDateTimeAndStatus("empployee");
                        Console.WriteLine("Add Employee ");
                        Console.Write("\n\n\tEnter National Code : ");
                        string signUpEmployee_nationalCode = Console.ReadLine();
                        //Valid National Code
                        if (!Regex.IsMatch(signUpEmployee_nationalCode, validNationalCode))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\n\tThe national code is not entered correctly");
                            Thread.Sleep(3000);
                            goto dbAddEmployee;
                        }
                        //If there is a user with this National Code in the database
                        if (users.Any(t => t.Code == signUpEmployee_nationalCode))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\tbefore a User Registered By This NationalCode As {users.FirstOrDefault(t => t.Code == signUpEmployee_nationalCode).Role.Name}");
                            Thread.Sleep(4000);
                            goto employeeMenu;
                        }
                        Console.Write("\n\tEnter Name            : ");
                        string signUpEmployee_name = Console.ReadLine();
                        Console.Write("\n\tEnter Family          : ");
                        string signUpEmployee_family = Console.ReadLine();
                    enterMobile:
                        Console.Write("\n\tEnter Mobile          : ");
                        string signUpEmployee_mobile = Console.ReadLine();
                        //Valid National Code
                        if (!Regex.IsMatch(signUpEmployee_mobile, validMobile))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\n\tThe national code is not entered correctly");
                            Thread.Sleep(3000);
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("Add Employee ");
                            Console.Write($"\n\n\tEnter National Code : {signUpEmployee_nationalCode}");
                            Console.Write($"\n\tEnter Name            : {signUpEmployee_name}");
                            Console.Write($"\n\tEnter Family          : {signUpEmployee_family}");
                            goto enterMobile;
                        }
                        //If there is a user with this Mobile Number in the database
                        if (users.Any(t => t.Mobile == signUpEmployee_mobile))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine($"\n\n\tbefore a User Registered By This Mobile Number As {users.FirstOrDefault(t => t.Code == signUpEmployee_nationalCode).Role.Name}");
                            Thread.Sleep(4000);
                            goto employeeMenu;
                        }
                    enterPassword:
                        Console.Write("\n\tEnter Password        : ");
                        string signUpEmployee_password = Console.ReadLine();
                        Console.Write("\n\tEnter Repeat Password : ");
                        string repeatPassword = Console.ReadLine();
                        //Valid For Repeat Password
                        if (!Regex.IsMatch(signUpEmployee_password, repeatPassword))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Console.WriteLine("\n\n\tThe password is not the same as repeating it");
                            Thread.Sleep(3000);
                            Actions.printDateTimeAndStatus("employee");
                            Console.Write($"\n\n\tEnter National Code : {signUpEmployee_nationalCode}");
                            Console.Write($"\n\tEnter Name            : {signUpEmployee_name}");
                            Console.Write($"\n\tEnter Family          : {signUpEmployee_family}");
                            Console.Write($"\n\tEnter Mobile          : {signUpEmployee_mobile}");
                            goto enterPassword;

                        }
                        Console.Write("\n\tEnter Department      : ");
                        string signUpEmployee_Department = Console.ReadLine();
                    enterSalary:
                        Console.Write("\n\tEnter Salary          : ");
                        int signUpEmployee_Salary;
                        while (!int.TryParse(Console.ReadLine(), out signUpEmployee_Salary))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("salary");
                            Actions.printDateTimeAndStatus("empployee");
                            Console.WriteLine("Add Employee ");
                            Console.Write($"\n\n\tEnter National Code : {signUpEmployee_nationalCode}");
                            Console.Write($"\n\tEnter Name            : {signUpEmployee_name}");
                            Console.Write($"\n\tEnter Family          : {signUpEmployee_family}");
                            Console.Write($"\n\tEnter Mobile          : {signUpEmployee_mobile}");
                            Console.Write($"\n\tEnter Password        : {signUpEmployee_password}");
                            Console.Write($"\n\tEnter Repeat Password : {repeatPassword}");
                            Console.Write($"\n\tEnter Department      : {signUpEmployee_Department}");
                            goto enterSalary;
                        }


                        dbAddEmployee.Employees.Add(new Employee(signUpEmployee_nationalCode, signUpEmployee_name, signUpEmployee_family, signUpEmployee_mobile, signUpEmployee_password, signUpEmployee_Salary, signUpEmployee_Department,dbAddEmployee.Roles.First(t=>t.Name=="employee")));
                        dbAddEmployee.SaveChanges();
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine($"\n\n\t{signUpEmployee_name} {signUpEmployee_family} By This NationalCode({signUpEmployee_nationalCode}) Registered As Emplyee In {signUpEmployee_Department} Department.");
                        Actions.comeBackToMainMenu();
                        goto employeeMenu;
                    }

                #endregion

                #region See All Student
                case 6:
                    Actions.printDateTimeAndStatus("employee");
                    Actions.showAllStudent(false);
                    Actions.comeBackToMainMenu();
                    goto employeeMenu;
                #endregion

                #region See All Master
                case 7:
                    Actions.printDateTimeAndStatus("employee");
                    Actions.showAllMaster(false);
                    Actions.comeBackToMainMenu();
                    goto employeeMenu;
                #endregion

                #region See All Course
                case 8:
                    Actions.printDateTimeAndStatus("employee");
                    Actions.showAllCourse(true);
                    Actions.comeBackToMainMenu();
                    goto employeeMenu;
                #endregion

                #region Search Master

                case 9:
                    using (UniverCityDbContext dbSearchMaster = new UniverCityDbContext("undbcontext"))
                    {
                    searchMaster:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Search Master");
                        Console.WriteLine("\n\t1.Search By Id");
                        Console.WriteLine("\n\t2.Search By Name");
                        Console.WriteLine("\n\t3.Search By Family");
                        Console.WriteLine("\n\t4.Search By Mobile");
                        Console.Write("\n\n\tWitch Part : ");
                        int searchAnswer;
                        while (!int.TryParse(Console.ReadLine(), out searchAnswer))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Menu Operation");
                            goto searchMaster;
                        }
                        switch (searchAnswer)
                        {
                            //Search By Id
                            case 1:
                            searchById:
                                Actions.printDateTimeAndStatus("employee");
                                Console.Write("\n\tEnter Master Id For Search : ");
                                int searchMaster;
                                while (!int.TryParse(Console.ReadLine(), out searchMaster))
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Actions.warningEnterNumber("Master Id");
                                    goto searchById;
                                }
                                if (!dbSearchMaster.Masters.Any(t => t.PersonId == searchMaster))
                                {
                                    Actions.printDateTimeAndStatus("master");
                                    Actions.warningNotFound(searchMaster, "Master List");
                                    goto searchById;
                                }
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n" + dbSearchMaster.Masters.First(t => t.PersonId == searchMaster));
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            //Search By Name
                            case 2:
                            searchByName:
                                Actions.printDateTimeAndStatus("employee");
                                Console.Write("\n\tEnter Master Name For Search : ");
                                string masterName = Console.ReadLine();
                                if (!dbSearchMaster.Masters.Any(t => t.Name == masterName))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\tthere Isn't Any Master By This Name({masterName})");
                                    Thread.Sleep(3000);
                                    goto searchByName;
                                }
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine();
                                foreach (Master master in dbSearchMaster.Masters.Where(t => t.Name == masterName).ToList())
                                {
                                    Console.WriteLine(master);
                                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                                }
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            //Search By Family
                            case 3:
                            searchByFamily:
                                Actions.printDateTimeAndStatus("employee");
                                Console.Write("\n\tEnter Master Family For Search : ");
                                string masterFamily = Console.ReadLine();
                                if (!dbSearchMaster.Masters.Any(t => t.Family == masterFamily))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\tthere Isn't Any Master By This Family({masterFamily})");
                                    Thread.Sleep(3000);
                                    goto searchByFamily;
                                }
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine();
                                foreach (Master master in dbSearchMaster.Masters.Where(t => t.Family == masterFamily).ToList())
                                {
                                    Console.WriteLine(master);
                                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                                }
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            //Search By Mobile
                            case 4:
                            searchByMobile:
                                Actions.printDateTimeAndStatus("employee");
                                Console.Write("\n\tEnter Master Mobile For Search : ");
                                string masterMobile = Console.ReadLine();
                                if (!dbSearchMaster.Masters.Any(t => t.Mobile == masterMobile))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\tthere Isn't Any Master By This mobile({masterMobile})");
                                    Thread.Sleep(3000);
                                    goto searchByMobile;
                                }
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n" + dbSearchMaster.Masters.First(t => t.Mobile == masterMobile));
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            default:
                                goto searchMaster;
                        }
                    }
                #endregion

                #region Serach Student
                case 10:

                    using (UniverCityDbContext dbSearchStudent = new UniverCityDbContext("undbcontext"))
                    {
                    searchStudent:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Search Student");
                        Console.WriteLine("\n\t1.Search By Id");
                        Console.WriteLine("\n\t2.Search By Name");
                        Console.WriteLine("\n\t3.Search By Family");
                        Console.WriteLine("\n\t4.Search By Mobile");
                        Console.Write("\n\n\tWitch Part : ");
                        int searchAnswer;
                        while (!int.TryParse(Console.ReadLine(), out searchAnswer))
                        {
                            Actions.printDateTimeAndStatus("master");
                            Actions.warningEnterNumber("Menu Operation");
                            goto searchStudent;
                        }
                        switch (searchAnswer)
                        {
                            //Search By Id
                            case 1:
                                Actions.SearchStudentById("employee");
                                goto employeeMenu;

                            //Search By Name
                            case 2:
                                Actions.SearchStudentByName("employee");
                                goto employeeMenu;

                            //Search By Family
                            case 3:
                                Actions.SearchStudentByFamily("employee");
                                goto employeeMenu;

                            //Search By Mobile
                            case 4:
                                Actions.SearchStudentByMobile("emloyee");
                                goto employeeMenu;

                            default:
                                goto searchStudent;
                        }
                    }
                #endregion

                #region Search Course 
                case 11:
                    using (UniverCityDbContext dbSerachCourse = new UniverCityDbContext("undbcontext"))
                    {
                    SearchCourse:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Search Course");
                        Console.WriteLine("\n\t1.Search By Id");
                        Console.WriteLine("\n\t2.Search By Name");
                        Console.Write("\n\n\tWitch Part : ");
                        int answerSearchCourse;
                        while (!int.TryParse(Console.ReadLine(), out answerSearchCourse))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto SearchCourse;
                        }
                        switch (answerSearchCourse)
                        {
                            //Search By Id                             
                            case 1:
                                Actions.searchCourseById("employee");
                                goto employeeMenu;

                            case 2:
                                Actions.searchCourseByName("employee");
                                goto employeeMenu;

                            default:
                                goto SearchCourse;
                        }

                    }
                #endregion

                #region Search Score

                case 12:
                    using (UniverCityDbContext dbSerachScore = new UniverCityDbContext("undbcontext"))
                    {
                    searchScore:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Search Course");
                        Console.WriteLine("\n\t1.Serach By Id");
                        Console.WriteLine("\n\t2.Serach By Student");
                        Console.WriteLine("\n\t3.Serach By Course");
                        Console.Write("\n\n\tWitch Part? : ");
                        int searchScoreAnswer;
                        while (!int.TryParse(Console.ReadLine(), out searchScoreAnswer))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto searchScore;
                        }
                        switch (searchScoreAnswer)
                        {
                            // Search By Score Id
                            case 1:
                                Actions.searchScoreById("employee");
                                goto employeeMenu;

                            case 2:
                                //Search By StudentId
                                Actions.searchScoreByStudent("employee");
                                goto employeeMenu;

                            //Search By CourseId
                            case 3:
                                Actions.searchScoreByCourse("employee");
                                goto employeeMenu;

                            default:
                                goto searchScore;
                        }

                    }

                #endregion

                #region Delete Master
                case 13:
                    using (UniverCityDbContext dbDeleteMaster = new UniverCityDbContext("undbcontext"))
                    {
                    deleteMaster:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t|Delete Master|");
                        Actions.showAllMaster(false);
                        Console.Write("\n\tEnter Master Id For Delete her/him : ");
                        int masterId;
                        while (!int.TryParse(Console.ReadLine(), out masterId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("masterId");
                            goto deleteMaster;
                        }
                        if (!dbDeleteMaster.Masters.Any(t => t.PersonId == masterId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(masterId, "Master List");
                            goto employeeMenu;
                        }
                        Master masterFindForDelete = dbDeleteMaster.Masters.First(t => t.PersonId == masterId);
                    askAreYouSure:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\n\n**********************************************************************************************************************");
                        Console.WriteLine(masterFindForDelete);
                        if (masterFindForDelete.Courses.Count != 0)
                        {
                            Console.WriteLine("_____________\nCourse List : ");
                            foreach (Course course in masterFindForDelete.Courses.ToList())
                            {
                                Console.WriteLine(course);
                            }
                        }
                        Console.WriteLine("\n**********************************************************************************************************************");
                        Console.Write("\n\n\tAre You Sure For Delte This Master (1. Yes | 2. No) : ");
                        int areYouSure;
                        while (!int.TryParse(Console.ReadLine(), out areYouSure))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto askAreYouSure;
                        }

                        switch (areYouSure)
                        {
                            case 1:
                                if (masterFindForDelete.RegisterScore != null)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tThis Master Register Score For Many Student And You Can't Remove He/She.");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }
                                dbDeleteMaster.Masters.Remove(dbDeleteMaster.Masters.First(t => t.PersonId == masterFindForDelete.PersonId));
                                dbDeleteMaster.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\t{masterFindForDelete.Name} {masterFindForDelete.Family} Delete From Master List ...... ");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n\tNo Problem");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            default:
                                goto askAreYouSure;
                        }
                    }
                #endregion

                #region Delete Student

                case 14:
                    using (UniverCityDbContext dbDeleteStudent = new UniverCityDbContext("undbcontext"))
                    {
                    deleteStudent:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t|Delete Student|");
                        Actions.showAllStudent(false);
                        Console.Write("\n\tEnter Student Id For Delete her/him : ");
                        int studentId;
                        while (!int.TryParse(Console.ReadLine(), out studentId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Student Id");
                            goto deleteStudent;
                        }
                        if (!dbDeleteStudent.Students.Any(t => t.PersonId == studentId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(studentId, "Student List");
                            goto employeeMenu;
                        }
                        Student studentFindForDelete = dbDeleteStudent.Students.First(t => t.PersonId == studentId);
                    askAreYouSure:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\n\n**********************************************************************************************************************");
                        Console.WriteLine(studentFindForDelete);
                        if (studentFindForDelete.Courses.Count != 0)
                        {
                            Console.WriteLine("_____________\nCourse List : ");
                            foreach (Course course in studentFindForDelete.Courses.ToList())
                            {
                                Console.WriteLine(course);
                            }
                        }
                        Console.WriteLine("\n**********************************************************************************************************************");
                        Console.Write("\n\n\tAre You Sure For Delte This Student (1. Yes | 2. No) : ");
                        int areYouSure;
                        while (!int.TryParse(Console.ReadLine(), out areYouSure))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto askAreYouSure;
                        }

                        switch (areYouSure)
                        {
                            case 1:
                                dbDeleteStudent.Students.Remove(dbDeleteStudent.Students.First(t => t.PersonId == studentFindForDelete.PersonId));
                                dbDeleteStudent.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\t{studentFindForDelete.Name} {studentFindForDelete.Family} Delete From Student List ...... ");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n\tNo Problem");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            default:
                                goto askAreYouSure;
                        }
                    }
                #endregion

                #region Delete Course
                case 15:
                    using (UniverCityDbContext dbDeleteCourse = new UniverCityDbContext("undbcontext"))
                    {
                    deleteStudent:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t|Delete Course|");
                        Actions.showAllCourse(false);
                        Console.Write("\n\tEnter Course Id For Delete Its : ");
                        int courseId;
                        while (!int.TryParse(Console.ReadLine(), out courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Course Id");
                            goto deleteStudent;
                        }
                        if (!dbDeleteCourse.Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(courseId, "Course List");
                            goto employeeMenu;
                        }
                        Course courseFindForDelete = dbDeleteCourse.Courses.First(t => t.Id == courseId);
                    askAreYouSure:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\n\n**********************************************************************************************************************");
                        Console.WriteLine(courseFindForDelete);
                        if (courseFindForDelete.Master != null)
                        {
                            Console.WriteLine("_____________\nMaster : ");
                            Console.WriteLine(courseFindForDelete.Master);
                        }
                        Console.WriteLine("\n**********************************************************************************************************************");
                        Console.Write("\n\n\tAre You Sure For Delte This Course (1. Yes | 2. No) : ");
                        int areYouSure;
                        while (!int.TryParse(Console.ReadLine(), out areYouSure))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto askAreYouSure;
                        }

                        switch (areYouSure)
                        {
                            case 1:
                                dbDeleteCourse.Courses.Remove(dbDeleteCourse.Courses.First(t => t.Id == courseFindForDelete.Id));
                                dbDeleteCourse.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\t{courseFindForDelete.Name} By {courseFindForDelete.Unit} Delete From Course List ...... ");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n\tNo Problem");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            default:
                                goto askAreYouSure;
                        }
                    }

                #endregion

                #region Delete Score 
                case 16:
                deleteScore:
                    using (UniverCityDbContext dbDeleteScore = new UniverCityDbContext("undbcontext"))
                    {
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t|Delete Score|");
                        Console.WriteLine("\n\n\t1.Sort By Student Name");
                        Console.WriteLine("\n\t2.Sort By Course Name");
                        Console.Write("\n\n\n\tWitch Part Do You Want ? ");
                        int answerSortBy;
                        while (!int.TryParse(Console.ReadLine(), out answerSortBy))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto deleteScore;
                        }

                        switch (answerSortBy)
                        {

                            #region Sort By Student Name
                            case 1:
                            sortByStudentName:
                                Actions.printDateTimeAndStatus("employee");
                                Actions.showAllStudent(false);
                                Console.Write("\n\n\tWhich Student Do You Want See Her/Him Scores? ");
                                int studentId;
                                while (!int.TryParse(Console.ReadLine(), out studentId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Student Id");
                                    goto sortByStudentName;
                                }
                                if (!dbDeleteScore.Students.Any(t => t.PersonId == studentId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(studentId, "Student List");
                                    goto sortByStudentName;
                                }
                                Student studentFind = dbDeleteScore.Students.First(t => t.PersonId == studentId);
                                if (studentFind.Scores.Count == 0)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tThis Student Doesn't Have Any Score ......");
                                    Actions.comeBackToMainMenu();
                                }

                            chooseScore:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n-----------------------------------------------------------------------------------------------------------------------");
                                foreach (Score score in studentFind.Scores.ToList())
                                {
                                    Console.WriteLine(score);
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                                }
                                Console.Write("\n\n\tEnter Score Id For Delete Its : ");
                                int scoreId;
                                while (!int.TryParse(Console.ReadLine(), out scoreId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Score Id");
                                    goto chooseScore;
                                }
                                if (!studentFind.Scores.Any(t => t.Id == scoreId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(scoreId, "Score List");
                                    goto chooseScore;
                                }
                                dbDeleteScore.Scores.Remove(dbDeleteScore.Scores.First(t => t.Id == scoreId));
                                dbDeleteScore.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\tScore By this id [{scoreId}] Deleted From {studentFind.Name} {studentFind.Family}'s ScoreList ");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            #endregion

                            #region Sort By Course Name
                            case 2:
                            sortByCourseName:
                                Actions.printDateTimeAndStatus("employee");
                                Actions.showAllCourse(false);
                                Console.Write("\n\n\tWhich Course Do You Want See Its Scores? ");

                                int courseId;
                                while (!int.TryParse(Console.ReadLine(), out courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Course Id");
                                    goto sortByCourseName;
                                }
                                if (!dbDeleteScore.Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(courseId, "Student List");
                                    goto sortByCourseName;
                                }
                                Course courseFind = dbDeleteScore.Courses.First(t => t.Id == courseId);
                                if (courseFind.Scores.Count == 0)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tThis Course Doesn't Have Any Score ......");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }
                            chooseScoreInCourses:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n-----------------------------------------------------------------------------------------------------------------------");
                                foreach (Score score in courseFind.Scores.ToList())
                                {
                                    Console.WriteLine(score);
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                                }
                                Console.Write("\n\n\tEnter Score Id For Delete Its : ");
                                int courseScoreId;
                                while (!int.TryParse(Console.ReadLine(), out courseScoreId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Score Id");
                                    goto chooseScoreInCourses;
                                }
                                if (!courseFind.Scores.Any(t => t.Id == courseScoreId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(courseScoreId, "Score List");
                                    goto chooseScoreInCourses;
                                }
                                dbDeleteScore.Scores.Remove(dbDeleteScore.Scores.First(t => t.Id == courseScoreId));
                                dbDeleteScore.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\tScore By this id [{courseScoreId}] Deleted From {courseFind.Name}'s ScoreList ");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;
                            #endregion

                            default:
                                goto deleteScore;
                        }
                    }
                #endregion

                #region Edit Msater

                case 17:

                    using (UniverCityDbContext dbEditMaster = new UniverCityDbContext("undbcontext"))
                    {
                    editMasterInformation:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Edit Master Information");
                        Actions.showAllMaster(false);
                        Console.Write("\n\n\tWhich Master Do You Want Edit her/him Information ? ");
                        int masterId;
                        while (!int.TryParse(Console.ReadLine(), out masterId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Master Id");
                            goto editMasterInformation;
                        }
                        if (!dbEditMaster.Masters.Any(t => t.PersonId == masterId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(masterId, "Master List");
                            goto employeeMenu;
                        }
                        Master masterFind = dbEditMaster.Masters.First(t => t.PersonId == masterId);
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t1. Edit Name");
                        Console.WriteLine("\n\t2. Edit Family");
                        Console.WriteLine("\n\t3. Edit Password");
                        Console.WriteLine("\n\t4. Add Course To Her/Him Course List ");
                        Console.WriteLine("\n\t5. Edit Degree");
                        Console.WriteLine("\n\t6. Change Acsess");
                        Console.WriteLine("\n\n\tWhich Part ? :");
                        int answerEditMasterInformation;
                        while (!int.TryParse(Console.ReadLine(), out answerEditMasterInformation))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto employeeMenu;
                        }
                        switch (answerEditMasterInformation)
                        {
                            //edit Name
                            case 1:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Name");
                                Console.WriteLine($"\n\tMaster Previous Name : {masterFind.Name}");
                                Console.Write("\n\tEnte New Master Name : ");
                                dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Name = Console.ReadLine();
                                dbEditMaster.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tMaster Name Changed To {dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Name}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                                //Edit Family
                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Family");
                                Console.WriteLine($"\n\tMaster Previous Family : {masterFind.Family}");
                                Console.Write("\n\tEnter New Master Family : ");
                                dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Family = Console.ReadLine();
                                dbEditMaster.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tMaster Name Changed To {dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Family}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                                //Ediit Password
                            case 3:
                            editPassword:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Password");
                                Console.WriteLine($"\n\tMaster Previous Password : {masterFind.Password}");
                                Console.Write("\n\tEnter New Master Password : ");
                                string newPassword = Console.ReadLine();
                                Console.Write("\n\tEnter New Master Password Again : ");
                                string repeatNewPassword = Console.ReadLine();
                                if (newPassword != repeatNewPassword)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\tYour Repeat New Password Is Not True .. ");
                                    Thread.Sleep(3000);
                                    goto editPassword;
                                }
                                dbEditMaster.Masters.First(t => t.PersonId == masterId).Password = newPassword;
                                dbEditMaster.SaveChanges();
                                Actions.comeBackToMainMenu();

                                goto employeeMenu;
                                //Add Course
                            case 4:
                            addCourseToMyCourseList:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Course List\n\n");
                                foreach (Course course in dbEditMaster.Courses.Where(t => t.Master == null).ToList())
                                {
                                    Console.WriteLine($"{course}");
                                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                                }
                                Console.Write($"\n\tEnter CourseID For Add Its To Master {masterFind.Name}'s List : ");
                                int courseId;
                                while (!int.TryParse(Console.ReadLine(), out courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Course Id");
                                    goto addCourseToMyCourseList;
                                }
                                if (!dbEditMaster.Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(courseId, "CourseList");
                                    goto addCourseToMyCourseList;
                                }
                                if (dbEditMaster.Courses.First(t => t.Id == courseId).Master != null)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\tFor This Course Registered A Master ..... \n\n\tPlease Select Another Course ..... ");
                                    Thread.Sleep(4000);
                                    goto editMasterInformation;
                                }
                                dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Courses.Add(dbEditMaster.Courses.First(t => t.Id == courseId));
                                dbEditMaster.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\t{dbEditMaster.Courses.FirstOrDefault(t => t.Id == courseId).Name} Was Added To {masterFind.Name} {masterFind.Family}'s Courses List");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                                //Edit Degree
                            case 5:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Degree");
                                Console.WriteLine($"\n\tMaster Previous Degree : {masterFind.Degree}");
                                Console.Write("\n\tEnter New Master Degree : ");
                                dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Degree = Console.ReadLine();
                                dbEditMaster.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tYour Degree Changed To {dbEditMaster.Masters.First(t => t.PersonId == masterFind.PersonId).Degree}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                                //Change Acsess
                            case 6:
                                if (dbEditMaster.Masters.First(t => t.PersonId == masterId).IsActive == true)
                                {
                                    dbEditMaster.Students.First(t => t.PersonId == masterId).IsActive = false;
                                    dbEditMaster.SaveChanges();
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\t{masterFind.Name} {masterFind.Family}'s Acsess change To Block And He/She Can't Login.");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }
                                else
                                {
                                    dbEditMaster.Students.First(t => t.PersonId == masterId).IsActive = true;
                                    dbEditMaster.SaveChanges();
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\t{masterFind.Name} {masterFind.Family}'s Acsess change And Now He/She Can Login.");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }

                            default:
                                goto editMasterInformation;
                        }
                    }
                #endregion

                #region Edit Student
                case 18:

                    using (UniverCityDbContext dbEditStudent = new UniverCityDbContext("undbcontext"))
                    {
                    editMasterInformation:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Edit Student Information");
                        Actions.showAllStudent(false);
                        Console.Write("\n\n\tWhich Student Do You Want Edit her/him Information ? ");
                        int studentId;
                        while (!int.TryParse(Console.ReadLine(), out studentId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Master Id");
                            goto editMasterInformation;
                        }
                        //if User Enter Wrong StudentId
                        if (!dbEditStudent.Masters.Any(t => t.PersonId == studentId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(studentId, "Student List");
                            goto employeeMenu;
                        }
                        Student studentFind = dbEditStudent.Students.First(t => t.PersonId == studentId);
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t1. Edit Name");
                        Console.WriteLine("\n\t2. Edit Family");
                        Console.WriteLine("\n\t3. Edit Password");
                        Console.WriteLine("\n\t4. Add Course To Her/Him Course List ");
                        Console.WriteLine("\n\t5. Edit Degree");
                        Console.WriteLine("\n\t6. Change Acsess");
                        Console.WriteLine("\n\n\tWhich Part ? :");
                        int answerEditStudentInformation;
                        while (!int.TryParse(Console.ReadLine(), out answerEditStudentInformation))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto employeeMenu;
                        }
                        switch (answerEditStudentInformation)
                        {
                            //Edit Name
                            case 1:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Name");
                                Console.WriteLine($"\n\tStudent Previous Name : {studentFind.Name}");
                                Console.Write("\n\tEnte New Student Name : ");
                                dbEditStudent.Masters.First(t => t.PersonId == studentFind.PersonId).Name = Console.ReadLine();
                                dbEditStudent.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tStudent Name Changed To {dbEditStudent.Students.First(t => t.PersonId == studentFind.PersonId).Name}");
                                Thread.Sleep(3000);
                                goto employeeMenu;

                            //Edit Family
                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Family");
                                Console.WriteLine($"\n\tStudent Previous Family : {studentFind.Family}");
                                Console.Write("\n\tEnter New Student Family : ");
                                dbEditStudent.Students.First(t => t.PersonId == studentFind.PersonId).Family = Console.ReadLine();
                                dbEditStudent.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tStudent Name Changed To {dbEditStudent.Students.First(t => t.PersonId == studentFind.PersonId).Family}");
                                Thread.Sleep(3000);
                                goto employeeMenu;

                            //Edit Password
                            case 3:
                            editPassword:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Password");
                                Console.WriteLine($"\n\tStudent Previous Password : {studentFind.Password}");
                                Console.Write("\n\tEnter New Student Password : ");
                                string newPassword = Console.ReadLine();
                                Console.Write("\n\tEnter New Student Password Again : ");
                                string repeatNewPassword = Console.ReadLine();
                                if (newPassword != repeatNewPassword)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\tYour Repeat New Password Is Not True .. ");
                                    Thread.Sleep(3000);
                                    goto editPassword;
                                }
                                dbEditStudent.Masters.First(t => t.PersonId == studentId).Password = newPassword;
                                dbEditStudent.SaveChanges();
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            //Add Course To Course List
                            case 4:
                            addCourseToMyCourseList:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Course List\n\n");
                                Actions.showAllCourse(true);
                                Console.Write($"\n\tEnter CourseID For Add Its To Master {studentFind.Name}'s List : ");
                                int courseId;
                                while (!int.TryParse(Console.ReadLine(), out courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Course Id");
                                    goto addCourseToMyCourseList;
                                }
                                if (!dbEditStudent.Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(courseId, "CourseList");
                                    goto addCourseToMyCourseList;
                                }
                                dbEditStudent.Students.First(t => t.PersonId == studentFind.PersonId).Courses.Add(dbEditStudent.Courses.First(t => t.Id == courseId));
                                dbEditStudent.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\t{dbEditStudent.Courses.FirstOrDefault(t => t.Id == courseId).Name} Was Added To {studentFind.Name} {studentFind.Family}'s Courses List");
                                Thread.Sleep(3000);
                                goto employeeMenu;

                            //Edit Degree
                            case 5:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Degree");
                                Console.WriteLine($"\n\tMaster Previous Degree : {studentFind.Degree}");
                                Console.Write("\n\tEnter New Student Degree : ");
                                dbEditStudent.Students.First(t => t.PersonId == studentFind.PersonId).Degree = Console.ReadLine();
                                dbEditStudent.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tStudent Degree Changed To {dbEditStudent.Masters.First(t => t.PersonId == studentFind.PersonId).Degree}");
                                Thread.Sleep(3000);
                                goto employeeMenu;

                                //Change Acsess
                            case 6:

                                if (dbEditStudent.Students.First(t=>t.PersonId == studentId).IsActive == true)
                                {
                                    dbEditStudent.Students.First(t => t.PersonId == studentId).IsActive = false;
                                    dbEditStudent.SaveChanges();
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\t{studentFind.Name} {studentFind.Family}'s Acsess change To Block And He/She Can't Login.");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }
                                else
                                {
                                    dbEditStudent.Students.First(t => t.PersonId == studentId).IsActive = true;
                                    dbEditStudent.SaveChanges();
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\n\t{studentFind.Name} {studentFind.Family}'s Acsess change And Now He/She Can Login.");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }                               
                            default:
                                goto editMasterInformation;
                        }
                    }

                #endregion

                #region Edit Course
                case 19:
                    using (UniverCityDbContext dbEditCourse = new UniverCityDbContext("undbcontext"))
                    {
                    editCourse:
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("Edit Course Information");
                        Actions.showAllCourse(false);
                        Console.Write("\n\n\tWhich Course Do You Want Edit It's Information ? ");
                        int courseId;
                        while (!int.TryParse(Console.ReadLine(), out courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("Course Id");
                            goto editCourse;
                        }
                        //If User Enter Wrong CourseId
                        if (!dbEditCourse.Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningNotFound(courseId, "Course List");
                            goto employeeMenu;
                        }
                        Course courseFind = dbEditCourse.Courses.First(t => t.Id == courseId);
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t1. Edit Name");
                        Console.WriteLine("\n\t2. Edit Unit");
                        Console.WriteLine("\n\t3. Choose Master");
                        Console.WriteLine("\n\n\tWhich Part ? :");
                        int answerEditCourseInformation;
                        while (!int.TryParse(Console.ReadLine(), out answerEditCourseInformation))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto employeeMenu;
                        }
                        switch (answerEditCourseInformation)
                        {
                            //Edit Name
                            case 1:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("Edit Name");
                                Console.WriteLine($"\n\tPrevious Course Name : {courseFind.Name}");
                                Console.Write("\n\tEnte New Course Name : ");
                                dbEditCourse.Courses.First(t => t.Id == courseFind.Id).Name = Console.ReadLine();
                                dbEditCourse.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tCourse Name Changed To {dbEditCourse.Courses.First(t => t.Id == courseFind.Id).Name}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                            //Edit Unit
                            case 2:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\tPrevious Course({courseFind.Name}) Unit");
                            enterUnit:
                                Console.Write($"\n\tEnter New Unit For This Course");
                                int courseUnit;
                                while (!int.TryParse(Console.ReadLine(), out courseUnit))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Course Unit");
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine($"\n\tPrevious Course({courseFind.Name}) Unit");
                                    goto enterUnit;
                                }
                                dbEditCourse.Courses.First(t => t.Id == courseId).Unit = courseUnit;
                                dbEditCourse.SaveChanges();
                                Console.WriteLine($"\n\tCourse Unit Changed To {dbEditCourse.Courses.First(t => t.Id == courseFind.Id).Unit}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                            //Choose Master
                            case 3:
                            chooseMasterForCourse:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine("\n\n\tChoose Master For Course");
                                //If Course Has A Master
                                if (courseFind.Master != null)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tFor This Course Registered A Master ...");
                                    Console.WriteLine("\n\tPlease Choose Another Course .... ");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }
                                Actions.showAllMaster(false);
                                Console.Write("\n\n\tChoose Master Id For Cuorse's Master : ");
                                int masterId;
                                while (!int.TryParse(Console.ReadLine(), out masterId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("master Id");
                                    goto chooseMasterForCourse;
                                }
                                //If User Enter Wrong CourseId
                                if (!dbEditCourse.Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(masterId, "Master Id");
                                    goto employeeMenu;
                                }
                                dbEditCourse.Courses.First(t => t.Id == courseId).Master = dbEditCourse.Masters.First(t => t.PersonId == masterId);
                                dbEditCourse.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\tNow {dbEditCourse.Masters.First(t => t.PersonId == masterId).Name} Is Master Of {courseFind.Name}.");
                                Actions.comeBackToMainMenu();
                                goto employeeMenu;

                            default:
                                goto editCourse;
                        }
                    }

                #endregion

                #region Edit Score
                case 20:
                    using (UniverCityDbContext dbEditScore = new UniverCityDbContext("undbcontext"))
                    {
                        Actions.printDateTimeAndStatus("employee");
                        Console.WriteLine("\n\t|Edit Score|");
                        Console.WriteLine("\n\n\t1.Sort By Student Name");
                        Console.WriteLine("\n\t2.Sort By Course Name");
                        Console.Write("\n\n\n\tWitch Part Do You Want ? ");
                        int answerSortBy;
                        while (!int.TryParse(Console.ReadLine(), out answerSortBy))
                        {
                            Actions.printDateTimeAndStatus("employee");
                            Actions.warningEnterNumber("This Question");
                            goto deleteScore;
                        }

                        switch (answerSortBy)
                        {
                            #region Sort By Student Name
                            case 1:
                            sortByStudentName:
                                Actions.printDateTimeAndStatus("employee");
                                Actions.showAllStudent(false);
                                Console.Write("\n\n\tWhich Student Do You Want See Her/Him Scores? ");
                                int studentId;
                                while (!int.TryParse(Console.ReadLine(), out studentId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Student Id");
                                    goto sortByStudentName;
                                }
                                //If User Enter Wrong ID
                                if (!dbEditScore.Students.Any(t => t.PersonId == studentId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(studentId, "Student List");
                                    goto sortByStudentName;
                                }
                                Student studentFind = dbEditScore.Students.First(t => t.PersonId == studentId);
                                //If Student By This Id Does'nt Have Any Score
                                if (studentFind.Scores.Count == 0)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tThis Student Doesn't Have Any Score ......");
                                    Actions.comeBackToMainMenu();
                                }

                            chooseScore:
                                Actions.printDateTimeAndStatus("employee");
                                //show Student's Scores
                                Console.WriteLine("\n\n-----------------------------------------------------------------------------------------------------------------------");
                                foreach (Score score in studentFind.Scores.ToList())
                                {
                                    Console.WriteLine(score);
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                                }
                                Console.Write("\n\n\tEnter Score Id For Edit Its : ");
                                int scoreId_Student;
                                while (!int.TryParse(Console.ReadLine(), out scoreId_Student))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Score Id");
                                    goto employeeMenu;
                                }
                                //If User Enter Wrong Score Id
                                if (!studentFind.Scores.Any(t => t.Id == scoreId_Student))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(scoreId_Student, "Score List");
                                    goto chooseScore;
                                }
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\tScore Number : {dbEditScore.Scores.First(t => t.Id == scoreId_Student).ScoreNumber}");
                            enterscore:
                                Console.Write("\n\n\tEnter New Score Number : ");
                                int scoreNumber;
                                while (!int.TryParse(Console.ReadLine(), out scoreNumber))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Score Number");
                                    goto employeeMenu;
                                }
                                //Valid Score
                                if (scoreNumber > 20 || scoreNumber < 0)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tScore Number Is Not True .... ");
                                    Thread.Sleep(3000);
                                    goto enterscore;
                                }

                                dbEditScore.Scores.First(t => t.Id == scoreId_Student).ScoreNumber = scoreNumber;
                                dbEditScore.Scores.First(t => t.Id == scoreId_Student).Edit = dbEditScore.Employees.First(t => t.PersonId == employeeSign.PersonId);
                                dbEditScore.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"Score Number Change To {scoreNumber}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                            #endregion

                            #region Sort By Course Name
                            case 2:
                            sortByCourseName:
                                Actions.printDateTimeAndStatus("employee");
                                Actions.showAllCourse(false);
                                Console.Write("\n\n\tWhich Course Do You Want See Its Scores? ");

                                int courseId;
                                while (!int.TryParse(Console.ReadLine(), out courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Course Id");
                                    goto sortByCourseName;
                                }
                                //If User Enter Wrong Course ID
                                if (!dbEditScore.Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(courseId, "Course List");
                                    goto sortByCourseName;
                                }
                                Course courseFind = dbEditScore.Courses.First(t => t.Id == courseId);
                                //If Course By This Id Does'nt Have Any Score
                                if (courseFind.Scores.Count == 0)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tThis Course Doesn't Have Any Score ......");
                                    Actions.comeBackToMainMenu();
                                    goto employeeMenu;
                                }
                            chooseScoreInCourses:
                                Actions.printDateTimeAndStatus("employee");
                                //Show Course's Scores
                                Console.WriteLine("\n\n-----------------------------------------------------------------------------------------------------------------------");
                                foreach (Score score in courseFind.Scores.ToList())
                                {
                                    Console.WriteLine(score);
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                                }
                                Console.Write("\n\n\tEnter Score Id For Edit Its : ");
                                int scoreId_Course;
                                while (!int.TryParse(Console.ReadLine(), out scoreId_Course))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Score Id");
                                    goto chooseScoreInCourses;
                                }
                                //If User Enter Wrong Score ID
                                if (!courseFind.Scores.Any(t => t.Id == scoreId_Course))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningNotFound(scoreId_Course, "Score List");
                                    goto chooseScoreInCourses;
                                }

                            enterScoreNumber:
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"\n\n\tscore Number : {dbEditScore.Scores.First(t => t.Id == scoreId_Course).ScoreNumber}");
                                Console.Write("\n\n\tEnter New Score Number : ");
                                int scoreNumber_Course;
                                while (!int.TryParse(Console.ReadLine(), out scoreNumber_Course))
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Actions.warningEnterNumber("Score Number");
                                    goto employeeMenu;
                                }
                                //Valid Score
                                if (scoreNumber_Course > 20 || scoreNumber_Course < 0)
                                {
                                    Actions.printDateTimeAndStatus("employee");
                                    Console.WriteLine("\n\n\tScore Number Is Not True .... ");
                                    Thread.Sleep(3000);
                                    goto enterScoreNumber;
                                }

                                dbEditScore.Scores.First(t => t.Id == scoreId_Course).ScoreNumber = scoreNumber_Course;
                                dbEditScore.SaveChanges();
                                Actions.printDateTimeAndStatus("employee");
                                Console.WriteLine($"Score Number Change To {scoreNumber_Course}");
                                Thread.Sleep(3000);
                                goto employeeMenu;
                            #endregion

                            default:
                                goto deleteScore;
                        }
                    }
                #endregion

                #region Exit
                case 21:
                    goto login;
                #endregion

                #region Default
                default:
                    goto employeeMenu;
                    #endregion
            }
        #endregion

        #region Logged Student

        #region Menu
        SignIn_Student:

        studentMenu:
            Actions.printDateTimeAndStatus("student");
            Console.WriteLine("\n\tStudent Menu : \n");
            Console.WriteLine("   -------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("   | 1. Choose Course            |             2. See My All Course             |             3. Edit My Information |");
            Console.WriteLine("   |                             |                                              |                                    |");
            Console.WriteLine("   | 4. See My All Score         |             5. Seach Score                   |             6. Exit                |");
            Console.WriteLine("   -------------------------------------------------------------------------------------------------------------------");
            Console.Write("\n\n\tEnter number of the part you want : ");
            int answerStudentList;
            while (!int.TryParse(Console.ReadLine(), out answerStudentList))
            {
                Actions.printDateTimeAndStatus("student");
                Actions.warningEnterNumber("Menu");
                goto studentMenu;
            }
            #endregion

            switch (answerStudentList)
            {
                #region Choose Course
                case 1:
                    using (UniverCityDbContext dbChooseCourse = new UniverCityDbContext("undbcontext"))
                    {
                    chooseCourse:
                        Actions.printDateTimeAndStatus("student");
                        Console.WriteLine("Choose Course");
                        Actions.showAllCourse(true);
                        Console.Write("\n\n\tEnter Course Id For Add It's To Your Course List : ");
                        int courseId;
                        while (!int.TryParse(Console.ReadLine(), out courseId))
                        {
                            Actions.printDateTimeAndStatus("student");
                            Actions.warningEnterNumber("course Id");
                            goto chooseCourse;
                        }
                        if (!dbChooseCourse.Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("student");
                            Actions.warningNotFound(courseId, "Course List");
                            goto studentMenu;
                        }
                        Course courseFind = dbChooseCourse.Courses.First(t => t.Id == courseId);
                        if (dbChooseCourse.Students.First(t => t.PersonId == studentSign.PersonId).Courses.Any(t => t.Id == courseId))
                        {
                            Actions.printDateTimeAndStatus("student");
                            Console.WriteLine("\n\n\tThis Course Has Already Been Registered For You ....");
                            Actions.comeBackToMainMenu();
                            goto studentMenu;
                        }
                        dbChooseCourse.Students.First(t => t.PersonId == studentSign.PersonId).Courses.Add(courseFind);
                        dbChooseCourse.SaveChanges();
                        Actions.printDateTimeAndStatus("student");
                        Console.WriteLine($"\n\n\t{courseFind.Name} Add To Your Courses List ....... ");
                        Actions.comeBackToMainMenu();
                        goto studentMenu;
                    }
                #endregion

                #region See My All Course
                case 2:
                    using (UniverCityDbContext dbSeeMyAllCourse = new UniverCityDbContext("undbcontext"))
                    {
                        if (dbSeeMyAllCourse.Students.First(t => t.PersonId == studentSign.PersonId).Courses.Count == 0)
                        {
                            Actions.printDateTimeAndStatus("student");
                            Console.WriteLine("\n\n\tYou Don't Have Any Course....");
                            Thread.Sleep(4000);
                            goto studentMenu;
                        }
                        Actions.printDateTimeAndStatus("student");
                        Console.WriteLine("My Course \n\n");
                        foreach (Course course in dbSeeMyAllCourse.Students.First(t => t.PersonId == studentSign.PersonId).Courses.ToList())
                        {
                            Console.WriteLine(" " + course);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                        }
                        Actions.comeBackToMainMenu();
                        goto studentMenu;
                    }
                #endregion

                #region Edit My Information
                case 3:
                    using (UniverCityDbContext dbEditInformation = new UniverCityDbContext("undbcontext"))
                    {
                    editMyInformation:
                        Actions.printDateTimeAndStatus("student");
                        Console.WriteLine("Edit My Information");
                        Console.WriteLine("\n\t1. Edit My Name");
                        Console.WriteLine("\n\t2. Edit My Family");
                        Console.WriteLine("\n\t3. Edit Password");
                        Console.WriteLine("\n\n\tWhich Part ? :");
                        int answerEditMyInformation;
                        while (!int.TryParse(Console.ReadLine(), out answerEditMyInformation))
                        {
                            Actions.printDateTimeAndStatus("student");
                            Actions.warningEnterNumber("This Question");
                            goto studentMenu;
                        }
                        switch (answerEditMyInformation)
                        {
                            case 1:
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine("Edit Name");
                                Console.WriteLine($"\n\tYour Previous Name : {dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Name}");
                                Console.Write("\n\tEnter Your New Name : ");
                                dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Name = Console.ReadLine();
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine($"\n\tYour Name Changed To {dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Name}");
                                Thread.Sleep(3000);
                                goto studentMenu;
                            case 2:
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine("Edit Family");
                                Console.WriteLine($"\n\tYour Previous Family : {dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Family}");
                                Console.Write("\n\tEnter Your New Family : ");
                                dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Family = Console.ReadLine();
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine($"\n\tYour Name Changed To {dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Family}");
                                Thread.Sleep(3000);
                                goto studentMenu;

                            case 3:
                            editPassword:
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine("Edit Password");
                                Console.WriteLine($"\n\tYour Previous Password : {dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Password}");
                                Console.Write("\n\tEnter Your New Password : ");
                                string newPassword = Console.ReadLine();
                                Console.Write("\n\tEnter Your New Password Again : ");
                                string repeatNewPassword = Console.ReadLine();
                                if (newPassword != repeatNewPassword)
                                {
                                    Actions.printDateTimeAndStatus("student");
                                    Console.WriteLine("\n\tYour Repeat New Password Is Not True .. ");
                                    Thread.Sleep(3000);
                                    goto editPassword;
                                }
                                dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Password = newPassword;
                                dbEditInformation.SaveChanges();
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine($"\n\tYour Password Changed To {dbEditInformation.Students.First(t => t.PersonId == studentSign.PersonId).Password}\n\n\tPlease Try Again For Login .......");
                                Thread.Sleep(3000);
                                goto login;

                            default:
                                goto editMyInformation;
                        }
                    }
                #endregion

                #region See My All Score
                case 4:
                    using (UniverCityDbContext dbSeeMyAllScore = new UniverCityDbContext("undbcontext"))
                    {
                        if (dbSeeMyAllScore.Students.First(t => t.PersonId == studentSign.PersonId).Scores.Count == 0)
                        {
                            Actions.printDateTimeAndStatus("student");
                            Console.WriteLine("\n\n\tYou Don't Have Any Score....");
                            Thread.Sleep(4000);
                            goto studentMenu;
                        }
                        Actions.printDateTimeAndStatus("student");
                        Console.WriteLine("My All Score\n\n");
                        foreach (Score score in dbSeeMyAllScore.Students.First(t => t.PersonId == studentSign.PersonId).Scores.ToList())
                        {
                            Console.WriteLine(" " + score);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                        }
                        Actions.comeBackToMainMenu();
                        goto studentMenu;
                    }
                #endregion

                #region Search Score
                case 5:
                    using (UniverCityDbContext dbSearchScore = new UniverCityDbContext("undbcontext"))
                    {
                    searchScore:
                        if (dbSearchScore.Students.First(t => t.PersonId == studentSign.PersonId).Scores.Count == 0)
                        {
                            Actions.printDateTimeAndStatus("student");
                            Console.WriteLine("\n\n\tYou Don't Have Any Score....");
                            Thread.Sleep(4000);
                            goto studentMenu;
                        }

                        Actions.printDateTimeAndStatus("student");
                        Console.WriteLine("Search Score");
                        Console.WriteLine("\n\n\t1.Search By Id");
                        Console.WriteLine("\n\n\t2.Search By Course Name");
                        int searchScoreAnswer;
                        while (!int.TryParse(Console.ReadLine(), out searchScoreAnswer))
                        {
                            Actions.printDateTimeAndStatus("student");
                            Actions.warningEnterNumber("This Question");
                            goto searchScore;
                        }

                        switch (searchScoreAnswer)
                        {
                            case 1:
                                Actions.printDateTimeAndStatus("student");
                                Console.Write("Enter Score Id : ");
                                int scoreId;
                                while (!int.TryParse(Console.ReadLine(), out scoreId))
                                {
                                    Actions.printDateTimeAndStatus("student");
                                    Actions.warningEnterNumber("Score Id");
                                    goto searchScore;
                                }
                                if (!dbSearchScore.Students.First(t => t.PersonId == studentSign.PersonId).Scores.Any(t => t.Id == scoreId))
                                {
                                    Actions.printDateTimeAndStatus("student");
                                    Actions.warningNotFound(scoreId, "Your Scores");
                                    goto studentMenu;
                                }
                                Console.WriteLine($"\n {dbSearchScore.Students.First(t => t.PersonId == studentSign.PersonId).Scores.First(t => t.Id == scoreId)}");
                                Actions.comeBackToMainMenu();
                                goto studentMenu;

                            case 2:
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine("\n\nYour Courses");
                                foreach (Course course in dbSearchScore.Students.First(t => t.PersonId == studentSign.PersonId).Courses)
                                {
                                    Console.WriteLine("  " + course);
                                    Console.WriteLine("  ___________________________________________________________________");
                                }
                                int courseId;
                                while (!int.TryParse(Console.ReadLine(), out courseId))
                                {
                                    Actions.printDateTimeAndStatus("student");
                                    Actions.warningEnterNumber("Menu");
                                    goto studentMenu;
                                }
                                if (!dbSearchScore.Students.First(t => t.PersonId == studentSign.PersonId).Courses.Any(t => t.Id == courseId))
                                {
                                    Actions.printDateTimeAndStatus("student");
                                    Actions.warningNotFound(courseId, "Your Course List");
                                    goto studentMenu;
                                }
                                Actions.printDateTimeAndStatus("student");
                                Console.WriteLine("\n\n");
                                foreach (Score score in dbSearchScore.Students.First(t => t.PersonId == studentSign.PersonId).Scores.Where(t => t.Course.Id == courseId).ToList())
                                {
                                    Console.WriteLine(" " + score);
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                                }
                                Actions.comeBackToMainMenu();
                                goto studentMenu;

                            default:
                                goto searchScore;
                        }
                    }
                #endregion

                #region Exit 
                case 6:
                    goto login;
                #endregion

                #region Default
                default:
                    goto SignIn_Student;
                    #endregion
            }
            #endregion

        }
    }
}

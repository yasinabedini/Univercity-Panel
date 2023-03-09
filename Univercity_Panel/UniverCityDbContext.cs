using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure.Annotations;


namespace Univercity_Panel
{
    class UniverCityDbContext : DbContext
    {
        public UniverCityDbContext(string name) : base(name) { }


        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Score> Scores { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Role>().ToTable("Table_Role");
           
            modelBuilder.Entity<Role>().Property(t => t.Name)
                                                .HasColumnAnnotation("index", new IndexAnnotation(new IndexAttribute("roleName_Unique") { IsUnique = true }))
                                                .HasMaxLength(30)
                                                .IsRequired()
                                                .HasColumnType("varchar");



            modelBuilder.Entity<Master>().ToTable("Table_Master");
            modelBuilder.Entity<Master>().HasKey(t => t.PersonId); ;
            



            modelBuilder.Entity<Employee>().ToTable("Table_Employee");
            modelBuilder.Entity<Employee>().HasKey(t => t.PersonId);



            modelBuilder.Entity<Student>().ToTable("Table_Student");
            modelBuilder.Entity<Student>().HasKey(t => t.PersonId);



            modelBuilder.Entity<Score>().ToTable("Table_Score");            


            modelBuilder.Properties().Where(t => t.Name.StartsWith("PersonId")).Configure(t => t.HasColumnName("Id"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("NationalCode")).Configure(t => t.HasMaxLength(10).IsRequired().HasColumnType("varchar"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("Name")).Configure(t => t.HasMaxLength(20).IsRequired().HasColumnType("varchar"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("Family")).Configure(t => t.HasMaxLength(30).IsRequired().HasColumnType("varchar"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("Mobile")).Configure(t => t.HasMaxLength(11).IsRequired().HasColumnType("varchar"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("Password")).Configure(t => t.HasMaxLength(30).IsRequired().HasColumnType("varchar"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("Degree")).Configure(t => t.HasMaxLength(20).IsRequired().HasColumnType("varchar"));
            modelBuilder.Properties().Where(t => t.Name.StartsWith("Salary")).Configure(t => t.IsRequired());
            modelBuilder.Properties().Where(t => t.Name.StartsWith("RegisterDate")).Configure(t => t.IsRequired());
            modelBuilder.Properties().Where(t => t.Name.StartsWith("BirthDate")).Configure(t => t.IsRequired());
            modelBuilder.Properties().Where(t => t.Name.StartsWith("IsActive")).Configure(t => t.IsRequired());



            modelBuilder.Entity<Course>().ToTable("Table_Course");

            modelBuilder.Entity<Course>().Property(t => t.Name)
                                                  .HasColumnType("varchar")
                                                  .IsRequired()
                                                  .HasMaxLength(20)
                                                  .HasColumnAnnotation("index", new IndexAnnotation(new IndexAttribute("Course_Unique") { IsUnique = true }));
            modelBuilder.Entity<Course>().Property(t => t.Name)
                                                 .HasColumnType("varchar")
                                                 .IsRequired()
                                                 .HasMaxLength(20);
            modelBuilder.Entity<Course>().Property(t => t.Unit)
                                                  .IsRequired();



            modelBuilder.Entity<Score>().Property(t => t.ScoreNumber)
                                                 .IsRequired();
            modelBuilder.Entity<Score>().Property(t => t.RegisterDate)
                                                 .IsRequired();
            modelBuilder.Entity<Score>().Property(t => t.IsActive)
                                                .IsRequired();






            //modelBuilder.Entity<Role>()
            //    .HasMany(t => t.Students)
            //    .WithRequired(t => t.Role);                
            //modelBuilder.Entity<Role>()
            //    .HasMany(t => t.Employees)
            //    .WithRequired(t => t.Role);
            //modelBuilder.Entity<Role>()
            //    .HasMany(t => t.Masters)
            //    .WithRequired(t => t.Role);                
            //modelBuilder.Entity<Course>()
            //    .HasOptional(t => t.Master)
            //    .WithMany(t => t.Courses);
                
            //modelBuilder.Entity<Course>()
            //   .HasMany(t => t.Students)
            //   .WithMany(t => t.Courses);

            //modelBuilder.Entity<Course>()
            //    .HasMany(t => t.Scores)
            //    .WithRequired(t => t.Course);

            //modelBuilder.Entity<Score>()
            //     .HasRequired(t => t.Student)
            //     .WithMany(t => t.Scores);

            //modelBuilder.Entity<Score>()
            //    .HasRequired(t => t.Registerar)
            //    .WithMany(t => t.RegisterScore);

            //modelBuilder.Entity<Score>()
            //    .HasOptional(t => t.Edit)
            //    .WithMany(t => t.EditScore);
                





            base.OnModelCreating(modelBuilder);
        }






    }
}

using AllProjects.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AllProjects.Repository
{
    public interface IStudentOperation
    {
        int CreateStudent(Student student);
        int UpdateStudent(int studentId, Student student_new);
        int DeleteStudent(int studentId);
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int id);
        IEnumerable<Student> GetStudentByName(string name);
        int Count { get; }
    }
    public class StudentOperation : IStudentOperation
    {
        private string? _name;
        public string Path_Project
        {
            get
            {
                return _name ?? "";
            }
            set
            {
                _name = Directory.GetCurrentDirectory();
            }
        }

        private IDbConnection _db = new SqliteConnection();
        private IEnumerable<Student>? students { get; set; }
        public StudentOperation()
        {
            _db = new SqliteConnection("Data Source=" + Path.Combine(Directory.GetCurrentDirectory().ToString(), "Database", "Students.db"));
            SetupAdministrator();
        }
        public int Count => _db.QuerySingleOrDefault<int>("select count(Id) from Student");

        public int CreateStudent(Student student)
        {
            SetupStudent();
            try
            {
                _db.Execute("insert into Student values(" +
                    "@Id," +
                    "@FullName," +
                    "@Birth," +
                    "@Email," +
                    "@IsDeleted," +
                    "@Password," +
                    "@StudentId," +
                    "@AdministratorId," +
                    "@Course," +
                    "@Role" +
                    "@Created)", new
                    {
                        Id = student.Id,
                        FullName = student.FullName,
                        Birth = student.Birth,
                        Email = student.Email,
                        IsDeleted = student.IsDeleted,
                        Password = student.Password,
                        StudentId = Guid.NewGuid(),
                        AdministratorId = student?.CreatedBy?.AdministratorId,
                        Course = student?.Course,
                        Role = student?.Role,
                        Created = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                    });
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void SetupStudent()
        {
            var table = _db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Student';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "Student")
                return;
            
            _db.Execute("create table Student(" +
                "Id int, " +
                "FullName varchar(50), " +
                "Birth varchar(20), " +
                "Email varchar(50), " +
                "IsDeleted int, " +
                "Password varchar(50), " +
                "StudentId varchar(50), " +
                "Course int, " +
                "Role  int, " +
                "foreign key (AdministratorId) references Administrator(AdministratorId), " +
                "Created varchar(20))");
        }
        public void SetupAdministrator()
        {
            var table = _db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Administrator';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "Administrator")
                return;
            _db.Execute("create table Administrator(" +
                "Id int, " +
                "FullName varchar(50), " +
                "Birth varchar(50), " +
                "Email varchar(50), " +
                "IsDeleted int, " +
                "Password varchar(50), " +
                "AdministratorId varchar(50), " +
                "Course int, " +
                "Role  int, " +
                "Created varchar(50))");
            string sql_command = "insert into Administrator values(" +
                "@Id," +
                "@FullName," +
                "@Birth," +
                "@Email," +
                "@IsDeleted," +
                "@Password," +
                "@AdministratorId," +
                "@Course," +
                "@Role," +
                "@Created);";
            _db.Execute(sql_command, new
            {
                Id = 1,
                FullName = "Abdunazarov Ro'zimurod Ro'zinazar o'g'li",
                Birth = "23.11.2003 11:45",
                Email = "Ruzimurodabdunazarov@gmail.com",
                IsDeleted = false,
                Password = "Sw0rdf!sh",
                AdministratorId = Guid.NewGuid(),
                Course = 1,
                Role = Role.Administrator,
                Created = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            });
        }


        public IEnumerable<Student> GetAllStudents()
        {
            return _db.Query<Student>("select * from Student");
        }

        public Student GetStudentById(int id)
        {
            return _db.QuerySingleOrDefault<Student>("select * from Student where Id=@Id", new { Id = id });
        }

        public IEnumerable<Student> GetStudentByName(string name)
        {
            return _db.Query<Student>("select * from Student where FullName like @FullName", new { FullName = name });
        }

        public int UpdateStudent(int studentId, Student student_new)
        {
            try
            {
                string sql_command = "" +
                    "update Student set " +
                    "FullName = @FullName," +
                    "Birth = @Birth," +
                    "Id = @Id," +
                    "Email = @Email," +
                    "IsDeleted = @IsDeleted," +
                    "Password = @Password," +
                    "AdministratorId = @AdministratorId," +
                    "Course = @Course," +
                    "Role = @Role" +
                    "Created = @Created" +
                    "where StudentId=@StudentId";
                _db.Execute(sql_command, new
                {
                    Id = student_new?.Id,
                    FullName = student_new?.FullName,
                    Birth = student_new?.Birth,
                    Email = student_new?.Email,
                    IsDeleted = student_new?.IsDeleted,
                    Password = student_new?.Password,
                    AdministratorId = student_new?.CreatedBy?.AdministratorId,
                    Course = student_new?.Course,
                    Role = student_new?.Role,
                    Created = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                    StudentId = student_new?.StudentId,
                });
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DeleteStudent(int studentId)
        {
            try
            {
                _db.Execute("delete from Stuedent where @StudentId=StudentId", new { StudentId = studentId });

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

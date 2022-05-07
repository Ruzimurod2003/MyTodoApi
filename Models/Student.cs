namespace AllProjects.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime Birth { get; set; }
        public string? Email { get; set; }
        public bool IsDeleted { get; set; }
        public string? Password { get; set; }
        public string? StudentId { get; set; } 
        public int Course { get; set; }
        public Role Role { get; set; } = Role.Student;
        public Administrator? CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}

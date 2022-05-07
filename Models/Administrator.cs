namespace AllProjects.Models
{
    public class Administrator
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime Birth { get; set; }
        public string? Email { get; set; }
        public bool IsDeleted { get; set; }
        public string? Password { get; set; }
        public string? AdministratorId { get; set; }
        public Role Role { get; set; } = Role.Administrator;
        public DateTime Created { get; set; }
        public static string GetAdministratorId() { return AdministratorId ?? ""; }
    }
    public enum Role
    {
        Administrator,
        CreateTest,
        Student,
        Checker
    }
}

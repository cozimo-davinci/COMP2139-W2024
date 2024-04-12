namespace COMP2139_Lab1.Models.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public bool IsSuperAdmin { get; set; } = true;
        public bool IsAdmin { get; set; } = false;
    }
}

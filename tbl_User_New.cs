namespace InvterViewTest
{
    public class tbl_User_New
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Department { get; set; }
        public int MgrId { get; set; }
        public string Seniority { get; set; }
        public string EmpCode { get; set; }
        public string Role { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime DOJ { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public List<tbl_User_New> tbl_Userslst { get; set; }

    }
}

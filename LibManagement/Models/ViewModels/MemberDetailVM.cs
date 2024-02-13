namespace LibManagement.Models.ViewModels
{
    public class MemberDetailVM
    {
        public string MemberID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public double Fine { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public bool Available { get; set; }
        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [DisplayName("Member Id")]
        public string? MemberID { get; set; }
        //public Member Member { get; set; }

    }
}

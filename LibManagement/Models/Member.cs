using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace LibManagement.Models
{
    //TO DO: allow member to see self
    public class Member: IdentityUser
    {
        [DisplayName("Member ID")]
        public string MemberID { get; set; } //TO DO: generate member id
        [DisplayName("Full Name")]
        public string FullName { get; set; } 
    }
}

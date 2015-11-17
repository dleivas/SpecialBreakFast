using System.ComponentModel.DataAnnotations;


namespace SpecialBreakFast.Models
{
   
        public class ManageRoleViewModel
        {
        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
            public string UserName { get; set; }

        }

    
}
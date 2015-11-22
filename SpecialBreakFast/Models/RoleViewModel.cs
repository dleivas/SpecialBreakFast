using System.ComponentModel.DataAnnotations;


namespace SpecialBreakFast.Models
{
   
        public class RoleAddViewModel
        {
        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
            public string UserName { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }


    }

    public class GetRolesViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


    }


}
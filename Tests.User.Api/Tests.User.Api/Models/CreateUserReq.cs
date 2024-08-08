using System.ComponentModel.DataAnnotations;

namespace Tests.User.Api.Models
{
    public class CreateUserReq
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }
    }
}

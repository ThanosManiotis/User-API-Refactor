using System.ComponentModel.DataAnnotations;

namespace Tests.User.Api.Models
{
    public class UpdateUserReq : CreateUserReq
    {

        [Required]
        public int Id { get; set; }
    }
}

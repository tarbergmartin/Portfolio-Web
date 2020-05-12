using System.ComponentModel.DataAnnotations;

namespace Portfolio.Shared.DataModels
{
    public class SubmissionModel
    {
        [StringLength(75, ErrorMessage = "Your name is too long")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [StringLength(75, ErrorMessage = "Your email address is too long")]
        [Required(ErrorMessage = "Please provide an email address")]
        [EmailAddress(ErrorMessage = "Please provie a valid email address")]
        public string Email { get; set; }

        [StringLength(750, ErrorMessage = "Your message can only be 750 characters long")]
        [Required(ErrorMessage = "Please provide a message")]
        public string Message { get; set; }

        public string Recipient { get; set; }
    }
}

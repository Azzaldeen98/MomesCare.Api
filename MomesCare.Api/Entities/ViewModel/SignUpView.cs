using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel
{
    public class SignUpView {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

     
        public string DisplayName => $"{FirstName} {LastName}";


        public string Password { get; set; }

    }
}

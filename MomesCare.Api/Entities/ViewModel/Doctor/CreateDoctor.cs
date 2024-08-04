using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Doctor
{
    public class DoctorCreate
    {
        [Required]
        public string Descript { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Doctor
{
    public class DoctorUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Descript { get; set; }
    }

}

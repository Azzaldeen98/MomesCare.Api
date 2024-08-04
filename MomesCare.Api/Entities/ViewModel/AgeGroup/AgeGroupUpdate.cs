using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.AgeGroup
{
    public class AgeGroupUpdate : BaseAgeGroup
    {
        [Required]
        public int id { get; set; }

      
    }

}

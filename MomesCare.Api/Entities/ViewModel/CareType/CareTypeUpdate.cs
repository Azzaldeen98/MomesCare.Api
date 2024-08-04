using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.CareType
{
    public class CareTypeUpdate : BaseCareType
    {
        [Required]
        public int id { get; set; }

    }

}

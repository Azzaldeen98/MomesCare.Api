using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Baby
{
    public class BabyUpdate : BaseBaby
    {
        [Required]
        public int Id { get; set; }

    }


}

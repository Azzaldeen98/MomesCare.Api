using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Baby
{
    public class BabyIndex : BaseBaby
    {
        [Required]
        public int Id { get; set; }

        public int age { get; set; }

    }


}

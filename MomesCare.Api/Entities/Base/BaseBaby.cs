using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Base
{
    public class BaseBaby
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float Height { get; set; }
        [Required]
        public float Weight { get; set; }
        public string NumberOfResponsibleDoctor { get; set; }
        [AllowNull]
        public string urlImage { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
    

    }

}

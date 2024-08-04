using MomesCare.Api.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Base
{
    public class BaseCourseItem
    {

        [Required]
        public string title { get; set; }

        [AllowNull]
        public string descript { get; set; }

        [AllowNull]
        public string? url { get; set; }

        [Required]
        public MediaTypes mediaType { get; set; }
    }

}

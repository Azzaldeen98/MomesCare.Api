using MomesCare.Api.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Post
{
    public class PostCreate 
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
    }

}

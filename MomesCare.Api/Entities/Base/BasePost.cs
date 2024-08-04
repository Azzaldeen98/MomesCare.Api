using System.Diagnostics.CodeAnalysis;

namespace MomesCare.Api.Entities.Base
{
    public class BasePost
    {

       
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishedAt { get; set; }

    }
}

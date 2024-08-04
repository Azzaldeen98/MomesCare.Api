using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.Baby;
using System.ComponentModel.DataAnnotations;

namespace MomesCare.Api.Entities.ViewModel.Profile
{
    public class ProfileIndex
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public int posts { get; set; }

        // عدد الإعجابات من قبل المستخدمون الآخرون
        public int LikesReceived { get; set; }

        // عدد الإعجابات التي قمت بها على منشورات الآخرين
        public int LikesGiven { get; set; }

        public int comments{ get; set; }

        public List<BabyIndex> Babies { get; set; }

    }
}

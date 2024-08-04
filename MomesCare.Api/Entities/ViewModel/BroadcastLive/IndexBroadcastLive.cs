using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Entities.ViewModel.BroadcastLive
{
    public class IndexBroadcastLive : BaseBroadcastLive
    {
        public int id { get; set; }
        public string link { get; set; }

        public List<IndexJoinBroadcastLive> liveStreamJoiners { get; set; }

        public UserView user { get; set; }
    }

}

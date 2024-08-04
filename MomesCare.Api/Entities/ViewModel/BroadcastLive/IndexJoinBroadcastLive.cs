using MomesCare.Api.Entities.Base;

namespace MomesCare.Api.Entities.ViewModel.BroadcastLive
{



    public class IndexJoinBroadcastLive : BaseJoinBroadcastLive
    {

        public int Id { get; set; }

        public IndexBroadcastLive indexBroadcastLive { get; set; }

        public UserView user { get; set; }

    }

}

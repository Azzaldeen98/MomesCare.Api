using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;

namespace MomesCare.Api.Entities.ViewModel.CareType
{
    public class CareTypeIndex : BaseCareType
    {
        public int id { get; set; }

        public List<DailyCareTimesIndex> dailyCareTimes { get; set; }
    }

}

using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;

namespace MomesCare.Api.Entities.ViewModel.AgeGroup
{
    public class AgeGroupIndex : BaseAgeGroup
    {
        public int id { get; set; }

        public List<DailyCareTimesIndex> dailyCareTimes { get; set; }
    }

}

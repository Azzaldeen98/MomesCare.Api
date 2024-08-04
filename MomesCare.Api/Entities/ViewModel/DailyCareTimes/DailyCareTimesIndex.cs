using MomesCare.Api.Entities.Base;
using MomesCare.Api.Entities.ViewModel.AgeGroup;
using MomesCare.Api.Entities.ViewModel.CareType;

namespace MomesCare.Api.Entities.ViewModel.DailyCareTimes
{
    public class DailyCareTimesIndex : BaseDailyCareTimes
    {
        public int id { get; set; }

        public CareTypeIndex careType { get; set; }

        public AgeGroupIndex ageGroup { get; set; }
    }
}

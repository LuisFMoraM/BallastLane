using AutoMapper;
using BusinessLogic.Models;

namespace MedicationApi.MapperProfiles
{
    /// <summary>
    /// AutoMapper profile that allows generating 
    /// new instances of DataAccess.Entity.Medication or
    /// Models.Medication
    /// </summary>
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<Medication, DataAccess.Entities.Medication>().ReverseMap();
        }
    }
}

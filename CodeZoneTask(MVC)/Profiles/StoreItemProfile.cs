using AutoMapper;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;

namespace CodeZoneTask_MVC_.Profiles
{
   
    public class StoreItemProfile : Profile
    {
        public StoreItemProfile()
        {
            CreateMap<StoreItem, StockIncreaseViewModel>().ReverseMap();
        }
    }
}

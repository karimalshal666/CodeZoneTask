using AutoMapper;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;

namespace CodeZoneTask_MVC_.Profiles
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Store, StoreViewModel>().ReverseMap();
        }
    }
}
using AutoMapper;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;

namespace CodeZoneTask_MVC_.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemViewModel>().ReverseMap();
        }
    }
}
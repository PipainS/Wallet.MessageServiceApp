using AutoMapper;
using MessageService.API.Constants;
using MessageService.API.Models;
using MessageService.API.Models.DTOs;

namespace MessageService.API.MappingProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.UserName) ? MessageConstants.DefaultUserName : src.UserName))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src =>
                    src.Timestamp.ToString(MessageConstants.TimestampFormat))); 
        }
    }
}

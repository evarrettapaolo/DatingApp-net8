using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<AppUser, MemberDto>()
        .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
        .ForMember(x => x.PhotoUrl, 
          o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
      CreateMap<Photo, PhotoDto>();
      CreateMap<MemberUpdateDto, AppUser>();
      CreateMap<RegisterDto, AppUser>();
      CreateMap<string, DateOnly>().ConvertUsing(d => DateOnly.Parse(d)); // * convert string to DateOnly
      CreateMap<Message, MessageDto>()
        .ForMember(x => x.SenderPhotoUrl, 
          o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
        .ForMember(x => x.RecipientPhotoUrl, 
          o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));

    }
  }
}
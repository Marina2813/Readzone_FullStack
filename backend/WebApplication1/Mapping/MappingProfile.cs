using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Mapping
{
    public class MappingProfile : Profile
    { 
        public MappingProfile()
        {
            
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.IsOwner, opt => opt.Ignore());


            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<User, UserDto>();

            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : "Unknown"))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category ?? "Anonymous"));

            CreateMap<CreatePostDto, Post>();



            //CreateMap<LoginDto, LoginRequest>();
            //CreateMap<ResetPasswordDto, User>(); 
        }

    }
}

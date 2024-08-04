using AutoMapper;

using Google.Apis.Util;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.ViewModel.AgeGroup;
using MomesCare.Api.Entities.ViewModel.Baby;
using MomesCare.Api.Entities.ViewModel.BroadcastLive;
using MomesCare.Api.Entities.ViewModel.CareType;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Courses;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;
using MomesCare.Api.Entities.ViewModel.Doctor;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Entities.ViewModel.Profile;
using System.Linq;
using Profile = MomesCare.Api.Entities.Models.Profile;

namespace MomesCare.Api.Helpers
{
    public class MappingConfig : AutoMapper.Profile
    {
        public MappingConfig()
        {





            /// Post
            CreateMap<PostCreate, Post>().ReverseMap();
            CreateMap<PostUpdate, Post>().ReverseMap();
            CreateMap<Post, PostIndex>()
                .ForMember(dest => dest.likes, opt => opt.MapFrom(src => src.likes != null ? src.likes.Count : 0))
            .ReverseMap();


            /// Comment
            CreateMap<Comment, CommentIndex>()
                    .ForMember(dest => dest.likes, opt => opt.MapFrom(src => src.likes != null ? src.likes.Count : 0))
            .ReverseMap();

            CreateMap<CommentCreate, Comment>().ReverseMap();
            CreateMap<CommentUpdate, Comment>().ReverseMap();


            /// Baby
            CreateMap<Baby, BabyIndex>()
                 .ForMember(dest => dest.age, opt => opt.MapFrom(src => Helper.CalculateAge(src.BirthDay)))
            .ReverseMap();
            CreateMap<BabyCreate, Baby>().ReverseMap();
            CreateMap<BabyUpdate, Baby>().ReverseMap();

            /// Profile
            CreateMap<Profile, ProfileIndex>().ReverseMap();
            CreateMap<ProfileCreate, Profile>().ReverseMap();
            CreateMap<ProfileUpdate, Profile>().ReverseMap();

            /// ApplicationUser & ProfileIndex
            CreateMap<ApplicationUser, ProfileIndex>().ReverseMap();


            /// Course
            CreateMap<Course, CourseIndex>().ReverseMap();
            CreateMap<CreateCourse, Course>().ReverseMap();
            CreateMap<UpdateCourse, Course>().ReverseMap();


            /// CourseItem
            CreateMap<CourseItem, CourseItemIndex>().ReverseMap();
            CreateMap<CreateCourseItem, CourseItem>().ReverseMap();
            CreateMap<UpdateCourseItem, CourseItem>().ReverseMap();

            /// CourseItem
            CreateMap<Doctor, DoctorIndex>().ReverseMap();
            CreateMap<DoctorCreate, Doctor>().ReverseMap();
            CreateMap<DoctorUpdate, Doctor>().ReverseMap();



            /// BroadcastLive
            CreateMap<BroadcastLive, IndexBroadcastLive>()
                  .ForMember(dest => dest.user, opt => opt.Ignore())
                  //.ForMember(dest => dest.descript, opt => opt.MapFrom(src => src.descript))
                  //.ForMember(dest => dest.link, opt => opt.MapFrom(src => src.link))
            .ReverseMap();

           CreateMap<CreateBroadcastLive, BroadcastLive>().ReverseMap();
            //ForMember(dest => dest.descript, opt => opt.MapFrom(src => $"{src.descript} {src.link}"))
            CreateMap<UpdateBroadcastLive, BroadcastLive>()
                   //.ForMember(dest => dest.descript, opt => opt.MapFrom(src => $"{src.descript} {src.link}"))
                .ReverseMap();   
            
            /// JoinBroadcastLive
            CreateMap<JoinBroadcastLive, IndexJoinBroadcastLive>()
                     .ForMember(dest => dest.user, opt => opt.Ignore())
            .ReverseMap();   
            CreateMap<CreateBroadcastLive, JoinBroadcastLive>().ReverseMap();
            CreateMap<UpdateBroadcastLive, JoinBroadcastLive>().ReverseMap();


            /// FCMToken
            CreateMap<FCMToken, CloudMessagingToken>().ReverseMap();

            /// DailyCareTimes 
            CreateMap<DailyCareTimes, DailyCareTimesIndex>()
               .ForMember(dest => dest.ageGroup, opt => opt.Ignore())
               .ForMember(dest => dest.careType, opt => opt.Ignore())
            .ReverseMap();
            CreateMap<DailyCareTimesCreate, DailyCareTimes>()
                .ForMember(dest => dest.time, opt => opt.MapFrom(src => Helper.ConvertToTimeSpan(src.time)))
                .ReverseMap();
            CreateMap<DailyCareTimesUpdate, DailyCareTimes>()
                   .ForMember(dest => dest.time, opt => opt.MapFrom(src => Helper.ConvertToTimeSpan(src.time)))
                   .ReverseMap();


            /// CareType 
            CreateMap<CareType, CareTypeIndex>()
               .ForMember(dest => dest.dailyCareTimes, opt => opt.Ignore())
            .ReverseMap();
            CreateMap<CareTypeCreate, CareType>().ReverseMap();
            CreateMap<CareTypeUpdate, CareType>().ReverseMap();

            /// AgeGroup 
            CreateMap<AgeGroup, AgeGroupIndex>()
               .ForMember(dest => dest.dailyCareTimes, opt => opt.Ignore())
            .ReverseMap();
            CreateMap<AgeGroupCreate, AgeGroup>().ReverseMap();
            CreateMap<AgeGroupUpdate, AgeGroup>().ReverseMap();
        }
    }
}
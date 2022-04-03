using AutoMapper;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Entity.Model;

namespace Training.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TRAINING, CreateTrainingRequestDto>();
            CreateMap<CreateTrainingRequestDto, TRAINING>();
            CreateMap<TRAINING, TrainingsDto>();
            CreateMap<TrainingsDto, TRAINING>();
            CreateMap<TRAINING, UpdateTrainingRequestDto>();
            CreateMap<UpdateTrainingRequestDto, TRAINING>();
            CreateMap<TrainingListDto, TRAINING>();
            CreateMap<TRAINING, TrainingListDto>();
        }
    }
}

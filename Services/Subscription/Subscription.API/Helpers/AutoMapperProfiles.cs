using AutoMapper;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Entity.Model;

namespace Subscription.API.Helpers
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
            CreateMap<CreateSubscriptionRequestDto, SUBSCRIPTION>();
            CreateMap<SUBSCRIPTION, CreateSubscriptionRequestDto>();
            CreateMap<SUBSCRIPTION, SubscriptionsDto>();
            CreateMap<SubscriptionsDto, SUBSCRIPTION>();
            CreateMap<SubscriptionDetailDto, SubscriptionDetailMappingDto>();
            CreateMap<SubscriptionDetailMappingDto, SubscriptionDetailDto>();
        }
    }
}

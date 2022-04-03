using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.BusinessFlow.Subscription.Factory;
using CourseSubscription.Business.Operator;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using CourseSubscription.Entity.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Subscription
{
    public class SubscriptionInsertFlow : BusinessFlowBase<object, object>
    {
        private readonly ITrainingsService _trainingsService;
        private readonly ICoursesServices _coursesServices;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private UsersDto users;
        private string token;
        private CreateSubscriptionRequestDto createSubscriptionRequest;
        private static SubscriptionsDto createSubscriptionResponse;
        private static TRAINING trainingEntity;

        public SubscriptionInsertFlow(IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory, ISubscriptionService subscriptionService, ITrainingsService trainingsService, ICoursesServices coursesServices)
        {
            _subscriptionService = subscriptionService;
            _trainingsService = trainingsService;
            _coursesServices = coursesServices;
            _configuration = configuration;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            createSubscriptionResponse = new();
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest(req);
            await CheckUser(); // Check if user exists, active or not
            CheckTraining(); // Check if training exists
            CheckSubscriptionsForUser(); // Check if user has another subscription in same month
            InsertSubscription();

            return createSubscriptionResponse;
        }

        #region Helpers

        protected void PrepareRequest(object req)
        {
            createSubscriptionRequest = (CreateSubscriptionRequestDto)req;
            token = _configuration["GoRestApi:Token"];
        }

        protected async Task CheckUser()
        {
            var usersOutput = await GoRestApiFactory.UserServices(_httpClientFactory).GetUserById(token, createSubscriptionRequest.USR_AUTO_KEY.ToString());
            users = JsonConvert.DeserializeObject<UsersDto>(usersOutput);
            if (users == null)
                throw new ServiceException(404, SubscriptionConstants.UsersNotFound, null);

            if (!users.STATUS.Equals("active"))
                throw new ServiceException(404, SubscriptionConstants.UserIsNotActive, null);

        }

        protected void CheckTraining()
        {
            trainingEntity = new TRAINING();
            trainingEntity = _trainingsService.GetTrainingById(createSubscriptionRequest.TRA_AUTO_KEY);
            if (trainingEntity == null)
                throw new ServiceException(404, TrainingConstants.TrainingNotFound, null);

        }

        protected void CheckSubscriptionsForUser()
        {
            FihristOperator op = new FihristOperator();
            List<Sp_UserSubscription> entity = op.GetUserSubscriptions(createSubscriptionRequest.USR_AUTO_KEY);

            if (entity.Any(x => x.MONTH == trainingEntity.MONTH))
                throw new ServiceException(404, SubscriptionConstants.UserCanNotSubscribe, null);

        }

        protected void InsertSubscription()
        {
            var entity = _mapper.Map<SUBSCRIPTION>(createSubscriptionRequest);
            var result = _subscriptionService.CreateSubscription(entity);
            createSubscriptionResponse = _mapper.Map<SubscriptionsDto>(result);
        }

        #endregion
    }
}

using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.Operator;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Subscription
{
    public class SubscriptionListFlow : BusinessFlowBase<object, object>
    {
        private readonly ITrainingsService _trainingsService;
        private readonly ICoursesServices _coursesServices;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private SubscriptionFilterDto subscriptionListRequest;
        private PagedList<SubscriptionListDto> subscriptionListResponse;
        private PagedList<SubscriptionDetailDto> subscriptionDetailsResponse;

        public SubscriptionListFlow(IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory, ISubscriptionService subscriptionService, ITrainingsService trainingsService, ICoursesServices coursesServices)
        {
            _subscriptionService = subscriptionService;
            _trainingsService = trainingsService;
            _coursesServices = coursesServices;
            _configuration = configuration;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;            
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest(req);
            GetSubscriptions();

            return subscriptionListResponse;
        }

        #region Helpers

        protected void PrepareRequest(object req)
        {
            subscriptionListRequest = (SubscriptionFilterDto)req;
        }

        protected void GetSubscriptions()
        {
            FihristOperator op = new FihristOperator();
            subscriptionListResponse = op.GetUserSubscriptions(subscriptionListRequest);

            if (subscriptionListResponse.Count < 1)
                throw new ServiceException(404, SubscriptionConstants.SubscriptionsNotFound, null);

        }
        
        protected void GetSubscriptionsDetails()
        {
            FihristOperator op = new FihristOperator();
            subscriptionListResponse = op.GetUserSubscriptions(subscriptionListRequest);

            if (subscriptionListResponse.Count < 1)
                throw new ServiceException(404, SubscriptionConstants.SubscriptionsNotFound, null);

        }

        #endregion
    }
}

using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.BusinessFlow.Subscription.Factory;
using CourseSubscription.Business.Operator;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Subscription
{
    public class SubscriptionsDetailsFlow : BusinessFlowBase<object, object>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly ISubscriptionService _subscriptionService;
        private SubscriptionDetailFilterDto subscriptionDetailsRequest;
        private string token;
        private PagedList<SubscriptionDetailDto> subscriptionDetailsResponse;

        public SubscriptionsDetailsFlow(IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory, ISubscriptionService subscriptionService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _subscriptionService = subscriptionService;
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest(req);
            GetSubscriptionsDetails();

            return subscriptionDetailsResponse;
        }

        #region Helpers

        protected void PrepareRequest(object req)
        {
            subscriptionDetailsRequest = (SubscriptionDetailFilterDto)req;
            token = _configuration["GoRestApi:Token"];
        }

        protected void GetSubscriptionsDetails()
        {
            var subs = _subscriptionService.GetSubscriptions();

            if (subs != null && subs.Count > 0)
            {
                List<SubscriptionWithUserDto> filledSubsList = new List<SubscriptionWithUserDto>();
                foreach (var item in subs)
                {
                    SubscriptionWithUserDto addItem = new SubscriptionWithUserDto();

                    var user = GetUserById(item.USR_AUTO_KEY);
                    if (user != null)
                    {
                        addItem.SUB_AUTO_KEY = item.SUB_AUTO_KEY;
                        addItem.TRA_AUTO_KEY = item.TRA_AUTO_KEY;
                        addItem.USR_AUTO_KEY = item.USR_AUTO_KEY;
                        addItem.USER_EMAIL = user.Result.EMAIL;
                        addItem.USER_GENDER = user.Result.GENDER;
                        addItem.USER_NAME = user.Result.NAME;
                        filledSubsList.Add(addItem);
                    }
                }

                FihristOperator op = new FihristOperator();
                subscriptionDetailsResponse = op.GetSubscriptionsDetails(subscriptionDetailsRequest, filledSubsList);

                if (subscriptionDetailsResponse.Count < 1)
                    throw new ServiceException(404, SubscriptionConstants.SubscriptionsNotFound, null);
            }

        }

        protected async Task<UsersDto> GetUserById(decimal usr_auto_key)
        {
            var response = await GoRestApiFactory.UserServices(_httpClientFactory).GetUserById(token, usr_auto_key.ToString());
            var userResponse = JsonConvert.DeserializeObject<UsersDto>(response);

            return userResponse;
        }

        #endregion
    }

}

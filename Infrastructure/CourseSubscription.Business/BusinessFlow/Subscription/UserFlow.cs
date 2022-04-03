using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.BusinessFlow.Subscription.Factory;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using CourseSubscription.Entity.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Subscription
{
    public class UserFlow : BusinessFlowBase<object, object>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private string token;
        private List<UsersDto> usersResponse;

        public UserFlow(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            usersResponse = new();
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest();
            await GetUsers();

            return usersResponse;
        }

        #region Helpers

        protected void PrepareRequest()
        {
            token = _configuration["GoRestApi:Token"];
        }

        protected async Task GetUsers()
        {
            var response = await GoRestApiFactory.UserServices(_httpClientFactory).GetUsers(token);
            usersResponse = JsonConvert.DeserializeObject<List<UsersDto>>(response);
            if (usersResponse == null)
                throw new ServiceException(404, SubscriptionConstants.UsersNotFound, null);
        }

        #endregion
    }
}

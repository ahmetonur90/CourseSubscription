using CourseSubscription.Business.BusinessFlow.Subscription.Factory.v1;
using System.Net.Http;

namespace CourseSubscription.Business.BusinessFlow.Subscription.Factory
{
    public class GoRestApiFactory
    {
        static readonly object _lockUserServices = new object();
        private static UserServices _userService;

        public static UserServices UserServices(IHttpClientFactory httpClientFactory)
        {
            if (_userService == null)
            {
                lock (_lockUserServices)
                {
                    if (_userService == null)
                        _userService = new UserServices(httpClientFactory);
                }
            }
            else
            {
                lock (_lockUserServices)
                {
                    _userService = new UserServices(httpClientFactory);
                }
            }
            return _userService;
        }
    }
}

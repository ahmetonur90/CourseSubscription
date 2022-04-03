using CourseSubscription.Business.BusinessFlow.Subscription.Factory.v1.Abstract;
using CourseSubscription.Core.Util.Result.ServiceException;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Subscription.Factory.v1
{
    public class UserServices : IUserServices
    {
        #region Define
        private readonly IHttpClientFactory _httpClientFactory;
        private ServiceExceptionFactoryForApi _serviceExceptionFactoryForApi = (name, response) => null;

        public UserServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serviceExceptionFactoryForApi = ServiceDefaultException.ExceptionFactoryForApi;
        }

        private static readonly Random Jitterer = new();

        private static readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy =
            Policy.HandleResult<HttpResponseMessage>(
                message => ((int)message.StatusCode) == 429 || (int)message.StatusCode > 500)
            .WaitAndRetryAsync(2, retryAttempt =>
            {
#if DEBUG
                Debug.WriteLine($"Retrying because of transient error. Attempt {retryAttempt}");
#endif
                return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                + TimeSpan.FromMilliseconds(Jitterer.Next(0, 1000));
            });

        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy =
            Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 503)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        private readonly AsyncPolicyWrap<HttpResponseMessage> _resilientPolicy =
            CircuitBreakerPolicy.WrapAsync(TransientErrorRetryPolicy);

        private ServiceExceptionFactoryForApi ServiceException
        {
            get
            {
                if (_serviceExceptionFactoryForApi != null && _serviceExceptionFactoryForApi.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _serviceExceptionFactoryForApi;
            }
            set { _serviceExceptionFactoryForApi = value; }
        }

        #endregion

        /// <summary>
        /// The users are not internal to the association, we will need to consume a third-party service Go REST API
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetUsers(string token)
        {
            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
                throw new Exception("Service is currently unavailable");

            var httpClient = _httpClientFactory.CreateClient("GoRestApi");
            httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {token}");
            var response = await _resilientPolicy.ExecuteAsync(() =>
                                  httpClient.GetAsync("users"));

            if (!response.IsSuccessStatusCode)
                if (ServiceException != null)
                {
                    Exception exception = ServiceException("GetUsers for GoRestApi", response);
                    if (exception != null) throw exception;
                }

            var responseText = await response.Content.ReadAsStringAsync();
            return responseText;
        }

        /// <summary>
        /// Gets user detail by user id via Go REST API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<string> GetUserById(string token, string req)
        {
            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
                throw new Exception("Service is currently unavailable");

            var httpClient = _httpClientFactory.CreateClient("GoRestApi");
            httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {token}");
            var response = await _resilientPolicy.ExecuteAsync(() =>
                                  httpClient.GetAsync($"users/{req}"));

            if (!response.IsSuccessStatusCode)
                if (ServiceException != null)
                {
                    Exception exception = ServiceException("GetUserById for GoRestApi", response);
                    if (exception != null) throw exception;
                }

            var responseText = await response.Content.ReadAsStringAsync();
            return responseText;
        }
    }
}

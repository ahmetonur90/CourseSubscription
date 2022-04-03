using CourseSubscription.Core.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Web;
using Xunit;
using Xunit.Abstractions;

namespace Subscription.Test
{
    public class SubscriptionListTest
    {
        private readonly ITestOutputHelper output;

        public SubscriptionListTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        // The user information in the database should consist of up-to-date information from the API.
        [Fact]
        public async void GetSubscriptionList_FilterTest()
        {
            //Arrange
            var client = new TestClientProvider().Client;

            SubscriptionDetailFilterDto req = new SubscriptionDetailFilterDto()
            {
                CourseName = ".Net Web API",
                Month = "March",
                PageNumber = 1,
                PageSize = 10,
                TrainingName = "Web API Spring Session",
                UserName = ""
            };

            var query = ToQueryString(req);

            // Act
            var response = await client.GetAsync($"api/subscription/v1/operations/getsubscriptiondetails?{query}");
            var parsed = JToken.Parse(await response.Content.ReadAsStringAsync());

            output.WriteLine("*** {0}", parsed);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void GetSubscriptionList_PaginationTest()
        {
            //Arrange
            var client = new TestClientProvider().Client;

            SubscriptionDetailFilterDto req = new SubscriptionDetailFilterDto()
            {
                CourseName = "",
                Month = "",
                PageNumber = 1,
                PageSize = 1,
                TrainingName = "Web API",
                UserName = ""
            };

            var query = ToQueryString(req);

            // Act
            var response = await client.GetAsync($"api/subscription/v1/operations/getsubscriptiondetails?{query}");
            var responseStr = await response.Content.ReadAsStringAsync();
            var parsed = JToken.Parse(responseStr);
            var userResponse = JsonConvert.DeserializeObject<ObjectActionResultModel>(responseStr);

            output.WriteLine("*** {0}", parsed);

            // Assert
            Assert.NotNull(userResponse.data);
        }

        #region Helpers

        public static string ToQueryString(object request, string separator = ",")
        {
            if (request == null)
                return "";

            // Get all properties on the object
            var properties = request.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(request, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return HttpUtility.UrlDecode(string.Join("&", properties.Select(x => string.Concat(Uri.EscapeDataString(x.Key), "=", Uri.EscapeDataString(x.Value.ToString())))));
        }

        #endregion
    }
}

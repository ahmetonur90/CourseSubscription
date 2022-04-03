using System.Net.Mime;
using System.Text.Json;

namespace CourseSubscription.Core.Util.Result.ServiceException
{
    public class ServiceDefaultException
    {
        /// <summary>
        ///  Derived for use in services using Api
        /// </summary>
        public static readonly ServiceExceptionFactoryForApi ExceptionFactoryForApi = (methodName, response) =>
        {
            var status = (int)response.StatusCode;
            if (status >= 400)
            {
                if (response.Content.Headers.ContentType.MediaType.Equals(MediaTypeNames.Application.Json))
                    return new ServiceException(
                        errorCode: status,
                        message: string.Format("Error calling {0}: {1}", methodName, response.ReasonPhrase),
                        errorContent: JsonSerializer.Deserialize<object>(response.Content.ReadAsStringAsync().Result));
                else
                    return new ServiceException(
                        errorCode: status,
                        message: string.Format("Error calling {0}: {1}", methodName, response.ReasonPhrase),
                        errorContent: response.Content.ReadAsStringAsync().Result);
            }
            return null;
        };

        /// <summary>
        ///  Derived for posible Soap services in the future
        /// </summary>
        public static readonly ServiceExceptionFactoryForSoap ExceptionFactoryForSoap = (methodName, errorContent) =>
        {

            return new ServiceException(
                errorCode: 400,
                message: string.Format("Error calling {0}: {1}", methodName, ""),
                errorContent: errorContent);
        };
    }

}

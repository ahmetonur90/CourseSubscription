using System;
using System.Net.Http;

namespace CourseSubscription.Core.Util.Result.ServiceException
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    public delegate Exception ServiceExceptionFactoryForApi(string methodName, HttpResponseMessage response);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="errorContent"></param>
    /// <returns></returns>
    public delegate Exception ServiceExceptionFactoryForSoap(string methodName, dynamic errorContent);
}

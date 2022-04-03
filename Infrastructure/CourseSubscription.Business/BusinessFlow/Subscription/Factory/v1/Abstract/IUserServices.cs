using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Subscription.Factory.v1.Abstract
{
    interface IUserServices
    {
        Task<string> GetUsers(string token);
        Task<string> GetUserById(string token, string req);
    }
}

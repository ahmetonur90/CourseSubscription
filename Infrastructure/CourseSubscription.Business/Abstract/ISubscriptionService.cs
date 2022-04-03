using CourseSubscription.Entity.Model;
using System.Collections.Generic;

namespace CourseSubscription.Business.Abstract
{
    public interface ISubscriptionService
    {
        SUBSCRIPTION CreateSubscription(SUBSCRIPTION entity);
        List<SUBSCRIPTION> GetSubscriptions();
        
    }
}

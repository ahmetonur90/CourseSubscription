using CourseSubscription.Business.Abstract;
using CourseSubscription.Data.Abstract;
using CourseSubscription.Entity.Model;
using System.Collections.Generic;

namespace CourseSubscription.Business.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public SubscriptionService(ISubscriptionsRepository subscriptionsRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
        }

        public SUBSCRIPTION CreateSubscription(SUBSCRIPTION entity)
        {
            return _subscriptionsRepository.Add(entity);
        }

        public List<SUBSCRIPTION> GetSubscriptions()
        {
            return _subscriptionsRepository.GetList();
        }
    }
}

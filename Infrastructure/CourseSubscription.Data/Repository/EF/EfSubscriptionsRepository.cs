﻿using CourseSubscription.Core.Data.EF;
using CourseSubscription.Data.Abstract;
using CourseSubscription.Entity.Model;

namespace CourseSubscription.Data.Repository.EF
{
    public class EfSubscriptionsRepository : EfEntityRepository<SUBSCRIPTION, EFContext>, ISubscriptionsRepository { }
}

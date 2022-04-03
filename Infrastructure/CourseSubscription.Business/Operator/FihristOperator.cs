using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Data.Repository;
using CourseSubscription.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourseSubscription.Business.Operator
{
    public class FihristOperator
    {
        public PagedList<SubscriptionListDto> GetUserSubscriptions(SubscriptionFilterDto filter)
        {
            using (EFContext _dbContex = new EFContext())
            {
                var data = (from s in _dbContex.SUBSCRIPTION
                            join t in _dbContex.TRAINING
                            on s.TRA_AUTO_KEY equals t.TRA_AUTO_KEY
                            select new SubscriptionListDto()
                            {
                                MONTH = t.MONTH,
                                SUBSCRIPTION_CODE = "S" + s.SUB_AUTO_KEY,
                                TRAINING_CODE = t.CODE,
                                TRAINING_NAME = t.NAME
                            });

                if (!string.IsNullOrEmpty(filter.Month))
                    data = data.Where(x => x.MONTH.Contains(filter.Month) ||
                    x.MONTH.ToLower().Contains(Utilise.ConvertToENChar(filter.Month.ToLower())) ||
                    x.MONTH.ToUpper().Contains(Utilise.ConvertToENChar(filter.Month.ToUpper()))
                    );

                if (!string.IsNullOrEmpty(filter.TrainingName))
                    data = data.Where(x => x.TRAINING_NAME.Contains(filter.TrainingName) ||
                    x.TRAINING_NAME.ToLower().Contains(Utilise.ConvertToENChar(filter.TrainingName.ToLower())) ||
                    x.TRAINING_NAME.ToUpper().Contains(Utilise.ConvertToENChar(filter.TrainingName.ToUpper()))
                    );

                return PagedList<SubscriptionListDto>.ToPagedList(data, filter.PageNumber, filter.PageSize);
            }
        }

        public List<Sp_UserSubscription> GetUserSubscriptions(decimal usr_auto_key)
        {
            using (EFContext _dbContex = new EFContext())
            {
                return _dbContex.GetUserSubscriptions(usr_auto_key);
            }
        }

        public PagedList<SubscriptionDetailDto> GetSubscriptionsDetails(SubscriptionDetailFilterDto filter, List<SubscriptionWithUserDto> subsList)
        {
            List<SUBSCRIPTION> asd = new List<SUBSCRIPTION>();

            using (EFContext _dbContex = new EFContext())
            {
                var data = (from s in subsList
                            join t in _dbContex.TRAINING
                            on s.TRA_AUTO_KEY equals t.TRA_AUTO_KEY
                            join c in _dbContex.COURSE
                            on t.COU_AUTO_KEY equals c.COU_AUTO_KEY
                            select new SubscriptionDetailDto()
                            {
                                COURSE_CODE = c.CODE,
                                COURSE_NAME = c.NAME,
                                TRAINING_MONTH = t.MONTH,
                                USER_NAME = s.USER_NAME,
                                USER_EMAIL = s.USER_EMAIL,
                                USER_GENDER = s.USER_GENDER,
                                TRAINING_CODE = t.CODE,
                                TRAINING_NAME = t.NAME
                            });

                if (!string.IsNullOrEmpty(filter.Month))
                    data = data.Where(x => x.TRAINING_MONTH.Contains(filter.Month) ||
                    x.TRAINING_MONTH.ToLower().Contains(Utilise.ConvertToENChar(filter.Month.ToLower())) ||
                    x.TRAINING_MONTH.ToUpper().Contains(Utilise.ConvertToENChar(filter.Month.ToUpper()))
                    );

                if (!string.IsNullOrEmpty(filter.TrainingName))
                    data = data.Where(x => x.TRAINING_NAME.Contains(filter.TrainingName) ||
                    x.TRAINING_NAME.ToLower().Contains(Utilise.ConvertToENChar(filter.TrainingName.ToLower())) ||
                    x.TRAINING_NAME.ToUpper().Contains(Utilise.ConvertToENChar(filter.TrainingName.ToUpper()))
                    );

                if (!string.IsNullOrEmpty(filter.CourseName))
                    data = data.Where(x => x.COURSE_NAME.Contains(filter.CourseName) ||
                    x.COURSE_NAME.ToLower().Contains(Utilise.ConvertToENChar(filter.CourseName.ToLower())) ||
                    x.COURSE_NAME.ToUpper().Contains(Utilise.ConvertToENChar(filter.CourseName.ToUpper()))
                    );

                if (!string.IsNullOrEmpty(filter.UserName))
                    data = data.Where(x => x.USER_NAME.Contains(filter.UserName) ||
                    x.USER_NAME.ToLower().Contains(Utilise.ConvertToENChar(filter.UserName.ToLower())) ||
                    x.USER_NAME.ToUpper().Contains(Utilise.ConvertToENChar(filter.UserName.ToUpper()))
                    );

                return PagedList<SubscriptionDetailDto>.ToPagedList(data.AsQueryable(), filter.PageNumber, filter.PageSize);
            }
        }

        public PagedList<TrainingListDto> GetTrainigs(TrainingFilterDto filter)
        {
            using (EFContext _dbContex = new EFContext())
            {
                var data = (from t in _dbContex.TRAINING
                            join c in _dbContex.COURSE
                            on t.COU_AUTO_KEY equals c.COU_AUTO_KEY
                            select new TrainingListDto()
                            {
                                CODE = t.CODE,
                                COURSE = c.NAME,
                                MONTH = t.MONTH,
                                NAME = t.NAME,
                                STATUS = t.STATUS,
                                TRA_AUTO_KEY = t.TRA_AUTO_KEY
                            });

                if (!string.IsNullOrEmpty(filter.Month))
                    data = data.Where(x => x.MONTH.Contains(filter.Month) ||
                    x.MONTH.ToLower().Contains(Utilise.ConvertToENChar(filter.Month.ToLower())) ||
                    x.MONTH.ToUpper().Contains(Utilise.ConvertToENChar(filter.Month.ToUpper()))
                    );

                if (!string.IsNullOrEmpty(filter.TrainingName))
                    data = data.Where(x => x.NAME.Contains(filter.TrainingName) ||
                    x.NAME.ToLower().Contains(Utilise.ConvertToENChar(filter.TrainingName.ToLower())) ||
                    x.NAME.ToUpper().Contains(Utilise.ConvertToENChar(filter.TrainingName.ToUpper()))
                    );

                if (!string.IsNullOrEmpty(filter.TrainingCode))
                    data = data.Where(x => x.CODE.Contains(filter.TrainingCode) ||
                    x.CODE.ToLower().Contains(Utilise.ConvertToENChar(filter.TrainingCode.ToLower())) ||
                    x.CODE.ToUpper().Contains(Utilise.ConvertToENChar(filter.TrainingCode.ToUpper()))
                    );

                return PagedList<TrainingListDto>.ToPagedList(data, filter.PageNumber, filter.PageSize);
            }
        }

        public decimal GetSeqNextValue(string value)
        {
            using (EFContext _dbContex = new EFContext())
            {
                decimal res = 0;

                res = _dbContex.NextValueForSequence(value);

                return res;
            }

        }
    }
}

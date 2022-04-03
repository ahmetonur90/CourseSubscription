using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.Operator;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using CourseSubscription.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Training
{
    public class TrainingListFlow : BusinessFlowBase<object, object>
    {
        private static TrainingFilterDto trainingListRequest;
        private static PagedList<TrainingListDto> trainingListResponse;

        public TrainingListFlow()
        {
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest(req);
            GetTrainingList();

            return trainingListResponse;
        }

        #region Helpers

        protected void PrepareRequest(object req)
        {
            trainingListRequest = (TrainingFilterDto)req;
        }

        protected void GetTrainingList()
        {
            FihristOperator op = new FihristOperator();
            trainingListResponse = op.GetTrainigs(trainingListRequest);

            if (trainingListResponse.Count < 1)
                throw new ServiceException(404, SubscriptionConstants.SubscriptionsNotFound, null);
        }

        #endregion
    }
}

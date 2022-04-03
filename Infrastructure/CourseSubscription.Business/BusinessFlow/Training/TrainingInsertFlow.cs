using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.Operator;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using CourseSubscription.Entity.Model;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Training
{
    public class TrainingInsertFlow : BusinessFlowBase<object, object>
    {
        private readonly ITrainingsService _trainingsService;
        private readonly ICoursesServices _coursesServices;
        private readonly IMapper _mapper;
        private CreateTrainingRequestDto createTrainingRequest;
        private static TrainingsDto createTrainingResponse;

        public TrainingInsertFlow(IMapper mapper, ITrainingsService trainingsService, ICoursesServices coursesServices)
        {
            _trainingsService = trainingsService;
            _coursesServices = coursesServices;
            _mapper = mapper;
            createTrainingResponse = new();
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest(req);
            CheckCourse(); // Check if course exists and active
            CheckTrainings(); // Be sure about one training should have one course
            InsertTraining();
            return createTrainingResponse;
        }

        #region Helpers

        protected void PrepareRequest(object req)
        {
            createTrainingRequest = (CreateTrainingRequestDto)req;
        }

        protected void CheckCourse()
        {
            var course = _coursesServices.GetCourseById(createTrainingRequest.COU_AUTO_KEY);
            if (course == null)
                throw new ServiceException(500, CourseConstants.CourseNotFound, null);

            if (!course.STATUS.Equals("Active"))
                throw new ServiceException(500, CourseConstants.CourseIsNotActive, null);

        }

        protected void CheckTrainings()
        {
            var currentTrainings = _trainingsService.GetTrainingsByCourseId(createTrainingRequest.COU_AUTO_KEY);
            if (currentTrainings.Count > 0)
                throw new ServiceException(404, TrainingConstants.TrainingExceeded, null);

        }

        protected void InsertTraining()
        {
            var entity = _mapper.Map<TRAINING>(createTrainingRequest);
            FihristOperator op = new FihristOperator();
            decimal code = op.GetSeqNextValue("TRAINING_CODE_SEQ");
            entity.CODE = $"T{code}";
            var result = _trainingsService.CreateTraining(entity);
            createTrainingResponse = _mapper.Map<TrainingsDto>(result);
        }

        #endregion

    }
}

using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using CourseSubscription.Entity.Model;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow.Training
{
    public class TrainingUpdateFlow : BusinessFlowBase<object, object>
    {
        private readonly ITrainingsService _trainingsService;
        private readonly ICoursesServices _coursesServices;
        private readonly IMapper _mapper;
        private UpdateTrainingRequestDto updateTrainingRequest;
        private static TrainingsDto updateTrainingResponse;

        public TrainingUpdateFlow(IMapper mapper, ITrainingsService trainingsService, ICoursesServices coursesServices)
        {
            _trainingsService = trainingsService;
            _coursesServices = coursesServices;
            _mapper = mapper;
            updateTrainingResponse = new();
        }

        public override async Task<object> Execute(object req)
        {
            PrepareRequest(req);            
            CheckTraining(); //Check if training exists and be sure about one training should have one course
            CheckCourse(); //Check if course exists
            UpdateTraining();
            return updateTrainingResponse;
        }

        #region Helpers

        protected void PrepareRequest(object req)
        {
            updateTrainingRequest = (UpdateTrainingRequestDto)req;
        }

        protected void CheckTraining()
        {
            var training = _trainingsService.GetTrainingById(updateTrainingRequest.TRA_AUTO_KEY);
            if (training == null)
                throw new ServiceException(404, TrainingConstants.TrainingNotFound, null);

            var currentTrainings = _trainingsService.GetTrainingsByCourseId(updateTrainingRequest.COU_AUTO_KEY, training.COU_AUTO_KEY);
            if (currentTrainings.Count > 0)
                throw new ServiceException(404, TrainingConstants.TrainingExceeded, null);

        }

        protected void CheckCourse()
        {
            var course = _coursesServices.GetCourseById(updateTrainingRequest.COU_AUTO_KEY);
            if (course == null)
                throw new ServiceException(404, CourseConstants.CourseNotFound, null);

            if (!course.STATUS.Equals("Active"))
                throw new ServiceException(404, CourseConstants.CourseIsNotActive, null);

        }

        protected void UpdateTraining()
        {
            var entity = _mapper.Map<TRAINING>(updateTrainingRequest);
            var result = _trainingsService.UpdateTraining(entity);
            updateTrainingResponse = _mapper.Map<TrainingsDto>(result);
        }

        #endregion

    }

}

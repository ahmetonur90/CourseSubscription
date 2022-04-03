using CourseSubscription.Business.Abstract;
using CourseSubscription.Data.Abstract;
using CourseSubscription.Entity.Model;
using System.Collections.Generic;

namespace CourseSubscription.Business.Services
{
    public class TrainingsService : ITrainingsService
    {
        private readonly ITrainingsRepository _trainingsRepository;

        public TrainingsService(ITrainingsRepository trainingsRepository)
        {
            _trainingsRepository = trainingsRepository;
        }

        public TRAINING CreateTraining(TRAINING entity)
        {
            return _trainingsRepository.Add(entity);
        }

        public List<TRAINING> GetTrainigList()
        {
            return _trainingsRepository.GetList();
        }

        public TRAINING GetTrainingById(decimal tra_auto_key)
        {
            return _trainingsRepository.Get(x => x.TRA_AUTO_KEY == tra_auto_key);
        }

        public List<TRAINING> GetTrainingsByCourseId(decimal cou_auto_key)
        {
            return _trainingsRepository.GetList(x => x.COU_AUTO_KEY == cou_auto_key);
        }

        public List<TRAINING> GetTrainingsByCourseId(decimal cou_auto_key, decimal current_cou_auto_key)
        {
            return _trainingsRepository.GetList(x => x.COU_AUTO_KEY == cou_auto_key && x.COU_AUTO_KEY != current_cou_auto_key);
        }

        public TRAINING UpdateTraining(TRAINING entity)
        {
            return _trainingsRepository.Update(entity);
        }
    }
}

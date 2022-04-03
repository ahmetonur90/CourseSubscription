using CourseSubscription.Entity.Model;
using System.Collections.Generic;

namespace CourseSubscription.Business.Abstract
{
    public interface ITrainingsService
    {
        TRAINING CreateTraining(TRAINING entity);
        TRAINING UpdateTraining(TRAINING entity);
        TRAINING GetTrainingById(decimal tra_auto_key);
        List<TRAINING> GetTrainigList();
        List<TRAINING> GetTrainingsByCourseId(decimal cou_auto_key);
        List<TRAINING> GetTrainingsByCourseId(decimal cou_auto_key, decimal current_cou_auto_key);
    }
}

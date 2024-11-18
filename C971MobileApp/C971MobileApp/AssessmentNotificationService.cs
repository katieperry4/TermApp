using C971MobileApp.Models;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971MobileApp
{
    public class AssessmentNotificationService
    {

        public async Task ScheduleAssessmentNotification(Assessment assessment)
        {
            var startNotification = new NotificationRequest()
            {
                NotificationId = assessment.AssessmentId * 10 + 1,
                Title = $"{assessment.AssessmentName} is starting today!",
                Description = "Don't forget!",
                Schedule = 
                {
                    NotifyTime = assessment.AssessmentStart.AddHours(7)
                }
            };
            await LocalNotificationCenter.Current.Show(startNotification);

            var endNotification = new NotificationRequest()
            {
                NotificationId = assessment.AssessmentId * 10 + 2,
                Title = $"{assessment.AssessmentName} is ending today.",
                Description = "Don't forget!",
                Schedule = 
                {
                    NotifyTime = assessment.AssessmentEnd.AddHours(7)
                }
            };
            await LocalNotificationCenter.Current.Show(endNotification);
        }


        public async Task UpdateAssessmentNotification(Assessment assessment)
        {
            await CancelAssessmentNotification(assessment.AssessmentId);

            await ScheduleAssessmentNotification(assessment);
        }

        public async Task CancelAssessmentNotification(int assessmentId)
        {
            LocalNotificationCenter.Current.Cancel(assessmentId * 10 + 1);
            LocalNotificationCenter.Current.Cancel(assessmentId * 10 + 2);
        }
    }
}

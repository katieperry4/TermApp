using C971MobileApp.Models;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971MobileApp
{
    public class CourseNotificationService
    {
        public async Task ScheduleCourseNotification(Course course)
        {
            var startNotification = new NotificationRequest
            {
                NotificationId = course.CourseID * 10 + 1,
                Title = $"{course.CourseName} is starting today!",
                Description = "Don't forget!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = course.CourseStart.AddHours(7)
                }
            };
            await LocalNotificationCenter.Current.Show(startNotification);

            var endNotification = new NotificationRequest
            {
                NotificationId = course.CourseID * 10 + 2,
                Title = $"{course.CourseName} is ending today.",
                Description = "Don't forget!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = course.CourseEnd.AddHours(7)
                }
            };
            await LocalNotificationCenter.Current.Show(endNotification);
        }


        public async Task UpdateCourseNotification(Course course)
        {
            await CancelCourseNotification(course.CourseID);

            await ScheduleCourseNotification(course);
        }

        public async Task CancelCourseNotification(int courseId)
        {
            LocalNotificationCenter.Current.Cancel(courseId * 10 +1);
            LocalNotificationCenter.Current.Cancel(courseId * 10 +2);
        }
    }
}

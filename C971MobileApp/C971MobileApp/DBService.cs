using C971MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971MobileApp
{
    public class DBService
    {
        private const string DB_Name = "app_DB.db3";
        private readonly SQLiteAsyncConnection _connection;

        public DBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));
            _connection.CreateTableAsync<Term>();
            _connection.CreateTableAsync<Course>();
            _connection.CreateTableAsync<Assessment>();
        }

        public async Task SeedData()
        {
            var termCount = await _connection.Table<Term>().CountAsync();
            if (termCount == 0)
            {
                await _connection.InsertAsync(new Term { TermName = "Term 1", TermStart = new DateTime(2024, 9, 1), TermEnd = new DateTime(2025, 2, 28) });
            }

            var courseCount = await _connection.Table<Course>().CountAsync();
            if (courseCount == 0)
            {
                var term = await _connection.Table<Term>().FirstOrDefaultAsync();
                if (term != null)
                {
                    await _connection.InsertAsync(new Course {CourseName = "Hardware and Operating Systems", CourseStatus = "In Progress", CIName = "Anika Patel", CIEmail = "anika.patel@strimeuniversity.com", CIPhone = "555-123-4567", CourseStart = new DateTime(2024, 10,15), CourseEnd = new DateTime(2024,11,30), CourseNotes = "This is a very difficult class, it's a good thing I have this notes section to write down all of my thoughts.", StartNotif = false, EndNotif = false, TermId=term.TermId });
                }
            }

            var assessmentCount = await _connection.Table<Assessment>().CountAsync();
            if (assessmentCount == 0) 
            {
                var course = await _connection.Table<Course>().FirstOrDefaultAsync();
                if (course != null) 
                {
                    await _connection.InsertAsync(new Assessment {AssessmentName = "Hardware PA", AssessmentType = "Performance", CourseId = course.CourseID, AssessmentStart = new DateTime(2024, 11, 8), AssessmentEnd = new DateTime(2024, 11, 15), StartNotif = false, EndNotif = false});
                    await _connection.InsertAsync(new Assessment {AssessmentName = "Proctored Test", AssessmentType = "Objective", CourseId = course.CourseID, AssessmentStart = new DateTime(2024, 11, 20), AssessmentEnd = new DateTime(2024, 11,21), StartNotif = false, EndNotif = false });
                }
            }

        }

        public async Task<List<Term>> GetAllTerms()
        {
            return await _connection.Table<Term>().ToListAsync();
        }

        public async Task<Term> GetTermById(int termId)
        {
            return await _connection.Table<Term>().Where(x => x.TermId == termId).FirstOrDefaultAsync();
        }

        public async Task<List<Course>> GetAllCourses(int termId)
        {
            return await _connection.Table<Course>().Where(x => x.TermId == termId).ToListAsync();
        }

        public async Task<Course> GetCourseById(int courseId)
        {
            return await _connection.Table<Course>().Where(x => x.CourseID == courseId).FirstOrDefaultAsync();
        }

        public async Task<Assessment> GetAssessmentById(int assessmentId)
        {
            return await _connection.Table<Assessment>().Where(x => x.AssessmentId == assessmentId).FirstOrDefaultAsync();
        }

        public async Task<List<Assessment>> GetAssessments(int courseId)
        {
            return await _connection.Table<Assessment>().Where(x => x.CourseId == courseId).ToListAsync();
        }

        public async Task AddTerm(Term term)
        {
            await _connection.InsertAsync(term);
        }

        public async Task<int> AddCourse(Course course)
        {
            await _connection.InsertAsync(course);
            return course.CourseID;
        }

        public async Task<int> AddAssessment(Assessment assessment)
        {
            await _connection.InsertAsync(assessment);
            return assessment.AssessmentId;
        }

        public async Task UpdateTerm(Term term)
        {
            await _connection.UpdateAsync(term);
        }

        public async Task<int> UpdateCourse(Course course)
        {
            await _connection.UpdateAsync(course);
            return course.CourseID;
        }

        public async Task UpdateAssessment(Assessment assessment)
        {
            await _connection.UpdateAsync(assessment);
        }

        public async Task DeleteTerm(Term term)
        {
            await _connection.DeleteAsync(term);
        }

        public async Task DeleteAllTerms()
        {
            await _connection.ExecuteAsync("DELETE FROM Term");
        }

        public async Task DeleteCourse(Course course)
        {
            await _connection.DeleteAsync(course);
        }

        public async Task DeleteAssessment(Assessment assessment)
        {
            await _connection.DeleteAsync(assessment);
        }
    }
}

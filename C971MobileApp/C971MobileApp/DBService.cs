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

        public async Task<List<Assessment>> GetAssessments(int courseId)
        {
            return await _connection.Table<Assessment>().Where(x => x.CourseId == courseId).ToListAsync();
        }

        public async Task AddTerm(Term term)
        {
            await _connection.InsertAsync(term);
        }

        public async Task AddCourse(Course course)
        {
            await _connection.InsertAsync(course);
        }

        public async Task AddAssessment(Assessment assessment)
        {
            await _connection.InsertAsync(assessment);
        }

        public async Task UpdateTerm(Term term)
        {
            await _connection.UpdateAsync(term);
        }

        public async Task UpdateCourse(Course course)
        {
            await _connection.UpdateAsync(course);
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

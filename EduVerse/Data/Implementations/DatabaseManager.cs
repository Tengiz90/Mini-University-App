using EduVerse.Data.Interfaces;
using EduVerse.Models;
using Microsoft.EntityFrameworkCore;

namespace EduVerse.Data.Implementations
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly AppDbContext _db;

        public DatabaseManager(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> RemoveStudentByIdAsync(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return false;

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTeacherByIdAsync(int id)
        {
            var teacher = await _db.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _db.Teachers.Remove(teacher);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCourseByIdAsync(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course == null) return false;

            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task AddStudentAsync(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            _db.Teachers.Add(teacher);
            await _db.SaveChangesAsync();
        }

        public async Task AddCourseByTeacherIdAsync(int teacherId, Course course)
        {
           
            
            course.TeacherId = teacherId;
           

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
        }
        public async Task AddMarksByCourseIdAndStudentIdAsync(int courseId, int studentId, SingleCourseMarks marks)
        {
            await _db.SingleCourseMarks.AddAsync(marks);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            var exists = await _db.SingleCourseMarks.AnyAsync(m => m.StudentId == studentId && m.CourseId == courseId);
            if (exists) return false;

            var marks = new SingleCourseMarks
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _db.SingleCourseMarks.Add(marks);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnenrollStudentFromCourseAsync(int studentId, int courseId)
        {
            var marks = await _db.SingleCourseMarks
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseId == courseId);
            if (marks == null) return false;

            _db.SingleCourseMarks.Remove(marks);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ModifyStudentMarksAsync(SingleCourseMarks updatedMarks)
        {
            var marks = await _db.SingleCourseMarks
                .FirstOrDefaultAsync(m => m.StudentId == updatedMarks.StudentId && m.CourseId == updatedMarks.CourseId);
            if (marks == null) return false;

            
            marks.Homework1 = updatedMarks.Homework1;
            marks.Homework2 = updatedMarks.Homework2;
            marks.Homework3 = updatedMarks.Homework3;
            marks.Homework4 = updatedMarks.Homework4;
            marks.Homework5 = updatedMarks.Homework5;
            marks.Homework6 = updatedMarks.Homework6;
            marks.Homework7 = updatedMarks.Homework7;
            marks.Homework8 = updatedMarks.Homework8;
            marks.Homework9 = updatedMarks.Homework9;
            marks.Homework10 = updatedMarks.Homework10;
            marks.MidtermExam = updatedMarks.MidtermExam;
            marks.Presentation = updatedMarks.Presentation;
            marks.FinalExam = updatedMarks.FinalExam;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<SingleCourseMarks?> ViewStudentMarksByCourseAsync(int studentId, int courseId)
        {
            return await _db.SingleCourseMarks
                .Include(m => m.Student)
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseId == courseId);
        }

        public async Task<bool> DoesStudentWithSuchEmailandPasswordExistAsync(string email, string password)
        {
            var student = await _db.Students.FirstOrDefaultAsync(s => s.Email == email);
            if (student != null && BCrypt.Net.BCrypt.Verify(password, student.PasswordHash))
                return true;

            return false;
        }

        public async Task<bool> DoesTeacherWithSuchEmailandPasswordExistAsync(string email, string password)
        {
            var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Email == email);
            if (teacher != null && BCrypt.Net.BCrypt.Verify(password, teacher.PasswordHash))
                return true;


            return false;
        }
        public async Task<bool> DoesTeacherWithSuchIdAlreadyExistAsync(int teacheriD)
        {
            return await _db.Teachers.AnyAsync(t => t.Id == teacheriD);
          
        }
        public async Task<bool> DoesStudentWithSuchIdAlreadyExistAsync(int studentId)
        {
            return await _db.Students.AnyAsync(s => s.Id == studentId);
         
        }
        public async Task<bool> DoesTeacherWithSuchEmailAlreadyExistAsync(string email)
        {
            return await _db.Teachers.AnyAsync(t => t.Email == email);
        }

        public async Task<bool> DoesStudentWithSuchEmailAlreadyExistAsync(string email)
        {
            return await _db.Students.AnyAsync(s => s.Email == email);
        }


        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _db.Students.FirstAsync(s => s.Email == email);
        }

        public async Task<Teacher> GetTeacherByEmailAsync(string email)
        {
            return await _db.Teachers.FirstAsync(t => t.Email == email);
        }

        public async Task<List<Student>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _db.SingleCourseMarks
                .Where(m => m.CourseId == courseId)
                .Include(m => m.Student)
                .Select(m => m.Student)
                .ToListAsync();
        }
        public async Task<List<Course>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            return await _db.Courses
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();
        }
        public async Task<int> GetCountOfAllCoursesAsync()
        {
            return await _db.Courses.CountAsync();
        }
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _db.Courses.ToListAsync();
        }
        public async Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId)
        {
            return await _db.SingleCourseMarks.AnyAsync(m => m.StudentId == studentId && m.CourseId == courseId);
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _db.Students.FirstAsync(s  => s.Id == id);
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _db.Courses.FirstAsync(c => c.Id == id);
        }

    }
}

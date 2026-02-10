using EduVerse.Models;

namespace EduVerse.Data.Interfaces
{
    public interface IDatabaseManager
    {
       
        Task<bool> RemoveStudentByIdAsync(int id);
        Task<bool> RemoveTeacherByIdAsync(int id);
        Task<bool> RemoveCourseByIdAsync(int id);

        Task AddStudentAsync(Student student);
        Task AddTeacherAsync(Teacher teacher);
        Task AddCourseByTeacherIdAsync(int id, Course course);
        Task AddMarksByCourseIdAndStudentIdAsync(int courseId, int studentId, SingleCourseMarks marks);


        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<bool> UnenrollStudentFromCourseAsync(int studentId, int courseId);

       
        Task<bool> ModifyStudentMarksAsync(SingleCourseMarks updatedMarks);
        Task<SingleCourseMarks?> ViewStudentMarksByCourseAsync(int studentId, int courseId);

        Task<bool> DoesStudentWithSuchEmailandPasswordExistAsync(string email, string password);
        Task<bool> DoesTeacherWithSuchEmailandPasswordExistAsync(string email, string password);
        Task<bool> DoesTeacherWithSuchIdAlreadyExistAsync(int teacheriD);
        Task<bool> DoesStudentWithSuchIdAlreadyExistAsync(int studentId);
        Task<bool> DoesTeacherWithSuchEmailAlreadyExistAsync(string email);
        Task<bool> DoesStudentWithSuchEmailAlreadyExistAsync(string email);

        Task<Student> GetStudentByEmailAsync(string email);
        Task<Teacher> GetTeacherByEmailAsync(string email);
        Task<List<Student>> GetStudentsByCourseIdAsync(int courseId);
        Task<List<Course>> GetCoursesByTeacherIdAsync(int courseId);
        Task<int> GetCountOfAllCoursesAsync();
        Task<List<Course>> GetAllCoursesAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Course> GetCourseByIdAsync(int id);

        Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId);




    }
}

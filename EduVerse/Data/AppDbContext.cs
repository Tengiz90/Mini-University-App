using EduVerse.Models;
using Microsoft.EntityFrameworkCore;

namespace EduVerse.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<CourseCategoryMapping> CourseCategoryMappings => Set<CourseCategoryMapping>();
        public DbSet<SingleCourseMarks> SingleCourseMarks => Set<SingleCourseMarks>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CourseCategoryMapping>()
                .HasKey(cc => new { cc.CourseId, cc.Category });

            modelBuilder.Entity<CourseCategoryMapping>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.CourseCategories)
                .HasForeignKey(cc => cc.CourseId);


            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);


            modelBuilder.Entity<SingleCourseMarks>()
                .HasKey(m => new { m.StudentId, m.CourseId });

            modelBuilder.Entity<SingleCourseMarks>()
                .HasOne(m => m.Student)
                .WithMany(s => s.CourseMarks)
                .HasForeignKey(m => m.StudentId);

            modelBuilder.Entity<SingleCourseMarks>()
                .HasOne(m => m.Course)
                .WithMany(c => c.CourseMarks)
                .HasForeignKey(m => m.CourseId);

     

        }
    }
}

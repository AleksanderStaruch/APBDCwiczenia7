using Cwiczenia7.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cwiczenia7.DAL
{
    public class DB
    {
        PjatkDb pjatkDb;
        public DB()
        {
            pjatkDb = new PjatkDb();
        }

        public List<Student> GetStudents()
        {
            var students = pjatkDb.Students;

            return students.Include(s => s.Study).ToList();
        }

        public List<Study> GetStudies()
        {
            var studies = pjatkDb.Studies;

            return studies.ToList();
        }

        public List<Subject> GetSubjects()
        {
            var studies = pjatkDb.Subjects;

            return studies.ToList();
        }

        public List<Student_Subject> GetStudentSubjects(int studentid)
        {
            var r = pjatkDb.Student_Subject;

            return r.Where(s => s.IdStudent==studentid).ToList();
        }

        public void AddStudent(Student student)
        {
            pjatkDb.Students.Add(student);

            pjatkDb.SaveChanges();
        }

        public void DeleteStudent(Student student)
        {
            pjatkDb.Students.Remove(pjatkDb.Students.Single(s=>s.IdStudent==student.IdStudent));

            pjatkDb.SaveChanges();
        }

        public void EditStudent(Student student)
        {
            Student s = (from x in pjatkDb.Students
                          where x.IdStudent==student.IdStudent
                          select x).First();

            s.IdStudent = student.IdStudent;
            s.LastName = student.LastName;
            s.FirstName = student.FirstName;
            s.Address = student.Address;
            s.IndexNumber = student.IndexNumber;
            s.IdStudies = student.IdStudies;
            
            pjatkDb.SaveChanges();
        }

        public void AddStudent_Subject(Student_Subject s)
        {
            pjatkDb.Student_Subject.Add(s);

            pjatkDb.SaveChanges();
        }

        public void DeleteStudent_Subject(Student_Subject ss)
        {
            pjatkDb.Student_Subject.Remove(pjatkDb.Student_Subject.Single(s=>s.IdStudent==ss.IdStudent && s.IdSubject==ss.IdSubject));

            pjatkDb.SaveChanges();
        }

        public void DeleteStudent_Subject(int StudentId)
        {
            var listSS = new DB().GetStudentSubjects(StudentId);

            var idList = (from ss in listSS select ss.IdSubject).ToList();

            foreach (int id in idList)
            {
                Student_Subject student_subject = new Student_Subject
                {
                    IdStudent = StudentId,
                    IdSubject = id
                };

                new DB().DeleteStudent_Subject(student_subject);
            }
        }
        
    }
}

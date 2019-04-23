using Cwiczenia7.DAL;
using Cwiczenia7.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cwiczenia7
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        Student student;
        int StudentId;
        public StudentWindow(Student student)
        {
            InitializeComponent();
            this.student = student;

            SetComboBox();

            if (!string.IsNullOrEmpty(student.LastName))
            {
                StudentId = student.IdStudent;
                Text1.Text = student.LastName;
                Text2.Text = student.FirstName;
                Text3.Text = student.Address;
                Text4.Text = student.IndexNumber;
                ComboBox.Text = GetStudyName(student.IdStudies);

                SetListBox(StudentId);
                
                Check();
                try
                {
                    int.TryParse(Text4.Text.Substring(1), out int n);
                }
                catch (Exception)
                {
                    Text4.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                SetListBox(0);
                StudentId = NewStudentId();
            }

        }

        public void SetComboBox()
        {
            var list = new DB().GetStudies();
            ComboBox.Items.Clear();
            foreach (Study s in list)
            {
                ComboBox.Items.Add(s.Name);
            }
            ComboBox.SelectedItem = list[0].Name;
        }
        public void SetListBox(int n)
        {
            var listSS = new DB().GetStudentSubjects(n);

            var r = (from ss in listSS
                     select ss.IdSubject
                     ).ToList();

            
            var list = new DB().GetSubjects();
            ListBox.Items.Clear();
            int i = 1;
            foreach (Subject s in list)
            {
                CheckBox checkBox = new CheckBox
                {
                    Content = s.Name
                };
                if (r.Contains(i))
                {
                    checkBox.IsChecked=true;
                }
                i++;
                ListBox.Items.Add(checkBox);
            }
        }
        private void Check()
        {
            if (String.IsNullOrWhiteSpace(Text1.Text))
            {
                Text1.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                Text1.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (String.IsNullOrWhiteSpace(Text2.Text))
            {
                Text2.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                Text2.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (String.IsNullOrWhiteSpace(Text3.Text))
            {
                Text3.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                Text3.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (!Text4.Text.StartsWith("s"))
            {
                Text4.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                Text4.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (String.IsNullOrWhiteSpace(ComboBox.Text))
            {
                ComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ComboBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }
        private int NewStudentId()
        {
            var list = new DB().GetStudents();
            var id = (from s in list
                              select s.IdStudent).Max();

            return Convert.ToInt32(id+1);
        }
        private int NewStudyId()
        {
            var list = new DB().GetStudies();

            var id = (from s in list
                      where s.Name == ComboBox.Text
                      select s.IdStudies).Max();


            return Convert.ToInt32(id);
        }
        private string GetStudyName(int n)
        {
            var list = new DB().GetStudies();

            var r = (from s in list
                      where s.IdStudies==n
                      select s.Name).First();
            
            return r;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Check();
            bool tryy = false;
            try
            {
                tryy = Int32.TryParse(Text4.Text.Substring(1), out int n);
            }
            catch (ArgumentOutOfRangeException)
            {
                
            }

            if (Text1.Text != null && Text2.Text != null && Text3.Text != null && Text4.Text.StartsWith("s") && tryy && ComboBox.Text != null)
            {
                student.IdStudent = StudentId;
                student.LastName = Text1.Text;
                student.FirstName = Text2.Text;
                student.Address = Text3.Text;
                student.IndexNumber = Text4.Text;
                student.IdStudies = NewStudyId();

                //listbox
                new DB().DeleteStudent_Subject(StudentId);
                
                var list = ListBox.Items;
                int i = 1;
                foreach(CheckBox o in list)
                {
                    var r = o.Content;
                    var student_subject = new Student_Subject
                    {
                        IdStudent = StudentId,
                        IdSubject = i
                    };

                    if (o.IsChecked==true)
                    {
                        new DB().AddStudent_Subject(student_subject);
                    }
                    i++;
                }

                Close();
            }
            else
            {
                Text4.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}

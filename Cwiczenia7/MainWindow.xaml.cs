using Cwiczenia7.DAL;
using Cwiczenia7.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cwiczenia7
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Student> list;
        public MainWindow()
        {
            InitializeComponent();
            SetDataGrid();
        }

        public void SetDataGrid()
        {
            //DataGrid.Items.Clear();
            var l = new DB().GetStudents();
            var resoult = from student in l
                          select student;
            list = new ObservableCollection<Student>(resoult.ToList());

            DataGrid.ItemsSource = list;
        }
        
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Student tmp = new Student();
            
            Window dodajStudenta = new StudentWindow(tmp);
            dodajStudenta.ShowDialog();

            if (!string.IsNullOrWhiteSpace(tmp.LastName))
            {
                list.Add(tmp);
                new DB().AddStudent(tmp);
            }
            SetDataGrid();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = DataGrid.SelectedItems.Count-1; -1 < i; i= DataGrid.SelectedItems.Count-1)
            {

                Student tmp = (Student)DataGrid.SelectedItems[i];
                
                new DB().DeleteStudent_Subject(tmp.IdStudent);

                list.Remove(tmp);
                new DB().DeleteStudent(tmp);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Student tmp = (Student)DataGrid.SelectedItem;
            Window dodajStudenta = new StudentWindow(tmp);
            dodajStudenta.ShowDialog();
            new DB().EditStudent(tmp);

            SetDataGrid();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = DataGrid.SelectedItems.Count;
            Count.Content = "Wybrałes " + n + " studentów";
        }
    }
}

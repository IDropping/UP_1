using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class Courses1 : Window
{
    private ObservableCollection<CourseClass> Courses { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public Courses1()
    {
        InitializeComponent();
        Courses = new ObservableCollection<CourseClass>();
        _connection = new MySqlConnectionStringBuilder()
        {
            Server = "10.10.1.24",
            Database = "pro1_3",
            UserID = "user_01",
            Password = "user01pro"
        };
        ShowTable();
    }

    private void ShowTable()
    {
        using (var connection = new MySqlConnection(_connection.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Courses";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Courses.Add(new CourseClass()
                        {
                            IDCourse = reader.GetInt32("ID"),
                            Courses = reader.GetString("курс"),
                            CourseGroup = reader.GetInt32("группа"),
                            CourseTeacher = reader.GetInt32("преподователь")
                        });
                    }
                }
            }
            connection.Close();
        }
        CourseDataGrid.ItemsSource = Courses ;
    }

    private void AddCourses_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Courses (ID, курс, группа, преподователь)" +
                                  "VALUES ('" + Convert.ToInt32(idCourseBox.Text) + 
                                  "','" + coursesBox.Text +
                                  "','" + Convert.ToInt32(GroupCourseBox.Text) + 
                                  "','" + Convert.ToInt32(teacherCourseBox.Text) + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    Courses.Add(new CourseClass()
                    {
                        IDCourse = Convert.ToInt32(idCourseBox.Text),
                        Courses = coursesBox.Text,
                        CourseGroup = Convert.ToInt32(GroupCourseBox.Text),
                        CourseTeacher = Convert.ToInt32(teacherCourseBox.Text)
                    });
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            cnn.Close();
        }
    }

    private void DelCourses_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = CourseDataGrid.SelectedItem as CourseClass;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Courses";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Courses.Remove(del);
        CourseDataGrid.ItemsSource = Courses;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class Attendance1 : Window
{
    private ObservableCollection<AttendanceClass> Attend { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public Attendance1()
    {
        InitializeComponent();
        Attend = new ObservableCollection<AttendanceClass>();
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
                command.CommandText = "SELECT * FROM Attendance";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Attend.Add(new AttendanceClass()
                        {
                            IDAttend = reader.GetInt32("ID"),
                            AttClient = reader.GetInt32("клиент"),
                            AttCourse = reader.GetInt32("курс"),
                            AttGrade = reader.GetInt32("оценка"),
                            VisitPercent = reader.GetInt32("процент_посещения"),
                            DateVisit = reader.GetString("дата_посещения")
                        });
                    }
                }
            }

            connection.Close();
        }

        AttendanceDataGrid.ItemsSource = Attend;
    }

    private void AddAttend_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Attendance (ID, курс, группа, процента_посещения, дата_посещения)" +
                                  "VALUES ('" + Convert.ToInt32(idAttendBox.Text) +
                                  "','" + Convert.ToInt32(clientBox.Text) +
                                  "','" + Convert.ToInt32(courseBox.Text) +
                                  "','" + Convert.ToInt32(gradeBox.Text) + 
                                  "','" + dateBox.Text + 
                                  "','" + Convert.ToInt32(persentBox.Text + "')");
                try
                {
                    cmd.ExecuteNonQuery();
                    Attend.Add(new AttendanceClass()
                    {
                        IDAttend = Convert.ToInt32(idAttendBox.Text),
                        AttClient = Convert.ToInt32(clientBox.Text),
                        AttCourse = Convert.ToInt32(courseBox.Text),
                        AttGrade = Convert.ToInt32(gradeBox.Text),
                        DateVisit = dateBox.Text,
                        VisitPercent = Convert.ToInt32(persentBox.Text)
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

    private void DelAttend_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = AttendanceDataGrid.SelectedItem as AttendanceClass;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Attendance";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Attend.Remove(del);
        AttendanceDataGrid.ItemsSource = Attend;
    }

    // Описана сортировка по убыванию и возрастанию по столбцу VisitPersent
    private void OrderBy_OnClick(object? sender, RoutedEventArgs e)
    {
        var ToHigh = new List<AttendanceClass>(Attend);
        ToHigh = ToHigh.OrderByDescending(x => x.VisitPercent).ToList();
        AttendanceDataGrid.ItemsSource = ToHigh;
    }

    private void OrderByDescending_OnClick(object? sender, RoutedEventArgs e)
    {
        var ToLow = new List<AttendanceClass>(Attend);
        ToLow = ToLow.OrderBy(x => x.VisitPercent).ToList();
        AttendanceDataGrid.ItemsSource = ToLow;
    }
}
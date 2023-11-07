using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class AddTeacher1 : Window
{
    private ObservableCollection<AddTeacher> Teachers { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public AddTeacher1()
    {
        InitializeComponent();
        Teachers = new ObservableCollection<AddTeacher>();
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
                command.CommandText = "SELECT * FROM Teacher";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teachers.Add(new AddTeacher()
                        {
                            IDTeach = reader.GetInt32("ID"),
                            TeacherName = reader.GetString("имя"),
                            TeacherSurname = reader.GetString("фамилия")
                        });
                    }
                }
            }
            connection.Close();
        }

        TeacherDataGrid.ItemsSource = Teachers;
    }

    private void AddTeacher_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Teacher (ID, имя, фамилия)" +
                                  "VALUES ('" + Convert.ToInt32(idTeachBox.Text) + 
                                  "','" + nameBox.Text +
                                  "','" + surnameBox.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    Teachers.Add(new AddTeacher()
                    {
                        IDTeach = Convert.ToInt32(idTeachBox.Text),
                        TeacherSurname = nameBox.Text,
                        TeacherName = surnameBox.Text
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

    private void DelTeacher_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = TeacherDataGrid.SelectedItem as AddTeacher;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Teacher";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Teachers.Remove(del);
        TeacherDataGrid.ItemsSource = Teachers;
    }
}
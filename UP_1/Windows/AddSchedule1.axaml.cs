using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class AddSchedule1 : Window
{
    private ObservableCollection<AddShedule> Shedules { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public AddSchedule1()
    {
        InitializeComponent();
        Shedules = new ObservableCollection<AddShedule>();
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
                command.CommandText = "SELECT * FROM Schedule";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Shedules.Add(new AddShedule()
                        {
                            IDSched = reader.GetInt32("ID"),
                            Day = reader.GetString("день"),
                            Time = reader.GetString("время")
                        });
                    }
                }
            }

            connection.Close();
        }

        ScheduleDataGrid.ItemsSource = Shedules;
    }

    private void AddSchedule_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Schedule (ID, день, время)" +
                                  "VALUES ('" + Convert.ToInt32(idSchedBox.Text) + 
                                  "','" + dayBox.Text +
                                  "','" + timeBox.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    Shedules.Add(new AddShedule()
                    {
                        IDSched = Convert.ToInt32(idSchedBox.Text),
                        Day = dayBox.Text,
                        Time = timeBox.Text
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

    private void DelSchedule_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = ScheduleDataGrid.SelectedItem as AddShedule;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Schedule";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Shedules.Remove(del);
        ScheduleDataGrid.ItemsSource = Shedules;
    }
}
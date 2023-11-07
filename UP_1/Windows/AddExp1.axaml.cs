using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class AddExp1 : Window
{
    private ObservableCollection<AddExperience> Experiences { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public AddExp1()
    {
        InitializeComponent();
        Experiences = new ObservableCollection<AddExperience>();
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
                command.CommandText = "SELECT * FROM Experience";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Experiences.Add(new AddExperience()
                        {
                            IDExp = reader.GetInt32("ID"),
                            ExpLevel = reader.GetString("Уровень_владения")
                        });
                    }
                }
            }
            connection.Close();
        }
        ExperienceDataGrid.ItemsSource = Experiences;    
    }

    private void AddExp_onClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Experience (ID, уровень_владения)" +
                                  "VALUES ('" + Convert.ToInt32(idExpBox.Text) + 
                                  "','" + ExpBox.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    Experiences.Add(new AddExperience()
                    {
                        IDExp = Convert.ToInt32(idExpBox.Text),
                        ExpLevel = ExpBox.Text
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

    private void DelExp_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = ExperienceDataGrid.SelectedItem as AddExperience;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Experience";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Experiences.Remove(del);
        ExperienceDataGrid.ItemsSource = Experiences;
    }
}
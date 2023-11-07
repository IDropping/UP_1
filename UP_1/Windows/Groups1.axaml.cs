using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1;

public partial class Groups1 : Window
{
    private ObservableCollection<GroupsClass> Groups { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public Groups1()
    {
        InitializeComponent();
        Groups = new ObservableCollection<GroupsClass>();
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
                command.CommandText = "SELECT * FROM SchoolGroup";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Groups.Add(new GroupsClass()
                        {
                            IDGroup = reader.GetInt32("ID"),
                            Group = reader.GetString("группа"),
                            GroupSchedule = reader.GetInt32("расписание")
                        });
                    }
                }
            }
            connection.Close();
        }

        GroupsDataGrid.ItemsSource = Groups;
    }

    private void AddGroup_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO SchoolGroup (ID, фамилия, имя)" +
                                  "VALUES ('" + Convert.ToInt32(idGroupBox.Text) + 
                                  "','" + groupBox.Text +
                                  "','" + scheduleBox.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    Groups.Add(new GroupsClass()
                    {
                        IDGroup = Convert.ToInt32(idGroupBox.Text),
                        Group = groupBox.Text,
                        GroupSchedule = Convert.ToInt32(scheduleBox.Text)
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

    private void DelGroup_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = GroupsDataGrid.SelectedItem as GroupsClass;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM SchoolGroup";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Groups.Remove(del);
        GroupsDataGrid.ItemsSource = Groups;
    }
}
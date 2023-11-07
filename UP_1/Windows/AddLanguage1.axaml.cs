using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class AddLanguage1 : Window
{
    private ObservableCollection<AddLanguage> Languages { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public AddLanguage1()
    {
        InitializeComponent();
        Languages = new ObservableCollection<AddLanguage>();
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
                command.CommandText = "SELECT * FROM Language";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Languages.Add(new AddLanguage()
                        {
                            IDLang = reader.GetInt32("ID"),
                            Language = reader.GetString("язык")
                        });
                    }
                }
            }

            connection.Close();
        }

        LanguageDataGrid.ItemsSource = Languages;
    }

    private void AddLang_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Language (ID, язык)" +
                                  "VALUES ('" + Convert.ToInt32(idLangBox.Text) + 
                                  "','" + LanguageBox.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    Languages.Add(new AddLanguage()
                    {
                        IDLang = Convert.ToInt32(idLangBox.Text),
                        Language = LanguageBox.Text
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

    private void DelLang_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = LanguageDataGrid.SelectedItem as AddLanguage;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Language";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Languages.Remove(del);
        LanguageDataGrid.ItemsSource = Languages;
    }
}
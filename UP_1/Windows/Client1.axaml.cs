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

public partial class Client1 : Window
{
    private ObservableCollection<ClientClass> Clients { get; set; } 
    private MySqlConnectionStringBuilder _connection;
    public Client1()
    {
        // Описано подключение к БД MySQL
        InitializeComponent();
        Clients = new ObservableCollection<ClientClass>();
        _connection = new MySqlConnectionStringBuilder()
        {
            Server = "10.10.1.24",
            Database = "pro1_3",
            UserID = "user_01",
            Password = "user01pro"
        };
        ShowTable();
    }
    
    // Метод, в котором происходит вывод данных из таблицы MySQL
    private void ShowTable()
    {
        // Подключение к БД
        using (var connection = new MySqlConnection(_connection.ConnectionString))
        {
            // Открытие подключения
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                // Команда MySQL для выборки данных
                command.CommandText = "SELECT * FROM Client";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Внесение данных в таблицу
                        Clients.Add(new ClientClass()
                        {
                            IDClient = reader.GetInt32("ID"),
                            ClientSurname = reader.GetString("фамилия"),
                            ClientName = reader.GetString("имя"),
                            TelephoneNumber = reader.GetString("номер_телефона"),
                            Email = reader.GetString("электронная_почта"),
                            Birthday = reader.GetString("дата_рождения"),
                            ClientLanguage = reader.GetInt32("язык"),
                            ClientGroup = reader.GetInt32("группа"),
                            ClientExperience = reader.GetInt32("уровень_владения_языком")
                        });
                    }
                }
            }
            // Закрытие подключения
            connection.Close();
        }
        ClientDataGrid.ItemsSource = Clients;
    }
    
    // В этом методе описано добавление данных в таблицу
    private void AddClient_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                // Написан SQL запрос для внесения данных
                cmd.CommandText = "INSERT INTO Client (ID, фамилия, имя, номер_телефона, электронная_почта, дата_рождения, язык, группа, уровень_владения_языком)" +
                                  "VALUES ('" + Convert.ToInt32(idClientBox.Text) + 
                                  "','" + surnameClientBox.Text +
                                  "','" + nameClientBox.Text + 
                                  "','" + telephoneClientBox.Text + 
                                  "','" + emailClientBox.Text + 
                                  "','" + birthdayClientBox.Text +
                                  "','" + Convert.ToInt32(LanguageClientBox.Text) +
                                  "','" + Convert.ToInt32(GroupClientBox.Text) +
                                  "','" + Convert.ToInt32(expClientBox.Text) + "')";
                try
                {
                    // Отправка на выполнение SQL запроса
                    cmd.ExecuteNonQuery();
                    Clients.Add(new ClientClass()
                    {
                        IDClient = Convert.ToInt32(idClientBox.Text),
                        ClientSurname = surnameClientBox.Text,
                        ClientName = nameClientBox.Text,
                        TelephoneNumber = Convert.ToString(telephoneClientBox.Text),
                        Email = emailClientBox.Text,
                        Birthday = birthdayClientBox.Text,
                        ClientLanguage = Convert.ToInt32(LanguageClientBox.Text),
                        ClientGroup = Convert.ToInt32(GroupClientBox.Text),
                        ClientExperience = Convert.ToInt32(expClientBox.Text)
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

    // Описан метод удаления данных при выделении строки в таблице
    private void DelClient_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = ClientDataGrid.SelectedItem as ClientClass;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Client";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        Clients.Remove(del);
        ClientDataGrid.ItemsSource = Clients;
    }

    // Реализован поиск по столбцу Surname
    private void SearchBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var SearchName = new List<ClientClass>(Clients);
        SearchName = SearchName.Where(x => x.ClientSurname.Contains(searchBox.Text)).ToList();
        ClientDataGrid.ItemsSource = SearchName;
    }
}
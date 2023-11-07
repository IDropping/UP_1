using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP_1.UP;

namespace UP_1.Windows;

public partial class FinancialAccounting1 : Window
{
    private ObservableCollection<FinancialAccountingClass> financing { get; set; }
    private MySqlConnectionStringBuilder _connection;
    public FinancialAccounting1()
    {
        InitializeComponent();
        financing = new ObservableCollection<FinancialAccountingClass>();
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
                command.CommandText = "SELECT * FROM Financial_Accounting";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        financing.Add(new FinancialAccountingClass()
                        {
                            IDFinance = reader.GetInt32("ID"),
                            FinanceClient = reader.GetInt32("клиент"),
                            FinanceCourse = reader.GetString("курс"),
                            SumToPay = reader.GetInt32("сумма_к_оплате"),
                            DatePay = reader.GetString("дата_оплаты")
                        });
                    }
                }
            }
            connection.Close();
        }
        FinanceDataGrid.ItemsSource = financing;
    }

    private void AddFinance_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        {
            cnn.Open();
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Financial_Accounting (ID, клиент, курс, сумма_к_оплате, дата_оплаты)" +
                                  "VALUES ('" + Convert.ToInt32(idFinanceBox.Text) + 
                                  "','" + Convert.ToInt32(clientFinanceBox.Text) +
                                  "','" + courseFinanceBox.Text + 
                                  "','" + Convert.ToInt32(sumBox.Text) + 
                                  "','" + payBox.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                    financing.Add(new FinancialAccountingClass()
                    {
                        IDFinance = Convert.ToInt32(idFinanceBox.Text),
                        FinanceClient = Convert.ToInt32(clientFinanceBox.Text),
                        FinanceCourse = courseFinanceBox.Text,
                        SumToPay = Convert.ToInt32(sumBox.Text),
                        DatePay = payBox.Text
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

    private void DelFinance_OnClick(object? sender, RoutedEventArgs e)
    {
        var del = FinanceDataGrid.SelectedItem as FinancialAccountingClass;
        using (var cnn = new MySqlConnection(_connection.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Financial_Accounting";
            cnn.Open();
            var delete = cmd.ExecuteNonQuery();
        }
        financing.Remove(del);
        FinanceDataGrid.ItemsSource = financing;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using ScottPlot.Avalonia;
using UP_1.UP;

namespace UP_1.Windows;

public partial class Gistogram : Window
{
    private MySqlConnectionStringBuilder _connection;
    public Gistogram()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

   /* private void LoadDataAndPlot()
    {
        _connection = new MySqlConnectionStringBuilder()
        {
            Server = "10.10.1.24",
            Database = "pro1_3",
            UserID = "user_01",
            Password = "user01pro"
        };
        using (MySqlConnection connection = new MySqlConnection(_connection.ConnectionString))
        {
            connection.Open();
            string query = "SELECT VALUE FROM Attendance";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            var values = new List<double>();
            while (reader.Read())
            {
                double value = reader.GetDouble(0);
                values.Add(value);
            }

            AvaPlot avaPlot1 = this.Find<AvaPlot>("plotView");
            avaPlot1.Plot.PlotBar(values.ToArray());
            avaPlot1.Plot.Title("Histogram");
            avaPlot1.Plot.XLabel("Value");
            avaPlot1.Plot.YLabel("Count");
        }
    }*/
}
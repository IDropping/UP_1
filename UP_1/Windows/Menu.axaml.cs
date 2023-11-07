using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using UP_1.UP;
using UP_1.Windows;

namespace UP_1;

public partial class Menu : Window
{
    public Menu()
    {
        InitializeComponent();
    }

    // Для каждой кнопки описан переход к другому окну
    private void ClientChange_OnClick(object? sender, RoutedEventArgs e)
    {
        // Скрыть основное меню
        new Menu().Hide();
        
        // Открыть новое окно
        new Client1().Show(this);
    }

    private void CoursesChange_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new Courses1().Show(this);
    }

    private void FinanceChange_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new FinancialAccounting1().Show(this);
    }

    private void GroupsChange_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new Groups1().Show(this);
    }

    private void AddExpirience_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new AddExp1().Show(this);
    }

    private void AddLanguage_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new AddLanguage1().Show(this);
    }

    private void AddSchedule_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new AddSchedule1().Show(this);
    }

    private void AddTeacher_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new AddTeacher1().Show(this);
    }

    private void AddAttendance_OnClick(object? sender, RoutedEventArgs e)
    {
        new Menu().Hide();
        new Attendance1().Show(this);
    }
}
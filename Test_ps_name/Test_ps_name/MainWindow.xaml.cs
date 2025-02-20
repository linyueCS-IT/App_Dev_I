using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace Test_ps_name
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Clicked(object sender, RoutedEventArgs e)
        {
            string cs = @"URI=file:C:\sqlite\chinook.db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            string employeeId = txtId.Text ;
            string employeeFirstname = txtFirstName.Text ;
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = @$"SELECT * FROM employees WHERE EmployeeId=’ + {employeeId} + ’";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr}");
            }

            if (employeeId == cmd.CommandText)
            {
                MessageBox.Show("Login Successful!");
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid employee Id, please try again!");
            }
        }
    }
}
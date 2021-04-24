using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace TermProject
{
    /// <summary>
    /// Interaction logic for Checkins.xaml
    /// </summary>
    public partial class Checkins : Window
    {
        private string bid = "";
        private string uid = "";
        public Checkins(string bid, string bname, string uid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            this.Title = "Checkins for " + bname;
            this.uid = uid;
            LoadGraphData();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb2; password=11587750";
        }

        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            myf(reader);
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void SetData(NpgsqlDataReader R)
        {
            string month = R.GetString(0);
            if (month == "01")
            {
                January.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "02")
            {
                February.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "03")
            {
                March.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "04")
            {
                April.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "05")
            {
                May.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "06")
            {
                June.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "07")
            {
                July.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "08")
            {
                August.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "09")
            {
                September.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "10")
            {
                October.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "11")
            {
                November.Height = R.GetDouble(1) * 5.0;
            }
            else if (month == "12")
            {
                December.Height = R.GetDouble(1) * 5.0;
            }
        }

        private void LoadGraphData()
        {
            string sqlstr = "select checkinmonth, Cast (count(checkinmonth) as float) from checkin where business_id = '" + this.bid + "' GROUP BY checkinmonth ORDER BY checkinmonth";
            executeQuery(sqlstr, SetData);
        }

        private void InsertCheckin(NpgsqlDataReader R)
        {
        }

        private void CheckinButton_Click(object sender, RoutedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            string day = DateTime.Now.ToString("dd");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string sqlstr = "INSERT INTO checkin(business_id, checkinyear, checkinmonth, checkinday, checkintime) VALUES('" + this.bid + "','" + year + "','" + month + "','" + day + "','" + time + "');";
            executeQuery(sqlstr, InsertCheckin);
            LoadGraphData();
        }
    }
}

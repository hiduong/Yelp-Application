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
    /// Interaction logic for Tips.xaml
    /// </summary>
    public partial class Tips : Window
    { 
        public class BusinessTips
        {
            public string date { get; set; }
            public string name { get; set; }
            public string likes { get; set; }
            public string text { get; set; }
            public string tipUid { get; set; }
        }

        private string bid = "";
        private string uid = "";
        public Tips(string bid, string bname, string uid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            this.Title = "Tips by Users for " + bname;
            this.uid = uid;
            AddColumns2Grid();
            LoadTips();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdbA; password=Potass1osql";
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

        private void executeUpdate(string sqlstr)
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
                        var updater = cmd.ExecuteNonQuery();
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

        private void AddColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Date";
            col1.Binding = new Binding("date");
            col1.Width = 140;
            TipsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "User Name";
            col2.Binding = new Binding("name");
            col2.Width = 100;
            TipsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Likes";
            col3.Binding = new Binding("likes");
            col3.Width = 50;
            TipsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = 600;
            TipsGrid.Columns.Add(col4);
        }

        private void AddTipGrid(NpgsqlDataReader R)
        {
            TipsGrid.Items.Add(new BusinessTips() { date = R.GetDateTime(0).ToString(), name = R.GetString(1).ToString(), likes = R.GetInt16(2).ToString(), text = R.GetString(3).ToString(), tipUid = R.GetString(4) });
        }

        private void LoadTips()
        {
            string sqlstr = "SELECT tipdate, username, likecount, tiptext, user_id FROM (SELECT business_id, tipdate, username, likecount, tiptext, tip.user_id FROM yelpuser, tip WHERE yelpuser.user_id = tip.user_id) temp WHERE business_id='" + this.bid +  "' ORDER BY likecount DESC, username";
            executeQuery(sqlstr, AddTipGrid);
        }

        private void InsertTip(NpgsqlDataReader R)
        {
        }

        private void AddTipButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlstr = "INSERT INTO tip(business_id, tipdate, likecount, tiptext, user_id) VALUES('"+this.bid+ "',CURRENT_TIMESTAMP,0,'" + TextBox.Text.ToString()+"','"+this.uid+"');";
            executeQuery(sqlstr, InsertTip);
            TipsGrid.Items.Clear();
            LoadTips();
        }

        private void LikeTipButton_Click(object sender, RoutedEventArgs e)
        {

            // Need Current user - uid is userid
            // Also need to check if a tip is selected

            // Simply update the tip like count, trigger will handle user tiplike count
            
            if (TipsGrid.SelectedIndex > -1)
            {
                BusinessTips T = TipsGrid.Items[TipsGrid.SelectedIndex] as BusinessTips;
                string sqlstr = "UPDATE tip SET likecount = likecount + 1 WHERE business_id = '" + bid + "' AND user_id = '" + T.tipUid + "' AND tipdate = '" + T.date + "'";
                // Turns out this doesn't work if you have your system time set to military time ONLY FOR new tips
                //System.Windows.MessageBox.Show(sqlstr);
                executeUpdate(sqlstr);
            }

            // DO NOT NEED TO RESET MAIN VIEW, no like count there
            //update views?
            //MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            //wnd.SearchButton_Click(sender, e);
            TipsGrid.Items.Clear();
            LoadTips();
        }
    }
}

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
    /// Interaction logic for UserInformation.xaml
    /// </summary>
    public partial class UserInformation : Window
    {

        // NameBox : to edit name text box
        // StarsBox : to edit stars text box
        // FansBox : to edit fans text box
        // YelpingSinceBox : to edit yelping since box
        // Votes - FunnyBox, CoolBox, UsefulBox
        // TipCountBox
        // TotalTipsLikeBox
        // LatBox
        // LongBox
        // FriendsDataGrid
        // FriendsLatestTipsDataGrid

        // user id of the current user
        // use this for queries related to the current user
        private string uid = "FgQCX3ztjhellw2hyRedxg";


        // TODO : Implement Friends class
        // properties same as user
        public class Friends
        {
            public string user_id { get; set; }
            public string username { get; set; }
            public DateTime joindate { get; set; }
            public int tipcount { get; set; }
            public int tiplikecount { get; set; }
            public int votecount { get; set; }
            public int fancount { get; set; }
            public double averagestars { get; set; }
            public float lattitude { get; set; }
            public float longitudet { get; set; }
            public string latesttip { get; set; }
            public int funny { get; set; }
            public int cool { get; set; }
            public int useful { get; set; }
        }


        // TODO : Implement Friends Tips class
        // properties : username, business, text, date all strings
        public class FriendsTips
        {
            public string username { get; set; }
            public string business { get; set; }
            public string city { get; set; }
            public string text { get; set; }
            public string date { get; set; }
        }

        public UserInformation(string uid)
        {
            InitializeComponent();
            this.uid = uid;
            this.Title = "User Information";
            SetUserInformation();
            AddColumns2FriendGrid();
            AddColumns2FriendLatestTipsGrid();
            AddFriends();
            AddFriendsLatestTips();
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

        // TODO : Implement method to add columns to the friends data grid
        private void AddColumns2FriendGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("username");
            col1.Width = 80;
            FriendsDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "TotalLikes";
            col2.Binding = new Binding("tiplikecount");
            col2.Width = 80;
            FriendsDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "AvgStars";
            col3.Binding = new Binding("averagestars");
            col3.Width = 80;
            FriendsDataGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Yelping Since";
            col4.Binding = new Binding("joindate");
            col4.Width = 200;
            FriendsDataGrid.Columns.Add(col4);
        }


        // TODO : Implement method to add columns to the friends latest tips data grid
        private void AddColumns2FriendLatestTipsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("username");
            col1.Width = 80;
            FriendsLatestTipsDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("business");
            col2.Width = 150;
            FriendsLatestTipsDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 80;
            FriendsLatestTipsDataGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = 200;
            FriendsLatestTipsDataGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Date";
            col5.Binding = new Binding("date");
            col5.Width = 200;
            FriendsLatestTipsDataGrid.Columns.Add(col5);
        }


        // TODO: Imeplement method that populates the friends data grid with the query results
        private void AddFriendsHelper(NpgsqlDataReader R)
        {
            FriendsDataGrid.Items.Add(new Friends() { username = R.GetString(0), tiplikecount = R.GetInt32(1), averagestars = R.GetDouble(2), joindate = R.GetDateTime(3) });
        }

        // TODO : Implement method to query the friends of the current user id
        // Execute query with AddFriendsHelper
        private void AddFriends()
        {
            string sqlstr = "SELECT friend.username, friend.tiplikecount, friend.averagestars, friend.joindate FROM friendship INNER JOIN yelpuser friender ON friender.user_id = friender_id INNER JOIN yelpuser friend ON friend.user_id = friend_id WHERE friender.user_id = '" + uid + "\'";
            executeQuery(sqlstr, AddFriendsHelper);

        }

        // TODO: Imeplement method that populates the friends latest tips data grid with the query results
        private void AddFriendsLatestTipsHelper(NpgsqlDataReader R)
        {
            FriendsLatestTipsDataGrid.Items.Add(new FriendsTips() { username = R.GetString(0), business = R.GetString(1), city = R.GetString(2), text = R.GetString(4), date = R.GetTimeStamp(3).ToString() });
        }

        // TODO : Implement method to query the friends latest tips of the current user id
        // Execute query with AddFriendsHelper
        private void AddFriendsLatestTips()
        {
            string sqlstr = "select username, businessname, city, tipdate, tiptext from(select * from(select * from(select * from friendship where friender_id = '" + uid + "') temp, yelpuser where user_id = friend_id and latesttipdate IS NOT Null and latesttipbusiness IS NOT Null) temp2, tip where tip.user_id = friend_id and latesttipbusiness = tip.business_id and latesttipdate = tipdate) temp3, business where temp3.business_id = business.business_id";
            executeQuery(sqlstr, AddFriendsLatestTipsHelper);
        }

        // TODO : Get the result from the returned query and set results to the text boxes
        private void SetUser(NpgsqlDataReader R)
        {
            NameBox.Text = R.GetString(1);
            StarsBox.Text = R.GetDouble(7).ToString();
            FansBox.Text = R.GetInt32(6).ToString();
            YelpingSinceBox.Text = R.GetDateTime(2).ToString();
            FunnyBox.Text = R.GetInt32(10).ToString();
            CoolBox.Text = R.GetInt32(11).ToString();
            UsefulBox.Text = R.GetInt32(12).ToString();
            TipCountBox.Text = R.GetInt32(3).ToString();
            TotalTipsLikeBox.Text = R.GetInt32(4).ToString();
            LatBox.Text = R.GetDouble(8).ToString();
            LongBox.Text = R.GetDouble(9).ToString();
        }

        // TODO : Query with the user id and change the text of the text boxes with the result
        // Execute Query SetUser to obtain the results and set textboxes
        private void SetUserInformation()
        {
            string sqlstr = "SELECT * FROM yelpuser WHERE user_id = '" + this.uid + "\'";
            executeQuery(sqlstr, SetUser);
        }

        // TODO : Update the table to include the new lat and long
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            double temp = 0.0;
            if (double.TryParse(LongBox.Text, out temp) == true && double.TryParse(LatBox.Text, out temp) == true)
            {
                string sqlstr = "UPDATE yelpuser SET latitude = '" + LatBox.Text + "', longitude = '" + LongBox.Text + "' WHERE user_id ='" + this.uid + "'";
                executeQuery(sqlstr, null);
            }
        }
    }
}

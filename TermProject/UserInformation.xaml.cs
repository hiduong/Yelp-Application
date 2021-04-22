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
        private string uid = "";


        // TODO : Implement Friends class
        // properties same as user
        public class Friends
        {
        }


        // TODO : Implement Friends Tips class
        // properties : username, business, text, date all strings
        public class FriendsTips
        {
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
        }


        // TODO : Implement method to add columns to the friends latest tips data grid
        private void AddColumns2FriendLatestTipsGrid()
        {
        }


        // TODO: Imeplement method that populates the friends data grid with the query results
        private void AddFriendsHelper(NpgsqlDataReader R)
        {
        }

        // TODO : Implement method to query the friends of the current user id
        // Execute query with AddFriendsHelper
        private void AddFriends()
        {
        }

        // TODO: Imeplement method that populates the friends latest tips data grid with the query results
        private void AddFriendsLatestTipsHelper(NpgsqlDataReader R)
        {
        }

        // TODO : Implement method to query the friends latest tips of the current user id
        // Execute query with AddFriendsHelper
        private void AddFriendsLatestTips()
        {
        }


        // TODO : Get the result from the returned query and set results to the text boxes
        private void SetUser(NpgsqlDataReader R)
        {
        }

        // TODO : Query with the user id and change the text of the text boxes with the result
        // Execute Query SetUser to obtain the results and set textboxes
        private void SetUserInformation()
        {
        }


        // TODO : Update the table to include the new lat and long
        private void Update_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}

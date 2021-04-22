﻿using System;
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

        // TODO : Implement method to add columns to the friends data grid
        private void AddColumns2FriendGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 80;
            FriendsDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "TotalLikes";
            col2.Binding = new Binding("totallikes");
            col2.Width = 80;
            FriendsDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "AvgStars";
            col3.Binding = new Binding("avgstars");
            col3.Width = 80;
            FriendsDataGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Yelping Since";
            col4.Binding = new Binding("yelpingsince");
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

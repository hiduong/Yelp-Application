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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace TermProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            AddState();
            AddColumns2Grid();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb; password=11587750";
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

        private void AddColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "BusinessName";
            col1.Binding = new Binding("name");
            col1.Width = 255;
            ResultsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "City";
            col2.Binding = new Binding("city");
            col2.Width = 150;
            ResultsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "State";
            col3.Binding = new Binding("state");
            col3.Width = 80;
            ResultsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "";
            col4.Binding = new Binding("bid");
            col4.Width = 200;
            ResultsGrid.Columns.Add(col4);
        }

        private void AddState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            StateList.Items.Add(reader.GetString(0));
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

        private void AddCity(NpgsqlDataReader R)
        {
            CityBox.Items.Add(R.GetString(0));
        }

        private void StateList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            CityBox.Items.Clear();
            AddedListBox.Items.Clear();
            if (StateList.SelectedIndex > -1)
            {
                string sqlstr = "SELECT distinct city FROM business WHERE state = '" + StateList.SelectedItem.ToString() + "' ORDER BY city";
                executeQuery(sqlstr, AddCity);
            }
        }

        private void AddZipcode(NpgsqlDataReader R)
        {
            ZipcodeListBox.Items.Add(R.GetString(0));
        }

        private void CityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ZipcodeListBox.Items.Clear();
            AddedListBox.Items.Clear();
            if (CityBox.SelectedIndex > -1)
            {
                string sqlstr = "SELECT distinct zipcode FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' ORDER BY zipcode";
                executeQuery(sqlstr, AddZipcode);
            }
        }


        private void AddCategory(NpgsqlDataReader R)
        {
            CategoryListBox.Items.Add(R.GetString(0));
        }

        private void ZipcodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryListBox.Items.Clear();
            AddedListBox.Items.Clear();
            if (ZipcodeListBox.SelectedIndex > -1)
            {
                string sqlstr = "SELECT distinct categoryname FROM (SELECT distinct business_id FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, hascategory WHERE temp.business_id=hascategory.business_id ORDER BY categoryname";
                executeQuery(sqlstr, AddCategory);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedListBox.Items.Contains(CategoryListBox.SelectedItem.ToString()) == false)
            {
                AddedListBox.Items.Add(CategoryListBox.SelectedItem.ToString());
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryListBox.SelectedIndex > -1)
            {
                if(AddedListBox.Items.Contains(CategoryListBox.SelectedItem.ToString()) == true)
                {
                    AddedListBox.Items.Remove(CategoryListBox.SelectedItem.ToString());
                }        
            }
        }

        private void AddGridRow(NpgsqlDataReader R)
        {
            ResultsGrid.Items.Add(new Business() { name = R.GetString(0), city = R.GetString(1), state = R.GetString(2), bid = R.GetString(3) });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            if (StateList.SelectedIndex > -1 && CityBox.SelectedIndex > -1 && ZipcodeListBox.SelectedIndex > -1)
            {
                ResultsGrid.Items.Clear();
                if (AddedListBox.Items.Count > 0)
                {
                    string temp = "";
                    foreach(string s in AddedListBox.Items)
                    {
                        temp = temp + "categoryname='" + s + "'" + " AND ";
                    }
                    string categoryquery = temp.Remove(temp.Length - 5, 5);
                    string sqlstr = "SELECT distinct businessname, city, state, temp.business_id FROM (SELECT distinct * FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, hascategory WHERE temp.business_id=hascategory.business_id AND " + categoryquery;
                    executeQuery(sqlstr, AddGridRow);
                }
                else
                {
                    string sqlstr = "SELECT distinct businessname, city, state, temp.business_id FROM (SELECT distinct * FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, hascategory WHERE temp.business_id=hascategory.business_id";
                    executeQuery(sqlstr, AddGridRow);
                }
            }
        }
    }
}

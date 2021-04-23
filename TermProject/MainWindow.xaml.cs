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
        private string currentUserName = "Lee Ann";
        private string currentUserID = "FgQCX3ztjhellw2hyRedxg";

        private bool[] priceFilters;
        private bool[] attributeFilters;
        private bool[] mealFilters;

        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string address { get; set; }
            public string zipcode { get; set; }
            public string distance { get; set; }
            public string rating { get; set; }
            public string tipcount { get; set; }
            public string checkincount { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            AddState();
            AddColumns2Grid();
            populateSortMethods();

            priceFilters = new bool[4];
            initBoolsFalse(priceFilters);
            attributeFilters = new bool[10];
            initBoolsFalse(attributeFilters);
            mealFilters = new bool[6];
            initBoolsFalse(mealFilters);
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

        private void AddColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "BusinessName";
            col1.Binding = new Binding("name");
            col1.Width = 201;
            ResultsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            col2.Width = 200;
            ResultsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 135;
            ResultsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");
            col4.Width = 50;
            ResultsGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Zipcode";
            col5.Binding = new Binding("zipcode");
            col5.Width = 60;
            ResultsGrid.Columns.Add(col5);

            DataGridTextColumn colDist = new DataGridTextColumn();
            colDist.Header = "Distance (mi.)";
            colDist.Binding = new Binding("distance");
            colDist.Width = 100;
            ResultsGrid.Columns.Add(colDist);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Stars";
            col6.Binding = new Binding("rating");
            col6.Width = 40;
            ResultsGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "# of Tips";
            col7.Binding = new Binding("tipcount");
            col7.Width = 60;
            ResultsGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Total\nCheckins";
            col8.Binding = new Binding("checkincount");
            col8.Width = 60;
            ResultsGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "";
            col9.Binding = new Binding("bid");
            col9.Width = 200;
            ResultsGrid.Columns.Add(col9);
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
            if (CategoryListBox.SelectedIndex > -1)
            {
                if (AddedListBox.Items.Contains(CategoryListBox.SelectedItem.ToString()) == false)
                {
                    AddedListBox.Items.Add(CategoryListBox.SelectedItem.ToString());
                }
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
            ResultsGrid.Items.Add(new Business() { name = R.GetString(0), address = R.GetString(1), city = R.GetString(2), state = R.GetString(3), zipcode = R.GetString(4), distance = R.GetDouble(5).ToString(), rating = R.GetDouble(6).ToString(), tipcount = R.GetInt16(7).ToString(), checkincount = R.GetInt16(8).ToString(), bid = R.GetString(9) });
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
                        if (s.Contains("'") == true)
                        {
                            string t = s.Replace("'", "''");
                            temp = temp + "categories LIKE '%" + t + "%'" + " AND ";
                        }
                        else
                        {
                            temp = temp + "categories LIKE '%" + s + "%'" + " AND ";
                        }
                    }
                    string categoryquery = temp.Remove(temp.Length - 5, 5);
                    //Euclidean
                    //string sqlstr = "SELECT distinct business.businessname, address, city, state, zipcode, ROUND(CAST(SQRT(POWER((business.latitude - my_user.latitude) * 69.2, 2) + POWER((business.longitude - my_user.longitude) * 69.2, 2)) as numeric), 3) as distance, rating, tipcount, checkincount, business.business_id FROM (SELECT distinct businessname, array_to_string(array_agg(categoryname), ' , ') AS categories FROM(SELECT distinct * FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, hascategory WHERE temp.business_id = hascategory.business_id GROUP BY businessname) temp1, (SELECT latitude, longitude FROM yelpuser WHERE username = '" + currentUserName + "' AND user_id = '" + currentUserID + "') my_user, business WHERE temp1.businessname = business.businessname AND city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "' AND " + categoryquery;
                    //Haversine
                    string sqlstr = "SELECT distinct business.businessname, address, city, state, zipcode, ROUND(CAST(3958.8 * 2.0 * ASIN(SQRT(POWER(SIN(RADIANS(business.latitude - my_user.latitude) / 2.0), 2) + COS(RADIANS(business.latitude)) * COS(RADIANS(my_user.latitude)) * POWER(SIN(RADIANS(business.longitude - my_user.longitude) / 2.0), 2))) as numeric), 3) as distance, rating, tipcount, checkincount, business.business_id FROM (SELECT distinct businessname, array_to_string(array_agg(categoryname), ' , ') AS categories FROM(SELECT distinct * FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, hascategory WHERE temp.business_id = hascategory.business_id GROUP BY businessname) temp1, (SELECT latitude, longitude FROM yelpuser WHERE username = '" + currentUserName + "' AND user_id = '" + currentUserID + "') my_user, business WHERE temp1.businessname = business.businessname AND city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "' AND " + categoryquery + getAttributeSelections(true) + " " + getBusinessSort(true);
                    executeQuery(sqlstr, AddGridRow);
                }
                else
                {
                    //Euclidean
                    //string sqlstr = "SELECT distinct businessname, address, city, state, zipcode, ROUND(CAST(SQRT(POWER((temp.latitude - my_user.latitude) * 69.2, 2) + POWER((temp.longitude - my_user.longitude) * 69.2, 2)) as numeric), 3) as distance, rating, tipcount, checkincount, temp.business_id FROM (SELECT distinct * FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, (SELECT latitude, longitude FROM yelpuser WHERE username = '" + currentUserName + "' AND user_id = '" + currentUserID + "') my_user, hascategory WHERE temp.business_id=hascategory.business_id";
                    //Haversine
                    string sqlstr = "SELECT distinct businessname, address, city, state, zipcode, ROUND(CAST(3958.8 * 2.0 * ASIN(SQRT(POWER(SIN(RADIANS(temp.latitude - my_user.latitude) / 2.0), 2) + COS(RADIANS(temp.latitude)) * COS(RADIANS(my_user.latitude)) * POWER(SIN(RADIANS(temp.longitude - my_user.longitude) / 2.0), 2))) as numeric), 3) as distance, rating, tipcount, checkincount, temp.business_id FROM (SELECT distinct * FROM business WHERE city = '" + CityBox.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND zipcode = '" + ZipcodeListBox.SelectedItem.ToString() + "') temp, (SELECT latitude, longitude FROM yelpuser WHERE username = '" + currentUserName + "' AND user_id = '" + currentUserID + "') my_user, hascategory WHERE temp.business_id=hascategory.business_id" + getAttributeSelections(false) + " " + getBusinessSort(false);
                    executeQuery(sqlstr, AddGridRow);
                }
            }
        }

        private void ResultsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsGrid.SelectedIndex > -1)
            {
                Business B = ResultsGrid.Items[ResultsGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    Tips tipsWindow = new Tips(B.bid.ToString(), B.name.ToString(), this.currentUserID);
                    tipsWindow.Show();
                    ResultsGrid.SelectedIndex = -1;
                }
            }
        }

        private void AddUserID(NpgsqlDataReader R)
        {
            UserIDBox.Items.Add(R.GetString(0));
        }

        // handler to handle text change for searching user by name
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            UserIDBox.Items.Clear();
            string sqlstr = "SELECT user_id FROM yelpuser WHERE username = '" + SearchUserBox.Text + "'";
            executeQuery(sqlstr, AddUserID);
        }


        private void SetUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserIDBox.SelectedIndex > -1)
            {
                this.currentUserID = UserIDBox.SelectedItem.ToString();
                CurrentUserID.Content = this.currentUserID;
                this.currentUserName = SearchUserBox.Text;
                CurrentUserName.Content = this.currentUserName;

                SearchButton_Click(sender, e);
            }
        }

        private void ViewUserInfo_Click(object sender, RoutedEventArgs e)
        {
            UserInformation UserWindow = new UserInformation(this.currentUserID);
            UserWindow.Show();
        }

        private void populateSortMethods()
        {
            businessSortComboBox.Items.Add("Business Name");
            businessSortComboBox.Items.Add("Highest Rating");
            businessSortComboBox.Items.Add("Most Tips");
            businessSortComboBox.Items.Add("Most Check-Ins");
            businessSortComboBox.Items.Add("Nearest");

            businessSortComboBox.SelectedValue = "Business Name";
        }

        private string getPriceFilters()
        {
            //Check through tick boxes and append a WHERE condition for each ticked
            return "";
        }

        private string getAttributeFilters()
        {
            return "";
        }

        private string getMealFilters()
        {
            return "";
        }

        private string getBusinessSort(bool categoryQuery)
        {
            //Check the drop down and return sorting method for ORDER BY()

            string result = "ORDER BY ";

            bool bFlag = false;

            switch (businessSortComboBox.SelectedValue)
            {
                case "Business Name":
                    bFlag = true;
                    if (categoryQuery)
                    {
                        result += "business.businessname ASC";
                    } else
                    {
                        result += "businessname ASC";
                    }
                    break;
                case "Highest Rating":
                    result += "rating DESC";
                    break;
                case "Most Tips":
                    result += "tipcount DESC";
                    break;
                case "Most Check-Ins":
                    result += "checkincount DESC";
                    break;
                case "Nearest":
                    result += "distance ASC";
                    break;
            }

            if (!bFlag)
            {
                if (categoryQuery)
                    result += ", business.businessname ASC";
                else
                    result += ", businessname ASC";
            }

            return result;
        }

        private void businessSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchButton_Click(sender, e);
        }


        private static void initBoolsFalse(bool[] boolArr)
        {
            for(int i = 0; i < boolArr.Length; i++)
            {
                boolArr[i] = false;
            }
        }

        private string getAttributeSelections(bool categoryQuery)
        {
            string result = " AND ";

            string businessRef;

            if (categoryQuery)
            {
                businessRef = "business.business_id";
            } else
            {
                businessRef = "temp.business_id";
            }

            result += businessRef + " IN (SELECT business_id FROM business NATURAL JOIN attribute WHERE attributename = ";

            bool hasAttributeFilter = false;

            int index = 0;

            foreach (bool b in priceFilters)
            {

                if (b)
                {
                    if (hasAttributeFilter)
                    {
                        result += " GROUP BY business_id) AND " + businessRef + " IN (SELECT business_id FROM business NATURAL JOIN attribute WHERE attributename = ";
                    }
                    switch (index)
                    {
                        case 0:
                            result += "'RestaurantsPriceRange1'";
                            break;
                        case 1:
                            result += "'RestaurantsPriceRange2'";
                            break;
                        case 2:
                            result += "'RestaurantsPriceRange3'";
                            break;
                        case 3:
                            result += "'RestaurantsPriceRange4'";
                            break;
                    }
                    hasAttributeFilter = true;
                }
                index++;
            }
            index = 0;
            foreach (bool b in attributeFilters)
            {
                if (b)
                {
                    if (hasAttributeFilter)
                    {
                        result += " GROUP BY business_id) AND " + businessRef + " IN (SELECT business_id FROM business NATURAL JOIN attribute WHERE attributename = ";
                    }
                    switch (index)
                    {
                        case 0:
                            result += "'BusinessAcceptsCreditCards'";
                            break;
                        case 1:
                            result += "'RestaurantsReservations'";
                            break;
                        case 2:
                            result += "'WheelchairAccessible'";
                            break;
                        case 3:
                            result += "'OutdoorSeating'";
                            break;
                        case 4:
                            result += "'GoodForKids'";
                            break;
                        case 5:
                            result += "'RestaurantsGoodForGroups'";
                            break;
                        case 6:
                            result += "'RestaurantsDelivery'";
                            break;
                        case 7:
                            result += "'RestaurantsTakeOut'";
                            break;
                        case 8:
                            result += "'WiFi'";
                            break;
                        case 9:
                            result += "'BikeParking'";
                            break;
                    }
                    hasAttributeFilter = true;
                }
                index++;
            }
            index = 0;
            foreach (bool b in mealFilters)
            {
                if (b)
                {
                    if (hasAttributeFilter)
                    {
                        result += " GROUP BY business_id) AND " + businessRef + " IN (SELECT business_id FROM business NATURAL JOIN attribute WHERE attributename = ";
                    }
                    switch (index)
                    {
                        case 0:
                            result += "'breakfast'";
                            break;
                        case 1:
                            result += "'lunch'";
                            break;
                        case 2:
                            result += "'brunch'";
                            break;
                        case 3:
                            result += "'dinner'";
                            break;
                        case 4:
                            result += "'dessert'";
                            break;
                        case 5:
                            result += "'latenight'";
                            break;
                    }
                    hasAttributeFilter = true;
                }
                index++;
            }

            if (!hasAttributeFilter)
            {
                return "";
            }

            result += " GROUP BY business_id)";
            
            return result;

        }

        // Price Checkbox Handlers
        private void price1Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            priceFilters[0] = true;
            SearchButton_Click(sender, e);
        }

        private void price1Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            priceFilters[0] = false;
            SearchButton_Click(sender, e);
        }

        private void price2Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            priceFilters[1] = true;
            SearchButton_Click(sender, e);
        }

        private void price2Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            priceFilters[1] = false;
            SearchButton_Click(sender, e);
        }

        private void price3Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            priceFilters[2] = true;
            SearchButton_Click(sender, e);
        }

        private void price3Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            priceFilters[2] = false;
            SearchButton_Click(sender, e);
        }

        private void price4Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            priceFilters[3] = true;
            SearchButton_Click(sender, e);
        }

        private void price4Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            priceFilters[3] = false;
            SearchButton_Click(sender, e);
        }

        private void creditcardCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[0] = true;
            SearchButton_Click(sender, e);
        }

        private void creditcardCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[0] = false;
            SearchButton_Click(sender, e);
        }

        private void reservationCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[1] = true;
            SearchButton_Click(sender, e);
        }

        private void reservationCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[1] = false;
            SearchButton_Click(sender, e);
        }

        private void wheelchairCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[2] = true;
            SearchButton_Click(sender, e);
        }

        private void wheelchairCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[2] = false;
            SearchButton_Click(sender, e);
        }

        private void outdoorCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[3] = true;
            SearchButton_Click(sender, e);
        }

        private void outdoorCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[3] = false;
            SearchButton_Click(sender, e);
        }

        private void goodForKidsCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[4] = true;
            SearchButton_Click(sender, e);
        }

        private void goodForKidsCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[4] = false;
            SearchButton_Click(sender, e);
        }

        private void goodForGroupsCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[5] = true;
            SearchButton_Click(sender, e);
        }

        private void goodForGroupsCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[5] = false;
            SearchButton_Click(sender, e);
        }

        private void deliveryCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[6] = true;
            SearchButton_Click(sender, e);
        }

        private void deliveryCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[6] = false;
            SearchButton_Click(sender, e);
        }

        private void takeOutCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[7] = true;
            SearchButton_Click(sender, e);
        }

        private void takeOutCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[7] = false;
            SearchButton_Click(sender, e);
        }

        private void freeWifiCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[8] = true;
            SearchButton_Click(sender, e);
        }

        private void freeWifiCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[8] = false;
            SearchButton_Click(sender, e);
        }

        private void bikeParkingCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters[9] = true;
            SearchButton_Click(sender, e);
        }

        private void bikeParkingCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            attributeFilters[9] = false;
            SearchButton_Click(sender, e);
        }

        private void breakfastCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            mealFilters[0] = true;
            SearchButton_Click(sender, e);
        }

        private void breakfastCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            mealFilters[0] = false;
            SearchButton_Click(sender, e);
        }

        private void lunchCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            mealFilters[1] = true;
            SearchButton_Click(sender, e);
        }

        private void lunchCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            mealFilters[1] = false;
            SearchButton_Click(sender, e);
        }

        private void brunchCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            mealFilters[2] = true;
            SearchButton_Click(sender, e);
        }

        private void brunchCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            mealFilters[2] = false;
            SearchButton_Click(sender, e);
        }

        private void dinnerCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            mealFilters[3] = true;
            SearchButton_Click(sender, e);
        }

        private void dinnerCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            mealFilters[3] = false;
            SearchButton_Click(sender, e);
        }

        private void dessertCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            mealFilters[4] = true;
            SearchButton_Click(sender, e);
        }

        private void dessertCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            mealFilters[4] = false;
            SearchButton_Click(sender, e);
        }

        private void lateNightCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            mealFilters[5] = true;
            SearchButton_Click(sender, e);
        }

        private void lateNightCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            mealFilters[5] = false;
            SearchButton_Click(sender, e);
        }
    }
}

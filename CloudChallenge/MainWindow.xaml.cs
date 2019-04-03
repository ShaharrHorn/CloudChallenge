using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


namespace CloudChallenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string firstQuestionFileName = "Q1.txt";
        public const string secondQuestionFileName = "Q2.txt";

        public MainWindow()
        {
            string response;
            InitializeComponent();
            try
            {
                //---------------Q1------------------------
                response = RequestHandler.readAndSendHttpRequest(firstQuestionFileName);
                List<User> markFollowers = RequestHandler.jsonToUsersArray(response);
                populateListViewWithUsers(markFollowersListView, markFollowers);

                //---------------Q2------------------------
                response = RequestHandler.readAndSendHttpRequest(secondQuestionFileName);
                List<User> userFollowing = RequestHandler.jsonToUsersArray(response);
                populateListViewWithUsers(fictitiousFollowingListView, userFollowing);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        void populateListViewWithUsers(ListView listView, List<User> users)
        {
            foreach (User user in users)
            {
                if (!String.IsNullOrEmpty(user.full_name))
                    listView.Items.Add(user.full_name);
                else
                    listView.Items.Add(user.username);
            }
        }
    }
}
  

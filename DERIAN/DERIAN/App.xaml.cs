using DERIAN.Views;
using DERIAN.ViewTables;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DERIAN
{
    public partial class App : Application
    {

        private static CollectionViewTableController collecontroller;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static CollectionViewTableController ColleController
        {
            get
            {
                if(collecontroller == null)
                {
                    collecontroller = new CollectionViewTableController();
                }
                return collecontroller;
            }
        }
    }
}

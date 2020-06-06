using DERIAN.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public int iduser2;
        public LoginPage()
        {
            SetValue(NavigationPage.HasNavigationBarProperty, false);

            InitializeComponent();

            if (IsTableExists("RegUserTable") == true)
            {
                this.botonRegistro.IsVisible = false;
            }
           }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
 
        }

        async void Handle_Clicked_1(object sender, System.EventArgs e)
        {

            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            if (IsTableExists("RegUserTable") == true)
            {
                var myquery = db.Table<RegUserTable>().Where(u => u.Nombre.Equals(EntryUserName.Text) && u.Pin.Equals(EntryPassword.Text)).FirstOrDefault();

                if (myquery != null)
                {
                    iduser2 = db.Table<RegUserTable>().Where(u => u.Nombre.Equals(EntryUserName.Text) && u.Pin.Equals(EntryPassword.Text)).FirstOrDefault().UserId;


                    App.Current.MainPage = new NavigationPage(new HomePage(iduser2));
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Error!", "Usuario y/o Pin inválidos", "Ok", "Cancelar");

                        /* if (result)
                             await Navigation.PushAsync(new LoginPage());
                         else
                             await Navigation.PushAsync(new LoginPage());
                             */
                    });
                }
            }         

        }
        private bool IsTableExists(string v)
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            try
            {
                var tableInfo = db.GetTableInfo(v);
                if (tableInfo.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DERIAN.Tables;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<RegUserTable>();

            var item = new RegUserTable()
            {
                Nombre = EntryUserName.Text,
                Pin = EntryUserPassword.Text,
                Email = EntryUserEmail.Text,
                Numero = EntryUserNumber.Text,
                is_premium = false
            };

            db.Insert(item);
            Device.BeginInvokeOnMainThread(async () =>
                 {
                     var result = await this.DisplayAlert("Registrado!", "Registro exitoso", "OK", "Cancelar");

                     if (result)
                         App.Current.MainPage = new NavigationPage(new LoginPage());
                 });
        }

    }
}
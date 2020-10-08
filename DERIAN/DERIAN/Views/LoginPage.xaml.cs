using DERIAN.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private int iduser2;
        public LoginPage()
        {
            SetValue(NavigationPage.HasNavigationBarProperty, false);

            InitializeComponent();

            if (IsTableExists("RegUserTable") == true)
            {
                this.botonRegistro.IsVisible = false;
            }
            else
            {
                this.botonRegistro.IsVisible = true;
                this.EntryUserName.IsVisible = false;
                this.EntryPassword.IsVisible = false;
                this.botonAcceder.IsVisible = false;
                this.botonRecuperar.IsVisible = false;
            }
        } 

        private async Task<bool> validarFormulario()
        {
            //Valida si el valor en el Entry se encuentra vacio o es igual a Null
            if (String.IsNullOrWhiteSpace(EntryUserName.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo del nombre es obligatorio.", "OK");
                return false;
            }
            //Valida que solo se ingresen letras
            else if (!EntryUserName.Text.ToCharArray().All(Char.IsLetter))
            {
                await this.DisplayAlert("Advertencia", "Sólo se permiten letras.", "OK");
                return false;
            }
            else
            {
                //Valida si se ingresan caracteres especiales
                string caractEspecial = @"^[^ ][a-zA-Z ]+[^ ]$";
                bool resultado = Regex.IsMatch(EntryUserName.Text, caractEspecial, RegexOptions.IgnoreCase);
                if (!resultado)
                {
                    await this.DisplayAlert("Advertencia", "No se aceptan caracteres especiales, intente de nuevo.", "OK");
                    return false;
                }
            }

               
            if (String.IsNullOrWhiteSpace(EntryPassword.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo de PIN es obligatorio.", "OK");
                return false;
            }
            //Valida si la cantidad de digitos ingresados es menor a 6
            else if (EntryPassword.Text.Length != 6)
            {
                await this.DisplayAlert("Advertencia", "El PIN debe tener 6 digitos.", "OK");
                return false;
            }
            else
            {
                //Valida que solo se ingresen numeros
                if (!EntryPassword.Text.ToCharArray().All(Char.IsDigit))
                {
                    await this.DisplayAlert("Advertencia", "El formato del PIN es incorrecto, solo se aceptan numeros.", "OK");
                    return false;
                }
            }
            return true;
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());

        }
        async void Recuperar_pin(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RecoverPin());

        }

        async void Handle_Clicked_1(object sender, System.EventArgs e)
        {

            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            if (await validarFormulario())
            {
                if (IsTableExists("RegUserTable") == true)
                {
                    var myquery = db.Table<RegUserTable>().Where(u => u.Nombre.Equals(EntryUserName.Text) && u.Pin.Equals(EntryPassword.Text)).FirstOrDefault();

                    if (myquery != null)
                    {
                        iduser2 = db.Table<RegUserTable>().Where(u => u.Nombre.Equals(EntryUserName.Text) && u.Pin.Equals(EntryPassword.Text)).FirstOrDefault().UserId;
                       // App.Current.MainPage = new NavigationPage(new HomePage(iduser2));


                       App.Current.MainPage = new MainShell(iduser2);
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
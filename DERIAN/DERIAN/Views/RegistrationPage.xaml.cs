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
using System.Text.RegularExpressions;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);

            Title = "Registro";
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#262626");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#F29F2C");

        }

        private async Task<bool> validarFormulario()
        {

            //Valida si se ha ingresado un número telefónico
            if (String.IsNullOrWhiteSpace(EntryUserNumber.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo del número celular es obligatorio.", "OK");
                return false;
            }
            //Valida si la cantidad de digitos ingresados es igual o menor a 15
            else if (EntryUserNumber.Text.Length > 15)
            {
                await this.DisplayAlert("Advertencia", "Sobran digitos, favor de ingresar su numero a máximo 15 digitos.", "OK");
                return false;
            }
            else
            {
                //Valida que solo se ingresen numeros
                if (!EntryUserNumber.Text.ToCharArray().All(Char.IsDigit))
                {
                    await this.DisplayAlert("Advertencia", "El formato del celular es incorrecto, solo se aceptan numeros.", "OK");
                    return false;
                }
            }

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
            //Valida si la cantidad de digitos ingresados es igual o menor a 35
            if (EntryUserName.Text.Length > 35)
            {
                await this.DisplayAlert("Advertencia", "Límite de carácteres excedido, favor de ingresar menor a 35 digitos.", "OK");
                return false;
            }


            if (String.IsNullOrWhiteSpace(EntryUserPassword.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo de PIN es obligatorio.", "OK");
                return false;
            }
            //Valida si la cantidad de digitos ingresados es menor a 6
            else if (EntryUserPassword.Text.Length != 6)
            {
                await this.DisplayAlert("Advertencia", "El PIN debe tener 6 digitos.", "OK");
                return false;
            }
            else
            {
                //Valida que solo se ingresen numeros
                if (!EntryUserPassword.Text.ToCharArray().All(Char.IsDigit))
                {
                    await this.DisplayAlert("Advertencia", "El formato del PIN es incorrecto, solo se aceptan numeros.", "OK");
                    return false;
                }
            }


            if (String.IsNullOrWhiteSpace(EntryUserEmail.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo del correo electronico es obligatorio.", "OK");
                return false;
            }
            else
            {
                //Valida que el formato del correo sea valido
                bool isEmail = Regex.IsMatch(EntryUserEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    await this.DisplayAlert("Advertencia", "El formato del correo electrónico es incorrecto, revíselo e intente de nuevo.", "OK");
                    return false;
                }
            }

            return true;
        }
        async void metodoCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void intentoRegistro(object sender, System.EventArgs e)
        {
            if (await validarFormulario())
            {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            db.CreateTable<RegUserTable>();

            var item = new RegUserTable()
            {
                Nombre = EntryUserName.Text,
                Pin = int.Parse(EntryUserPassword.Text),
                Email = EntryUserEmail.Text,
                Numero = EntryUserNumber.Text,
                is_premium = false
            };

            db.Insert(item);
            Device.BeginInvokeOnMainThread(async () =>
                 {
                     var result = await this.DisplayAlert("Registrado!", "Registro exitoso", "OK", "Cancelar");
                     App.Current.MainPage = new NavigationPage(new LoginPage());
                 });
            }

        }

    }
}
using DERIAN.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Net.Wifi.Hotspot2.Pps;
using System.ComponentModel;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoverPin : ContentPage
    {
        public RecoverPin()
        {
            InitializeComponent();


            Title = "Recuperación de PIN";
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#262626");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#F29F2C");
        }

        SmtpClient SmtpServer;
        private int pinuser;
        private String username;

        async void recuperar_pin(object sender, System.EventArgs e)
        {

            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            if (await validarFormulario())
            {
                if (IsTableExists("RegUserTable") == true)
                {
                    var myquery = db.Table<RegUserTable>().Where(u => u.Email.Equals(EntryUserEmail.Text)).FirstOrDefault();

                    if (myquery != null)
                    {
                        pinuser = db.Table<RegUserTable>().Where(u => u.Email.Equals(EntryUserEmail.Text)).FirstOrDefault().Pin;
                        username = db.Table<RegUserTable>().Where(u => u.Email.Equals(EntryUserEmail.Text)).FirstOrDefault().Nombre;

                        try
                        {
                            SmtpServer = new SmtpClient("smtp.gmail.com");

                            SmtpServer.Port = 587;
                            SmtpServer.Host = "smtp.gmail.com";
                            SmtpServer.EnableSsl = true;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("DERIANCollector@gmail.com", Credenials.Passwor);

                            SmtpServer.SendAsync(Credenials.FromEmal, EntryUserEmail.Text, "Recuperación de PIN", "Estimado/a " + username + ", su PIN es: " + pinuser, "xyz123d");
                            SmtpServer.SendCompleted += emailSendCompleted;
                        }
                        catch (Exception ex)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var result = await this.DisplayAlert("Error!", "Email no enviado!", "Ok", "Cancelar");
                            });
                        }

                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Error!", "Email no encontrado!", "Ok", "Cancelar");                             
                        });
                    }
                }

            }
        }

        private void emailSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Enviado!", "PIN enviado al email", "OK", "Cancelar");
                App.Current.MainPage = new NavigationPage(new LoginPage());
            });
        }

        private async Task<bool> validarFormulario()
        {
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

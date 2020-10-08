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
    public partial class AddCustomItemPage : ContentPage
    {
        private int idItem, idCus;
        public AddCustomItemPage()
        {
        }

        public AddCustomItemPage(int idcustom, int iditemu)
        {
            this.idItem = iditemu;
            this.idCus = idcustom;
            InitializeComponent();


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);


            if (IsTableExists("Campo_custom_item") == true)
            {
                labelNombre.Text = db.Table<Campo_custom>().Where(u => u.Id.Equals(this.idCus)).FirstOrDefault().nombre_campo;


            }

        }

        void GuardarCustom(object sender, System.EventArgs e)
        {

            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<Campo_custom_item>();
            string propiedad = db.Table<Campo_custom>().Where(u => u.Id.Equals(this.idCus)).FirstOrDefault().nombre_campo;

            var valorCustom = new Campo_custom_item()
            {
                valor = propiedad+": "+EntryValor.Text,
                IdCampoCustom = this.idCus,
                IdItem = this.idItem

            };

            int Count = db.Table<Campo_custom_item>().Where(u => u.IdCampoCustom.Equals(this.idCus) && u.IdItem.Equals(this.idItem)).Count();
            if (Count >= 1)
            {
                db.Update(db.Table<Campo_custom_item>().Where(u => u.IdCampoCustom.Equals(this.idCus) && u.IdItem.Equals(this.idItem)).FirstOrDefault());

            }
            else
            {
                db.Insert(valorCustom);
            }
            
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Agregada!", "custom agregado", "OK", "Cancelar");

                if (result)
                    /* await Navigation.PushAsync(new CollectionPage(this.idcolle ,this.idusu)); */
                    await Navigation.PopAsync();
            });

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
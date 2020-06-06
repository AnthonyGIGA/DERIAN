using DERIAN.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCampoCustom : ContentPage
    {
        private int idcollect;
        public AddCampoCustom()
        { }
        public AddCampoCustom(int Idcolle)
        {
            InitializeComponent();
            this.idcollect = Idcolle; 

             
        }

        

        void agregar_campo(object sender, System.EventArgs e)
        {
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<Campo_custom>();

            var item = new Campo_custom()
            {
                nombre_campo = EntryValor.Text, 
                IdColeccion = this.idcollect

            };

            db.Insert(item);
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Agregada!", "Campo agregado", "OK", "Cancelar");

                if (result) 
                    await Navigation.PopAsync();
            });
        }

    }
}
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
    public partial class VerCamposCustom : ContentPage
    {
        public int idCollect;
        public VerCamposCustom(int parametro1)
        {
            this.idCollect = parametro1;
            SetValue(NavigationPage.HasNavigationBarProperty, false);

            InitializeComponent();
            Init();

        }

        public VerCamposCustom() { }

        public void Init()
        {


        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);


            if (IsTableExists("Campo_custom") == true)
            {
                ListaCampos.ItemsSource = db.Table<Campo_custom>().Where(u => u.IdColeccion.Equals(this.idCollect));

                int Count = db.Table<Campo_custom>().Count();
                if (Count >= 4)
                {
                    this.botonAgregarCampoCustom.IsVisible = false;
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

        async void agregar_campo_custom(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCampoCustom(this.idCollect));
        }


    }
}
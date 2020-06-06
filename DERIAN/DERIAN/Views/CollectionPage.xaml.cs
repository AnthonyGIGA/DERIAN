using DERIAN.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionPage : ContentPage
    {
        public int idcolle, idusu;
        public string nombrecolle;
        public CollectionPage()
        { }
        public CollectionPage(int idColeccion,string nombrecolleccion , int idUsuario)
        {
            this.nombrecolle = nombrecolleccion;
            this.idcolle = idColeccion;
            this.idusu = idUsuario;
            InitializeComponent();


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);


            if (IsTableExists("ItemViewTable") == true)
            {
                this.titulocolle.Text = nombrecolle;
                ListaItems.ItemsSource = db.Table<ItemViewTable>().Where(u => u.IdColeccion.Equals(this.idcolle));

                 

            }

        }

        async void click_agregar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddItemPage(this.idcolle, this.idusu));
        }

        async void agregar_campo_custom(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new VerCamposCustom(this.idcolle));
        }

        async void AbrirItem(object sender, SelectionChangedEventArgs e)
        {

            int current = (e.CurrentSelection.FirstOrDefault() as ItemViewTable).Id;

            await Navigation.PushAsync(new MasterPage(current, this.idcolle));

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
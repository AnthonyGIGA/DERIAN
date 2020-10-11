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
using DERIAN.Views;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private ObservableCollection<CollectionViewTable> items = new ObservableCollection<CollectionViewTable>();
        private int iduser4;
         

        public HomePage(int parametro1)
        {
            this.iduser4 = parametro1;

            Title = "Colecciones";

            SetValue(NavigationPage.HasNavigationBarProperty, false);
             
            InitializeComponent();
            
            var adBanner = new AdBanner();
            adBanner.Size = AdBanner.Sizes.MediumRectangle;
            ContenedorAd.Children.Add(adBanner);

            Init();

         }

        public HomePage() { }

        public void Init()
        {
            
            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);


            if (IsTableExists("CollectionViewTable") == true )
            {
                ListaColecciones.ItemsSource = db.Table<CollectionViewTable>().Where(u => u.IdUsuario.Equals(this.iduser4));

                int Count = db.Table<CollectionViewTable>().Where(u => u.IdUsuario.Equals(this.iduser4)).Count();
                //if (Count >= 1)
                //{
                //    this.botonAgregarColle.IsVisible = false;
                //}
                //else {
                //    this.botonAgregarColle.IsVisible = true;
                //}

            }

        }

        private bool IsTableExists(string v)
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            try
            {
                var tableInfo = db.GetTableInfo(v);
                if(tableInfo.Count > 0)
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
        //public void OnDelete(object sender, System.EventArgs e)
        //{
        //    var item = (MenuItem)sender;
        //    var model = (CollectionViewTable)item.CommandParameter;
        //    this.items.Remove(model);
        //    App.ColleController.DeleteCollectionViewTable(model.Id);
        //}

        async void agregarColle(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCollectionPage(this.iduser4));
        }
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        async void AbrirColeccion(object sender, SelectionChangedEventArgs e)
        {

            string idcolle = (e.CurrentSelection.FirstOrDefault() as CollectionViewTable).Id;
            string nombrecolle = (e.CurrentSelection.FirstOrDefault() as CollectionViewTable).nombre;

            await Navigation.PushAsync(new CollectionPage(idcolle, nombrecolle, this.iduser4));

        }
    }
}
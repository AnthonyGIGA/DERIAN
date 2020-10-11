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
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
            Init();
        }

        public int idItem, idColle;


        public MasterPage(int iditem, int idcoll)
        {
            this.idColle = idcoll;
            this.idItem = iditem;
            InitializeComponent();
            Init();

        }

        void Init() {
            //Detail = new NavigationPage(new ItemPage(this.idItem, this.idColle));
        }

        public void CloseThisMaster(){
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Ítem eliminado!", "ítem eliminado", "OK", "Cancelar");

                if (result)
                    await Navigation.PopAsync();
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            
            if (IsTableExists("Campo_custom") == true)
            {
                ListaCustom.ItemsSource = db.Table<Campo_custom>().Where(u => u.IdColeccion.Equals(this.idColle));

            }          
        }

        async void AbrirCustom(object sender, SelectionChangedEventArgs e)
        {
            //string idcustom = (e.CurrentSelection.FirstOrDefault() as Campo_custom).Id;
            //int iditemu = idItem;

            //await Navigation.PushAsync(new AddCustomItemPage(idcustom, iditemu));
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
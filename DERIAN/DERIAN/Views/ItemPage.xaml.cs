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
    public partial class ItemPage : ContentPage
    {
        private string idItem;
        private string idColle; 

        public ItemPage()
        {
            InitializeComponent();
        }
        
        public ItemPage(string iditem,string idcoll)
        {
            this.idColle = idcoll;
            this.idItem = iditem; 
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);


            if (IsTableExists("ItemViewTable") == true)
            {
                Title = db.Table<ItemViewTable>().Where(u => u.Id.Equals(this.idItem)).FirstOrDefault().nombre;
                imagenItem.Source = db.Table<ItemViewTable>().Where(u => u.Id.Equals(this.idItem)).FirstOrDefault().imagen;
            }
            //if (IsTableExists("Campo_custom_item") == true)
            //{
            //    ListaCampos.ItemsSource = db.Table<Campo_custom_item>().Where(u => u.IdItem.Equals(this.idItem));
            //}

            List<Campo_custom> nombrescampos =
               db.Query<Campo_custom>("SELECT nombre_campo FROM Campo_custom WHERE IdColeccion = ?", this.idColle);
            List<Campo_custom_item> valorcampos =
                            db.Query<Campo_custom_item>("SELECT valor FROM Campo_custom_item WHERE IdItem = ?", this.idItem);

            for (int i = 0; i < nombrescampos.Count(); i++)
            {
                Label nuevoEntry1 = new Label();
                try
                {
                    nuevoEntry1.Text = nombrescampos[i].nombre_campo;
                }
                catch (Exception e) { }
                Label nuevoEntry2 = new Label();
                try
                {
                    nuevoEntry2.Text = valorcampos[i].valor;
                }
                catch (Exception e) { }
                ListaCampos.Children.Add(nuevoEntry1);
                ListaCampos.Children.Add(nuevoEntry2);
            }
        }

        async void click_modificar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UpdateItemPage(this.idItem));
        }

        void DeleteItem_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var confirm = await this.DisplayAlert("Eliminar Ítem!", "Estás Seguro?", "OK", "Cancelar");

                if (confirm)
                {
                    var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                    var db = new SQLiteConnection(dbpath);

                    var table = db.Table<ItemViewTable>();
                    var toDelete = table.Where(x => x.Id == idItem).FirstOrDefault();
                    if (toDelete != null)
                    {
                        db.Delete(toDelete);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Eliminado!", "Ítem eliminado", "OK", "Cancelar");

                            if (result)
                                await Navigation.PopAsync();
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Error!", "No se pudo eliminar", "OK", "Cancelar");
                        });
                    }

                }
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
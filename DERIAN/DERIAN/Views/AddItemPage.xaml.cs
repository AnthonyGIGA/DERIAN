using Android.Graphics;
using DERIAN.Tables;
using Plugin.Media;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Xamarin.Forms.Internals;
using System.Data;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPage : ContentPage
    {
        private int  idusu;
        private string idcolle;
             
        public AddItemPage()
        {
            InitializeComponent();
        }

        public AddItemPage(string idcollec, int idusua)
        {
            this.idcolle = idcollec;
            this.idusu = idusua;
            InitializeComponent();
            Title = "Agregar Objeto";
            exampleimage.Source = "derian2.png";

            this.labelpath.IsVisible = false;

            Campos = new ObservableCollection<Campo_custom>();

            takePhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "image.jpg"
                });
                if (file == null)
                    return;
                await DisplayAlert("File Location", file.Path, "OK");
                labelpath.Text = file.Path;
                exampleimage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            };
            takeImage.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Error", "Subida de imagen no soportada.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 40
                });
                if (file == null)
                    return;
                /* await DisplayAlert("File Location", file.Path, "OK"); */
                labelpath.Text = file.Path;
                exampleimage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });                              
            };

            //for (int i = 0; i < 12; i++)
            //{
            //    Entry.ClassIdProperty(i).IsVisible = false; 
            //}

            //int index = 1;
            //foreach(var entry in camposCustom.Children)
            //{
            //    //if(entry.Name == $"EntryName{index}")
            //    //entry.IsVisible = false; 
            //}

            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            //int Contar = db.Table<Campo_custom>().Where(u => u.IdColeccion.Equals(this.idcolle)).Count();

            List<Campo_custom> nombrescampos =
                db.Query<Campo_custom>("SELECT nombre_campo FROM Campo_custom WHERE IdColeccion = ?", this.idcolle);
            List<Campo_custom> idscampos =
                            db.Query<Campo_custom>("SELECT Id FROM Campo_custom WHERE IdColeccion = ?", this.idcolle);

            for (int i = 0; i < nombrescampos.Count(); i++)
            {
                Entry nuevoEntry = new Entry();
                try { 
                    nuevoEntry.Placeholder = nombrescampos[i].nombre_campo;
                    nuevoEntry.StyleId = idscampos[i].Id.ToString(); 
                } catch (Exception e) { }
                camposCustom.Children.Add(nuevoEntry);
            }

        }
        

        /*  void manejando(object sender, System.EventArgs e)
        {
            Views.AddItemPage vm = this.BindingContext as Views.AddItemPage;
            List<string> valores = new List<string>();


            foreach (Campo_custom c in ListaCustom.ItemsSource)
            {
                valores.Add(c.nombre_campo);
            }
            StringBuilder builder = new StringBuilder();
            foreach (string safePrime in valores)
            {
                // Append each int to the StringBuilder overload.
                builder.Append(safePrime).Append(" ");
            }
            string result = builder.ToString();
            /// beamos.Text = result;
        } */

        public ObservableCollection<Campo_custom> Campos
        
           { get; set; }
        

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);          
        }


        private bool IsTableExists(string v)
        {
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
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


        private void tomarFoto(object sender, System.EventArgs e)
        {
            /* TakePhoto(); */
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "Myimage.jpg",
                Directory = "sample"
            });

            if (file == null)
            {
                return;
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);


        }

        private async Task<bool> validarFormulario()
        {
            foreach (Entry entrada in camposCustom.Children) 
            { 
                if (String.IsNullOrWhiteSpace(entrada.Text))
                {
                    await this.DisplayAlert("Advertencia", "El campo del nombre es obligatorio.", "OK");
                    return false;
                }
                //Valida si la cantidad de digitos ingresados es igual o menor a 35
                if (entrada.Text.Length > 35)
                {
                    await this.DisplayAlert("Advertencia", "Límite de carácteres excedido, favor de ingresar máximo 35 digitos.", "OK");
                    return false;
                }
            }
            return true;
        }


        async void RegistrarItem(object sender, System.EventArgs e)
        {
            if (await validarFormulario())
            {
                var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                db.CreateTable<ItemViewTable>();
                db.CreateTable<Campo_custom_item>();

                string nuevoId = DateTime.Now.ToString();

                var item = new ItemViewTable()
                {
                    Id = nuevoId,
                    nombre = EntryName.Text,
                    imagen = labelpath.Text,
                    fecha_creacion = DateTime.Now.ToString(),
                    IdColeccion = this.idcolle
                };

                foreach (Entry entry in camposCustom.Children)
                {
                    var valorCustoms = new Campo_custom_item()
                    {
                        valor = entry.Text,
                        IdCampoCustom = Int32.Parse(entry.StyleId),
                        IdItem = nuevoId
                    };
                    db.Insert(valorCustoms);
                }

                    db.Insert(item);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Agregado!", "Objeto agregado", "OK", "Cancelar");

                    if (result)
                        /* await Navigation.PushAsync(new CollectionPage(this.idcolle ,this.idusu)); */
                        await Navigation.PopAsync();
                });
            }
                
        }
    }
}
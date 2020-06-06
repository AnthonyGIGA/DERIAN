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

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPage : ContentPage
    {
        public int idcolle, idusu;
             
        public AddItemPage()
        {
            InitializeComponent();
        }

        public AddItemPage(int idcollec, int idusua)
        {
            this.idcolle = idcollec;
            this.idusu = idusua;
            InitializeComponent();

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


        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<ItemViewTable>();

            var item = new ItemViewTable()
            {
                nombre = EntryName.Text, 
                imagen = labelpath.Text,
                fecha_creacion = DateTime.Now.ToString(),
                IdColeccion = this.idcolle

            };

            db.Insert(item);
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Agregada!", "ítem agregado", "OK", "Cancelar");

                if (result)
                    /* await Navigation.PushAsync(new CollectionPage(this.idcolle ,this.idusu)); */
                    await Navigation.PopAsync();
            });
        }
    }
}
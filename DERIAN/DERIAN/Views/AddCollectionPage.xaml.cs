using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DERIAN.Tables;
using Plugin.Media;
using Android.Graphics;

namespace DERIAN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCollectionPage : ContentPage
    {
        private int idusu;
        //Random generator = new Random();

        public AddCollectionPage()
        { }
        public AddCollectionPage(int Idusuario)
        {
            InitializeComponent();
            this.idusu = Idusuario;
            this.labelpath.IsVisible = false;
            exampleimage.Source = "derian2.png";

            Title = "Agregar Colección";

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

        async void RegistrarColle(object sender, System.EventArgs e)
        {
            if (await validarFormulario())
            {
                var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                db.CreateTable<CollectionViewTable>();

                var item = new CollectionViewTable()
                {
                    Id = DateTime.Now.ToString(),
                    nombre = EntryName.Text,
                    tipo = EntryType.Text,
                    imagen = labelpath.Text,
                    fecha_creacion = DateTime.Now.ToString(),
                    IdUsuario = this.idusu
                };

                //db.Insert(item);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //var result = await this.DisplayAlert("Agregada!", "Colección agregada con la ID: " + item.Id, "OK", "Cancelar");

                    //if (result)
                    await Navigation.PushAsync(new VerCamposCustom(item));
                    //await Navigation.PopAsync();
                });
            }
        }

        private async Task<bool> validarFormulario()
        {
            if (String.IsNullOrWhiteSpace(EntryName.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo del nombre es obligatorio.", "OK");
                return false;
            }
            //Valida si la cantidad de digitos ingresados es igual o menor a 15
            if (EntryName.Text.Length > 20)
            {
                await this.DisplayAlert("Advertencia", "Límite de carácteres excedido, favor de ingresar máximo 20 carácteres.", "OK");
                return false;
            }
            return true;
        }
    }
}
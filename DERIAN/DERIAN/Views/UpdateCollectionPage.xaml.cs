using Android.Graphics;
using DERIAN.Tables;
using Plugin.Media;
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
    public partial class UpdateCollectionPage : ContentPage
    {
        private string idColle;
        private String nombreColle;

        public UpdateCollectionPage()
        {
            InitializeComponent();
        }

        public UpdateCollectionPage(string idcolle, String nombrecolle)
        {
            this.idColle = idcolle;
            this.nombreColle = nombrecolle;
            InitializeComponent();
            this.labelpath.IsVisible = false;

            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            string imagenfirst = db.Table<CollectionViewTable>().Where(u => u.Id.Equals(this.idColle)).FirstOrDefault().imagen;
            exampleimage.Source = imagenfirst;
            labelpath.Text = imagenfirst;
            EntryName.Text = db.Table<CollectionViewTable>().Where(u => u.Id.Equals(this.idColle)).FirstOrDefault().nombre;
            EntryType.Text = db.Table<CollectionViewTable>().Where(u => u.Id.Equals(this.idColle)).FirstOrDefault().tipo;


            Title = "Modificar Colección";

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
                labelpath.Text = file.Path;
                exampleimage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            };

        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

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
            if (String.IsNullOrWhiteSpace(EntryName.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo del nombre es obligatorio.", "OK");
                return false;
            }
            //Valida si la cantidad de digitos ingresados es igual o menor a 15
            if (EntryName.Text.Length >= 15)
            {
                await this.DisplayAlert("Advertencia", "Límite de carácteres excedido, favor de ingresar menor a 15 digitos.", "OK");
                return false;
            }
            return true;
        }


        async void modificar_Colle(object sender, System.EventArgs e)
        {
            if (await validarFormulario())
            {
                var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);

                db.CreateTable<CollectionViewTable>();
                db.Query<CollectionViewTable>("Update CollectionViewTable set nombre ='" + EntryName.Text + "', tipo='" + EntryType.Text + "', imagen='" + labelpath.Text + "' WHERE Id = '" + this.idColle + "'");

                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Modificado!", "Colección modificada", "OK", "Cancelar");

                    if (result)
                        await Navigation.PopAsync();
                });
            }

        }
    }
}
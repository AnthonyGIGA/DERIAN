﻿using Android.Graphics;
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
    public partial class UpdateItemPage : ContentPage
    {
        private int idItem;

        public UpdateItemPage()
        {
            InitializeComponent();
        }

        public UpdateItemPage(int iditem)
        {
            this.idItem = iditem; 
            InitializeComponent();

            this.labelpath.IsVisible = false;
             

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


        private bool IsTableExists(string v)
        {
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
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


        async void modificar_Item(object sender, System.EventArgs e)
        {
            if (await validarFormulario())
            {
                var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);

                db.CreateTable<ItemViewTable>();
                db.Query<ItemViewTable>("Update ItemViewTable set nombre ='" + EntryName.Text + "', imagen='" + labelpath.Text + "' WHERE Id = " + this.idItem + "");

                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Modificado!", "ítem modificado", "OK", "Cancelar");

                    if (result)
                        await Navigation.PopAsync();
                });
            }
                
        }
    }
}
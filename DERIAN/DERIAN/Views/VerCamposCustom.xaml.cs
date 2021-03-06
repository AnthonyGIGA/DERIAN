﻿using DERIAN.Tables;
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
        private string idCollect;
        private Boolean confirmado = false;
        private Boolean recarga = false;
        private CollectionViewTable objetoColeccion;

        public VerCamposCustom(CollectionViewTable coleccion)
        {
            this.idCollect = coleccion.Id;
            this.objetoColeccion = coleccion;
            SetValue(NavigationPage.HasNavigationBarProperty, false);

            InitializeComponent();

            Title = "Campos Personalizados";

            Init();

        }

        public VerCamposCustom() { }

        public void Init()
        {

        }

        //protected override void OnParentSet()
        //{
        //    base.OnParentSet();
        //    if (Parent == null)
        //    {
        //        if(this.recarga == false)
        //        {
        //            if (this.confirmado == false)
        //            {
        //                var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
        //                var db = new SQLiteConnection(dbpath);

        //                var table = db.Table<CollectionViewTable>();
        //                var toDelete = table.Where(x => x.Id == this.idCollect).FirstOrDefault();
        //                if (toDelete != null)
        //                {
        //                    db.Delete(toDelete);
        //                }
        //            }
        //        }               
        //    }
        //}
         

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            if (IsTableExists("Campo_custom") == true)
            {
                ListaCampos.ItemsSource = db.Table<Campo_custom>().Where(u => u.IdColeccion.Equals(this.idCollect));

                int Count = db.Table<Campo_custom>().Where(u => u.IdColeccion.Equals(this.idCollect)).Count();
                if (Count >= 12)
                {
                    this.botonAgregarCampoCustom.IsVisible = false;
                }
            }
        }

        async void agregar_campo(object sender, System.EventArgs e)
        {
            if (await validarFormulario())
            {
                var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                db.CreateTable<Campo_custom>();

                var item = new Campo_custom()
                {
                    nombre_campo = EntryValor.Text,
                    IdColeccion = this.idCollect
                };

                db.Insert(item);
                Device.BeginInvokeOnMainThread(async () =>
                {                    
                        //await Navigation.PopAsync();
                        this.recarga = true;
                        var vUpdatedPage = new VerCamposCustom(this.objetoColeccion); 
                        Navigation.InsertPageBefore(vUpdatedPage, this); 
                        Navigation.PopAsync();                      
                });
            }
        }

        private async Task<bool> validarFormulario()
        {
            if (String.IsNullOrWhiteSpace(EntryValor.Text))
            {
                await this.DisplayAlert("Advertencia", "El campo del nombre es obligatorio.", "OK");
                return false;
            }
            //Valida si la cantidad de digitos ingresados es igual o menor a 15
            if (EntryValor.Text.Length > 15)
            {
                await this.DisplayAlert("Advertencia", "Límite de carácteres excedido, favor de ingresar máximo 15 digitos.", "OK");
                return false;
            }
            return true;
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

        async void BorrarCampoCustom(object sender, SelectionChangedEventArgs e)
        {
            int idCampo= (e.CurrentSelection.FirstOrDefault() as Campo_custom).Id;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var confirm = await this.DisplayAlert("Eliminar Campo!", "Estás Seguro?", "OK", "Cancelar");

                if (confirm)
                {
                    var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                    var db = new SQLiteConnection(dbpath);

                    var table = db.Table<Campo_custom>();
                    var toDelete = table.Where(x => x.Id == idCampo).FirstOrDefault();
                    if (toDelete != null)
                    {
                        db.Delete(toDelete);
                        Device.BeginInvokeOnMainThread(async () =>
                        { 
                            this.recarga = true;
                            var vUpdatedPage = new VerCamposCustom(this.objetoColeccion);
                            Navigation.InsertPageBefore(vUpdatedPage, this);
                            Navigation.PopAsync();
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

        async void agregarColeccion(object sender, System.EventArgs e)
        {
            var dbpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<CollectionViewTable>();

            db.Insert(this.objetoColeccion);

            var result = await this.DisplayAlert("Agregado!", "Colección Agregada!", "OK", "Cancelar");
             
            this.confirmado = true;
            Navigation.PopToRootAsync(); 
        }


    }
}
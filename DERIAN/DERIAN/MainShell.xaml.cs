using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DERIAN.Views;
using SQLite;
using System.IO;
using DERIAN.Tables;

namespace DERIAN
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainShell : Shell
    {
        public int elfiltro;
        private String _userName;
        public String UserName
        {
            get => _userName;
            set
            {
                if (value != _userName)
                {
                    _userName= value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        public MainShell(int iduser)
        {
            this.elfiltro = iduser;
             
            InitializeComponent();


            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            if (IsTableExists("RegUserTable") == true)
            {
                this._userName = db.Table<RegUserTable>().Where(u => u.UserId.Equals(this.elfiltro)).FirstOrDefault().Nombre;

            }

            BindingContext = this;

            ShellSection shell_section = new ShellSection
            {
                Title = "Colecciones",
                Icon = "coleccionIcon.png",
            };

            shell_section.Items.Add(new ShellContent() { Content = new HomePage(elfiltro) });

            mainShell.Items.Add(shell_section);
            CurrentItem = shell_section;
             
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
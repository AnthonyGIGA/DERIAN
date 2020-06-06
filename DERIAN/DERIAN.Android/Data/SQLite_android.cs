using DERIAN.Tables;
using DERIAN.ViewTables;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms; 

namespace DERIAN.Droid.Data
{
    public class SQLite_android : ISQLite
    {
        public SQLite_android() { }
        public SQLiteConnection GetConnection()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);

            return db;



        }
    }

}
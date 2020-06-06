using DERIAN.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DERIAN.ViewTables
{
    public class CollectionViewTableController
    {

        private static object locker = new object();

        private SQLiteConnection database;

        public CollectionViewTableController()
        {
            this.database = DependencyService.Get<ISQLite>().GetConnection();
            this.database.CreateTable<CollectionViewTable>();

        }

        public IEnumerator<CollectionViewTable> GetCollectionViewTable()
        {
            lock (locker)
            {
                if(this.database.Table<CollectionViewTable>().Count() == 0)
                {
                    return null;
                }
                else 
                {
                    return this.database.Table<CollectionViewTable>().GetEnumerator();
                }
            }
        }

        public int SaveCollectionViewTable(CollectionViewTable item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    this.database.Update(item);
                    return item.Id;
                }
                else
                {
                    return this.database.Insert(item); 
                }
            }
        }

        public int DeleteCollectionViewTable(int id)
        {
            lock (locker)
            {

                return this.database.Delete<CollectionViewTable>(id);
            }
        }



    }
}

using Android.Graphics;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DERIAN.Tables
{
    public class CollectionViewTable
    {
        [PrimaryKey] 
        public string Id { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }
        public string tipo { get; set; }
        public string fecha_creacion { get; set; }
        public int IdUsuario { get; set; }


    }
}

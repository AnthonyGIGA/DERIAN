using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DERIAN.Tables
{
    public class Campo_custom
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string nombre_campo { get; set; } 
        public string IdColeccion { get; set; }
    }
}

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
        public int IdColeccion { get; set; }
        public string valor { get; set; }
    }
}

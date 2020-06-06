using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DERIAN.Tables
{
    public class Campo_custom_item
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string valor { get; set; } 
        public int IdItem { get; set; }
        public int IdCampoCustom { get; set; }
    }
}

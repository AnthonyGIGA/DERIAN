using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DERIAN.Tables
{
    public class RegUserTable
    {
        [PrimaryKey]
        [AutoIncrement]
        public int UserId { get; set; }
        public string Nombre { get; set; }
        public string Pin { get; set; }
        public string Email { get; set; }
        public string Numero { get; set; }

        public bool is_premium { get; set; }

    }
}
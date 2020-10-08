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
    public partial class AcercaDePage : ContentPage
    {
        public AcercaDePage()
        {
            InitializeComponent();
            Title = "Acerca de DERIAN";
        }
    }
}
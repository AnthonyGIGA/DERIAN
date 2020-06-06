using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DERIAN.Droid
{

    [Activity(Theme = "@style/Theme.Splash",
        MainLauncher = true,
        NoHistory = true,
        Icon = "@drawable/logo")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Create your application here
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(300);
            StartActivity(typeof(MainActivity));

        }
    }
}
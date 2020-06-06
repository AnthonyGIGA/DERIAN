using System;
using Xamarin.Forms;

namespace DERIAN
{
    public static class Constants
    {
        public static string AdUnitIdTest
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "ca-app-pub-3940256099942544/6300978111";
                        // return "ca-app-pub-9041565262171061/1322636719";

                    // case Device.iOS:
                    //   return "ca-app-pub-3940256099942544/2934735716";

                    default:
                        throw new InvalidOperationException("Invalid platform");
                }
            }
        }
    }
}

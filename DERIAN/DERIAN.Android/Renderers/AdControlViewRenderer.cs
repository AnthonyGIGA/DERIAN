using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using DERIAN.Controls;
using DERIAN.Droid.Renderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(AdControlView), typeof(AdControlViewRenderer))]
namespace DERIAN.Droid.Renderers
{
    public class AdControlViewRenderer : ViewRenderer<AdControlView, AdView>
    {
        private AdView _adView;

        public AdControlViewRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
                SetNativeControl(CreateAdView());
        }

        private AdView CreateAdView()
        {
            if (_adView != null)
                return _adView;

            _adView = new AdView(Context)
            {
                AdUnitId = Element.AdUnitId,
                AdSize = AdSize.SmartBanner
            };

            _adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            AdRequest.Builder b = new AdRequest.Builder();
            _adView.LoadAd(b.Build());

            return _adView;
        }
    }
}
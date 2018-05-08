using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using ZXing.Mobile;

namespace MacLookup.Droid
{
    [Activity(Label = "MacLookup", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //string dbPath = FileAccessHelper.GetLocalFilePath("macaddr.zip");
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            MobileBarcodeScanner.Initialize(Application);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            var appz = new App(new AndroidInitializer());
            //appz.dbpath = dbPath;
            
            LoadApplication(appz);
        }
        public override void OnBackPressed() {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed)) {
                // Do something if there are some pages in the `PopupStack`
            } else {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) {
            //global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}


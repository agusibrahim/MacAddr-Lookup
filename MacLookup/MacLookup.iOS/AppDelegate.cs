using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace MacLookup.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UITabBar.Appearance.SelectedImageTintColor = Color.FromHex("#FF2D55").ToUIColor();
            Rg.Plugins.Popup.Popup.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            global::Xamarin.Forms.Forms.Init();
            app.StatusBarStyle = UIStatusBarStyle.LightContent;
            
            if(Window!=null)
            Window.TintColor= Color.FromHex("#5856D6").ToUIColor();
            //string dbPath = FileAccessHelper.GetLocalFilePath("macaddr.zip");
            var appz = new App(new iOSInitializer());
            //appz.dbpath = dbPath;
            LoadApplication(appz);

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {

        }
    }
}

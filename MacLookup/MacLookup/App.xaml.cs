using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using MacLookup.Interfaces;
using MacLookup.Models;
using Prism;
using Prism.Ioc;
using MacLookup.ViewModels;
using MacLookup.Views;
using Prism.Plugin.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using SQLite;
using FileAccess = System.IO.FileAccess;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MacLookup
{
    public partial class App : PrismApplication
    {
        public static SQLiteConnection db;
        private static HttpClient http;
        public static string dbpath;

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            http=new HttpClient();
            await NavigationService.NavigateAsync("NavigationPage/HomeTabbed");
            dbpath = DependencyService.Get<IFileHelper>().GetLocalFilePath("macdata.zip");
            db = new SQLiteConnection(dbpath);
            db.CreateTable<Mac>();
            
        }
        public static async Task<string> GetCSV(string url)
        {
            http.MaxResponseContentBufferSize = long.MaxValue;
            http.Timeout=TimeSpan.FromMinutes(7);
            return await http.GetStringAsync(url);
        }

        

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<HomeTabbed>();
            containerRegistry.RegisterForNavigation<LookupPage>();
            containerRegistry.RegisterForNavigation<BrowsePage>();
            containerRegistry.RegisterForNavigation<MorePage>();
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<DownloadPopup>();
        }
    }
}

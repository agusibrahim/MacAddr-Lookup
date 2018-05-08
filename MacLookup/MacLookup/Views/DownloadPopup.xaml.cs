using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace MacLookup.Views
{
    public partial class DownloadPopup : PopupPage {
        public DownloadPopup()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        
    }
}

using MacLookup.Models;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MacLookup.ViewModels {

    public class LookupPageViewModel : BindableBase {

        public LookupPageViewModel(IPageDialogService dialogService, INavigationService navigationService) {
            _dialogService = dialogService;
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                if (App.db.Table<Mac>().Count() > 23000) return false;
                Device.BeginInvokeOnMainThread(async () => {
                    var res = await _dialogService.DisplayAlertAsync("Download Database",
                        "Untuk mempercepat pencarian dan untuk kapabilitas offline kami sarankan untuk mendownload data terlebih dahulu, ukuran data sekitar 2 MB. Pastikan kamu terhubung ke Internet, proses download akan membutuhkan waktu beberapa menit",
                        "Lanjutkan", "Batal");
                    if (res) {
                        if (CrossConnectivity.IsSupported && CrossConnectivity.Current.IsConnected)
                            await navigationService.NavigateAsync("DownloadPopup");
                        else
                            await dialogService.DisplayAlertAsync("NO CONNECTION",
                                "There is no internet connection detected, please Turn On Mobile Data or connect into WIFI",
                                "OK");
                    }
                });

                return false;
            });
        }

        private string _organisationName = "...";

        public string OrganisationName {
            get => _organisationName;
            set => SetProperty(ref _organisationName, value);
        }

        private string _organisationAddr = "...";

        public string OrganisationAddr {
            get => _organisationAddr;
            set => SetProperty(ref _organisationAddr, value);
        }

        private string _macInput = "";

        public string MacInput {
            get => _macInput;
            set => SetProperty(ref _macInput, value);
        }

        private DelegateCommand _performSearch;

        public DelegateCommand PerformSearch =>
            _performSearch ?? (_performSearch = new DelegateCommand(ExecutePerformSearch));

        private void ExecutePerformSearch() {
            //_dialogService.DisplayAlertAsync("ALERT", MacInput, "OK");
            //if(true) return;

            if (MacInput.Replace(":", "").Length != 12) return;
            string cc = "1xx";
            cc = _macInput.Replace(":", "").ToLower().Substring(0, 6);
            var res = from s in App.db.Table<Mac>()
                      where s.Assignment.ToLower().StartsWith(cc)
                      select s;
            var mac = res.FirstOrDefault();
            if (mac != null) {
                OrganisationName = mac.OrganizationName;
                OrganisationAddr = mac.OrganizationAddress;
            } else {
                OrganisationName = "...";
                OrganisationAddr = "...";
                _dialogService.DisplayAlertAsync("NOT FOUND",
                    MacInput + " is not found in our database. Please correct your input", "OK");
            }
        }

        private DelegateCommand _openBarcodeScanner;

        public DelegateCommand OpenBarcodeScanner =>
            _openBarcodeScanner ?? (_openBarcodeScanner = new DelegateCommand(ExecuteOpenBarcodeScanner));

        private async void ExecuteOpenBarcodeScanner() {
            //_dialogService.DisplayAlertAsync("Title", "Hello", "OK");

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            scanner.BottomText = "Scan to Your Mac Address Barcode/QRCode";
            var result = await scanner.Scan();
            if (result == null) return;
            if (Regex.IsMatch(result.Text, "^([0-9A-Fa-f]{2}[:-]?){5}([0-9A-Fa-f]{2})$")) {
                MacInput = result.Text.Replace("-", ":");
                ExecutePerformSearch();
            } else {
                await _dialogService.DisplayAlertAsync("Invalid Mac", result.Text + " isnt valid MAC Address.", "OK");
            }
        }

        private DelegateCommand _downloadData;
        private DelegateCommand _ClearInputMac;

        public DelegateCommand ClearInputMac =>
            _ClearInputMac ?? (_ClearInputMac = new DelegateCommand(ExecuteClearInputMac));

        private void ExecuteClearInputMac() {
            MacInput = "";
            OrganisationName = "...";
            OrganisationAddr = "...";
        }

        private IPageDialogService _dialogService;
    }
}
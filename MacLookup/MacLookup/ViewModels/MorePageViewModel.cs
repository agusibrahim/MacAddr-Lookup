using MacLookup.Events;
using MacLookup.Models;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MacLookup.ViewModels {

    public class MorePageViewModel : BindableBase {

        public MorePageViewModel(IPageDialogService dialogService, INavigationService navigationService, IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;
            _dialogService = dialogService;
            FetchDbInfo();
            _eventAggregator.GetEvent<DownloadEvent>().Subscribe(downloadEventCallback);
        }

        private void FetchDbInfo() {
            Device.BeginInvokeOnMainThread(() => {
                DbCount = App.db.Table<Mac>().Count();
                if (File.Exists(App.dbpath))
                    LastUpdateDate = File.GetCreationTime(App.dbpath);
                else LastUpdateDate = DateTime.MinValue;
            });
        }

        private async void downloadEventCallback(string obj) {
            if (obj.Equals(DownloadEvent.EVENT_SUCCESS)) {
                await _dialogService.DisplayAlertAsync("UPDATE DATA", "Data updated successfully", "OK");
                FetchDbInfo();
            } else {
                var res = await _dialogService.DisplayAlertAsync("UPDATE DATA", "Failed to update data. please try again", "Try Again", "Cancel");
                if (res) await CheckConnectionAndDownload();
            }
        }

        private DateTime _lastUpdateDate;

        public DateTime LastUpdateDate {
            get => _lastUpdateDate;
            set => SetProperty(ref _lastUpdateDate, value);
        }

        private int _DbCount;
        private IPageDialogService _dialogService;

        public int DbCount {
            get => _DbCount;
            set => SetProperty(ref _DbCount, value);
        }

        private DelegateCommand _updateDataAction;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        public DelegateCommand UpdateDataAction =>
            _updateDataAction ?? (_updateDataAction = new DelegateCommand(ExecuteUpdateDataAction));

        private async void ExecuteUpdateDataAction() {
            var res = await _dialogService.DisplayAlertAsync("UPDATE DATA", "Update data akan membutuhkan waktu beberapa menit (tergantung kualitas internet). Jangan tutup aplikasi selagi download berlangsung.", "Lanjutkan", "Batal");
            if (res) {
                await CheckConnectionAndDownload();
            }
        }

        private async Task CheckConnectionAndDownload() {
            if (CrossConnectivity.IsSupported && CrossConnectivity.Current.IsConnected)
                await _navigationService.NavigateAsync("DownloadPopup");
            else
                await _dialogService.DisplayAlertAsync("NO CONNECTION",
                    "There is no internet connection detected, please Turn On Mobile Data or connect into WIFI",
                    "OK");
        }
    }
}
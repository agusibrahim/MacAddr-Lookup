using MacLookup.Events;
using MacLookup.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MacLookup.ViewModels {

    public class BrowsePageViewModel : BindableBase {

        public BrowsePageViewModel(IPageDialogService dialogService, IEventAggregator eventAggregator) {
            _dialogService = dialogService;
            beginFetchingData();
            eventAggregator.GetEvent<DownloadEvent>().Subscribe(downloadEventCallback);
        }

        private void beginFetchingData() {
            Device.BeginInvokeOnMainThread(() => {
                foreach (var mac in App.db.Table<Mac>().OrderBy(x => x.OrganizationName).ToList()) {
                    MacData.Add(new MacModel {
                        Assigment = mac.Assignment,
                        OrganisationName = mac.OrganizationName,
                        OrganisationAddress = mac.OrganizationAddress,
                        Registry = mac.Registry
                    });
                    masterdata.Add(new MacModel {
                        Assigment = mac.Assignment,
                        OrganisationName = mac.OrganizationName,
                        OrganisationAddress = mac.OrganizationAddress,
                        Registry = mac.Registry
                    });
                }
            });
        }

        private void downloadEventCallback(string obj) {
            if (obj.Equals(DownloadEvent.EVENT_SUCCESS)) {
                beginFetchingData();
            }
        }

        private DelegateCommand<string> _performSearch;

        public DelegateCommand<string> PerformSearch =>
            _performSearch ?? (_performSearch = new DelegateCommand<string>(ExecutePerformSearch));

        private void ExecutePerformSearch(string kw) {
            if (string.IsNullOrEmpty(kw)) {
                MacData = masterdata;
                return;
            }
            MacData = masterdata.Where(x => x.OrganisationName.ToLower().Contains(kw.ToLower()) || x.Assigment.ToLower().Contains(kw.ToLower())).ToList();
        }

        private List<MacModel> _macs = new List<MacModel>();
        private IPageDialogService _dialogService;
        private List<MacModel> masterdata = new List<MacModel>();

        public List<MacModel> MacData {
            get => _macs;
            set => SetProperty(ref _macs, value);
        }

        private DelegateCommand<MacModel> _getDetail;

        public DelegateCommand<MacModel> GetDetail =>
            _getDetail ?? (_getDetail = new DelegateCommand<MacModel>(ExecuteGetDetail));

        private void ExecuteGetDetail(MacModel macModel) {
            _dialogService.DisplayAlertAsync("DETAIL", $"Registry: {macModel.Registry}\nAsseignment: {macModel.Assigment}\nOrganization Name: {macModel.OrganisationName}\nOrganization Address: {macModel.OrganisationAddress}\n", "OK");
        }
    }
}
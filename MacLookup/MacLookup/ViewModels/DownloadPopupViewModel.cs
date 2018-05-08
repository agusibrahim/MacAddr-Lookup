using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MacLookup.Events;
using MacLookup.Models;
using Prism.Events;
using Prism.Navigation;
using TinyCsvParser;
using TinyCsvParser.Tokenizer.RFC4180;
using Xamarin.Forms;

namespace MacLookup.ViewModels
{
	public class DownloadPopupViewModel : BindableBase
	{
	    private INavigationService _navigationService;
	    private IEventAggregator _eventAggregator;

	    public DownloadPopupViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
	    {
	        _eventAggregator = eventAggregator;
            _navigationService = navigationService;
            ExecuteDownloadData();
        }
	    private async void ExecuteDownloadData()
	    {
	        var eventResponse = "";
	        try
	        {
	            var csvraw = await App.GetCSV("https://standards.ieee.org/develop/regauth/oui/oui.csv");//http://demo0369881.mockable.io/macdata
                var options = new Options('"', '\\', ',');
	            var tokenizer = new RFC4180Tokenizer(options);
	            CsvParserOptions csvParserOptions = new CsvParserOptions(true, tokenizer);
	            OUIObjectMap csvMapper = new OUIObjectMap();
	            CsvParser<Mac> csvParser = new CsvParser<Mac>(csvParserOptions, csvMapper);
	            var opts = new CsvReaderOptions(new[] {Environment.NewLine});
	            var result = csvParser.ReadFromString(opts, csvraw);
	            System.Diagnostics.Debug.WriteLine("Writing to db...");
	            App.db.BeginTransaction();
	            foreach (var mac in result)
	            {
	                App.db.InsertOrReplace(mac.Result);
	            }
	            App.db.Commit();
	            System.Diagnostics.Debug.WriteLine("Writing to db OK");
	            eventResponse = DownloadEvent.EVENT_SUCCESS;
	        }
	        catch (Exception e)
	        {
	            eventResponse = DownloadEvent.EVENT_FAILED;
	        }
	        _eventAggregator.GetEvent<DownloadEvent>().Publish(eventResponse);
            await _navigationService.ClearPopupStackAsync();
	    }
    }
}

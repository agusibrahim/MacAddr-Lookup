using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Services;

namespace MacLookup.ViewModels
{
	public class HomeTabbedViewModel : BindableBase,IDestructible
	{
        public HomeTabbedViewModel(IPageDialogService dialogService)
        {
            
        }

	    public void Destroy()
	    {
	        App.db.Close();
	    }
	}
}

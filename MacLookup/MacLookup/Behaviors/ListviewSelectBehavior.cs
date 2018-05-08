using System;
using System.Collections.Generic;
using System.Text;
using MacLookup.Models;
using MacLookup.ViewModels;
using Xamarin.Forms;

namespace MacLookup.Behaviors
{
    public class ListviewSelectBehavior:Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemTapped += Bindable_ItemSelected;
        }

        private void Bindable_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            ((sender as ListView).BindingContext as BrowsePageViewModel).GetDetail.Execute((sender as ListView).SelectedItem as MacModel);
            (sender as ListView).SelectedItem = null;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemTapped -= Bindable_ItemSelected;
        }

        
    }
}

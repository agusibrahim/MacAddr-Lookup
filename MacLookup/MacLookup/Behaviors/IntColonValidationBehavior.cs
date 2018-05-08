using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MacLookup.Behaviors
{
    public class IntColonValidationBehavior : Behavior<Entry> {
        public static IntColonValidationBehavior Instance = new IntColonValidationBehavior();
        private int count = 0;
        private int segcount = 3;
        ///
        /// Attaches when the page is first created.
        /// 

        protected override void OnAttachedTo(Entry entry) {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        ///
        /// Detaches when the page is destroyed.
        /// 

        protected override void OnDetachingFrom(Entry entry) {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args) {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue)) {
                // If the new value is longer than the old value, the user is
                if (args.OldTextValue != null && args.NewTextValue.Length < args.OldTextValue.Length)
                    return;

                var value = args.NewTextValue;
                if (value.Length > 17) {
                    ((Entry)sender).Text = value.Remove(value.Length - 1);
                    return;
                }

                if (Regex.IsMatch(value.ToLower(), ".*[^a-f0-9\\:].*"))
                {
                    ((Entry)sender).Text = value.Remove(value.Length - 1);
                    return;
                }
                if (value.Length == 2)
                {
                    ((Entry)sender).Text += ":";
                    return;
                }
                if (value.Length == 5) {
                    ((Entry)sender).Text += ":";
                    return;
                }
                if (value.Length == 8) {
                    ((Entry)sender).Text += ":";
                    return;
                }
                if (value.Length == 11) {
                    ((Entry)sender).Text += ":";
                    return;
                }
                if (value.Length == 14) {
                    ((Entry)sender).Text += ":";
                    return;
                }

                

                ((Entry)sender).Text = args.NewTextValue.ToUpper();
            }
        }
    }
}

﻿using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MacLookup.Views {

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimatingDotsControl : ContentView {

        public AnimatingDotsControl() {
            InitializeComponent();
        }

        private bool _animationStarted;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Renderer") {
                if (!_animationStarted) {
                    // start the animation on element rendering
                    _animationStarted = true;

                    RunAnimations();
                } else {
                    // abort the animation on element disposing
                    this.AbortAnimation("loadingIndicatorPulseAnimation");
                }
            }
        }

        private void RunAnimations() {
            var pulseAnimation1 = new Animation();

            pulseAnimation1.Add(0, 0.33, new Animation(alpha => Dot1.Opacity = alpha, 1, 0, Easing.CubicOut, () => Dot1.FadeTo(1)));
            pulseAnimation1.Add(0.33, 0.66, new Animation(alpha => Dot2.Opacity = alpha, 1, 0, Easing.CubicOut, () => Dot2.FadeTo(1)));
            pulseAnimation1.Add(0.66, 0.99, new Animation(alpha => Dot3.Opacity = alpha, 1, 0, Easing.CubicOut, () => Dot3.FadeTo(1)));

            pulseAnimation1.Commit(this, "loadingIndicatorPulseAnimation", 10, 1100, null, null, () => true);
        }
    }
}
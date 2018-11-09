using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using SoCalCodeCamp.DeepDive.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SoCalCodeCamp.DeepDive
{
    public partial class App
    {
        public App() : base() { }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
#if DEBUG
            SetForDebugging();
#else
            AppCenter.Start(Secrets.AppCenterSecret, typeof(Analytics), typeof(Crashes), typeof(Push));
#endif

            var result = await NavigationService.NavigateAsync("LoginPage");
            if(!result.Success)
            {
#if DEBUG
                Debugger.Break();
#else
                Crashes.TrackError(result.Exception, new Dictionary<string, string>
                {
                    { "navigationPath", "LoginPage" }
                });
#endif
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            
        }

#if DEBUG
        private void SetForDebugging()
        {
            Xamarin.Forms.Internals.Log.Listeners.Add(new DebugListener());
        }

        private class DebugListener : Xamarin.Forms.Internals.LogListener
        {
            public override void Warning(string category, string message) =>
                Debug.WriteLine($"    {category}: {message}");
        }
#endif
    }
}

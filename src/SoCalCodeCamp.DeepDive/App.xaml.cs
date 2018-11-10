using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using SoCalCodeCamp.AuthDemo.Services;
using SoCalCodeCamp.DeepDive.Helpers;
using SoCalCodeCamp.DeepDive.Services;
using SoCalCodeCamp.DeepDive.Views;
using SoCalCodeCamp.DeepLinking;
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

            var ea = Container.Resolve<IEventAggregator>();
            ea.GetEvent<AuthDemo.Events.UserAuthenticatedEvent>().Subscribe(OnUserAuthenticated);
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
            containerRegistry.Register<IGravatarService, GravatarService>();
            containerRegistry.Register<IAADOptions, AuthOptions>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<HomePage>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DeepLinkingModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<AuthDemo.AuthModule>();
            moduleCatalog.AddModule<LoggingDemo.LoggingModule>();
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

        private async void OnUserAuthenticated(AuthenticationResult result)
        {
            Container.Resolve<IModuleManager>().LoadModule(nameof(DeepLinkingModule));
            var jwt = new JsonWebToken(result.AccessToken);
            var email = jwt.Claims.First(x => x.Type == "emails").Value;
            var navResult = await NavigationService.NavigateAsync($"/HomePage?email={email}/NavigationPage/ViewA");

            if(!navResult.Success)
            {
                Debugger.Break();
            }
        }
    }
}

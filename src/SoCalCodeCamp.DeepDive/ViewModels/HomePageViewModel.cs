using Prism.Mvvm;
using Prism.Navigation;

namespace SoCalCodeCamp.DeepDive.ViewModels
{
    public class HomePageViewModel : BindableBase, INavigatingAware
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            if(parameters.ContainsKey("email"))
                Email = parameters.GetValue<string>("email");
        }
    }
}

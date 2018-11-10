using SoCalCodeCamp.DeepDive.Services;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SoCalCodeCamp.DeepDive.Converters
{
    public class EmailToGravatarConverter : IValueConverter
    {
        private IGravatarService _gravatarService { get; }

        public EmailToGravatarConverter(IGravatarService gravatarService)
        {
            _gravatarService = gravatarService;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return null;

            var email = (string)value;

            return new UriImageSource()
            {
                Uri = new Uri(_gravatarService.GetGravatarUri(email)),
                CacheValidity = TimeSpan.FromDays(7),
                CachingEnabled = true
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(parameter is Entry email))
            {
                return false;
            }

            var isFocused = (bool)value;
            var isInvalidEmail = !isFocused && !CheckValidEmail(email.Text);

            return !isFocused && isInvalidEmail;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }

        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <param name="email">Gets the email</param>
        /// <returns>Returns the boolean value.</returns>
        private static bool CheckValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email) && !email.EndsWith(".");
        }
    }
}

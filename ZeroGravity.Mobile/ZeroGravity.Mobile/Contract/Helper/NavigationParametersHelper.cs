using System.Linq;
using Prism.Navigation;

namespace ZeroGravity.Mobile.Contract.Helper
{
    public static class NavigationParametersHelper
    {
        public static INavigationParameters CreateNavigationParameter<T>(T parameter)
        {
            return new NavigationParameters { { typeof(T).ToString(), parameter } };
        }

        public static T GetNavigationParameters<T>(INavigationParameters navigationParameters) where T : class
        {
            if (navigationParameters == null)
            {
                return null;
            }

            var key = navigationParameters.Keys.FirstOrDefault(_ => _ == typeof(T).ToString());
            if (!string.IsNullOrWhiteSpace(key))
            {
                return navigationParameters[key] as T;
            }

            return null;
        }

        public static void AddNavigationParameters<T>(INavigationParameters parameters, T parameter) where T : class
        {
            parameters?.Add(typeof(T).ToString(), parameter);
        }
    }
}

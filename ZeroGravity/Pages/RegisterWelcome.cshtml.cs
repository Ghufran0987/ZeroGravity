using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ZeroGravity.Pages
{
    public class RegisterWelcomeModel : PageModel
    {
        public RegisterWelcomeModel(IStringLocalizer<RegisterWelcomeModel> stringLocalizer)
        {
            StringLocalizer = stringLocalizer;
        }

        public IStringLocalizer<RegisterWelcomeModel> StringLocalizer;

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}

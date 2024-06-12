using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ZeroGravity.Pages
{
    public class EmailChangedModel : PageModel
    {
        public EmailChangedModel(IStringLocalizer<EmailChangedModel> stringLocalizer)
        {
            StringLocalizer = stringLocalizer;
        }

        public IStringLocalizer<EmailChangedModel> StringLocalizer;
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}

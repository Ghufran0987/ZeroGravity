using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ZeroGravity.Pages
{
    public class ErrorModel : PageModel
    {
        public ErrorModel(IStringLocalizer<ErrorModel> stringLocalizer)
        {
            StringLocalizer = stringLocalizer;
        }

        public string Message { get; private set; }
        public IStringLocalizer<ErrorModel> StringLocalizer { get; }

        public IActionResult OnGet(string message)
        {
            Message = message;
            return Page();
        }
    }
}

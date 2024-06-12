using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ZeroGravity.Pages
{
    public class FitBitCallbackModel : PageModel
    {
        public IStringLocalizer<FitBitCallbackModel> StringLocalizer;

        public FitBitCallbackModel(IStringLocalizer<FitBitCallbackModel> stringLocalizer)
        {
            StringLocalizer = stringLocalizer;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
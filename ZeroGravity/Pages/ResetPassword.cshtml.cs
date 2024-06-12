using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using ZeroGravity.Interfaces;
using LocRes = ZeroGravity.Resources.Pages.ResetPasswordModel;

namespace ZeroGravity.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IAccountService _accountService;
        
        [BindProperty]
        public ResetPasswordInputModel Input { get; set; }

        public bool Result { get; private set; }

        private string Token { get; set; }

        public ResetPasswordModel(IAccountService accountService, 
            IStringLocalizer<ResetPasswordModel> stringLocalizer)
        {
            _accountService = accountService;
            StringLocalizer = stringLocalizer;
        }

        public IStringLocalizer<ResetPasswordModel> StringLocalizer { get; }

        public IActionResult OnGet(string token)
        {
            Token = token;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = Request.Path.Value.Split('/').Last();
                    _accountService.ResetPassword(token, Input.Email, Input.Password);
                    Result = true;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    Result = false;
                }
            }

            return Page();
        }
    }

    public class ResetPasswordInputModel
    {
        [Required(
            ErrorMessageResourceType = typeof(LocRes), 
            ErrorMessageResourceName = nameof(LocRes.Email_Required))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(LocRes),
            ErrorMessageResourceName = nameof(LocRes.Email_Validation))]
        [Display(
            ResourceType = typeof(LocRes),
            Name = nameof(LocRes.Email_Validation))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(
            ErrorMessageResourceType = typeof(LocRes),
            ErrorMessageResourceName = nameof(LocRes.Password_Required))]
        [MinLength(6, 
            ErrorMessageResourceType = typeof(LocRes),
            ErrorMessageResourceName = nameof(LocRes.Password_MinLength))]
        [Display(
            ResourceType = typeof(LocRes),
            Name = nameof(LocRes.Password_Display))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(
            ErrorMessageResourceType = typeof(LocRes),
            ErrorMessageResourceName = nameof(LocRes.ConfirmPassword_Required))]
        [Compare(nameof(Password),
            ErrorMessageResourceType = typeof(LocRes),
            ErrorMessageResourceName = nameof(LocRes.ConfirmPassword_Compare))]
        [Display(
            ResourceType = typeof(LocRes),
            Name = nameof(LocRes.ConfirmPassword_Display))]
        public string ConfirmPassword { get; set; }
    }
}

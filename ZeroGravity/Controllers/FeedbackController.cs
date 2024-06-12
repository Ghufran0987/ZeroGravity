using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;
using AccountResponse = ZeroGravity.Models.Accounts.AccountResponse;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        private readonly IStringLocalizer<FeedbackController> _stringLocalizer;
        private readonly AppSettings _appSettings;

        public FeedbackController(
            IFeedbackService feedbackService,
            IEmailService emailService,
            IAccountService accountService,
            IStringLocalizer<FeedbackController> stringLocalizer,
            IOptions<AppSettings> appSettings)
        {
            _feedbackService = feedbackService;
            _emailService = emailService;
            _accountService = accountService;
            _stringLocalizer = stringLocalizer;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<FeedbackSummaryDto>> GetByTargetdate(int id, DateTime date)
        {
            VerifyAccountId(id);
            var feedbackSummaryDto = await _feedbackService.GetFeedbackByAccountIdAndDateAsync(id, date);

            return Ok(feedbackSummaryDto);
        }

        [Authorize]
        [HttpPost("coaching/interest")]
        public async Task<ActionResult<bool>> SubmitCoachingInterest(CoachingInterestModel model)
        {
            try
            {
                VerifyAccountId(model?.UserId);

                await Task.Run(() =>
                {
                    var account = _accountService.GetById(model.UserId);

                    var to = _appSettings.CoachingSettings.EmailFrom;
                    var subject = _stringLocalizer[MessageType.Feedback.CoachingMailSubject];
                    var body = GetSubmitCoachingInterestBody(account, model.CoachingOptions);

                    _emailService.Send(to, subject, body);
                });

                return Ok(true);
            }
            catch
            {
                return NotFound(false);
            }
        }

        private string GetSubmitCoachingInterestBody(AccountResponse account, CoachingType coachingType)
        {
            var body = new StringBuilder();

            body.Append("<p>");

            string coaching;
            if (coachingType == CoachingType.Nutrition)
            {
                coaching = _stringLocalizer[MessageType.Feedback.CoachingNutrition];
            }
            else if (coachingType == CoachingType.Personal)
            {
                coaching = _stringLocalizer[MessageType.Feedback.CoachingPersonal];
            }
            else
            {
                coaching = _stringLocalizer[MessageType.Feedback.CoachingMental];
            }

            var name = $"{account.FirstName} {account.LastName}";
            var msg = _stringLocalizer[MessageType.Feedback.CoachingMailBody];
            body.Append(string.Format(msg, name, coaching, account.Email));
            body.Append("</p>");

            return body.ToString();
        }

        private string GetSubmitCoachingInterestBody(AccountResponse account, List<string> options)
        {
            var body = new StringBuilder();

            body.Append("<p>");
            var name = $"{account.FirstName} {account.LastName}";
            // var msg = _stringLocalizer[MessageType.Feedback.CoachingMailBody];
            body.AppendLine("Name: " + name);
            body.Append("</p>");
            body.Append("<p>");
            body.AppendLine("Email: " + account.Email);
            body.Append("</p>");
            body.Append("<p>");
            body.AppendLine("Selected Coaching Options:");
            body.Append("</p>");
            foreach (var s in options)
            {
                body.Append("<p>");
                body.Append(s);
                body.Append("</p>");
                body.AppendLine();
            }

            return body.ToString();
        }
    }
}
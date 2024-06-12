namespace ZeroGravity.Shared.Models
{
    public class MessageType
    { 
        public static string RegistrationSuccessful = "RegistrationSuccessful";
        public static string VerificationSuccessful = "VerificationSuccessful";
        public static string MailOrPasswordIncorrect = "MailOrPasswordIncorrect";
        public static string TokenRequired = "TokenRequired";
        public static string Unauthorized = "Unauthorized";
        public static string TokenRevoked = "TokenRevoked";
        public static string ForgotPassword = "ForgotPassword";
        public static string TokenValid = "TokenValid";
        public static string PasswordResetSuccessful = "PasswordResetSuccessful";
        public static string AccountDeleted = "AccountDeleted";
        public static string MailAlreadyRegistered = "MailAlreadyRegistered";
        public static string VerificationFailed = "VerificationFailed";
        public static string InvalidToken = "InvalidToken";
        public static string MailAlreadyTaken = "MailAlreadyTaken";
        public static string AccountNotFound = "AccountNotFound";
        public static string AccountNotVerified = "AccountNotVerified";
        public static string ProfileImageUploadFailed = "ProfileImageUploadFailed";
        public static string NewEmailRequestedSuccessful = "NewEmailRequestedSuccessful";
        public static string PasswordChangedSuccessful = "PasswordChangedSuccessful";
        public static string PasswordOldWrong = "PasswordOldWrong";
        public static string PasswordNewEmpty = "PasswordNewEmpty";
        public static string PasswordNewNotMatch = "PasswordNewNotMatch";
        public static string PasswordTooShort = "PasswordTooShort";



        public static ResetPasswordMessageType ResetPassword = new ResetPasswordMessageType();
        public static RegisterWelcomeMessageType RegisterWelcome = new RegisterWelcomeMessageType();
        public static ErrorMessageType Error = new ErrorMessageType();
        public static EmailChangedMessageType EmailChanged = new EmailChangedMessageType();
        public static FeedbackMessageType Feedback = new FeedbackMessageType();
        public static FitbitCallbackMessageType FitbitCallback = new FitbitCallbackMessageType();
    }

    public class ResetPasswordMessageType
    {
        public string Title = "Title";
        public string Email = "Email";
        public string Password = "Password";
        public string ConfirmPassword = "ConfirmPassword";
        public string ResetPassword = "Submit.ResetPassword";
        public string ResetSuccessful = "ResetSuccessful";
    }

    public class RegisterWelcomeMessageType
    {
        public string WelcomeTitle = "Welcome.Title";
        public string WelcomeMessage = "Welcome.Message";
    }

    public class FitbitCallbackMessageType
    {
        public string CallbackTitle = "Callback.Title";
        public string CallbackMessage = "Callback.Message";
    }

    public class ErrorMessageType
    {
        public string Title = "Error.Title";
    }

    public class EmailChangedMessageType
    {
        public string EmailChangedTitle = "EmailChanged.Title";
        public string EmailChangedMessage = "EmailChanged.Message";
    }

    public class FeedbackMessageType
    {
        public string CoachingMailBody = "Coaching_Mail_Body";
        public string CoachingMailSubject = "Coaching_Mail_Subject";
        public string CoachingNutrition = "Coaching_Nutrition";
        public string CoachingPersonal = "Coaching_Personal";
        public string CoachingMental = "Coaching_Mental";
    }

}

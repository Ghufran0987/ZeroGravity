using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FitbitCallbackDto
    {
        public string Code { get; set; }

        public string Access_Token { get; set; }

        public string Refresh_Token { get; set; }

        public string User_Id { get; set; }

        public string Scope { get; set; }

        public string Token_Type { get; set; }

        public int Expires_In { get; set; }

        public string State { get; set; }
    }
}

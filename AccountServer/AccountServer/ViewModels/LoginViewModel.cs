﻿namespace AccountServer.ViewModels
{
    public class LoginViewModel
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public string user_id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}
namespace Luveck.Service.Security.Utils.Jwt
{
    public class JwtSetting
    {
        public string securityKey { get; set; }
        public string validIssuer { get; set; }
        public string validAudience { get; set; }
    }
}

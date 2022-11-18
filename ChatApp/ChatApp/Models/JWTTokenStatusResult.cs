namespace ChatApp.Models
{
    public class JWTTokenStatusResult
    {
        public string Token { get; set; }
        public bool IsAuthorized { get; set; }
        public User UserInfo { get; set; }
    }
}

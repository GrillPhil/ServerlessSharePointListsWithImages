namespace MyListApp.Auth
{
    public class AuthOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string Resource { get; set; }
        public string Scope { get; set; }
    }
}

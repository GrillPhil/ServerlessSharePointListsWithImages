using System.Threading.Tasks;

namespace MyListApp.Auth
{
    public interface IAuthService
    {
        public Task<string> GetAccessTokenAsync();
    }
}

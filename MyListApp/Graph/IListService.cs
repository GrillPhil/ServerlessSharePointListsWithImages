using System.Threading.Tasks;

namespace MyListApp.Graph
{
    public interface IListService
    {
        Task CreateItemAsync(string siteId, string listId, ListItem item);
    }
}

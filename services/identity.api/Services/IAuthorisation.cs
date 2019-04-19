using System.Threading.Tasks;

namespace Identity.Api.Services
{
    public interface IAuthorisation
    {
        Task<string> GetTestValue();
    }
}
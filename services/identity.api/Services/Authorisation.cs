using System.Threading.Tasks;

namespace Identity.Api.Services
{
    public class Authorisation: IAuthorisation
    {
        public Task<string> GetTestValue(){
            return Task.FromResult("Hello World");
        }
    }
}
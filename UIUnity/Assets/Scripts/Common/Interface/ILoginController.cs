using System.Threading.Tasks;

namespace Common.Interface
{
    public interface ILoginController : IController
    {
        string Token { get; }
        
        Task<(bool resultOk, string messageError)> Login(string userName, string password);
    }
}
using System.Threading.Tasks;

namespace Industria4.Depedencies
{
    public interface IDependencyChecker
    {
        Task WaitForReady();
    }
}

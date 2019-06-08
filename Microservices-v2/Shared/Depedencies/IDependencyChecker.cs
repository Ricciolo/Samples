using System.Threading.Tasks;

namespace Muuvis.Depedencies
{
    public interface IDependencyChecker
    {
        Task WaitForReady();
    }
}

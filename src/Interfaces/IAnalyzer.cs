using System.Threading.Tasks;

namespace nuget_audit.Interfaces
{
    public interface IAnalyzer
    {
        Task<bool> Analyze();
    }
}
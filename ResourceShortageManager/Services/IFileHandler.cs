using ResourceShortageManager.Models;

namespace ResousceShortageManager.Services
{
    public interface IFileHandler
    {
        List<Shortage> LoadShortages();
        void SaveShortages(List<Shortage> shortages);
    }
}

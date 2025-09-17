namespace AdvertisingPlatforms.Services
{
    public interface IAdPlatformService
    {
        List<string> FindPlatformsForLocation(string location);
        void LoadFile(IFormFile file);
        Dictionary<string, List<string>> GetCurrentData();
    }
}
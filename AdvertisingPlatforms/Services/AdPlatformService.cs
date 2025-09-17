using System.Text;

namespace AdvertisingPlatforms.Services
{
    public class AdPlatformService : IAdPlatformService
    {
        private Dictionary<string, List<string>> locationToPlatforms = new Dictionary<string, List<string>>(); // путь - локации

        private readonly object lockObject = new object();

        public void LoadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Файл был пустой");
            }

            lock (lockObject)
            {
                var newData = new Dictionary<string, List<string>>();

                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            var parts = line.Split(':');

                            string platformName = parts[0].Trim();
                            string locationsString = parts[1].Trim();

                            var locations = locationsString.Split(',')
                                .Select(l => l.Trim())
                                .Where(l => !string.IsNullOrEmpty(l))
                                .ToList();

                            foreach (var location in locations)
                            {
                                if (!newData.ContainsKey(location))
                                {
                                    newData[location] = new List<string>();
                                }

                                if (!newData[location].Contains(platformName))
                                {
                                    newData[location].Add(platformName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //
                        }
                    }
                }

                locationToPlatforms = newData;
            }
        }

        public List<string> FindPlatformsForLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("Локация не может быть пустой");
            }

            if (!location.StartsWith("/"))
            {
                throw new ArgumentException("Локация должна начинаться с /");
            }


            var result = new HashSet<string>();

            var locationParts = location.Trim('/').Split('/');

            for (int i = locationParts.Length; i >= 0; i--)
            {
                string currentPath = "/" + string.Join("/", locationParts.Take(i));

                if (i == 0) currentPath = "/";

                if (locationToPlatforms.TryGetValue(currentPath, out var platforms))
                {
                    foreach (var platform in platforms)
                    {
                        result.Add(platform);
                    }
                }
            }

            return result.OrderBy(p => p).ToList();
        }

        public Dictionary<string, List<string>> GetCurrentData()
        {
            lock (lockObject)
            {
                return new Dictionary<string, List<string>>(locationToPlatforms);
            }
        }
    }
}

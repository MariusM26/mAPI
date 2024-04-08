#nullable disable
namespace UiTests.Models
{
    public class ApplicationSettings
    {
        public static ApplicationSettings Instance { get; private set; }


        public static void Configure(ApplicationSettings applicationSettings)
        {
            Instance = applicationSettings;
        }
    }
}
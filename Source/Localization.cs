using MyConferenceCalls.Resources;

namespace MyConferenceCalls
{
    public class Localization
    {
        private static AppResources localizedResources = new AppResources();
        public AppResources LocalizedResources { get { return localizedResources; } }
    }
}

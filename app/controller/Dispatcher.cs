
namespace TribalWarsBot
{
    /** Dispatcher handles all concurency logic -- singleton pattern*/
    public sealed class Dispatcher
    {
        private static Dispatcher? instance = null;
        private static readonly Object _browserLock = new ();
        private static readonly Object  _configLock = new (); // ---toto asi bude robit problem pri vymenach villagov na pozadi chodu, uvidím---

        public static Dispatcher GetInstance()
        {
            if (instance == null) instance = new Dispatcher();
            return instance;
        }
        // Only one instance of WebDriver browser should be running
        public static Object GetBrowserLock()
        {
            return _browserLock;
        }
        // When config is loaded/updated should not be accessed by other threads
        public static Object GetConfigLock()
        {
            return _configLock;
        }
    }
}

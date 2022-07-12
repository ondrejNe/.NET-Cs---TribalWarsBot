using OpenQA.Selenium.Chrome;

namespace TribalWarsBot
{
    public class Browser
    {
        private static ChromeDriver? CHROME_DRIVER = null;
        private static ChromeOptions? CHROME_OPTIONS = null;
        /** Wait times the browser waits for element to load, aftewards error is thrown */
        private const int DEFAULT_WAIT_TIME = 5;
        private static readonly int PAGE_LOAD_WAIT_TIME = DEFAULT_WAIT_TIME;
        private static readonly int IMPLICIT_WAIT_TIME  = DEFAULT_WAIT_TIME;
        private static readonly int ASYNC_JS_WAIT_TIME  = DEFAULT_WAIT_TIME;
        /** Set's new wait time to improve response and waiting time */
        public static void SetNewWaitTimes(int time)
        {
            if (CHROME_DRIVER == null) return;
            CHROME_DRIVER.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(time);
            CHROME_DRIVER.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(time);
            CHROME_DRIVER.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(time);
        }
        /** Returns default wait times */
        public static void SetDefaultWaitTimes()
        {
            if (CHROME_DRIVER == null) return;
            CHROME_DRIVER.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(PAGE_LOAD_WAIT_TIME);
            CHROME_DRIVER.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(IMPLICIT_WAIT_TIME);
            CHROME_DRIVER.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(ASYNC_JS_WAIT_TIME);
        }
        /** Returns singleton of chrome options -- how the browser should be started */
        // headless seem to not work with some elements including button classes
        // should look into it
        public static ChromeOptions GetOptionsInstance()
        {
            if (CHROME_OPTIONS == null)
            {
                CHROME_OPTIONS = new ChromeOptions();
                CHROME_OPTIONS.AddArgument("start-maximized");
                //CHROME_OPTIONS.AddArgument("headless");
                CHROME_OPTIONS.AddArgument("disable-infobars");
                //CHROME_OPTIONS.AddExcludedArgument("enable-automation");
                CHROME_OPTIONS.AddExcludedArgument("enable-logging");
            }
            return CHROME_OPTIONS;
        }
        /** Returns existing or new webdirver instance. If new the browser is opened 
         ** with chrome options */
        public static ChromeDriver GetDriverInstance()
        {
            if (CHROME_DRIVER == null)
            {
                CHROME_DRIVER = new ChromeDriver(GetOptionsInstance());
                CHROME_DRIVER.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(PAGE_LOAD_WAIT_TIME);
                CHROME_DRIVER.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(IMPLICIT_WAIT_TIME);
                CHROME_DRIVER.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(ASYNC_JS_WAIT_TIME);
            }
            return CHROME_DRIVER;
        }
        /** Correct way to close web driver instance. Subsequently close the task and window etc. */
        public static void CloseDriverInstance()
        {
            GetDriverInstance().Close();
            GetDriverInstance().Quit();
            CHROME_DRIVER = null;
            CHROME_OPTIONS = null;
        }
    }
}

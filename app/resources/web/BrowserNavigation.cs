using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TribalWarsBot
{ 
    internal class BrowserNavigation
    {
        /** Loads the tribal wars specified URL */
        public static bool NavigateMainPage()
        {
            Browser.GetDriverInstance().Navigate().GoToUrl(TWConfig.GetTWConfig().GAME_URL);
            return true;
        }
        /** Logins to tribal wars account with specified credentials */
        public static bool LoginMainPage()
        {
            try
            {
                ChromeDriver DRIVER = Browser.GetDriverInstance();
                // Input login data
                DRIVER.FindElement(By.Id("user")).SendKeys(TWConfig.GetTWConfig().WORLD_PLAYER_NAME);
                DRIVER.FindElement(By.Id("password")).SendKeys(TWConfig.GetTWConfig().WORLD_PLAYER_PASS);
                // Look for h-captcha
                bool found = CaptchaBreaker.DetectHcaptcha();
                // Try to Login
                DRIVER.FindElement(By.ClassName("btn-login")).Click();
                // If there is h-captcha
                // h-captcha shows up after login try
                if(found) CaptchaBreaker.SolveHcaptcha();
                // Login to correct world
                IReadOnlyList<IWebElement> elements = DRIVER.FindElements(By.ClassName("world_button_active"));
                foreach(IWebElement element in elements)
                {
                    if(element.Text.Equals(TWConfig.GetTWConfig().WORLD_NAME))
                    {
                        element.Click();
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // error happened
                return false;
            }
            return true;
        }
        /** After successful login only. Loads URL of village place and navigates to scavenge page */
        public static bool NavigateScavengeVillage(ScavengeVillage village)
        {
            ChromeDriver DRIVER = Browser.GetDriverInstance();
            try
            {
                DRIVER.Navigate().GoToUrl(TWConfig.GetTWConfig().WORLD_URL + "village=" + village.Id + "&screen=place&mode=scavenge");
            }
            catch(Exception)
            {
                // error happened
                return false;
            }
            return true;
        }
    }
}

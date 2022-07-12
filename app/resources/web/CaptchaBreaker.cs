using OpenQA.Selenium;
using TwoCaptcha.Captcha;

namespace TribalWarsBot
{
    /** Class designed to break h-captcha on websites */
    internal class CaptchaBreaker
    {
        /** Returns whether h-captha is present in the website a prepares the site for breaking */
        public static bool DetectHcaptcha()
        {
            try
            {
                IWebElement captcha = Browser.GetDriverInstance().FindElement(By.Id("captcha"));
                if(captcha.GetAttribute("class") == "h-captcha")
                {
                    // Get H captcha callback function reference before it gets rendered on screen
                    // it is necessary, workings: we get reference for callback before render
                    // get reference -> h-captcha render is called after some action --> 
                    // captcha gets solved --> textareas are filled --> callback with answer must be called
                    // I don't know how to access callback when h-captcha has been already rendered
                    IJavaScriptExecutor jse = ((IJavaScriptExecutor)Browser.GetDriverInstance());
                    jse.ExecuteScript("window.originalRender = hcaptcha.render;");
                    jse.ExecuteScript("window.hcaptcha.render = (container, params) => { window.hcaptchaCallback = params.callback; return window.originalRender(container, params); }");
                }
                return true;
            }
            catch (Exception)
            {
                // no h-capthca present on site
                return false;
            }
        }
        /** Solves the h-capthca using 3rd party API (2capthca.com) */
        public static void SolveHcaptcha()
        {
            // --------- Console --------- //
            Console.WriteLine("Solving captcha");
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            // --------- Console --------- //
            TwoCaptcha.TwoCaptcha solver = new(TWConfig.GetTWConfig().CAPTCHA_TOKEN);
            HCaptcha captcha = new();
            captcha.SetSiteKey(Browser.GetDriverInstance().FindElement(By.Id("captcha")).GetAttribute("data-sitekey"));
            captcha.SetUrl(Browser.GetDriverInstance().Url);
            try
            {
                solver.Solve(captcha).Wait();
                // --------- Console --------- //
                Console.WriteLine("Captcha solved");
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                // --------- Console --------- //
                IJavaScriptExecutor jse = ((IJavaScriptExecutor)Browser.GetDriverInstance());
                // Submit answer to textareas and call render-apriori saved callback
                jse.ExecuteScript("document.getElementsByName('g-recaptcha-response')[0].innerHTML = '" + captcha.Code + "';");
                jse.ExecuteScript("document.getElementsByName('h-captcha-response')[0].innerHTML = '" + captcha.Code + "';");
                jse.ExecuteScript("window.hcaptchaCallback('" + captcha.Code + "');");
            }
            catch (Exception e)
            {
                Console.WriteLine("Captcha error occurred: " + e.Message);
                throw new Exception("Captcha yielded error" + e.Message);
            }
        }
    }
}

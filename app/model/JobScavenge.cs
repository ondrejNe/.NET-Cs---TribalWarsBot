﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;

namespace TribalWarsBot
{
    /** Scavenge thread handling a single village*/
    internal class JobScavenge : Job
    {
        // Village where the scavenge should be handled
        public ScavengeVillage village;

        private readonly List<int> unitsInVillageTotal = new(TWConst.UNIT_CAPACITY_LENGHT);
        private readonly List<int> unitsInVillageAvail = new(TWConst.UNIT_CAPACITY_LENGHT);
        // Web elements
        private IReadOnlyList<IWebElement>? scavengeOptions;
        private IReadOnlyList<IWebElement>? unitButtons;
        private IReadOnlyList<IWebElement>? unitInputFields;
        // Web controll driver from Selenium
        private ChromeDriver? DRIVER;

        public JobScavenge(ScavengeVillage village)
        {
            this.village = village;
            this.TaskID = Cryptography.ComputeSha256Hash(village.Name);
            this.TaskType = TASK_TYPE.SCAVENGE;
        }
        /** Stops the scavenging script. Last/current scavenge will finish, but new one will not start 
         ** by this instance */
        public void Stop()
        {
            this.Run = false;
        }
        /** Starts the scavenging script for current village as specified and loaded from config */
        public void Start()
        {
            int errorCounter = 0;
            while (this.Run)
            {
                lock (Dispatcher.GetBrowserLock())
                {
                    DRIVER = Browser.GetDriverInstance();
                    try
                    {
                        GetToScavenge();
                        IReadOnlyList<IWebElement> countdowns = DRIVER.FindElements(By.ClassName("return-countdown"));
                        if (countdowns.Count > 0)
                        {
                            WaitForScavenge();
                        }
                        else
                        {
                            StartScavenge();
                            WaitForScavenge();
                        }
                        // Reset error counter
                        errorCounter = 0;
                    }
                    catch (Exception)
                    {
                        // Error occured
                        errorCounter++;
                        // 200s timetout
                        this.TimeoutTime = 200;
                        if(errorCounter == 5)
                        {
                            // --------- Console --------- //
                            Console.WriteLine("Scavenge job terminated due to 5 times unavailability");
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                            // --------- Console --------- //
                        }
                    }
                    Browser.CloseDriverInstance();
                }
                // --------- Console --------- //
                Console.WriteLine("Sleeping for " + TimeoutTime);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                // --------- Console --------- //
                SleepTimetout();
            }
        }
        /** Sets the job timeout based on remaining scavenge options */
        public void WaitForScavenge()
        {
            LocateElements();
            this.TimeoutTime = 0;
            if (scavengeOptions == null) return;
            foreach (IWebElement elem in scavengeOptions)
            {
                try
                {
                    IWebElement option = elem.FindElement(By.ClassName("return-countdown"));
                    String[] time = option.Text.Split(":");
                    int timeout1 = int.Parse(time[0]) * 3600 + int.Parse(time[1]) * 60 + int.Parse(time[2]);
                    this.TimeoutTime = Math.Max(timeout1, TimeoutTime);
                }
                catch (Exception) { /* Error has no effect */}
            }
            TimeoutTime += 5;
        }
        /** Locates and saves basic web elements for further processing */
        public void LocateElements()
        {
            if (DRIVER == null) return;
            scavengeOptions = DRIVER.FindElements(By.ClassName("scavenge-option"));
            unitButtons = DRIVER.FindElements(By.ClassName("units-entry-all"));
            unitInputFields = DRIVER.FindElements(By.ClassName("unitsInput"));
        }
        /** Loads tribal wars & logins & gets to scavenge of the job's village */
        public void GetToScavenge()
        {
            BrowserNavigation.NavigateMainPage();
            BrowserNavigation.LoginMainPage();
            BrowserNavigation.NavigateScavengeVillage(village);
        }
        /** Sets the scavenge units and sends them out (handles web elements inputs & clicks) */
        public void StartScavenge()
        {
            LocateElements();
            if (scavengeOptions == null) return;

            bool[] optionReady = { true, true, true, true };
            int optionReadyCount = optionReady.Length;
            // Reduce unavailable options
            for (int i = 0; i < optionReady.Length; i++)
            {
                try
                {
                    scavengeOptions[i].FindElement(By.ClassName("inactive-view"));
                }
                catch (Exception)
                {
                    optionReady[i] = false;
                    optionReadyCount--;
                }
            }
            if (optionReadyCount == 0) return;
            // Haul determination functions
            double totalPossibleHaul = CalculatePossibleVillageHaul();
            double dividerMultiplier = CalculateVillageHaulMultiplier(optionReady);
            if (dividerMultiplier == 0) return;
            // Send scavenging option
            int partialHaul = (int)(totalPossibleHaul / dividerMultiplier);
            for (int i = optionReadyCount - 1; i > -1; i--)
            {
                if (optionReady[i]) { SendScavenge(i, partialHaul); }
            }
        }
        /** Returns total capacity by units in village */
        private double CalculatePossibleVillageHaul()
        {
            SetVillageUnits();
            int haul = 0;
            int i = 0;
            foreach (var capacity in TWConst.UNIT_CAPACITIES)
            {
                haul += unitsInVillageAvail[i] * capacity;
                i++;
            }
            return haul;
        }
        /** Extracts village units from webpage and saves their counts */
        private void SetVillageUnits()
        {
            // Empty arrays
            unitsInVillageTotal.Clear();
            unitsInVillageAvail.Clear();
            if (unitButtons == null) throw new Exception("Unit buttons were not found in scavenge");
            // Set total array
            foreach(WebElement elem in unitButtons)
            {
                String s = elem.Text;
                // Parentheses are translated as the -number ; -1 corrects this behavior
                unitsInVillageTotal.Add(int.Parse(s, NumberStyles.AllowParentheses) * -1);
            }
            // Removing herald -- does not effect next code -- just reminder
            unitsInVillageTotal.RemoveAt(unitsInVillageTotal.Count - 1);
            // Set allowed units to be used in scavenge array
            village.SetAllowedCounts(unitsInVillageTotal, unitsInVillageAvail);
        }
        /** Returns multiplier divider based on number of available scavenging options */
        private static double CalculateVillageHaulMultiplier(bool[] optionReady)
        {
            double ret = 0;
            for (int i = 0; i < optionReady.Length; i++)
            {
                if (optionReady[i]) ret += TWConst.HAUL_MULTIPLIER[i];
            }
            return ret;
        }
        /** Send the specific scaveng option */
        private void SendScavenge(int optionNumber, int partialHaul)
        {
            if (unitButtons == null) throw new Exception("Unit buttons were not found in scavenge");
            if (unitInputFields == null) throw new Exception("Unit input fields were not found in scavenge");
            // Calculate necessary haul
            int desiredHaul = (int)(partialHaul * TWConst.HAUL_MULTIPLIER[optionNumber]);
            int desiredUnitCount, actualUnitCount;

            for (int i = 0; i < unitsInVillageAvail.Count; i++)
            {
                // Number of this specific unit i would want to have
                desiredUnitCount = desiredHaul / TWConst.UNIT_CAPACITIES[i];
                // Number i actualy can get from available units
                actualUnitCount = Math.Min(desiredUnitCount, unitsInVillageAvail[i]);
                // Enter the number
                unitInputFields[i].SendKeys(actualUnitCount.ToString());
                // Lower the reamaining haul
                desiredHaul -= actualUnitCount * TWConst.UNIT_CAPACITIES[i];
                // Update available village units
                unitsInVillageAvail[i] -= actualUnitCount;
            }
            ClickScavengeOption(optionNumber);
        }
        /** Clicks the web element regarding the specific scavenging option */
        private void ClickScavengeOption(int option)
        {
            if (scavengeOptions == null) throw new Exception("Scavenge options were not found in scavenge");
            scavengeOptions[option].FindElement(By.ClassName("free_send_button")).Click();
            Thread.Sleep(1000);
        }
    }
}

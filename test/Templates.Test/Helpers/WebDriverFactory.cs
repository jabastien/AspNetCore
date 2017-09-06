﻿using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Templates.Test.Helpers
{
    public static class WebDriverFactory
    {
        public static bool HostSupportsBrowserAutomation
            => IsAppVeyor || OSSupportsEdge();

        private static bool IsAppVeyor
            => Environment.GetEnvironmentVariables().Contains("APPVEYOR");

        public static IWebDriver CreateWebDriver()
        {
            // Where possible, it's preferable to use Edge because it's
            // far faster to automate than Chrome/Firefox. But on AppVeyor
            // only Firefox is available.
            var result = IsAppVeyor ? new FirefoxDriver() : (IWebDriver)new EdgeDriver();
            result.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            return result;
        }

        private static int GetWindowsVersion()
        {
            var osDescription = RuntimeInformation.OSDescription;
            var windowsVersion = Regex.Match(osDescription, "^Microsoft Windows (\\d+)\\..*");
            return windowsVersion.Success ? int.Parse(windowsVersion.Groups[1].Value) : -1;
        }

        private static bool OSSupportsEdge()
        {
            var windowsVersion = GetWindowsVersion();
            return (windowsVersion >= 10 && windowsVersion < 2000)
                || (windowsVersion >= 2016);
        }
    }
}

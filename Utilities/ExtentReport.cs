using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowProjectFramework.Utilities
{
    public class ExtentReport
    {

        public static ExtentReports _extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;
        
        public static string path = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultFolder = path.Substring(0, path.IndexOf("bin")) + "TestResults\\";

        public static void ExtentReporterInitialise()
        {
            var htmlreporter = new ExtentHtmlReporter(testResultFolder);
            htmlreporter.Config.DocumentTitle = "Automation Report";
            htmlreporter.Config.ReportName = "Automation Specflow";
            htmlreporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(htmlreporter);


        }

        public static void FlushReport()
        {
            _extentReports.Flush();
        }

        public static string TakeScreenshot(IWebDriver driver, ScenarioContext scenario)
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            String screenshotLocation = Path.Combine(path, scenario.ScenarioInfo.Description + ".png");
            screenshot.SaveAsFile(screenshotLocation, ScreenshotImageFormat.Png);
            
            return screenshotLocation;
        }
    }
}

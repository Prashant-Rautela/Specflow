using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SpecFlowProjectFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SpecFlowProjectFramework.Hooks
{
    [Binding]
    public sealed class Hooks : ExtentReport
    {
        private readonly IObjectContainer objectContainer;
        private IWebDriver driver;

        public Hooks(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }


        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ExtentReporterInitialise();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            FlushReport();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {

            _feature = _extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            Console.WriteLine("after feature");
        }

        [BeforeScenario]

        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }


        [BeforeStep]
        public void BeforeStep(ScenarioContext scenarioContext)

        {
            Console.WriteLine("before scenario");


        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            Console.WriteLine("after step");

            var steptype = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            var stepDescription = scenarioContext.StepContext.StepInfo.Text;

            if (scenarioContext.TestError == null)
            {

                switch ((steptype)
)
                {
                    case ("Given"):
                        _scenario.CreateNode<Given>(stepDescription);
                        break;

                    case ("When"):
                        _scenario.CreateNode<When>(stepDescription);
                        break;

                    case ("Then"):
                        _scenario.CreateNode<Then>(stepDescription);
                        break;

                    case ("And"):
                        _scenario.CreateNode<And>(stepDescription);
                        break;

                    default:
                        break;
                }

            }


            if (scenarioContext.TestError != null)
            {
                string screenshotLocation = TakeScreenshot(driver, scenarioContext);
                switch (steptype)
                {

                    case ("Given"):
                        _scenario.CreateNode<Given>(stepDescription).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                        break;
                    case ("When"):
                        _scenario.CreateNode<When>(stepDescription).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                        break;
                    case ("Then"):
                        _scenario.CreateNode<Then>(stepDescription).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                        break;
                    case ("And"):
                        _scenario.CreateNode<And>(stepDescription).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                        break;

                    default:
                        break;

                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = objectContainer.Resolve<IWebDriver>();
            if (driver != null)
            {
                driver.Quit();
            }


        }

    
        }
    }

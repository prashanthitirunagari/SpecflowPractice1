using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using System.IO;
using TechTalk.SpecFlow;
using SpecflowPractice;

namespace SpecflowPractice.Hooks
{
    [Binding]
    public sealed class SpecHooks
    {


        private Helper _driverHelper;

        private static FeatureContext _featureContext;
        private static ScenarioContext _scenarioContext;
        private static ExtentReports _extentReport;
        private static ExtentTest _featureName;
        private static ExtentTest _scenarioName;


        public SpecHooks(Helper driverHelper, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _featureContext = featureContext;
            _driverHelper = driverHelper;
            _scenarioContext = scenarioContext;

        }
        [BeforeTestRun]
        public static void InitializeSettings()
        {
            var path = Directory.GetCurrentDirectory().Replace(@"\bin\Debug\net5.0", "") + @"\Report\TestResult.html";
            var htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            _extentReport = new ExtentReports();
            _extentReport.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            _extentReport.Flush();
        }

        [BeforeFeature]
        public static void BeforeFeatureStart(FeatureContext featureContext)
        {
            _featureContext = featureContext;
            if (null != _featureContext)
            {
                _featureName = _extentReport.CreateTest<Feature>(_featureContext.FeatureInfo.Title,
                    _featureContext.FeatureInfo.Description);
            }
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            if (null != _scenarioContext && null != _featureName)
            {
                _scenarioName = _featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title,
                   _scenarioContext.ScenarioInfo.Description);
            }

        }

        [AfterStep]
        public void AfterStep()
        {
            ScenarioBlock scenarioBlock = _scenarioContext.CurrentScenarioBlock;
            var exception = _scenarioContext.TestError;
            var mediaEntityPass = _driverHelper.CaptureScreenshot(_scenarioContext.ScenarioInfo.Title.Trim());

            if (null == exception)
            {
                switch (scenarioBlock)
                {
                    case ScenarioBlock.Given:
                        _scenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Pass("Pass",mediaEntityPass);
                        //_scenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case ScenarioBlock.When:
                        _scenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case ScenarioBlock.Then:
                        _scenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    default:
                        _scenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                }
            }
            else if (null != exception)
            {
                var mediaEntity = _driverHelper.CaptureScreenshot(_scenarioContext.ScenarioInfo.Title.Trim());
                switch (scenarioBlock)
                {
                    case ScenarioBlock.Given:
                        _scenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);
                        break;
                    case ScenarioBlock.When:
                        _scenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);
                        break;
                    case ScenarioBlock.Then:
                        _scenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);
                        break;
                    default:
                        _scenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);
                        break;
                }
            }

        }


        [AfterScenario]
        public void AfterScenario()
        {
            _driverHelper.driver.Quit();
        }

    }
}

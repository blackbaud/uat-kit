using Blackbaud.UAT.SpecFlow.Selenium;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.UnitTestProvider;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.Configuration;

[assembly: RuntimePlugin(typeof(RuntimePlugin))]

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    public class RuntimePlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            runtimePluginEvents.RegisterGlobalDependencies += RegisterDependencies;
        }

        public void RegisterDependencies(object sender, RegisterGlobalDependenciesEventArgs eventArgs)
        {
            var runtimeProvider = new NUnitRuntimeProvider();

            eventArgs.ObjectContainer.RegisterInstanceAs<IUnitTestRuntimeProvider>(runtimeProvider, "BBTest");
        }
    }
}

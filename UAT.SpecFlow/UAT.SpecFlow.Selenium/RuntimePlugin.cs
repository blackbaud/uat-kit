using Blackbaud.UAT.SpecFlow.Selenium;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.UnitTestProvider;

[assembly: RuntimePlugin(typeof(RuntimePlugin))]

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    public class RuntimePlugin : IRuntimePlugin
    {
        public void RegisterConfigurationDefaults(TechTalk.SpecFlow.Configuration.RuntimeConfiguration runtimeConfiguration)
        {

        }

        public void RegisterCustomizations(BoDi.ObjectContainer container, TechTalk.SpecFlow.Configuration.RuntimeConfiguration runtimeConfiguration)
        {

        }

        public void RegisterDependencies(BoDi.ObjectContainer container)
        {
            var runtimeProvider = new NUnitRuntimeProvider();

            container.RegisterInstanceAs<IUnitTestRuntimeProvider>(runtimeProvider, "BBTest");
        }
    }
}

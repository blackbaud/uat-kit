using Blackbaud.UAT.SpecFlow.Selenium;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Utils;

[assembly: GeneratorPlugin(typeof(GeneratorPlugin))]

namespace Blackbaud.UAT.SpecFlow.Selenium
{
    public class GeneratorPlugin : IGeneratorPlugin
    {
        public void RegisterConfigurationDefaults(TechTalk.SpecFlow.Generator.Configuration.SpecFlowProjectConfiguration specFlowConfiguration)
        {
            
        }

        public void RegisterCustomizations(BoDi.ObjectContainer container, TechTalk.SpecFlow.Generator.Configuration.SpecFlowProjectConfiguration generatorConfiguration)
        {
            
        }

        public void RegisterDependencies(BoDi.ObjectContainer container)
        {
            var projectSettings = container.Resolve<ProjectSettings>();

            var codeDomHelper = container.Resolve<CodeDomHelper>(projectSettings.ProjectPlatformSettings.Language);

            var generatorProvider = new BlueTestGeneratorProvider(codeDomHelper);

            container.RegisterInstanceAs<IUnitTestGeneratorProvider>(generatorProvider, "BBTest");
        }
    }
}

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
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters)
        {
            generatorPluginEvents.RegisterDependencies += RegisterDependencies;
        }

        public void RegisterDependencies(object sender, RegisterDependenciesEventArgs eventArgs)
        {
            var projectSettings = eventArgs.ObjectContainer.Resolve<ProjectSettings>();

            var codeDomHelper = eventArgs.ObjectContainer.Resolve<CodeDomHelper>(projectSettings.ProjectPlatformSettings.Language);

            var generatorProvider = new BlueTestGeneratorProvider(codeDomHelper);

            eventArgs.ObjectContainer.RegisterInstanceAs<IUnitTestGeneratorProvider>(generatorProvider, "BBTest");
        }
    }
}

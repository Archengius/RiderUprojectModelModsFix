using System.Threading;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ReSharperPlugin.RiderUprojectModelModsFix.Tests
{
    [ZoneDefinition]
    public class RiderUprojectModelModsFixTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<IRiderUprojectModelModsFixZone> { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<RiderUprojectModelModsFixTestEnvironmentZone> { }

    [SetUpFixture]
    public class RiderUprojectModelModsFixTestsAssembly : ExtensionTestEnvironmentAssembly<RiderUprojectModelModsFixTestEnvironmentZone> { }
}

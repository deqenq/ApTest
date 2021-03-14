#tool nuget:?package=coveralls.io&version=1.4.2

#addin nuget:?package=Cake.Coveralls&version=1.0.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

const string solutionPath = "ApTest.sln";
const string projectPath = "ApTest/ApTest.csproj";
const string testProjectPath = "DummyTestProject/DummyTestProject.csproj";
const string packageOutputDirectory = "dist";
const string testReportDirectory = "TestsOutput";
const string coverageReportDirectory = "CoverageResults";
readonly string coverageReport = $"{coverageReportDirectory}/coverage.xml";

Task("Clean")
    .Does(() =>
{
    CleanDirectories("**/bin");
    CleanDirectories("**/obj");
    CleanDirectory(testReportDirectory);
    CleanDirectory(coverageReportDirectory);
});

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solutionPath);
});

Task("Compile")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        Framework = "netstandard2.0"
    };

    DotNetCoreBuild(projectPath, settings);
});

Task("Test")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        Framework = "netcoreapp2.1",
        Loggers = new List<string>() { "trx" },
        VSTestReportPath = $"{testReportDirectory}/report.trx"
    };

    settings.ArgumentCustomization = 
        args => args.Append("/p:CollectCoverage=true")
        .Append($"/p:CoverletOutput=../{coverageReport}")
        .Append("/p:CoverletOutputFormat=opencover")
        .Append("/p:Exclude=[xunit.*]*");

    DotNetCoreTest(testProjectPath, settings);
});

Task("Package")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        OutputDirectory = packageOutputDirectory,
        Configuration = configuration
    };
    
    DotNetCorePack(projectPath, settings);
});

Task("Coverage-Report")
    .IsDependentOn("Test")
    .WithCriteria(BuildSystem.IsRunningOnAppVeyor)
    .WithCriteria(() => FileExists(coverageReport))
    .Does(() =>
{
    var settings = new CoverallsIoSettings
    {
        RepoToken = EnvironmentVariable("CoverallsRepoToken")
    };

    CoverallsIo(coverageReport, settings);
});

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);

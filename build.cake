var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

const string solutionPath = "ApTest.sln";
const string projectPath = "ApTest/ApTest.csproj";
const string testProjectPath = "DummyTestProject/DummyTestProject.csproj";

Task("Clean")
    .Does(() =>
{
    CleanDirectories("**/bin");
    CleanDirectories("**/obj");
});

Task("Restore-Packages")
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
        Logger = "trx",
        VSTestReportPath = "TestsOutput/report.trx"
    };

    DotNetCoreTest(testProjectPath, settings);
});

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);

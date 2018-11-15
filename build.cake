#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.Bower"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var solution = "./SoftSupply-Help.sln";
var project = "./SoftSupply-Help/SoftSupply-Help.csproj";

var outputDirectory = "build/";
var buildDir = Directory("./" + outputDirectory);
var outDir = System.IO.Path.GetFullPath(buildDir);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Restore-Bower-Packages")
    .IsDependentOn("Clean")
    .Does(() => 
{
	// bower install using bower.json
	Bower.Install(s => s.WithVerbose().UseWorkingDirectory(outDir));
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var settings_web = new MSBuildSettings()
			.WithTarget("WebPublish")
			.WithProperty("PackageLocation", new string[]{ outDir })
			.WithProperty("WebPublishMethod", new string[]{ "FileSystem" })
			.WithProperty("PublishUrl", new string[]{ outDir });
		settings_web.SetConfiguration(configuration);

		MSBuild(project, settings_web);
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

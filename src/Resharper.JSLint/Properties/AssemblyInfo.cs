using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("JSLint for Resharper")]
[assembly: AssemblyDescription("Adds JSLint validation to resharper highlighting")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Lars-Erik Aabech")]
[assembly: AssemblyProduct("JSLint for Resharper")]
[assembly: AssemblyCopyright("Copyright © Lars-Erik Aabech, 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.2.*")]

[assembly: ActionsXml("JSLintForResharper.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("JSLint for Resharper")]
[assembly: PluginDescription("Adds JSLint validation to resharper highlighting")]
[assembly: PluginVendor("Lars-Erik Aabech")]

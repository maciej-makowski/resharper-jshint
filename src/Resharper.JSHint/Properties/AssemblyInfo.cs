using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("JSHint for Resharper")]
[assembly: AssemblyDescription("Adds JSHint validation to resharper highlighting")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Maciej Makowski")]
[assembly: AssemblyProduct("JSHint for Resharper")]
[assembly: AssemblyCopyright("Copyright © Maciej Makowski, 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.2.*")]

[assembly: ActionsXml("JSHintForResharper.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("JSHint for Resharper")]
[assembly: PluginDescription("Adds JSHint validation to resharper highlighting")]
[assembly: PluginVendor("Maciej Makowski")]

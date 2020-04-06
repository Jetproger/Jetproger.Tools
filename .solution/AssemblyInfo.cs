#pragma warning disable 1616
#pragma warning disable 0436
using System;
using System.Reflection;
using System.Runtime.InteropServices;
[assembly: AssemblyTitle("Jetproger.Tools")]
[assembly: AssemblyDescription("Jetproger.Tools")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Jetproger")]
[assembly: AssemblyProduct("Jetproger.Tools")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("Jetproger")]
[assembly: AssemblyCulture("")]
[assembly: Guid("600aa7dc-ea39-4609-ab2a-3546bb5f8b72")]
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyDate(41213)]

[ComVisible(true), AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AssemblyDateAttribute : Attribute
{
    private readonly DateTime _assemblyDate;

    public AssemblyDateAttribute(DateTime assemblyDate)
    {
        _assemblyDate = assemblyDate;
    }
     
    public AssemblyDateAttribute(double assemblyDate)
    {
        assemblyDate = Math.Floor(assemblyDate);
        _assemblyDate = DateTime.FromOADate(assemblyDate);
    }
     
    public DateTime AssemblyDate
    {
        get { return _assemblyDate; }
    }
}
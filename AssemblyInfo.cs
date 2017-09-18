using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Jetproger.Tools.Convert")]
[assembly: AssemblyDescription("Jetproger.Tools.Convert")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Jetproger")]
[assembly: AssemblyProduct("Jetproger.Tools.Convert")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[ComVisible(true)]
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
[assembly: Guid("600aa7dc-ea39-4609-ab2a-3546bb5f8b72")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyName("")]
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyDate(41213)]

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
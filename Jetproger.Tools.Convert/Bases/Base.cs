using System;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        public static AppDomain own => Je.one.Get(OwnHolder, () => AppDomain.CurrentDomain.IsDefaultAppDomain() ? AppDomain.CurrentDomain : null);
        private static readonly AppDomain[] OwnHolder = { null };

        public static IAppExpander app => null;
        public interface IAppExpander { }

        public static IAopExpander aop => null;
        public interface IAopExpander { }

        public static IBinExpander bin => null;
        public interface IBinExpander { }

        public static ICmdExpander cmd => null;
        public interface ICmdExpander { }

        public static CryExpander cry => Je.one.Get(CryExpanderHolder, () => new CryExpander());
        private static readonly CryExpander[] CryExpanderHolder = { null };

        public static IErrExpander err => null;
        public interface IErrExpander { }

        public static IExtExpander ext => null;
        public interface IExtExpander { }

        public static IFssExpander fss => null;
        public interface IFssExpander { }

        public static IGuiExpander gui => null;
        public interface IGuiExpander { }

        public static ILogExpander log => null;
        public interface ILogExpander { }

        public static IMemExpander mem => null;
        public interface IMemExpander { }

        public static IOneExpander one => null;
        public interface IOneExpander { }

        public static ISqlExpander sql => null;
        public interface ISqlExpander { }

        public static IStrExpander str => null;
        public interface IStrExpander { }

        public static ISysExpander sys => null;
        public interface ISysExpander { }

        public static WebExpander web => Je.one.Get(WebExpanderHolder, () => new WebExpander());
        private static readonly WebExpander[] WebExpanderHolder = { null };

        public static WinExpander win => Je.one.Get(WinExpanderHolder, () => new WinExpander());
        private static readonly WinExpander[] WinExpanderHolder = { null };

        public static IXmlExpander xml => null;
        public interface IXmlExpander { }
    }

    public static class Je<T> where T : class
    {

        private static readonly IpcClient _IpcClient = new IpcClient();
        private static readonly Type _Type = typeof(T);

        public static T New(params object[] args)
        {
            return (T)Activator.CreateInstance(_Type, args);
        }

        public static T Onu()
        {
            return One(() => New());
        }

        public static T One(Func<T> factory)
        {
            return (T)_IpcClient.OfToOne(_Type, factory);
        }

        public static T One()
        {
            return (T)_IpcClient.OfOne(_Type);
        }

        public static void One(T value)
        {
            _IpcClient.ToOne(_Type, value);
        }

        public static T Fnu()
        {
            return Few(() => New());
        }

        public static T Few(Func<T> factory)
        {
            return (T)_IpcClient.OfToPool(_Type, factory);
        }

        public static T Few()
        {
            return (T)_IpcClient.OfPool(_Type);
        }

        public static void Few(T value)
        {
            _IpcClient.ToPool(_Type, value);
        }

        public static T Knu<TKey>(TKey key)
        {
            return Key(key, x => New(x));
        }

        public static T Key<TKey>(TKey key, Func<TKey, T> factory)
        { 
            return (T)_IpcClient.OfToStore(typeof(TKey), _Type, key, new Func<object, object>(x => factory((TKey)x)));
        }

        public static T Key<TKey>(TKey key)
        {
            return (T)_IpcClient.OfStore(typeof(TKey), _Type, key);
        }

        public static void Key<TKey>(TKey key, T value)
        {
            _IpcClient.ToStore(typeof(TKey), _Type, key, value);
        }
    }

    public static class J_<T> where T : Setting
    {

        private static readonly Type _Type = typeof(T);

        public static bool Is
        {
            get { return Je<T>.Knu(_Type).IsDeclared; }
        }

        public static string Sz
        {
            get { return Je<T>.Knu(_Type).Value; }
        }
    }
}
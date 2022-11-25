using System;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class f
    {
        public static AppDomain own => f.one.Get(OwnHolder, () => AppDomain.CurrentDomain.IsDefaultAppDomain() ? AppDomain.CurrentDomain : null);
        private static readonly AppDomain[] OwnHolder = { null };

        public static IAppExpander app => null;
        public interface IAppExpander { }

        public static IAopExpander aop => null;
        public interface IAopExpander { }

        public static IBinExpander bin => null;
        public interface IBinExpander { }

        public static ICmdExpander cmd => null;
        public interface ICmdExpander { }

        public static CryExpander cry => f.one.Get(CryExpanderHolder, () => new CryExpander());
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

        public static WebExpander web => f.one.Get(WebExpanderHolder, () => new WebExpander());
        private static readonly WebExpander[] WebExpanderHolder = { null };

        public static WinExpander win => f.one.Get(WinExpanderHolder, () => new WinExpander());
        private static readonly WinExpander[] WinExpanderHolder = { null };

        public static IXmlExpander xml => null;
        public interface IXmlExpander { }
    }

    public static class t<T> where T : class
    {
        private static readonly IpcClient _IpcClient = new IpcClient();
        private static readonly Type _Type = typeof(T);

        public static T _(params object[] args)
        {
            return (T)Activator.CreateInstance(_Type, args);
        }

        public static T one()
        {
            return one(() => _());
        }

        public static T one(Func<T> factory)
        {
            return (T)_IpcClient.OfToOne(_Type, factory);
        }

        public static bool one(out T value)
        {
            value = (T)_IpcClient.OfOne(_Type);
            return value != null;
        }

        public static void one(T value)
        {
            _IpcClient.ToOne(_Type, value);
        }

        public static T few()
        {
            return few(() => _());
        }

        public static T few(Func<T> factory)
        {
            return (T)_IpcClient.OfToPool(_Type, factory);
        }

        public static bool few(out T value)
        {
            value = (T)_IpcClient.OfPool(_Type);
            return value != null;
        }

        public static void few(T value)
        {
            _IpcClient.ToPool(_Type, value);
        }

        public static T key<TKey>(TKey k)
        {
            return key(k, x => _(x));
        }

        public static T key<TKey>(TKey key, Func<TKey, T> factory)
        { 
            return (T)_IpcClient.OfToStore(typeof(TKey), _Type, key, new Func<object, object>(x => factory((TKey)x)));
        }

        public static bool key<TKey>(TKey key, out T value)
        {
            value = (T)_IpcClient.OfStore(typeof(TKey), _Type, key);
            return value != null;
        }

        public static void key<TKey>(TKey key, T value)
        {
            _IpcClient.ToStore(typeof(TKey), _Type, key, value);
        }
    }

    public static class k<T> where T : Setting
    {
        private static readonly Type _Type = typeof(T);

        public static bool set
        {
            get { return t<T>.key(_Type).IsDeclared; }
        }
        public static string key
        {
            get { return t<T>.key(_Type).Value; }
        }
    }
}
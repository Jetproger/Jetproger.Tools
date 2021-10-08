using System;
using System.Reflection;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Jetproger.Tools.Inject.Bases
{
    public static class UnityExtensions
    {
        private static IUnityContainer Container => Je.One.Get(ContainerHolder, RegisterContainer);
        private static readonly IUnityContainer[] ContainerHolder = { null };
        private static void Register(IUnityContainer container) { }

        public static void Register(this Je.IInjectExpander expander)
        {
            Register(Container);
        }

        private static IUnityContainer RegisterContainer()
        {
            var section = new ExUnitySection();
            section.DeserializeSection(Jc.Rpc<UnityRemote>.Ge.GetConfig());
            var container = new UnityContainer();
            section.Configure(container);
            return container;
        }

        public static Type TypeOf<T>(this Je.IInjectExpander expander)
        {
            return Container.Resolve<T>().GetType();
        }

        public static Type TypeOf(this Je.IInjectExpander expander, Type type)
        {
            return Container.Resolve(type).GetType();
        }

        public static T Resolve<T>(this Je.IInjectExpander expander, Action<T> initialize)
        {
            var instance = Resolve<T>(expander);
            initialize(instance);
            return instance;
        }

        public static T Resolve<T>(this Je.IInjectExpander expander)
        {
            return Container.Resolve<T>();
        }

        public static object Resolve(this Je.IInjectExpander expander, Type type, Action<object> initialize)
        {
            var instance = Resolve(expander, type);
            initialize(instance);
            return instance;
        }

        public static object Resolve(this Je.IInjectExpander expander, Type type)
        {
            return Container.Resolve(type);
        }

        public static IMethodReturn Call(this Je.IInjectExpander expander, IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var next = getNext();
            if (next == null) return input.CreateMethodReturn(null);
            var result = next.Invoke(input, getNext);
            if (result == null) return input.CreateMethodReturn(null);
            if (result.Exception == null) return result;
            throw result.Exception;
        }

        public static string Info(this Je.IInjectExpander expander, IMethodInvocation input, string typeName = null)
        {
            var method = (MethodInfo)input.MethodBase;
            var parameters = method.GetParameters();
            var sb = new StringBuilder();
            typeName = string.IsNullOrWhiteSpace(typeName) ? method.ReflectedType?.FullName : typeName;
            sb.AppendFormat("{0} {1}.{2}(", method.ReturnType, typeName, method.Name);
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("{0}{1} {2}", (i > 0 ? ", " : ""), parameters[i].ParameterType, parameters[i].Name);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static string InfoEx(this Je.IInjectExpander expander, IMethodInvocation input)
        {
            var method = (MethodInfo)input.MethodBase;
            var parameters = method.GetParameters();
            var arguments = input.Arguments;
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1}.{2}(", method.ReturnType, method.ReflectedType?.FullName, method.Name);
            for (int i = 0; i < parameters.Length; i++)
            {
                var value = i < arguments.Count ? (arguments[i] != null ? arguments[i].ToString() : "<null>") : "<>";
                sb.AppendFormat("{0}{1} {2}: \"{3}\"", (i > 0 ? ", " : ""), parameters[i].ParameterType, parameters[i].Name, value);
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
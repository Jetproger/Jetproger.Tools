using System;
using System.Linq.Expressions;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Plugin.Commands
{
    public class Mapper<TTarget> : ICommandMapper
    {
        public string PropertyName { get; private set; }
        public Type SourceType { get; private set; }
        private readonly Type _propertyType;
        private readonly Delegate _setter;
        private readonly object _value;

        public Mapper(string targetName, object value)
        {
            _value = value;
            SourceType = null;
            string targetAssemblyName, targetTypeName, targetPropertyName;
            Ex.Dotnet.ParseName(targetName, out targetAssemblyName, out targetTypeName, out targetPropertyName);
            PropertyName = targetPropertyName;
            var targetType = Ex.Dotnet.GetType(targetAssemblyName, targetTypeName);
            if (targetType == null) return;
            var targetProperty = Ex.Dotnet.GetProperty(targetAssemblyName, targetTypeName, targetPropertyName);
            if (targetProperty == null) return;
            _propertyType = targetProperty.PropertyType;
            var targetExp = Expression.Parameter(targetType, "target");
            var valueExp = Expression.Parameter(targetProperty.PropertyType, "value");
            var targetPropertyExp = Expression.Property(targetExp, targetProperty);
            var assignExp = Expression.Assign(targetPropertyExp, valueExp);
            _setter = Expression.Lambda(assignExp, targetExp, valueExp).Compile();
        }

        public Mapper(Expression<Func<TTarget, object>> target, object value)
        {
            _value = value;
            SourceType = null;
            var targetType = typeof(TTarget);
            PropertyName = Ex.Dotnet.GetMemberName(target);
            var property = Ex.Dotnet.GetProperty(targetType, PropertyName);
            _propertyType = property.PropertyType;
            var targetExp = Expression.Parameter(targetType, "target");
            var valueExp = Expression.Parameter(property.PropertyType, "value");
            var propertyExp = Expression.Property(targetExp, property);
            var assignExp = Expression.Assign(propertyExp, valueExp);
            _setter = Expression.Lambda(assignExp, targetExp, valueExp).Compile();
        }

        void ICommandMapper.Map(object target, object source)
        {
            Map(target.As<TTarget>());
        }

        public void Map(TTarget target)
        {
            if (target != null) _setter?.DynamicInvoke(target, _value.As(_propertyType));
        }
    }

    public class Mapper<TTarget, TSource> : ICommandMapper
    {
        public string PropertyName { get; private set; }
        public Type SourceType { get; private set; }
        private readonly Type _propertyType;
        private readonly Delegate _getter;
        private readonly Delegate _setter;

        public Mapper(string targetName, string sourceName)
        {
            SourceType = typeof(TSource);
            string sourceAssemblyName, sourceTypeName, sourcePropertyName;
            Ex.Dotnet.ParseName(sourceName, out sourceAssemblyName, out sourceTypeName, out sourcePropertyName);
            var sourceType = Ex.Dotnet.GetType(sourceAssemblyName, sourceTypeName);
            if (sourceType == null) return;
            var sourceProperty = Ex.Dotnet.GetProperty(sourceAssemblyName, sourceTypeName, sourcePropertyName);
            if (sourceProperty == null) return;
            var sourceExp = Expression.Parameter(sourceType, "target");
            var sourcePropertyExp = Expression.Property(sourceExp, sourceProperty);
            _getter = Expression.Lambda(sourcePropertyExp, sourceExp).Compile();
            string targetAssemblyName, targetTypeName, targetPropertyName;
            Ex.Dotnet.ParseName(targetName, out targetAssemblyName, out targetTypeName, out targetPropertyName);
            PropertyName = targetPropertyName;
            var targetType = Ex.Dotnet.GetType(targetAssemblyName, targetTypeName);
            if (targetType == null) return;
            var targetProperty = Ex.Dotnet.GetProperty(targetAssemblyName, targetTypeName, targetPropertyName);
            if (targetProperty == null) return;
            _propertyType = targetProperty.PropertyType;
            var targetExp = Expression.Parameter(targetType, "target");
            var valueExp = Expression.Parameter(targetProperty.PropertyType, "value");
            var targetPropertyExp = Expression.Property(targetExp, targetProperty);
            var assignExp = Expression.Assign(targetPropertyExp, valueExp);
            _setter = Expression.Lambda(assignExp, targetExp, valueExp).Compile();
        }

        public Mapper(Expression<Func<TTarget, object>> target, Expression<Func<TSource, object>> source)
        {
            _getter = source.Compile();
            SourceType = typeof(TSource);
            var targetType = typeof(TTarget);
            PropertyName = Ex.Dotnet.GetMemberName(target);
            var property = Ex.Dotnet.GetProperty(targetType, PropertyName);
            _propertyType = property.PropertyType;
            var targetExp = Expression.Parameter(targetType, "target");
            var valueExp = Expression.Parameter(property.PropertyType, "value");
            var propertyExp = Expression.Property(targetExp, property);
            var assignExp = Expression.Assign(propertyExp, valueExp);
            _setter = Expression.Lambda(assignExp, targetExp, valueExp).Compile();
        }

        void ICommandMapper.Map(object target, object source)
        {
            Map(target.As<TTarget>(), source.As<TSource>());
        }

        public void Map(TTarget target, TSource source)
        {
            if (_getter == null || _setter == null || target == null || source == null) return;
            var sourceValue = _getter.DynamicInvoke(source).As(_propertyType);
            _setter.DynamicInvoke(target, sourceValue);
        }
    }

    public interface ICommandMapper
    {
        Type SourceType { get; }
        string PropertyName { get; }
        void Map(object target, object source);
    }
}
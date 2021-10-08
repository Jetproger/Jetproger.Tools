using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Convert.Works
{
    public class DataEntity : IDataEntity
    { 
        string IDataEntity.GetKey() { return GetKey(); }
        protected virtual string GetKey() { return null; }

        IEnumerable<string> IDataEntity.GetKeys() { return GetKeys(); }
        protected virtual IEnumerable<string> GetKeys() { return new string[0]; }

        IEnumerable<IDataEntity> IDataEntity.GetAll() { return GetAll(); }
        protected virtual IEnumerable<IDataEntity> GetAll() { yield break; }

        void IDataEntity.SetEntity(IDataEntity item) { SetEntity(item); }
        protected virtual void SetEntity(IDataEntity item) { }

        void IDataEntity.AddEntity(IDataEntity entity) { AddEntity(entity); }
        protected virtual void AddEntity(IDataEntity entity) { }

        public static readonly DataEntityDependencies Dependencies = new DataEntityDependencies();

        public class DataEntityDependencies
        {
            private readonly Dictionary<Type, Type[]> _dependencies = new Dictionary<Type, Type[]>();

            public Type[] this[Type type]
            {
                get
                {
                    if (!_dependencies.ContainsKey(type))
                    {
                        lock (_dependencies)
                        {
                            if (!_dependencies.ContainsKey(type)) _dependencies.Add(type, GetDependencies(type));
                        }
                    }
                    return _dependencies[type];
                }
            }

            private Type[] GetDependencies(Type type)
            {
                var list = new List<Type> { type };
                foreach (var attribute in type.GetCustomAttributes(true))
                {
                    var typeDependenciesAttrubute = attribute as TypeDependenciesAttrubute;
                    if (typeDependenciesAttrubute == null) continue;
                    list.AddRange(typeDependenciesAttrubute.Dependencies);
                    break;
                }
                return list.ToArray();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TypeDependenciesAttrubute : Attribute
    {
        public TypeDependenciesAttrubute(params Type[] types) { Dependencies = (new List<Type>(types)).ToArray(); } 
        public Type[] Dependencies { get; private set; }
    }
    public class DateTimeTypeAttribute : Attribute
    {
        public DateTimeKind Kind { get; private set; }
        public DateTimeTypeAttribute() : this(DateTimeKind.Utc) { }
        public DateTimeTypeAttribute(DateTimeKind kind) { Kind = kind; }
    }

    public interface IDataEntity
    {
        string GetKey();
        IEnumerable<string> GetKeys();
        IEnumerable<IDataEntity> GetAll();
        void SetEntity(IDataEntity item);
        void AddEntity(IDataEntity entity);
    }
}
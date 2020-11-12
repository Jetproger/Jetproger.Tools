using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Convert.Bases
{
    public class Entity : IEntity
    {
        public static readonly DataEntityDependencies Dependencies = new DataEntityDependencies();
        public static Entity Empty => Je<Entity>.One(() => new Entity());

        string IEntity.GetId()
        {
            return GetId();
        }

        protected virtual string GetId()
        {
            return string.Empty;
        }

        IEnumerable<string> IEntity.GetKeys()
        {
            return GetKeys();
        }

        protected virtual IEnumerable<string> GetKeys()
        {
            return new string[0];
        }

        IEnumerable<IEntity> IEntity.GetAll()
        {
            return GetAll();
        }

        protected virtual IEnumerable<IEntity> GetAll()
        {
            yield break;
        }

        void IEntity.SetItem(IEntity item)
        {
            SetItem(item);
        }

        protected virtual void SetItem(IEntity item)
        {
        }

        void IEntity.AddItem(IEntity entity)
        {
            AddItem(entity);
        }

        protected virtual void AddItem(IEntity entity)
        {
        }

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

    public interface IEntity
    {
        string GetId();
        IEnumerable<string> GetKeys();
        IEnumerable<IEntity> GetAll();
        void SetItem(IEntity entity);
        void AddItem(IEntity entity);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TypeDependenciesAttrubute : Attribute
    {
        public TypeDependenciesAttrubute(params Type[] types)
        {
            Dependencies = (new List<Type>(types)).ToArray();
        }

        public Type[] Dependencies
        {
            get; private set;
        }
    }
}
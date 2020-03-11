using System;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public abstract class Setting<T> : Setting
    {
        public abstract object GetValue(T defaultValue);

        protected Setting()
        {
            Code = GetType().Name;
        }

        [DataMember]
        public string Code { get; protected set; }

        [DataMember]
        public T Name { get; protected set; }

        [DataMember]
        public bool Is { get; protected set; }

        public override object GetValue()
        {
            return Name;
        }

        public override bool IsDeclared()
        {
            return Is;
        }
    }

    [Serializable]
    [DataContract]
    public abstract class Setting
    {
        public abstract object GetValue();
        public abstract bool IsDeclared();
    }
}
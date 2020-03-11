using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jc
{
    public static class Json<T> where T : BaseJson, new()
    {
        public static string Of(object value)
        {
            return One<T>.Ge.Of(value);
        }

        public static TOut To<TOut>(string json)
        {
            return One<T>.Ge.To<TOut>(json);
        }

        public static object To(string json, Type type)
        {
            return One<T>.Ge.To(json, type);
        }
    }

    public class BaseJson
    {
        private static readonly JavaScriptSerializer[] JsonDeserializerHolder = { null };

        private static readonly JavaScriptSerializer[] JsonSerializerHolder = { null };

        public virtual string Of(object o)
        {
            return GetJsonSerializer().Serialize(o);
        }

        public virtual T To<T>(string json)
        {
            return GetJsonDeserializer().Deserialize<T>(json);
        }

        public virtual object To(string json, Type type)
        {
            return GetJsonDeserializer().Deserialize(json, type);
        }

        private static JavaScriptSerializer GetJsonDeserializer()
        {
            return Je.One.Get(JsonDeserializerHolder, () => new JavaScriptSerializer());
        }

        private static JavaScriptSerializer GetJsonSerializer()
        {
            return Je.One.Get(JsonSerializerHolder, CreateJsonSerializer);
        }

        private static JavaScriptSerializer CreateJsonSerializer()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new NullPropertiesConverter() });
            return serializer;
        }

        private class NullPropertiesConverter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                var jsonExample = new Dictionary<string, object>();
                foreach (var p in obj.GetType().GetProperties())
                {
                    bool ignoreProp = p.IsDefined(typeof(ScriptIgnoreAttribute), true);
                    var value = p.GetValue(obj, BindingFlags.Public, null, null, null);
                    if (value != null && !ignoreProp) jsonExample.Add(p.Name, value);
                }
                return jsonExample;
            }

            public override IEnumerable<Type> SupportedTypes
            {
                get { return GetType().Assembly.GetTypes(); }
            }
        }
    }
}
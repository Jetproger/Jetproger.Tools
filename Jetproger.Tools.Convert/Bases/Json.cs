using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Jetproger.Tools.Convert.Bases
{
    public static class JsonExtensions
    {
        private static JavaScriptSerializer JsonDeserializer { get { return Ex.GetOne(JsonDeserializerHolder, () => new JavaScriptSerializer()); } }
        private static readonly JavaScriptSerializer[] JsonDeserializerHolder = { null };

        private static JavaScriptSerializer JsonSerializer { get { return Ex.GetOne(JsonSerializerHolder, CreateJavaScriptSerializer); } }
        private static readonly JavaScriptSerializer[] JsonSerializerHolder = { null };

        public static string Write(this IJsonExpander expander, object o)
        {
            var result = JsonSerializer.Serialize(o);
            return result;
        }

        public static T Read<T>(this IJsonExpander expander, string json)
        {
            return JsonDeserializer.Deserialize<T>(json);
        }

        public static object Read(this IJsonExpander expander, string json, Type type)
        {
            return JsonDeserializer.Deserialize(json, type);
        }

        private static JavaScriptSerializer CreateJavaScriptSerializer()
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
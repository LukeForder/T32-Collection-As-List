using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Behaviors
{
    public class CollectionIsListMessageFormatter : IClientMessageFormatter
    {
        private readonly IClientMessageFormatter _defaultFormatter;

        public CollectionIsListMessageFormatter(IClientMessageFormatter defaultFormatter)
        {
            _defaultFormatter = defaultFormatter;
        }

        public object DeserializeReply(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            object reply = _defaultFormatter.DeserializeReply(message, parameters);
            
            TransformReply(reply);

            return reply;
        }

        private void TransformCollection(object reply)
        {
            IEnumerable collection = reply as IEnumerable;
            IEnumerator enumerator = collection.GetEnumerator();
            if (enumerator == null)
                return;

            while(enumerator.MoveNext())
            {
                TransformReply(enumerator.Current);
            }
        }

        private void TransformReply(object instance)
        {
            if (instance is IEnumerable)
            {
                TransformCollection(instance);
            }
            else
            {

                List<PropertyInfo> properties =
                    instance
                        .GetType()
                        .GetProperties()
                        .Where(property => property.CanRead && property.CanWrite)
                        .Where(property => IsGenericCollection(property))
                        .ToList();

                // replace the underlying collection with a list-based implementation
                foreach (PropertyInfo property in properties)
                {
                    Type[] collectionType = property.PropertyType.GenericTypeArguments;

                    // invoke; new List<T>(IEnumerable<T>) 
                    object list = Activator.CreateInstance(typeof(List<>).MakeGenericType(collectionType), property.GetValue(instance));

                    // set the property value to the list based implementation
                    property.SetValue(instance, list);
                }
            }
        }

        private bool IsGenericCollection(PropertyInfo property)
        {
            Type propertyType = property.PropertyType;
            if (!propertyType.IsGenericType)
                return false;

            Type[] genericArgs = propertyType.GetGenericArguments();
            if (genericArgs.Length != 1)
                return false;

            return 
                typeof(ICollection<>)
                    .MakeGenericType(genericArgs)
                    .IsAssignableFrom(propertyType);
        }

        public System.ServiceModel.Channels.Message SerializeRequest(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters)
        {
            return _defaultFormatter.SerializeRequest(messageVersion, parameters);
        }
    }
}

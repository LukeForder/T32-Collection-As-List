using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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

            List<object> flaggedObjects = new List<object>();

            TransformReply(reply, flaggedObjects);

            return reply;
        }

        private void TransformCollection(object reply, List<object> flaggedObjects)
        {
            Contract.Requires(reply != null);
            Contract.Requires(flaggedObjects != null);
            
            IEnumerable collection = reply as IEnumerable;
            IEnumerator enumerator = collection.GetEnumerator();
            if (enumerator == null)
                return;

            while(enumerator.MoveNext() && !flaggedObjects.Contains(enumerator.Current))
            {
                flaggedObjects.Add(enumerator.Current);
                TransformReply(enumerator.Current, flaggedObjects);
            }
        }

        private void TransformReply(object instance, List<object> flaggedObjects)
        {
            if (instance is IEnumerable)
            {
                TransformCollection(instance, flaggedObjects);
            }
            else
            {
                Type instanceType = instance.GetType();

                var dataMembers =
                    instanceType
                        .GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    // this restricts us to members decorated with the DataMember attribute (fields, properties etc)
                        .Where(member => member.CustomAttributes.Any(a => a.AttributeType == typeof(DataMemberAttribute)))
                        .Select(member => new ClassDataMember((dynamic)member))
                        .Where(member => IsGenericCollection(member))
                        .ToList();

                // replace the underlying collection with a list-based implementation
                foreach (ClassDataMember member in dataMembers)
                {
                    Type[] collectionType = member.MemberType.GenericTypeArguments;

                    // invoke; new List<T>(IEnumerable<T>) 
                    object list = Activator.CreateInstance(
                        typeof(List<>).MakeGenericType(collectionType),
                        member.GetValue(instance));

                    // set the property value to the list based implementation
                    member.SetValue(instance, list);

                    TransformCollection(list, flaggedObjects);
                }
            }
        }
        
        private bool IsGenericCollection(ClassDataMember member)
        {
            Type memberType = member.MemberType;

            if (!memberType.IsGenericType)
                return false;

            Type[] genericArgs = memberType.GetGenericArguments();
            
            if (genericArgs.Length != 1)
                return false;

            return 
                typeof(ICollection<>)
                    .MakeGenericType(genericArgs)
                    .IsAssignableFrom(memberType);
        }

        public System.ServiceModel.Channels.Message SerializeRequest(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters)
        {
            return 
                _defaultFormatter
                    .SerializeRequest(messageVersion, parameters);
        }
    }
}

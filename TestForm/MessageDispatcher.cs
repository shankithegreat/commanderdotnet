using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TestForm
{
    public class MessageDispatcher
    {
        private static int countOfDispatchers;
        private Dictionary<Type, List<MessageCaller>> events = new Dictionary<Type, List<MessageCaller>>();


        public MessageDispatcher()
        {
            this.Number = countOfDispatchers++;
        }


        public readonly static MessageDispatcher Dispatcher = new MessageDispatcher();

        public int Number { get; private set; }


        public void Subscribe(object obj)
        {
            Type type = obj.GetType();

            foreach (MethodInfo member in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = member.GetCustomAttributes(typeof(MessageAttribute), true);
                if (attributes.Length == 0)
                {
                    continue;
                }

                foreach (object attribute in attributes)
                {
                    var types = member.GetParameters();

                    if (types.Length != 1)
                    {
                        throw new ArgumentException();
                    }

                    if (types[0].ParameterType != ((MessageAttribute)attribute).ArgumentType)
                    {
                        throw new ArgumentException();
                    }

                    List<MessageCaller> callList = GetCallList(attribute.GetType());

                    callList.Add(new MessageCaller(obj, member));
                }
            }
        }

        public void Invoke(MessageAttribute attribute)
        {
            Invoke(attribute, null);
        }

        public void Invoke(MessageAttribute attribute, MessageArgs param)
        {
            // Check values;
            if (param == null)
            {
                param = new MessageArgs();
            }

            if (attribute.ArgumentType != param.GetType())
            {
                throw new ArgumentException("Invalid parameter argument type");
            }

            List<MessageCaller> callList = GetCallList(attribute.GetType());

            callList.RemoveAll(val => !val.IsAlive);

            bool allAlive = true;
            foreach (var item in callList)
            {
                allAlive &= item.Call(new object[] { param });
            }

            if (!allAlive)
            {
                callList.RemoveAll(val => !val.IsAlive);
            }
        }

        public override string ToString()
        {
            return "Number of dispatcher = " + this.Number;
        }


        private List<MessageCaller> GetCallList(Type type)
        {
            if (!events.ContainsKey(type))
            {
                events.Add(type, new List<MessageCaller>());
            }

            return events[type];
        }


        private class MessageCaller
        {
            private MethodInfo method;
            private WeakReference reference;


            public MessageCaller(object target, MethodInfo method)
            {
                this.reference = new WeakReference(target);
                this.method = method;
            }


            public bool IsAlive { get { return reference.IsAlive; } }


            public bool Call(object[] parameters)
            {
                try
                {
                    object target = reference.Target;

                    method.Invoke(target, parameters);

                    return true;
                }
                catch (InvalidOperationException)
                {
                    // object is already dead and we need to remove it from the query
                    return false;
                }
                catch (TargetInvocationException)
                {
                    return false;
                }
            }
        }
    }

    public class MessageAttribute : Attribute
    {
        public Type ArgumentType = typeof(MessageArgs);
    }

    public class MessageArgs
    {
    }
}

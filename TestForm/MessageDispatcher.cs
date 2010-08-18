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
        private Dictionary<Type, List<MessageInfo>> events = new Dictionary<Type, List<MessageInfo>>();
        public readonly static MessageDispatcher Dispatcher = new MessageDispatcher();


        public void Subscribe(object obj)
        {
            Type type = obj.GetType();

            foreach (MethodInfo member in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = member.GetCustomAttributes(typeof(MessageAttribute), true);

                foreach (MessageAttribute attribute in attributes)
                {
                    ParameterInfo[] parameters = member.GetParameters();

                    if (parameters.Length != 1)
                    {
                        throw new ArgumentException();
                    }
                    if (parameters[0].ParameterType != attribute.ArgumentType)
                    {
                        throw new ArgumentException();
                    }

                    List<MessageInfo> callList = GetEventList(attribute.GetType());

                    callList.Add(new MessageInfo(obj, member));
                }
            }
        }

        public void Invoke(MessageAttribute attribute)
        {
            Invoke(attribute, new MessageArgs());
        }

        public void Invoke(MessageAttribute attribute, MessageArgs param)
        {
            if (param == null)
            {
                throw new ArgumentNullException();
            }
            if (attribute.ArgumentType != param.GetType())
            {
                throw new ArgumentException("Invalid parameter argument type");
            }

            List<MessageInfo> callList = GetEventList(attribute.GetType());

            // Clean
            callList.RemoveAll(val => !val.IsAlive);

            bool allAlive = true;
            foreach (MessageInfo item in callList)
            {
                allAlive &= item.Call(param);
            }

            // Clean
            if (!allAlive)
            {
                callList.RemoveAll(val => !val.IsAlive);
            }
        }


        private List<MessageInfo> GetEventList(Type type)
        {
            if (!events.ContainsKey(type))
            {
                events.Add(type, new List<MessageInfo>());
            }

            return events[type];
        }


        private class MessageInfo
        {
            private MethodInfo method;
            private WeakReference reference;


            public MessageInfo(object target, MethodInfo method)
            {
                this.reference = new WeakReference(target);
                this.method = method;
            }


            public bool IsAlive { get { return reference.IsAlive; } }


            public bool Call(object param)
            {
                try
                {
                    object target = reference.Target;

                    method.Invoke(target, new[] { param });

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

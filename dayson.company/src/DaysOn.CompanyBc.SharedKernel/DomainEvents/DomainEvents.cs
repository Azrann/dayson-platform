using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.CompanyBc.SharedKernel.Ioc;
using System.Threading.Tasks;

namespace DaysOn.CompanyBc.SharedKernel.DomainEvents
{
    /// <summary>
    /// Controller class that is used for domain to raise the events.
    /// </summary>
    public static class DomainEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static List<Delegate> _actions;

        public static List<Delegate> Actions
        {
            get
            {
                return _actions ?? (_actions = new List<Delegate>());
            }
            set
            {
                _actions = value;
            }
        }

        //Registers a callback for the given domain event
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            Actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            Actions = null;
        }

        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (ContainerManager.Container != null)
            {
                foreach (var handler in ContainerManager.Container.ResolveAll<IAsyncEventHandler<T>>())
                {
                    handler.Handle(args).Wait();
                }
                foreach (var handler in ContainerManager.Container.ResolveAll<IEventHandler<T>>())
                {
                    handler.Handle(args);
                }
            }

            foreach (var action in Actions.OfType<Action<T>>())
            {
                action(args);
            }
        }
    }
}
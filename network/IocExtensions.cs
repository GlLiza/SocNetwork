using Microsoft.Practices.Unity;

namespace network
{
    public static class IocExtensions
    {
        internal static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        internal static void BindInRequestScope<T>(this IUnityContainer container)
        {
            container.RegisterType<T>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }

        internal static void BindInSingletonScope<T>(UnityContainer container)
        {
            container.RegisterType<T>(new ContainerControlledLifetimeManager());
        }
    }
}
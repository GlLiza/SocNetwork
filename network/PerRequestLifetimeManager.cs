using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace network
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly string _key = string.Format("SingletonPerRequest{0}", Guid.NewGuid());

        public override object GetValue()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items.Contains(_key))
                return HttpContext.Current.Items[_key];
            return null;
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items.Contains(_key))
                HttpContext.Current.Items.Remove(_key);
        }

        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[_key] = newValue;
        }
    }
}
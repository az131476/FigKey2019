using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Provider;
using System.Web;

namespace OAuthWcfRestService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OAuthService
    {
        [WebGet(UriTemplate = "Contacts")]
        public List<Contact> Contacts()
        {
            var name = Thread.CurrentPrincipal.Identity.Name;
            return DataModel.Contacts.Where(c => c.Owner == name).ToList();
        }
    }


}

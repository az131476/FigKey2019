// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using System.ServiceModel.Description;
using ServiceModelEx.ServiceBus;

namespace ServiceModelEx
{
   static class Program
   {
      static void Main()
      {
         string issuer = "owner";
         string secret = "*********  Enter your secret here  **********";

         TransportClientEndpointBehavior credential = new TransportClientEndpointBehavior();
         credential.CredentialType = TransportClientCredentialType.SharedSecret;
         credential.Credentials.SharedSecret.IssuerName = issuer;
         credential.Credentials.SharedSecret.IssuerSecret = secret;

         ServiceRegistrySettings registeryBehavior = new ServiceRegistrySettings(DiscoveryType.Public);
         ////////////////////////////////////////////////////////////////////////////
         Console.WriteLine("Creating simple services...");

         ServiceHost host1 = new ServiceHost(typeof(MyService));
         host1.AddServiceEndpoint(typeof(IMyContract),new NetTcpRelayBinding(),@"sb://MyNamespace.servicebus.windows.net/MyService1");
         host1.Description.Endpoints[0].Behaviors.Add(registeryBehavior);
         host1.Description.Endpoints[0].Behaviors.Add(credential);
         host1.Open();

         ServiceHost host2 = new ServiceHost(typeof(MyService));
         host2.AddServiceEndpoint(typeof(IMyContract),new NetTcpRelayBinding(),@"sb://MyNamespace.servicebus.windows.net/Top/MyService2");
         host2.AddServiceEndpoint(typeof(IMyContract),new WS2007HttpRelayBinding(),@"https://MyNamespace.servicebus.windows.net/Top/Sub/MyService3");
         host2.Description.Endpoints[0].Behaviors.Add(registeryBehavior);
         host2.Description.Endpoints[0].Behaviors.Add(credential);
         host2.Description.Endpoints[1].Behaviors.Add(registeryBehavior);
         host2.Description.Endpoints[1].Behaviors.Add(credential);
         host2.Open();

         ////////////////////////////////////////////////////////////////////////////

         Console.WriteLine("Creating a buffer...");


         string bufferAddress = "https://MyNamespace.servicebus.windows.net/MyBuffer/";
         ServiceBusHelper.CreateBuffer(bufferAddress,secret);
    
         ////////////////////////////////////////////////////////////////////////////

         Console.WriteLine();
         Console.WriteLine();

         Console.WriteLine("Press any key to close services and junctions");
         Console.ReadLine();

         host1.Close();
         host2.Close();

         ServiceBusHelper.DeleteBuffer(bufferAddress,secret);
      }
   }
}

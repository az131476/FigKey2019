using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OAuthWcfRestService
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Owner { get; set; }
    }

    public class DataModel
    {
        public static List<Contact> Contacts;

        static DataModel()
        {
            Contacts = new List<Contact> {
              new Contact(){ Id=0, Name="Felix", Email="Felix@test.com", Owner = "jane" },
              new Contact(){ Id=1, Name="Wendy", Email="Wendy@test.com", Owner = "jane"},
              new Contact(){ Id=2, Name="John", Email="John@test.com", Owner = "john"},
              new Contact(){ Id=3, Name="Philip", Email="Philip@mail.com", Owner = "john"}
            };
        }
    }
}

﻿// The MIT License
//
// Copyright (c) 2006-2008 DevDefined Limited.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq.Expressions;

namespace ADOServices.OAuth
{
	[DataServiceKey("Id")]
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
		public string Owner { get; set; }
    }

	public class ContactsData
    {
        static Contact[] _contacts;
        
		static ContactsData()
        {
			_contacts = new Contact[]{
              new Contact(){ Id=0, Name="Mike", Email="mike@contoso.com", Owner = "jane" },
              new Contact(){ Id=1, Name="Saaid", Email="Saaid@hotmail.com", Owner = "jane"},
              new Contact(){ Id=2, Name="John", Email="j123@live.com", Owner = "john"},
              new Contact(){ Id=3, Name="Pablo", Email="Pablo@mail.com", Owner = "john"}};
        }

        public IQueryable<Contact> Contacts
        {
			get { return _contacts.AsQueryable<Contact>(); }
        }
	
	}
}

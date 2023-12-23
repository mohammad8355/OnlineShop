using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;
namespace BusinessLogicLayer.ContactService
{
    public class ContactsLogic
    {
        private readonly MainRepository<Contact> ContactRepository;
        public ContactsLogic(MainRepository<Contact> ContactRepository)
        {
            this.ContactRepository = ContactRepository;
        }
    }
}

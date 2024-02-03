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
        public async Task<bool> AddContact(Contact model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Content))
            {
                return false;
            }
            else
            {
                await ContactRepository.AddItem(model);
                return true;
            }
        }
        public bool EditContact(Contact model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Content))
            {
                return false;
            }
            else
            {
                ContactRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteContact(int Id)
        {

            if (await ContactRepository.DeleteItem(Id))
            {
                return true;
            }
            return false;
        }
        public Contact ContactDetail(int Id)
        {
            return ContactRepository.Get(c => c.Id == Id).Result.FirstOrDefault();

        }
        public List<Contact> ContactList()
        {
                return ContactRepository.Get().Result.ToList();
        }
    }
}

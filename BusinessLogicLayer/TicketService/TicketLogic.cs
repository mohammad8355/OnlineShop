using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.TicketService
{
    public class TicketLogic
    {
        private readonly MainRepository<Ticket> TicketRepository;
        public TicketLogic(MainRepository<Ticket> TicketRepository)
        {
            this.TicketRepository = TicketRepository;
        }
        public async Task<bool> AddTicket(Ticket model)
        {
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.User_Id) || model.LastUpdate.Date != DateTime.Now.Date || string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Status))
            {
                return false;
            }
            else
            {
                await TicketRepository.AddItem(model);
                return true;
            }
        }
        public bool EditTicket(Ticket model)
        {
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.User_Id) || model.LastUpdate.Date != DateTime.Now.Date || string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Status))
            {
                return false;
            }
            else
            {
                TicketRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteTicket(int Id)
        {
            if (await TicketRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Ticket TicketDetail(int Id)
        {
            var Ticket = TicketRepository.Get(c => c.Id == Id, c => c.commnets,h =>  h.User ).Result.FirstOrDefault();
            return Ticket;
        }
        public List<Ticket> TicketList()
        {
            var Ticket = TicketRepository.Get(null, c => c.commnets,g => g.User ).Result.ToList();
            return Ticket;
        }
    }
}

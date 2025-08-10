using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.TicketService
{
    public class TicketLogic
    {
        private readonly MainRepository<Ticket> TicketRepository;
        private readonly MainRepository<Commnet> commentRepository;
        public TicketLogic(MainRepository<Ticket> TicketRepository, MainRepository<Commnet> commentRepository)
        {
            this.TicketRepository = TicketRepository;
            this.commentRepository = commentRepository;
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
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.User_Id) 
                                                        || model.LastUpdate.Date != DateTime.Now.Date || string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Status))
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
            if(TicketRepository.Get(t => t.Id == Id).Select(c => c.commnets).ToList().Any())
            {

                var Ticket = TicketRepository.Get(c => c.Id == Id, c => c.commnets, h => h.User).FirstOrDefault();
                Ticket.commnets = commentRepository.Get(c => c.Ticket_Id == Ticket.Id,u => u.User).ToList();
                return Ticket;
            }
            else
            {
                var Ticket = TicketRepository.Get(c => c.Id == Id, h => h.User).FirstOrDefault();
                return Ticket;
            }
        }
        public List<Ticket> TicketList()
        {
            var comment = new List<Commnet>();
            if (TicketRepository.Get().Select(c => c.commnets).ToList().Any())
            {
           var ticket = TicketRepository.Get(null,g => g.commnets, g => g.User).ToList();
                foreach(var tic in ticket)
                {
                    comment.AddRange(commentRepository.Get(c => c.Ticket_Id == tic.Id,u => u.User).ToList());
                    tic.commnets = comment;
                }
                return ticket;
            }
            else
            {
                var ticket = TicketRepository.Get(null, g => g.User).ToList();
                return ticket;
            }
        }
        public List<Ticket> TicketListOfUser(string User_Id)
        {
            var tickets = TicketList().Where(t => t.User_Id == User_Id).ToList();
            return tickets;
        }
    }
}

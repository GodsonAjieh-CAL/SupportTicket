using Microsoft.AspNetCore.Mvc;

namespace SupportTicket.Services
{
    public interface ITicketService
    {
        Task<List<SupportTicket>> GetAll();
        Task<SupportTicket?> GetById(int id);
        Task<SupportTicket?> Create(string title);
        Task<SupportTicket?> Update(int id, string status);
        Task<SupportTicket?> Delete(int id);

    }
}

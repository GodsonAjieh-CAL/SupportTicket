using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportTicket.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly TicketOptions _options;
        private readonly ILogger<TicketService> _logger;

        public TicketService(AppDbContext context, IOptions<TicketOptions> options, ILogger<TicketService> logger)
        {
            _context = context;
            _options = options.Value;
            _logger = logger;
        }
            
        public async Task<List<SupportTicket>> GetAll()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<SupportTicket?> GetById(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if(ticket == null)
            {
                _logger.LogWarning("Ticket {id} not found", id);
                return null;
            }
            return ticket;
        }

        public async Task<SupportTicket?> Create(string title)
        {
            var openCount = await _context.Tickets.CountAsync(t => t.Status == "Open");

            if(openCount >= _options.MaxOpenTickets)
            {
                _logger.LogWarning("Ticket creation blocked: open ticket limit of {Max} reached", _options.MaxOpenTickets);
                return null;
            }

            var ticket = new SupportTicket
            {
                Title = title,
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created ticket {id}: {Title}", ticket.Id, ticket.Title);

            return ticket;
        }

        public async Task<SupportTicket?> Update(int id, string status)
        {
            var foundTicket = await _context.Tickets.FindAsync(id);

            if(foundTicket == null)
            {
                _logger.LogWarning("Ticket {Id} not found for update", id);
                return null;
            }

            foundTicket.Status = status;
            await _context.SaveChangesAsync();
            return foundTicket;
        }

        public async Task<SupportTicket?> Delete(int id)
        {
            var deleteTicket = await _context.Tickets.FindAsync(id);

            if (deleteTicket == null)
            {
                _logger.LogWarning("Ticket {Id} not found for delete", id);
                return null;
            }

            _context.Tickets.Remove(deleteTicket);
            await _context.SaveChangesAsync();

            return deleteTicket;
        }
    }
}

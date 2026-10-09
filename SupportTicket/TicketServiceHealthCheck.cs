using Microsoft.Extensions.Diagnostics.HealthChecks;
using SupportTicket.Services;

namespace SupportTicket
{
    public class TicketServiceHealthCheck : IHealthCheck
    {
        private readonly ITicketService _ticketService;

        public TicketServiceHealthCheck(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _ticketService.GetAll();
                return HealthCheckResult.Healthy("Ticket service is responding");
            }
            catch(Exception ex)
            {
                return HealthCheckResult.Unhealthy("Ticket service failed", ex);
            }
        }
    }
}

using Portfolio.Components.Classes;
using Portfolio.Shared.DataModels;
using System.Threading.Tasks;

namespace Portfolio.Components.Services
{
    public class EmailService
    {
        private readonly PortfolioApiClient _apiClient;

        public EmailService(PortfolioApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> SendEmailAsync(SubmissionModel model)
        {
            return await _apiClient.Client.PostJsonAsync("api/email", model);
        }
    }
}

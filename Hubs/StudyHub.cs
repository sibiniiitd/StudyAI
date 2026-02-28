using Microsoft.AspNetCore.SignalR;

namespace StudyMonitor.Hubs
{
    public class StudyHub : Hub
    {
        public async Task SendAlert(string sessionId, string message, string severity)
        {
            await Clients.All.SendAsync("ReceiveAlert", sessionId, message, severity);
        }

        public async Task UpdateFocusScore(string sessionId, double score)
        {
            await Clients.All.SendAsync("FocusScoreUpdated", sessionId, score);
        }
    }
}

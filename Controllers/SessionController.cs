using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudyMonitor.Hubs;
using StudyMonitor.Models;
using StudyMonitor.Services;

namespace StudyMonitor.Controllers
{
    public class SessionController : Controller
    {
        private readonly SessionStorageService _storage;
        private readonly IHubContext<StudyHub> _hub;

        public SessionController(SessionStorageService storage, IHubContext<StudyHub> hub)
        {
            _storage = storage;
            _hub = hub;
        }

        [HttpPost]
        public IActionResult Start([FromBody] StartSessionRequest request)
        {
            var session = _storage.CreateSession(request.StudentName, request.SubjectName, request.TargetMinutes);
            HttpContext.Session.SetString("CurrentSessionId", session.SessionId);
            return Json(new { success = true, sessionId = session.SessionId });
        }

        public IActionResult Monitor(string sessionId)
        {
            var session = _storage.GetSession(sessionId);
            if (session == null) return RedirectToAction("Index", "Home");
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> TrackEvent([FromBody] BehaviorEventRequest request)
        {
            var evt = new BehaviorEvent
            {
                Timestamp = DateTime.Now,
                EventType = request.EventType,
                Description = request.Description,
                Severity = request.EventType switch
                {
                    "phone_detected" => "alert",
                    "tab_switch" => "warning",
                    "look_away" => "warning",
                    _ => "info"
                }
            };

            _storage.AddEvent(request.SessionId, evt);

            var snapshot = new FocusSnapshot
            {
                Timestamp = DateTime.Now,
                FocusScore = request.FocusScore,
                IsLookingAtScreen = request.IsLookingAtScreen,
                PhoneDetected = request.PhoneDetected,
                IsYawning = request.IsYawning,
                Emotion = request.Emotion
            };
            _storage.AddSnapshot(request.SessionId, snapshot);

            // Send real-time alert via SignalR
            if (evt.Severity != "info")
            {
                await _hub.Clients.All.SendAsync("ReceiveAlert", request.SessionId, request.Description, evt.Severity);
            }
            await _hub.Clients.All.SendAsync("FocusScoreUpdated", request.SessionId, request.FocusScore);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult End([FromBody] EndSessionRequest request)
        {
            try
            {
                var report = _storage.GenerateReport(request.SessionId);
                return Json(new { success = true, reportId = request.SessionId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}

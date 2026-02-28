using Microsoft.AspNetCore.Mvc;
using StudyMonitor.Services;

namespace StudyMonitor.Controllers
{
    public class ReportController : Controller
    {
        private readonly SessionStorageService _storage;

        public ReportController(SessionStorageService storage)
        {
            _storage = storage;
        }

        public IActionResult Index(string sessionId)
        {
            var session = _storage.GetSession(sessionId);
            if (session?.Report == null)
            {
                // Generate if not already generated
                try
                {
                    var report = _storage.GenerateReport(sessionId);
                    return View(report);
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(session.Report);
        }
    }
}

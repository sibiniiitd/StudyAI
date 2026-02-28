using StudyMonitor.Models;
using System.Collections.Concurrent;

namespace StudyMonitor.Services
{
    public class SessionStorageService
    {
        private readonly ConcurrentDictionary<string, StudySession> _sessions = new();

        public StudySession CreateSession(string studentName, string subjectName, int targetMinutes)
        {
            var session = new StudySession
            {
                StudentName = studentName,
                SubjectName = subjectName,
                TargetMinutes = targetMinutes,
                StartTime = DateTime.Now,
                IsActive = true
            };
            _sessions[session.SessionId] = session;
            return session;
        }

        public StudySession? GetSession(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);
            return session;
        }

        public void AddEvent(string sessionId, BehaviorEvent evt)
        {
            if (_sessions.TryGetValue(sessionId, out var session))
            {
                session.Events.Add(evt);
                UpdateCounters(session, evt);
            }
        }

        public void AddSnapshot(string sessionId, FocusSnapshot snapshot)
        {
            if (_sessions.TryGetValue(sessionId, out var session))
            {
                session.FocusSnapshots.Add(snapshot);
            }
        }

        private void UpdateCounters(StudySession session, BehaviorEvent evt)
        {
            switch (evt.EventType)
            {
                case "tab_switch": session.TabSwitches++; break;
                case "look_away": session.LookAwayCount++; break;
                case "phone_detected": session.PhoneDetections++; break;
                case "yawn": session.YawnCount++; break;
            }
        }

        public SessionReport GenerateReport(string sessionId)
        {
            var session = GetSession(sessionId);
            if (session == null) throw new Exception("Session not found");

            // Return cached report if already generated
            if (!session.IsActive && session.Report != null)
                return session.Report;

            session.IsActive = false;
            session.EndTime = DateTime.Now;

            var duration = (int)(session.EndTime.Value - session.StartTime).TotalMinutes;
            var snapshots = session.FocusSnapshots;
            var avgFocus = snapshots.Any() ? snapshots.Average(s => s.FocusScore) : 0;

            var focusedTime = snapshots.Count(s => s.FocusScore >= 60);
            var distractedTime = snapshots.Count(s => s.FocusScore < 60);

            string grade = avgFocus >= 85 ? "A+" : avgFocus >= 75 ? "A" : avgFocus >= 65 ? "B" :
                           avgFocus >= 55 ? "C" : avgFocus >= 45 ? "D" : "F";

            var report = new SessionReport
            {
                SessionId = sessionId,
                StudentName = session.StudentName,
                SubjectName = session.SubjectName,
                SessionDate = session.StartTime,
                TotalDurationMinutes = duration,
                TargetMinutes = session.TargetMinutes,
                CompletionPercentage = Math.Min(100, (duration * 100.0) / session.TargetMinutes),
                OverallFocusScore = Math.Round(avgFocus, 1),
                PerformanceGrade = grade,
                FocusedMinutes = focusedTime,
                DistractedMinutes = distractedTime,
                TabSwitchCount = session.TabSwitches,
                LookAwayCount = session.LookAwayCount,
                PhoneDetectionCount = session.PhoneDetections,
                YawnCount = session.YawnCount,
                AISummary = GenerateAISummary(session, avgFocus, grade, duration),
                Strengths = GenerateStrengths(session, avgFocus),
                Improvements = GenerateImprovements(session),
                AIRecommendations = GenerateRecommendations(session, avgFocus),
                HourlyData = GenerateHourlyData(session),
                BestFocusPeriod = GetBestPeriod(session),
                WorstFocusPeriod = GetWorstPeriod(session)
            };

            session.Report = report;
            return report;
        }

        private string GenerateAISummary(StudySession session, double avgFocus, string grade, int duration)
        {
            var completionPct = Math.Min(100, (duration * 100.0) / session.TargetMinutes);
            var level = avgFocus >= 75 ? "excellent" : avgFocus >= 55 ? "moderate" : "low";
            return $"{session.StudentName} completed a {duration}-minute study session on {session.SubjectName}, " +
                   $"achieving {completionPct:F0}% of the {session.TargetMinutes}-minute target. " +
                   $"Overall focus was {level} at {avgFocus:F1}%, earning a grade of {grade}. " +
                   $"The AI monitoring detected {session.TabSwitches} tab switches, {session.LookAwayCount} look-aways, " +
                   $"and {session.PhoneDetections} phone usage incidents during the session.";
        }

        private List<string> GenerateStrengths(StudySession session, double avgFocus)
        {
            var strengths = new List<string>();
            if (avgFocus >= 70) strengths.Add("Maintained strong focus throughout the session");
            if (session.TabSwitches < 3) strengths.Add("Excellent digital discipline — minimal tab switching");
            if (session.PhoneDetections < 2) strengths.Add("Phone usage was well controlled");
            if (session.LookAwayCount < 5) strengths.Add("Stayed attentive to study material");
            if (session.YawnCount < 3) strengths.Add("Good energy levels maintained");
            if (!strengths.Any()) strengths.Add("Committed to completing the study session");
            return strengths;
        }

        private List<string> GenerateImprovements(StudySession session)
        {
            var improvements = new List<string>();
            if (session.TabSwitches >= 5) improvements.Add($"Reduce tab switching — detected {session.TabSwitches} times");
            if (session.PhoneDetections >= 3) improvements.Add($"Minimize phone distractions — detected {session.PhoneDetections} times");
            if (session.LookAwayCount >= 10) improvements.Add("Work on maintaining consistent screen focus");
            if (session.YawnCount >= 5) improvements.Add("Consider breaks or better sleep schedule to combat fatigue");
            if (!improvements.Any()) improvements.Add("Continue your current excellent study habits");
            return improvements;
        }

        private List<string> GenerateRecommendations(StudySession session, double avgFocus)
        {
            return new List<string>
            {
                avgFocus < 60 ? "Try the Pomodoro technique: 25 min focus, 5 min break" : "Keep using your current study rhythm",
                session.PhoneDetections > 2 ? "Put your phone in another room or use a focus app" : "Your phone discipline is strong — maintain it",
                session.YawnCount > 4 ? "Take a 10-minute power nap before your next session" : "Your energy management is good",
                "Review the focus timeline to identify your peak concentration hours",
                "Set up a dedicated, distraction-free study environment for next session"
            };
        }

        private List<HourlyBreakdown> GenerateHourlyData(StudySession session)
        {
            var data = new List<HourlyBreakdown>();
            if (!session.FocusSnapshots.Any()) return data;

            var groups = session.FocusSnapshots
                .GroupBy(s => s.Timestamp.ToString("HH:mm"))
                .Take(12);

            foreach (var g in groups)
            {
                data.Add(new HourlyBreakdown
                {
                    TimeLabel = g.Key,
                    FocusScore = Math.Round(g.Average(s => s.FocusScore), 1),
                    DistractionCount = g.Count(s => !s.IsLookingAtScreen || s.PhoneDetected)
                });
            }
            return data;
        }

        private string GetBestPeriod(StudySession session)
        {
            if (!session.FocusSnapshots.Any()) return "N/A";
            var best = session.FocusSnapshots.OrderByDescending(s => s.FocusScore).First();
            return best.Timestamp.ToString("h:mm tt");
        }

        private string GetWorstPeriod(StudySession session)
        {
            if (!session.FocusSnapshots.Any()) return "N/A";
            var worst = session.FocusSnapshots.OrderBy(s => s.FocusScore).First();
            return worst.Timestamp.ToString("h:mm tt");
        }
    }
}

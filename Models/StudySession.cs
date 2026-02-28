namespace StudyMonitor.Models
{
    public class StudySession
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string SubjectName { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int TargetMinutes { get; set; } = 180;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public List<BehaviorEvent> Events { get; set; } = new();
        public List<FocusSnapshot> FocusSnapshots { get; set; } = new();
        public int TotalFocusedSeconds { get; set; }
        public int TotalDistractedSeconds { get; set; }
        public int TabSwitches { get; set; }
        public int LookAwayCount { get; set; }
        public int PhoneDetections { get; set; }
        public int YawnCount { get; set; }
        public double AverageFocusScore { get; set; }
        public List<string> AIInsights { get; set; } = new();
        public SessionReport? Report { get; set; }
    }

    public class EndSessionRequest
    {
        public string SessionId { get; set; } = string.Empty;
    }

    public class BehaviorEvent
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "info"; // info, warning, alert
    }

    public class FocusSnapshot
    {
        public DateTime Timestamp { get; set; }
        public double FocusScore { get; set; } // 0-100
        public bool IsLookingAtScreen { get; set; }
        public bool PhoneDetected { get; set; }
        public bool IsYawning { get; set; }
        public string Emotion { get; set; } = "neutral";
    }

    public class SessionReport
    {
        public string SessionId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public DateTime SessionDate { get; set; }
        public int TotalDurationMinutes { get; set; }
        public int TargetMinutes { get; set; }
        public double CompletionPercentage { get; set; }
        public double OverallFocusScore { get; set; }
        public string PerformanceGrade { get; set; } = string.Empty;
        public int FocusedMinutes { get; set; }
        public int DistractedMinutes { get; set; }
        public int TabSwitchCount { get; set; }
        public int LookAwayCount { get; set; }
        public int PhoneDetectionCount { get; set; }
        public int YawnCount { get; set; }
        public List<string> Strengths { get; set; } = new();
        public List<string> Improvements { get; set; } = new();
        public List<string> AIRecommendations { get; set; } = new();
        public string AISummary { get; set; } = string.Empty;
        public List<HourlyBreakdown> HourlyData { get; set; } = new();
        public string BestFocusPeriod { get; set; } = string.Empty;
        public string WorstFocusPeriod { get; set; } = string.Empty;
    }

    public class HourlyBreakdown
    {
        public string TimeLabel { get; set; } = string.Empty;
        public double FocusScore { get; set; }
        public int DistractionCount { get; set; }
    }

    public class StartSessionRequest
    {
        public string StudentName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int TargetMinutes { get; set; } = 180;
    }

    public class BehaviorEventRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double FocusScore { get; set; }
        public bool IsLookingAtScreen { get; set; }
        public bool PhoneDetected { get; set; }
        public bool IsYawning { get; set; }
        public string Emotion { get; set; } = "neutral";
    }
}

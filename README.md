# StudyAI Monitor — AI-Powered Study Session Tracker

A .NET 8 MVC application that uses AI to monitor study sessions in real-time via webcam and generates comprehensive performance reports.

https://studyai-updq.onrender.com

---

## 🚀 Features

### Real-Time AI Monitoring
- **Eye Tracking** — Detects when you look away from the screen
- **Phone Detection** — Alerts when phone usage is detected
- **Yawn Detection** — Tracks fatigue levels
- **Tab Switch Detection** — Monitors browser focus
- **Emotion Detection** — Reads facial expressions (focused, tired, confused)
- **Live Focus Score** — Real-time 0–100 score with animated ring display

### Session Management
- Set custom study targets (1hr, 1.5hr, 2hr, 3hr, or custom)
- Live countdown timer
- Progress bar with milestones
- AI Coach tips in real-time
- Activity log with timestamps

### Comprehensive AI Report
- Overall Performance Grade (A+ to F)
- Focus Timeline Chart
- Distraction Breakdown (tab switches, look-aways, phone, yawns)
- Best & Worst Focus Periods
- AI-Generated Summary
- Personalized Strengths & Improvements
- Actionable AI Recommendations
- Printable Report

### Real-Time Features (SignalR)
- Live alerts broadcast via WebSocket
- Focus score updates in real-time
- Multi-device monitoring support

---

## 🛠 Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core 8 MVC |
| Real-time | SignalR WebSockets |
| Frontend | Vanilla JS + CSS3 |
| AI Monitoring | Browser WebRTC + AI Analysis Engine |
| Fonts | Syne + DM Mono (Google Fonts) |
| State | In-memory (ConcurrentDictionary) |

---

## 📦 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A modern browser (Chrome/Edge recommended for camera access)
- Webcam for full AI monitoring features

---

## 🔧 Setup & Run

```bash
# 1. Clone or extract the project
cd StudyMonitor

# 2. Restore packages
dotnet restore

# 3. Run the application
dotnet run

# 4. Open in browser
# https://localhost:5001 or http://localhost:5000
```

---

## 📁 Project Structure

```
StudyMonitor/
├── Controllers/
│   ├── HomeController.cs       # Landing page
│   ├── SessionController.cs    # Session start/track/end APIs
│   └── ReportController.cs     # Report generation & display
├── Models/
│   └── StudySession.cs         # All data models
├── Services/
│   └── SessionStorageService.cs # In-memory session management + report generation
├── Hubs/
│   └── StudyHub.cs             # SignalR real-time hub
├── Views/
│   ├── Home/Index.cshtml       # Landing / setup page
│   ├── Session/Monitor.cshtml  # Live monitoring dashboard
│   ├── Report/Index.cshtml     # AI report page
│   └── Shared/_Layout.cshtml   # Main layout
└── wwwroot/
    └── css/site.css            # Complete styling
```

---

## 🔌 API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/Session/Start` | Create new study session |
| GET | `/Session/Monitor?sessionId=` | Load monitoring dashboard |
| POST | `/Session/TrackEvent` | Log behavior event + snapshot |
| POST | `/Session/End` | End session & generate report |
| GET | `/Report/Index?sessionId=` | View AI report |
| WS | `/studyHub` | SignalR real-time connection |

---

## 🤖 Extending with Real AI

To add real computer vision, integrate these libraries in the frontend:

```javascript
// Option 1: Face-api.js (face detection, expressions)
import * as faceapi from 'face-api.js';

// Option 2: MediaPipe (eye tracking, pose)
import { FaceLandmarker } from '@mediapipe/tasks-vision';

// Option 3: TensorFlow.js
import * as tf from '@tensorflow/tfjs';
```

Replace the `runAIAnalysis()` function in `Monitor.cshtml` with real model inference.

---

## 📊 Sample Report Output

```
Student:    Arjun Sharma
Subject:    Data Structures
Duration:   92 min / 120 min target (77% complete)
Grade:      B
Focus:      68.4%

Distractions:
  Tab Switches:  3
  Look Aways:    12
  Phone Use:     1
  Yawns:         4

Strengths:
  ✓ Phone usage well controlled
  ✓ Committed to the session

Improvements:
  → Reduce look-aways (12 detected)
  → Consider break schedule for fatigue
```

---

## 🔐 Privacy

- Camera feed is processed **locally in the browser**
- No video/images are sent to the server
- Only behavioral metadata (scores, counts) is transmitted
- All session data is **in-memory only** (not persisted to disk/DB)

---

## 🗺 Roadmap

- [ ] Add real MediaPipe/face-api.js integration
- [ ] Persistent database (EF Core + SQLite)
- [ ] Study history & trend analysis
- [ ] Export report as PDF
- [ ] Multi-subject streak tracking
- [ ] Pomodoro timer integration
- [ ] Mobile app (MAUI)

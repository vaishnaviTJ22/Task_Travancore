🧟 Endless Zombie Survival
A fast-paced, wave-based zombie survival game built with Unity for Android. Test your reflexes as you defend against increasingly challenging waves of zombies!

🎮 Game Features
Core Gameplay
Endless Wave System - Survive increasingly difficult waves of zombies
Two Enemy Types - Standard and Armored zombies with different health pools
Progressive Difficulty - More enemies spawn each wave with reduced spawn delays
Wave Completion - Next wave only starts after eliminating all enemies
Visual & Audio Feedback
Hit Effects - Particle system feedback on enemy hits
Color Flash - Enemies flash red when damaged
Knockback System - Satisfying physics-based knockback on hits
Hit Animations - Smooth animation transitions for all enemy states
User Interface
Real-time Score Tracking - Points awarded for each enemy defeated
Health System - Visual health bar for the treasure you're defending
Wave Counter - Clear wave number display with transitions
Game Over Screen - Retry and return to menu options
🛠️ Technical Highlights
Optimizations
Object Pooling System - Efficient enemy reuse preventing garbage collection spikes
Dynamic Pool Management - Automatically adjusts pool size based on wave requirements
Capped Memory Usage - Maximum pool size prevents memory overflow
Mobile-Optimized - Designed for smooth 60 FPS on mid-range Android devices
Architecture
Singleton Pattern - GameManager, UIManager, and PoolManager for easy access
Component-Based Design - Modular scripts for maintainability
Coroutine-Based Systems - Non-blocking wave spawning and UI animations
Event-Driven Updates - Efficient UI updates only when needed
🎯 How to Play
Objective: Defend the treasure from zombie hordes
Controls: Tap/Click on zombies to eliminate them
Scoring: Earn points for each defeated enemy
Waves: Clear all enemies to progress to the next wave
Game Over: Occurs when treasure health reaches zero
🔧 Built With
Unity Version: 2022.3 LTS
Platform: Android (API Level 21+)
Scripting: C#
Rendering: Built-in Render Pipeline
UI: Unity UI (uGUI)
Particle System: Unity Particle System
Animation: Legacy Animation System

🛡️ Endless Defence Game – Unity Machine Test

📌 Project Overview

This project is a simple 3D endless defence game prototype developed using Unity, where the player must protect a central treasure/core from continuously spawning enemies.

Enemies approach the core in waves, and the player eliminates them using mouse clicks / screen taps.
The game ends when the treasure’s health reaches zero.

The focus of this project is on clean architecture, gameplay logic, scalability, and performance optimization, rather than art or complex mechanics.

🎮 Gameplay Mechanics

A central treasure is placed in the arena

Enemies spawn in progressive waves

Enemies move toward the treasure

Player destroys enemies by clicking/tapping on them

Each enemy reaching the treasure reduces its health

Game ends when treasure health becomes zero

Score increases for each enemy destroyed

👾 Enemy Types
🔹 Standard Enemy

Destroyed with a single tap

Faster movement speed

Lower score value

🔹 Armored Enemy

Requires multiple taps to destroy

Slower but tougher

Higher score value

Different death animation


🌊 Wave & Difficulty System

Wave number increases progressively

Enemy count increases every wave

Only the additional enemies required for the next wave are spawned

Existing enemies persist between waves

Spawn delay reduces gradually to increase difficulty

Wave number is displayed on the UI at the start of each wave

🚀 Performance Optimization
✅ Object Pooling

Implemented object pooling for enemies

Avoids frequent Instantiate() and Destroy()

Enemies are reused from the pool

Pool expands dynamically only when required

Optimized for mobile and low-end devices

🧱 Architecture & Code Design

Modular and readable C# scripts

No monolithic scripts

Clear separation of responsibilities

Key Design Patterns Used:

Inheritance (Enemy base class)

Polymorphism (different enemy behaviors)

Object Pooling

Coroutine-based wave handling

Core Scripts:

GameManager – Game state & score handling

EnemySpawner – Wave logic & incremental spawning

Enemy (abstract) – Shared enemy behavior

StandardEnemy, ArmoredEnemy – Specific enemy logic

EnemyPoolManager – Object pooling system

Treasure – Health & damage handling

UIManager – UI updates (score, wave, game over)

🖥️ User Interface

Score counter

Treasure health bar

Wave number display

Game Over screen with final score

UI is kept minimal and readable for both PC and mobile simulation.

🎯 Controls

Mouse Click (PC)

Screen Tap (Mobile simulation)

No complex controls are used as per the test requirements.

🛠️ Tools & Technologies

Unity Engine

C#

Unity UI System

Git for version contro

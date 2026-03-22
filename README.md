# COMP-490-Group-3

**USE UNITY EDITOR VERSION 6000.2.8f1**

If you run into errors you most likely have to go into settings and do the following:

* Edit >Project Settings > Player > Other Settings > Active Input Handling > Both

* Check if all task, videos, scripts, and sprites are assigned in the inspector.

# Experiment System – Quick Guide

This project uses small, focused scripts. Each script has one job.  
Use this guide when creating new levels.

---

# Core Idea

Everything flows like this:

**Interaction → Task → Visual Change**

---

# Scripts Overview

## 1. Task.cs
Represents a single step in the experiment.

**What it does:**
- Tracks if a task is completed
- Fires an event when completed

**How to use:**
- Create a GameObject for each task
- Attach `Task`
- Reference it in other scripts

---

## 2. TaskManager.cs
Controls task order and game progression.

**What it does:**
- Enforces correct order
- Applies penalties
- Tracks time
- Handles completion (video/end)

**How to use:**
- Add to one GameObject (e.g., `GameManager`)
- Assign tasks in order in the inspector

---

## 3. Container.cs
Handles drag-and-drop interactions.

**What it does:**
- Accepts ingredients/tools
- Transforms tools (e.g., Spoon → SpoonSoap)
- Triggers tasks
- Resets tools

**How to use:**
- Attach to:
  - Flask
  - Cup
  - Sink
  - Ingredient sources (Soap)
- Configure accepted ingredients in inspector

---

## 4. Interactive.cs
Handles click-based interactions. Used in flask swirling and cup stirring.

**What it does:**
- Validates task order
- Plays animation
- Completes task AFTER animation

**How to use:**
- Attach to clickable objects
- Assign:
  - Task
  - Animator
- Add animation event at end → `OnAnimationFinished`

---

## 5. SpriteOnTaskComplete.cs
Handles visual changes from tasks.

**What it does:**
- Listens for task completion
- Changes sprite

**How to use:**
- Attach to object with SpriteRenderer
- Add task → sprite pairs

IF ANIMATION CONTROLS SPRITE IT WILL BECOME CONFLICTED

---

# Animations

Use Animator for:
- Movement (stirring, swirling)
- Showing/hiding objects

Do NOT:
- Mix sprite control between animation and script

You can hide sprite if needed!
---

# Restart System

The game uses **scene reloading** to reset all state.

**What it does:**
- Resets all tasks, animations, and interactions automatically
- Avoids complex manual reset logic

**Implementation:**
```csharp
SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

---

# Pause System

The game supports pausing via:
- ESC key
- Pause button (UI)

**Behavior:**
- Freezes time using `Time.timeScale = 0`
- Locks all interactions
- UI remains active

**Drag Behavior:**
- Objects stay in place when paused mid-drag
- On resume, dragged objects return to original position

---

# Interaction Lock

A global interaction lock prevents input during:
- Pause
- Video playback
- Game end states

```csharp
TaskManager.IsInteractionLocked

---


---

## Win/Lose flow

You implemented:
- video → win panel
- lose panel on timeout

# 🏁 Game Flow

**Win Condition:**
- Complete all tasks in order
- Plays completion video
- Shows win panel after video ends

**Lose Condition:**
- Timer reaches zero
- Shows lose panel

---

# General Rules

- TaskManager = order
- Container = drag interactions
- Interactive = click + animation
- Task = event
- SpriteOnTaskComplete = visuals

---

# Example Flow

1. Drag ingredient → Container  
2. Container checks task → TaskManager validates  
3. Task completes → event fires  
4. Sprite updates OR animation plays  

---

# Common Mistakes

- Animator AND script both changing sprite  
- Missing animation event  
- Wrong Task reference  
- Overlapping active objects  

---

# Tips for New Levels

- Keep tasks simple and ordered  
- Separate logic from visuals  
- Reuse scripts  
- Test step-by-step  

---

If something breaks, check:
1. Task reference  
2. Task order  
3. Animation event  
4. Sprite control source  
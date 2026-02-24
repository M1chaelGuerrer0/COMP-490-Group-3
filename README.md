# COMP-490-Group-3

**USE UNITY EDITOR VERSION 6000.2.8f1**

If you run into errors you most likely have to go into settings and do the following:

* Edit >Project Settings > Player > Other Settings > Active Input Handling > Both

* Check if all task, videos, scripts, and sprites are assigned in the inspector.

## Architecture Overview

The experiment interaction system is built using a modular component-based design. Objects are separated by responsibility to keep the system scalable and maintainable for future experiments.

The core components involved are:
* Draggable
* Ingredient
* Container
* Task
* TaskManager

### Draggable

The Draggable component handles all mouse interaction:
* Click to pick up
* Drag movement
* Drop detection
* Overlap detection with containers

This script is reusable and attached to:

* All ingredient objects

* The cup (since it can also be poured into the flask)

Draggable does not decide what is correct or incorrect. It only detects whether it was dropped onto a valid Container.

### Ingredient

The Ingredient component defines:
* A unique ingredientID (string identifier)
* A reference to the Task that should complete when used correctly

This allows containers to determine:
* Whether they accept the ingredient
* Which task should be completed when accepted

Ingredients do not contain logic about where they can be placed — that responsibility belongs to the container.

### Container (Flask & Cup)

Both the Flask and Cup use the same Container script.

Each container defines:

* A list of accepted ingredient IDs
* The sprite to switch to when a specific ingredient is added

When a draggable object is dropped:
1. The container checks if the object has an Ingredient component.
2. It compares the ingredient’s ingredientID against its accepted list.
3. If valid:
   * The associated task is completed.
   * The container sprite updates.
4. If invalid:
    * A time penalty is applied through TaskManager.

This makes Flask and Cup behavior data-driven rather than hardcoded.

### Task System

Each ingredient references a Task component.

The TaskManager:

* Stores an ordered list of tasks
* Validates that tasks are completed in the correct sequence
* Applies time penalties for incorrect steps
* Stops the timer and plays the reaction video when all tasks are completed

This ensures:
* Step order is enforced
* Incorrect actions do not fail the level but add time penalties
* The experiment completion triggers only when all steps are correctly completed

### Design Benefits

This system provides:
* Clear separation of responsibilities
* Scalable architecture for additional experiments
* Reusable drag-and-drop logic
* Data-driven container behavior
* Controlled task ordering and penalty system

The design allows future levels to introduce new containers, ingredients, and mechanics without rewriting core systems.
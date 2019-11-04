# Tutorial System (Good Game)

## Structure
1. Each `Tutorial` is a series of steps designed to teach a specific concept (e.g. how to attack)
1. A `Tutorial` contains multiple `TutorialStep` instances in a linear sequence
1. Each `TutorialStep` has a receiver id
1. Within the game world, GameObjects can have the component (script) `TutorialStepReceiver`
1. These `TutorialStepReceiver` instances are intended to have IDs matching the tutorial step to which they correspond
1. On a per-case basis, their behavior is registered as a delegate, the timing of which is determined by the `TutorialManager` (whenever the corresponding `TutorialStep` is triggered)
1. All `TutorialStepReceiver` instances should be registered on the `TutorialManager` via the serialized array `Tutorial Step Receivers` in the Inspector view
1. The `Tutorial` instances are declared in the `DataInitializer` class (their steps are defined here)
1. The `DataInitializer` will create these tutorials as new within the `GameSave` data class, if a new save is started
1. Otherwise, the `DataInitializer` will load the existing the tutorials from the `GameSave` instance
1. The `GameSave` instance tracks the tutorial steps and the user's progress through them
1. The `TutorialManager` is responsible for updating this progress

# Utility AI
Utility AI implementation using Unity based on Dave Mark's Infinity Axis Utility AI. This is not an exact implementation as some concepts are renamed and rescoped in an attempt to improve and build it based on my preference.

 - Data-driven. Components are built from **ScriptableObjects** and editor-friendly **Serializable** classes.
 - Various `Consideration` scoring schemes. `Consideration` scoring can be extended further for a more customized behavior.
 - Consideration scores are multiplied which results to a final score. Final score scales based on the number of considerations. This is different from Dave Mark's solution. I used a root function instead of computing for a compensation factor.
 - `Decisions` are created and scored for each target and any additional data attached if any (eg. Throw a **stone** at **Slime A**).
 - `Consideration` scores can be cached. `Considerations` scored from the same agent, target, and additional data do not need to be scored again.
 - Various performance saviors. `Consideration` and `Decision` scorings are skipped if there's no point on scoring them anymore.
 - `Actions` have a complete life cycle (`OnEnter`, `OnUpdate`, `OnExit`). You can build the entire AI behavior without using an external planner/architecture (eg. GOAP, FSM, Behavior Tree). However, you can still choose to wire it to an external system if you wish (via calling it `OnEnter`).
 - `Actions` are dynamically instantiated Serializable C# classes. Properties of this class can be drawn in the editor via the **Serialize Reference Extensions** plugin.
 - Use the built-in `Reasoners` or create your own to build a list of `Decisions` based on self, target, or whatever you want to base it on. `Reasoners` can assign `additional data` to each `Decision`.
 - Create `Behaviors` to pair an `Action` and lists of `Considerations`. `Decisions` will be derived from `Behaviors`.
 - Attach `UtilityAgent` to the GameObject to drive the Utility AI behavior.
 - Attach `UtilityTarget` to the GameObject to allow `UtilityAgents` to mark this GameObject as a target.
 - Attach `Tags` to a `UtilityTarget` to filter targets for any given `Consideration` and `Behavior`.
 

# Installation
## Install via git URL
Open `Package Manager` > `Add package from git URL` option and enter:

```
https://github.com/Sylpheed/UtilityAI.git?path=Assets/Sylpheed/UtilityAI
```

## Dependencies

On top of Utility AI package, add these as well:

### Serialize Reference Extensions
```
https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/MackySoft.SerializeReferenceExtensions
```


# Components
## Consideration
- A single entity where a **certain scenario** is checked and given a **score**. This can be as simple as **IsHealthy**.
- Implemented as a `ScriptableObject` which can be used and reused across different `Behaviors`.
- Create your own custom `Consideration` by inheriting from a base `Consideration` class of your choise (see below).
- Can assign a `Priority` to allow a `Consideration` to be scored first over other considerations. This is useful if you want to skip expensive consideration checks.
- Can prematurely filter out targets. Bails out if the `Decision` doesn't contain a `UtilityTarget` with the specified `Tags`.

### Curve Consideration
- Score a consideration by using a `curve`.
### BoolConsideration
- Score a consideration using a `boolean`. This will resolve to a `1f` or `0f`.
### FixedConsideration
- Score a consideration using a `fixed value`.
### CompositeConsideration
- A collation of multiple `Considerations`. Score will be based on the assigned `Considerations`.
### CustomConsideration
- You can create your own scoring scheme by extending the `Consideration` base class.

## Behavior
- A pairing of `Action` and a list of `Considerations`. This provides a template on how each `Decision` will be laid out, chosen, and acted upon. Note that this is not the actual action.
- Implemented as a `ScriptableObject` which can be used and reused across different `UtilityAgents`.
- Assign a `Reasoner` to create `Decisions` based on this behavior.
- Can assign a `Weight` to increase the likelihood of a `Behavior` to be selected. The `Weight` will be multiplied to the final score.
- Can prematurely filter out targets to prevent from creating a `Decision` against a `UtilityTarget` without the required `Tag`.

## BehaviorSet
- A simple list of `Behaviors` used to group similar `Behaviors`. This only serves as an asset-friendly way of grouping behaviors. Internally, this will resolve to a flat list of `Behaviors`.
- Implemented as a `ScriptableObject`.
- Assigned to `UtilityAgent`. This means that you have to create a BehaviorSet for each type of unit or even exclusive to a specific unit (eg. boss).

## Action
- When a decision is chosen, an `Action` is invoked. This is the actual details of the action (eg. move, patrol, attack, etc).
- Implemented as a `Serializable` C# class. Inherit from this base class to create `Actions`. Add a `[System.Serializable]` attribute to the inherited class so that it can be drawn in the editor.
- Has optional lifecycle callbacks like `OnEnter`, `OnUpdate`, `OnExit`.
- You can drive the entire AI behavior through here without relying on external architecture/planner. If you chose to implement it outside of this framework, you can simply wire it via `OnEnter`.
- When an `Action` completes/cancelled, the `UtilityAgent` will be notified and will choose a new `Decision`. Actions can be concluded via calling `Exit`  anywhere within the `Action` or returning `false` in the overridable `ShouldExit` method.
- `OnEnter` can serve as a precondition. Return `false` to invalidate and cancel the action before it's executed.
- Since this is created runtime and has its own unique lifecycle, it will be unique to the chosen `Decision`. Feel free to cache variables. It will not affect other instances of `Actions`.

## Decision
- An evaluation why a certain `Behavior` should be taken by a `UtilityAgent`. This evaluation is represented by scoring all the `Considerations` within the behavior.
- Decision is a runtime construct. This will only be generated internally or via a custom `Reasoner`.
- A `Consideration` evaluates a `Decision`. The `Decision` has all the context needed. This contains the `Behavior` where it's derived from, `UtilityAgent`, `UtilityTarget` if any, additional `Data` if any.
- A list of `Decisions` is created, from which the best `Decision` is selected.
- Decision is unique per `UtilityTarget` and `Data` whenever they're applicable. This means for the same `Behavior`, there will be 2 `Decisions` if there are 2 valid `UtilityTargets`. These `Decisions` are scored separately.
- Has an optional `Data` field which can only be supplied by a custom `Reasonser`. A unique `Decision` will be created for each unique `Data`.

## Reasoner
- Builds a list of `Decisions` based on available data from `UtilityAgent` and `UtilityTargets`. This is rebuilt every time the `UtilityAgent` needs to decide.
- A `Reasoner` is assigned to a `Behavior` since the `Behavior` serves as a template for `Decisions`. You could say that `Decisions` are derived from `Behavior` but the `Reasoner` is the bridge to create `Decisions`.

### SelfReasoner
- Built-in. 
- Use this if the `Behavior` doesn't require a `UtilityTarget` and `Data`.
### TargetReasoner
- Built-in.
- Use this if the `Behavior` requires a `UtilityTarget` but doesn't require `Data`.
### CustomReasoner
- You can create your own `Reasoner` by deriving from the base class.
- If there is `Data` that needs to be evaluated, you are required to create a custom `Reasoner`.
- The objective of the `Reasoner` is to build a list of all possible `Decisions` based from the `Behavior` against all valid `UtilityTargets` and attach `Data` if applicable.
- If a `Behavior` requires a `UtilityTarget`, create a new `Decision` for each valid unique `UtilityTarget`.
- If a `Behavior` requires a `Data`, create a new `Decision` for each unique `Data`. For example, if you have 5 items to choose from to use against a specific `UtilityTarget`, you would create 5 `Decisions` for that single `UtilityTarget`. If there are 2 `UtilityTargets`, you would have a total of 10 `Decisions`.
- Add a `[System.Serializable]` attribute to the inherited class so that it can be selected from the editor.

## UtilityAgent
- Represents the brain of Utility AI.
- Attach this as a component to the `GameObject` of your AI agent.
- `UtilityAgent` are actors of `Actions`.
- Assign a `BehaviorSet` to add behavior.
- You can dynamically add/remove `Behavior`, list of `Behavior`, and `BehaviorSet` during runtime on top of the base `BehaviorSet`.
- Has a built-in `UtilityTarget` searching using Physics.SphereCast. `UtilityTargets` within radius are cached.

## UtilityTarget
- Represents an object that a `UtilityAgent` can interact with (eg. enemy, a location, etc.)
- Attach this as a component to the `GameObject`.
- Contains an optional list of `Tags` which can be used to filter out targets based on the requirements of `Behavior` and `Consideration`.

## Tag
- Tags a `UtilityTarget` (eg. Enemy, Food). Create and assign `Tags`so that you can easily filter out unnecessary `UtilityTargets`.
- Implemented as a `ScriptableObject`.
- Assign to `UtilityTarget` to mark it as having such properties (or `Tags`).
- Assign to `Consideration` and `Behavior` so that it'll only pick out `Decisions` that have `UtilityTargets` with the required `Tags`.

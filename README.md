# Utility AI
Utility AI implementation using Unity based on Dave Mark's Infinity Axis Utility AI. This is not an exact implementation as some concepts are renamed and rescoped in an attempt to improve and build it based on my preference.

 - Data-driven. Components are built from `ScriptableObjects` and editor-friendly `Serializable` classes.
 - Various `Consideration` scoring schemes. `Consideration` scoring can be extended further for a more customized behavior.
 - `Decisions` are created and scored for each target and any additional data attached if any (eg. Throw a `stone` at `Slime A`).
 - `Consideration` scores can be cached. `Considerations` scored from the same agent, target, and additional data do not need to be scored again.
 - Various performance saviors. `Consideration` and `Decision` scorings are skipped if there's no point on scoring them anymore.
 - `Actions` have a complete life cycle (`OnEnter`, `OnUpdate`, `OnExit`). You can build the entire AI behavior without using an external planner/architecture (eg. GOAP, FSM, Behavior Tree). However, you can still choose to wire it to an external system if you wish (via calling it `OnEnter`).
 - `Actions` are dynamically instantiated Serializable C# classes. Properties of this class can be drawn in the editor via the `Serialize Reference Extensions` plugin.
 - Use the built-in `Reasoners` or create your own to build a list of `Decisions` based on self, target, or whatever you want to base it on. `Reasoners` can assign `additional data` to each `Decision`.
 - Create `Behaviors` to pair an `Action` and lists of `Considerations`. `Decisions` will be derived from `Behaviors`.
 - Attach `UtilityAgent` to the GameObject to drive the Utility AI behavior.
 - Attach `UtilityTarget` to the GameObject to allow `UtilityAgents` to mark this GameObject as a target.
 - Attach `Tags` to a `UtilityTarget` to filter targets for any given `Consideration` and `Behavior`.
 

# Installation
## Install via git URL
Open `Package Manager` > `Add package from git URL` option and enter:

```
https://github.com/DengRodil/UtilityAI.git?path=Assets/Sylpheed/UtilityAI
```

## Dependencies

On top of Utility AI package, add these as well:

### Serialize Reference Extensions
```
https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/MackySoft.SerializeReferenceExtensions

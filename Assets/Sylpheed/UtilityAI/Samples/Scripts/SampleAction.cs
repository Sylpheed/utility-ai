using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    [System.Serializable]
    public class SampleAction : IAction
    {
        [SerializeField] private float _damage = 1f;
        
        public void Execute(UtilityAgent actor, UtilityTarget target = null)
        {
            Debug.Log($"[Action] {nameof(SampleAction)} executed. Agent: {actor.gameObject.name}. Target: {target?.gameObject.name ?? "None" }");
        }
    }
}
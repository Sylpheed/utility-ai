using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
{
    [System.Serializable]
    public class SampleAction : Action
    {
        [SerializeField] private float _damage = 1f;

        protected override bool OnEnter()
        {
            Debug.Log($"[Action] {nameof(SampleAction)} executed. Agent: {Agent.gameObject.name}. Target: {Target?.gameObject.name ?? "None" }");
            
            return true;
        }
    }
}
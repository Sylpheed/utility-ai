using UnityEngine;

namespace Sylpheed.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Within Target Proximity", menuName = "Utility AI/Consideration/Within Target Proximity")]
    public class WithinTargetProximityConsideration : BoolConsideration
    {
        [Header("Within Target Proximity")] 
        [SerializeField] private float _distanceThreshold;
        
        protected override bool OnEvaluateAsBool(Decision decision)
        {
            var distance = Vector3.Distance(decision.Agent.transform.position, decision.Target.transform.position);
            return distance <= _distanceThreshold;
        }
    }
}
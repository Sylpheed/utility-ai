using UnityEngine;

namespace Sylpheed.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Distance To Target", menuName = "Utility AI/Consideration/Distance To Target")]
    public class DistanceToTargetConsideration : Consideration
    {
        [Header("Distance To Target")] 
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        protected override float OnEvaluate(Decision decision)
        {
            var distance = Vector3.Distance(decision.Agent.transform.position, decision.Target.transform.position);
            if (distance > _maxDistance) return 0f;
            if (distance < _minDistance) return 1f;
            
            return EvaluateCurve(distance, _minDistance, _maxDistance);
        }
    }
}
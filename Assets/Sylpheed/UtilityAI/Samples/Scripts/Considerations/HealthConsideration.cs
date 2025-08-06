using Sylpheed.UtilityAI.Considerations;
using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    [CreateAssetMenu(fileName = "Health", menuName = "Utility AI/Consideration/Samples/Health")]
    public class HealthConsideration : CurveConsideration
    {
        [Header("Healthy")]
        [SerializeField] [Range(0, 1)] private float _min = 0f;
        [SerializeField] [Range(0, 1)] private float _max = 1f;
        
        protected override float OnEvaluate(Decision decision)
        {
            var health = decision.Agent.GetComponent<Health>();
            if (!health) return 0f;
            
            // Get hp threshold
            var min = health.Max * _min;
            var max = health.Max * _max;
            
            return EvaluateCurve(health.Current, min, max);
        }
    }
}
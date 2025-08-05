using Sylpheed.UtilityAI.Considerations;
using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    [CreateAssetMenu(fileName = "Random", menuName = "Utility AI/Consideration/Samples/Healthy")]
    public class HealthyConsideration : CurveConsideration
    {
        protected override float OnEvaluate(Decision decision)
        {
            var health = decision.Agent.GetComponent<Health>();
            if (!health) return 0f;
            
            return EvaluateCurve(health.Current, health.Max);
        }
    }
}
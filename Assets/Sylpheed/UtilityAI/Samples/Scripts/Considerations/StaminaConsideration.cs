using Sylpheed.UtilityAI.Considerations;
using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
{
    [CreateAssetMenu(fileName = "Health", menuName = "Utility AI/Consideration/Samples/Stamina")]
    public class StaminaConsideration : CurveConsideration
    {
        [Header("Healthy")]
        [SerializeField] [Range(0, 1)] private float _min = 0f;
        [SerializeField] [Range(0, 1)] private float _max = 1f;
        
        protected override float OnEvaluate(Decision decision)
        {
            var stamina = decision.Agent.GetComponent<Stamina>();
            if (!stamina) return 0f;
            
            // Get hp threshold
            var min = stamina.Max * _min;
            var max = stamina.Max * _max;
            
            return EvaluateCurve(stamina.Current, min, max);
        }
    }
}
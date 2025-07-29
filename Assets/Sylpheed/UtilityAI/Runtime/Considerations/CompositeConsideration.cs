using UnityEngine;

namespace Sylpheed.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Distance To Target", menuName = "Utility AI/Consideration/Composite")]
    public sealed class CompositeConsideration : Consideration
    {
        [Header("Composite")]
        [SerializeField] private Consideration[] _considerations;
        
        protected override float OnEvaluate(Decision decision)
        {
            var score = 1f;
            foreach (var consideration in _considerations)
                score *= consideration.Evaluate(decision);
            return score;
        }
    }
}
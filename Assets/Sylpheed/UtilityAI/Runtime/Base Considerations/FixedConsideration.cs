using UnityEngine;

namespace Sylpheed.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Fixed", menuName = "Utility AI/Consideration/Fixed")]
    public class FixedConsideration : Consideration
    {
        [SerializeField] [Range(0, 1)] private float _score;

        protected sealed override float OnEvaluate(Decision decision) => _score;
    }
}
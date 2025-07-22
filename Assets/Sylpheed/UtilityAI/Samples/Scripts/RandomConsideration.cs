using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    [CreateAssetMenu(fileName = "Random", menuName = "Utility AI/Consideration/Samples/Random")]
    public class RandomConsideration : Consideration
    {
        [SerializeField] private float _min = 0f;
        [SerializeField] private float _max = 1f;
        
        protected override float OnEvaluate(Decision decision)
        {
            var roll = Random.Range(_min, _max);
            return EvaluateCurve(roll, _max);
        }
    }
}
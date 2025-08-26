using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sylpheed.UtilityAI.Considerations
{
    [CreateAssetMenu(menuName = "Utility AI/Consideration/Composite")]
    public sealed class CompositeConsideration : Consideration
    {
        [Header("Composite")]
        [SerializeField] private ConsiderationDecorator[] _considerations;

        public override bool ShouldCacheScore 
            => base.ShouldCacheScore && _considerations.All(consideration => consideration.ShouldCacheScore);

        public sealed override IEnumerable<IConsideration> Children => _considerations;

        protected override float OnEvaluate(Decision decision)
        {
            var totalScore = 1f;
            foreach (var consideration in _considerations)
            {
                var score = consideration.Evaluate(decision);
                totalScore *= score;
                
                // Update the cache manually. Decision cannot automatically cache nested considerations
                decision.UpdateScoreCache(consideration, score);
            }
            return totalScore;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class UtilityAgent : MonoBehaviour
    {
        [SerializeField] private List<Behavior> _behaviors = new();

        private Decision EvaluateDecision(IReadOnlyList<Decision> decisions)
        {
            if (!decisions.Any()) return null;
            
            // Evaluate all decisions
            var bestDecision = decisions.First();
            var bestScore = 0f;
            foreach (var decision in decisions)
            {
                // Ignore decision that can no longer reach the current highest score
                if (decision.MaxScore < bestScore) continue;
                
                // Get score for this decision
                var score = decision.Evaluate(bestScore);
                
                // Decision beats current best decision. Update best decision.
                if (score > bestScore)
                {
                    bestDecision = decision;
                    bestScore = score;
                }
            }
            
            return bestDecision;
        }
    }
}
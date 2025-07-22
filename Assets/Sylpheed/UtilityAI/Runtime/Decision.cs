using JetBrains.Annotations;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class Decision
    {
        public Behavior Behavior { get; private set; }
        
        public UtilityAgent Agent { get; private set; }
        public GameObject Target { get; private set; }
        
        public float Score { get; private set; }
        public float MaxScore => Behavior.Weight;
        
        /// <summary>
        /// Evaluates all considerations for this behavior against a specific target (if applicable)
        /// </summary>
        /// <param name="scoreThreshold">Stops evaluating remaining considerations if this decision can no longer score higher than the threshold.</param>
        /// <returns>Score. Result is cached.</returns>
        public float Evaluate(float scoreThreshold = 0)
        {
            // Highest possible score
            var finalScore = Behavior.Weight;
            
            // Evaluate each consideration
            foreach (var consideration in Behavior.Considerations)
            {
                if (finalScore < scoreThreshold) break;
                var score =  consideration.Evaluate(this);
            }
            
            Score = finalScore;
            return finalScore;
        }
    }
}
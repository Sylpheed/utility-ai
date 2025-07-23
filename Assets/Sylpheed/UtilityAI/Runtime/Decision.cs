using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class Decision
    {
        public Behavior Behavior { get; private set; }
        
        public UtilityAgent Agent { get; private set; }
        public UtilityTarget Target { get; private set; }
        
        public float Score { get; private set; }
        public float MaxScore => Behavior.Weight;
        
        /// <summary>
        /// Evaluates all considerations for this behavior against a specific target (if applicable)
        /// </summary>
        /// <param name="scoreThreshold">Stops evaluating remaining considerations if this decision can no longer score higher than the threshold.</param>
        /// <returns>Score. Result is cached.</returns>
        public float Evaluate(float scoreThreshold = 0)
        {
            // Evaluate each consideration
            var finalScore = 1f;
            foreach (var consideration in Behavior.Considerations)
            {
                // Stop evaluating if this decision can no longer beat the score threshold
                if (finalScore * Behavior.Weight < scoreThreshold) break;
                
                // Stop evaluating if this decision is already vetoed by a consideration that scored 0.
                if (Mathf.Approximately(finalScore, 0)) break;
                
                var score =  consideration.Evaluate(this);
                finalScore *= score;
            }
            
            // Apply compensation factor based on number of considerations
            if (finalScore > 0) finalScore = Mathf.Pow(finalScore, 1f / Behavior.Considerations.Count);
            
            // Apply weight
            finalScore *= Behavior.Weight;
            
            Score = finalScore;
            return finalScore;
        }
        
        public class Builder
        {
            private readonly Decision _decision;

            public Builder(UtilityAgent agent, Behavior behavior)
            {
                _decision = new Decision
                {
                    Agent = agent,
                    Behavior = behavior
                };
            }
            
            public Decision Build() => _decision;

            public Builder WithTarget(UtilityTarget target)
            {
                _decision.Target = target;
                return this;
            }
        }
    }
}
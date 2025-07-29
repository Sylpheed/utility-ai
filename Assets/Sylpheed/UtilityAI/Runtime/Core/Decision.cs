using System.Collections.Generic;
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
        
        private object _data;
        public T Data<T>() where T : class => _data as T;
        public bool TryGetData<T>(out T data) where T : class => (data = _data as T) != null;

        /// <summary>
        /// Evaluates all considerations for this behavior against a specific target (if applicable)
        /// </summary>
        /// <param name="scoreThreshold">Stops evaluating remaining considerations if this decision can no longer score higher than the threshold.</param>
        /// /// <param name="scoreCache">Cache of score based on a permutation of agent, target, consideration, and data. If cached, skip evaluation and use cache.</param>
        /// <returns>Score. Result is cached.</returns>
        public float Evaluate(float scoreThreshold, IDictionary<int, float> scoreCache)
        {
            // Evaluate each consideration
            var finalScore = 1f;
            for (var i = 0; i < Behavior.Considerations.Count; i++)
            {
                var consideration = Behavior.Considerations[i];
                
                // Stop evaluating if this decision is already vetoed by a consideration that scored 0.
                if (Mathf.Approximately(finalScore, 0)) break;
                
                // Stop evaluating if this decision can no longer beat the score threshold
                var projectedMaxScore = Mathf.Pow(finalScore, 1f / (i + 1)) * Behavior.Weight;
                if (projectedMaxScore < scoreThreshold) break;
                
                // Skip evaluation if score is already cached. Else, evaluate
                var hash = BuildConsiderationHash(consideration);
                if (!scoreCache.TryGetValue(hash, out var score))
                {
                    score = consideration.Evaluate(this);
                    scoreCache[hash] = score;
                }
                
                finalScore *= score;
            }
            
            // Apply compensation factor based on number of considerations
            if (finalScore > 0) finalScore = Mathf.Pow(finalScore, 1f / Behavior.Considerations.Count);
            
            // Apply weight
            finalScore *= Behavior.Weight;
            
            Score = finalScore;
            return finalScore;
        }

        private int BuildConsiderationHash(Consideration consideration)
        {
            var hash = 17;
            hash = hash * 23 + (Agent?.GetHashCode() ?? 0);
            hash = hash * 23 + (Target?.GetHashCode() ?? 0);
            hash = hash * 23 + (_data?.GetHashCode() ?? 0);
            hash = hash * 23 + (consideration?.GetHashCode() ?? 0);
            return hash;
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
            
            public Builder WithData<T>(T data) 
                where T : class
            {
                _decision._data =  data;
                return this;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class UtilityAgent : MonoBehaviour
    {
        [SerializeField] private BehaviorSet _baseBehaviorSet;

        private List<BehaviorSet> _behaviorSets = new();
        private List<Behavior> _behaviors = new();

        private void Awake()
        {
            // Add base behavior
            AddBehaviors(_baseBehaviorSet);
        }

        #region Add/Remove Behaviors
        public void AddBehaviors(BehaviorSet behaviorSet)
        {
            _behaviorSets.Add(behaviorSet);
            _behaviors.AddRange(behaviorSet.Behaviors);
        }

        public void AddBehaviors(params Behavior[] behaviors)
        {
            _behaviors.AddRange(behaviors);
        }

        public void RemoveBehaviors(BehaviorSet behaviorSet)
        {
            _behaviorSets.Remove(behaviorSet);
            foreach (var behavior in behaviorSet.Behaviors)
                _behaviors.Remove(behavior);
        }
        
        public void RemoveBehaviors(params Behavior[] behaviors)
        {
            foreach (var behavior in behaviors)
                _behaviors.Remove(behavior);
        }
        #endregion
        
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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class UtilityAgent : MonoBehaviour
    {
        [SerializeField] private BehaviorSet _baseBehaviorSet;
        [SerializeField] private float _targetSearchRadius = 100f;
        [SerializeField] private float _maxTargetsPerDecision = 12;
        [SerializeField] private float _decisionInterval = 1f;
        
        [Header("Debug")]
        [SerializeField] private bool _logToConsole;

        public IReadOnlyList<UtilityTarget> Targets { get; private set; } = new List<UtilityTarget>();
        public Decision CurrentDecision { get; private set; }
        
        private List<BehaviorSet> _behaviorSets = new();
        private List<Behavior> _behaviors = new();
        private readonly RaycastHit[] _searchHits = new RaycastHit[30];
        private float _decisionTimer;

        private void Awake()
        {
            // Add base behavior
            AddBehaviors(_baseBehaviorSet);
        }

        private void Update()
        {
            if (_decisionTimer >= _decisionInterval)
            {
                var decisions = BuildDecisions();
                var decision = Decide(decisions);
                EnactDecision(decision);
                _decisionTimer = 0;
            }
            else
            {
                _decisionTimer += Time.deltaTime;
            }
        }

        private void EnactDecision(Decision decision)
        {
            if (decision == null)
            {
                Debug.Log("No decision evaluated.");
                return;
            }
            
            // Invoke a new action if decision changed based on behavior and target
            if (decision.Behavior != CurrentDecision?.Behavior && 
                decision.Target != CurrentDecision?.Target)
            {
                decision.Behavior.Action?.Execute(this, decision.Target);
            }
            
            CurrentDecision = decision;
            
            Debug.Log($"[Decision] {decision.Behavior.name} enacted. Score: {decision.Score:P2}");
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
        
        private Decision Decide(IReadOnlyList<Decision> decisions)
        {
            if (!decisions.Any()) return null;
            
            // Evaluate all decisions
            Decision bestDecision = null;
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

        private IReadOnlyList<Decision> BuildDecisions()
        {
            // Get all targets
            var allTargets = SearchTargets();
            
            // Build decisions from behaviors
            var decisions = new List<Decision>();
            foreach (var behavior in _behaviors)
            {
                if (behavior.RequiresTarget)
                {
                    // Create decision for each valid target with the same tags
                    var targetDecisions = allTargets
                        .Where(target => target.HasTags(behavior.RequiredTargetTags))
                        .Select(target =>
                            new Decision.Builder(this, behavior)
                                .WithTarget(target)
                                .Build()
                        )
                        .ToList();
                    decisions.AddRange(targetDecisions);
                }
                else
                {
                    // Create decision without a target
                    var decision = new Decision.Builder(this, behavior).Build();
                    decisions.Add(decision);
                }
            }
            
            return decisions;
        }

        private IReadOnlyList<UtilityTarget> SearchTargets()
        {
            // Get all utility targets within search radius and sort them by distance
            var size = Physics.SphereCastNonAlloc(transform.position, _targetSearchRadius, Vector3.down, _searchHits, _targetSearchRadius);
            var targets = _searchHits
                .Take(size)
                .Select(hit => hit.collider.GetComponentInParent<UtilityTarget>())
                .Where(target => target)
                .OrderBy(target => target.DistanceFromAgent(this))
                .ToList();

            Targets = targets;
            return targets;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.darkCyan;
            Gizmos.DrawWireSphere(transform.position, _targetSearchRadius);
        }
    }
}
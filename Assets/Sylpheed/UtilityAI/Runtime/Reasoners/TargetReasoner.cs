using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sylpheed.UtilityAI.Reasoners
{
    [System.Serializable]
    public class TargetReasoner : Reasoner
    {
        public override IReadOnlyCollection<Decision> BuildDecisions(UtilityAgent agent, Behavior behavior, IReadOnlyList<UtilityTarget> targets)
        {
            // Ignore non-targeted behavior
            if (!behavior.RequiresTarget)
            {
                Debug.LogWarning($"{nameof(TargetReasoner)} requires {nameof(Behavior)}.{nameof(Behavior.RequiresTarget)} to be true.");
                return Array.Empty<Decision>();
            }
            
            // Create decision for each valid target with the same tags
            return targets
                .Where(target => target.HasTags(behavior.RequiredTargetTags))
                .Select(target =>
                    new Decision.Builder(agent, behavior)
                        .WithTarget(target)
                        .Build()
                )
                .ToList();
        }
    }
}
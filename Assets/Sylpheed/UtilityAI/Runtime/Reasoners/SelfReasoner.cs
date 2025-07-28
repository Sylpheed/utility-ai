using System.Collections.Generic;

namespace Sylpheed.UtilityAI.Reasoners
{
    [System.Serializable]
    public class SelfReasoner : Reasoner
    {
        public override IReadOnlyCollection<Decision> BuildDecisions(UtilityAgent agent, Behavior behavior, IReadOnlyList<UtilityTarget> targets)
        {
            // Create decision without a target
            var decision = new Decision.Builder(agent, behavior).Build();
            return new[] { decision };
        }
    }
}
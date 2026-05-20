using System;
using System.Collections.Generic;
using System.Linq;

namespace Sylpheed.UtilityAI
{
    [Serializable]
    public abstract class Reasoner
    {
        public abstract IReadOnlyCollection<Decision> BuildDecisions(UtilityAgent agent, Behavior behavior,
            IReadOnlyList<UtilityTarget> targets);
    }
}
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class Decision
    {
        public Behavior Behavior { get; private set; }
        
        public UtilityAgent Agent { get; private set; }
        public float Score { get; private set; }

        public float Evaluate(GameObject target, float scoreThreshold = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
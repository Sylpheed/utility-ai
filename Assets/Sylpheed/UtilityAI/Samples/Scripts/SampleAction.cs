using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    [System.Serializable]
    public class SampleAction : IAction
    {
        [SerializeField] private float _damage = 1f;
        
        public void Execute(UtilityAgent actor, GameObject target = null)
        {
            
        }
    }
}
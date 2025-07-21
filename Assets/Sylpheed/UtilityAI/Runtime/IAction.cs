using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public interface IAction
    {
        void Execute(UtilityAgent actor, GameObject target = null);
    }
}
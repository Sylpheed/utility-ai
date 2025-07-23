using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public interface IAction
    {
        void Execute(UtilityAgent actor, UtilityTarget target = null);
    }
}
namespace Sylpheed.UtilityAI.Considerations
{
    public abstract class BoolConsideration : Consideration
    {
        /// <summary>
        /// Return true to opt-in. False to opt-out.
        /// </summary>
        /// <param name="decision"></param>
        /// <returns></returns>
        protected abstract bool OnEvaluateAsBool(Decision decision);

        protected sealed override float OnEvaluate(Decision decision) 
            => OnEvaluateAsBool(decision) ? 1f : 0f;
    }
    
    public abstract class BoolConsideration<TData> : BoolConsideration
    {
        protected abstract bool OnEvaluateAsBool(Decision decision, TData data);

        protected sealed override bool OnEvaluateAsBool(Decision decision)
        {
            if (decision.Data is not TData data) throw new System.Exception("Failed to cast decision data.");
            return OnEvaluateAsBool(decision, data);
        }
    }
}
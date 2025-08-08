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
}
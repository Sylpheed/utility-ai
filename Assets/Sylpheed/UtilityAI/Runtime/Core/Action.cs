using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public abstract class Action
    {
        public UtilityAgent Agent { get; private set; }
        public UtilityTarget Target { get; private set; }

        #region Overridables

        /// <summary>
        /// Called when this action has been executed. Return true to validate the action and continue execution.
        /// </summary>
        protected virtual bool OnEnter() => true;
        /// <summary>
        /// Called every frame while the action is being executed.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnUpdate(float deltaTime) { }
        /// <summary>
        /// Called every fixed update while the action is being executed.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnFixedUpdate(float deltaTime) { }
        /// <summary>
        /// Called when the action has been exited or interrupted. Use this for cleanup.
        /// </summary>
        protected virtual void OnExit() { }
        /// <summary>
        /// Use this if you want to have a separate function to check if the action should still execute.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldExit() { return false; }
        internal virtual void ExtractData(object data) { }
        #endregion

        private System.Action _onExit;
        private bool _executed;
        
        public void Execute(UtilityAgent agent, UtilityTarget target = null, object data = null, System.Action onExit = null)
        {
            if (_executed) throw new System.Exception("Action is already executed");
            _executed = true;
            
            Agent = agent;
            Target = target;
            _onExit = onExit;
            ExtractData(data);
            
            // Exit immediately if OnEnter failed
            if (!OnEnter())
            {
                OnExit();
            }
        }

        public void Interrupt()
        {
            OnExit();
        }
        
        public void Update(float deltaTime)
        {
            var shouldExit = ShouldExit();
            if (!shouldExit) OnUpdate(deltaTime);
            else Exit();
        }
        
        public void FixedUpdate(float deltaTime)
        {
            OnFixedUpdate(deltaTime);
        }
        
        /// <summary>
        /// Call this whenever the action is concluded either successfully or prematurely. This will force the agent to come up with a new decision.
        /// </summary>
        protected void Exit()
        {
            OnExit();
            _onExit?.Invoke();
        }
    }

    /// <summary>
    /// Action with reference to additional data.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class Action<TData> : Action
    {
        public TData Data { get; private set; }

        internal sealed override void ExtractData(object data)
        {
            if (data is TData casted) Data = casted;
            else throw new System.Exception("Failed to cast decision data.");
        }
    }
}
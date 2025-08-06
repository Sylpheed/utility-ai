using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public abstract class Action
    {
        public Decision Decision { get; private set; }
        public UtilityAgent Agent => Decision.Agent;
        public UtilityTarget Target => Decision.Target;
        public T Data<T>() where T : class => Decision.Data<T>();
        public bool TryGetData<T>(out T data) where T : class => (data = Decision.Data<T>()) != null;

        #region Overridables
        /// <summary>
        /// Called when this action has been executed.
        /// </summary>
        protected virtual void OnEnter() { }
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
        #endregion

        private System.Action _onExit;
        private bool _executed;
        
        public void Execute(Decision decision, System.Action onExit = null)
        {
            if (_executed) throw new System.Exception("Action is already executed");
            _executed = true;
            
            Decision = decision;
            _onExit = onExit;
            OnEnter();
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
}
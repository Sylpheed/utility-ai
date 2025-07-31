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
        protected virtual void OnEnter() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnFixedUpdate(float deltaTime) { }
        protected virtual void OnExit() { }
        #endregion

        private System.Action _onComplete;
        
        public void Execute(Decision decision, System.Action onComplete = null)
        {
            Decision = decision;
            _onComplete = onComplete;
            OnEnter();
        }

        public void Stop()
        {
            OnExit();
        }
        
        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }
        
        public void FixedUpdate(float deltaTime)
        {
            OnFixedUpdate(deltaTime);
        }
        
        /// <summary>
        /// Call this whenever the action is complete. This will force the agent to come up with a new decision.
        /// </summary>
        protected void Complete()
        {
            OnExit();
            _onComplete?.Invoke();
        }
    }
}
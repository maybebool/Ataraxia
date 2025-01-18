using UnityEngine;

namespace FiniteStateMachine.States {
    public class RunningState : BaseState{
        public override void OnEnter() {
            Time.timeScale = 1f;
            Debug.Log("Entering Running State");
        }
    }
}
using UnityEngine;

namespace StateMachines.States {
    public class RunningState : BaseState{
        public override void OnEnter() {
            // Ensure normal game speed
            Time.timeScale = 1f;
        
            // Hide any pause UI if needed
            Debug.Log("Entering Running State");
        }
    }
}
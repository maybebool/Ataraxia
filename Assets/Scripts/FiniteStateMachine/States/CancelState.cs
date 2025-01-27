using FiniteStateMachine.Structures;
using GameUI;
using SceneHandling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FiniteStateMachine.States {
    public class CancelState : BaseState {
        public override void OnEnter() {
            Debug.Log("Entering Cancel (Quit) State -> Loading Main Menu");
            SceneLoader.Instance.LoadNewScene(SceneNames.MainMenu);
        }
    }
}
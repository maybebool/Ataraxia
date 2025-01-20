using StateMachines;

namespace StateMachine.Interfaces {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}
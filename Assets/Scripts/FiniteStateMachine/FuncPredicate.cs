using System;
using StateMachines;

namespace FiniteStateMachine {
    public class FuncPredicate : IPredicate {
        readonly Func<bool> _func;
        
        public FuncPredicate(Func<bool> func) {
            _func = func;
        }
        
        public bool Evaluate() => _func.Invoke();
    }
}
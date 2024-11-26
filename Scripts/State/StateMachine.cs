using System;
using System.Collections.Generic;

namespace Miku.State
{
    // 출처 : https://www.youtube.com/watch?v=NnH6ZK5jt7o

    /// <summary>
    /// predicate 패턴 (트랜지션) + state 패턴과 nullable를 적극 활용한 better statemachine
    /// </summary>
    public class StateMachine
    {
        // 현 상태
        StateNode current;
        // 모든 상태
        Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();
        // 상태와 관계없이 언제든 전환될 수 있는 트랜지션들을 저장한다. 이를 ANY 트랜지션이라고 한다.
        HashSet<ITransition> anyTransitions = new HashSet<ITransition>();

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }

            current.State?.Update();
        }

        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }
    
        void ChangeState(IState newState)
        {
            if (newState == current.State) return;

            IState previousState = current.State;
            IState nextState = nodes[newState.GetType()].State;

            previousState?.OnExit();   
            nextState?.OnEnter();
            current = nodes[newState.GetType()];
        }

        ITransition GetTransition()
        {
            // ANY 트랜지션 중에 yes를 판단할 것 같은 트랜지션을 반환
            foreach (ITransition transition in anyTransitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }
            // 현 상태의 모든 트랜지션 중에 yes를 판단할 것 같은 트랜지션을 반환
            foreach (ITransition transition in current.Transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        /// <summary>
        /// node를 구해보고 없다면 노드를 딕셔너리에 더해서라도 구한다.
        /// </summary>
        /// <param name="state">요청할 상태</param>
        /// <returns>요청한 상태</returns>
        StateNode GetOrAddNode(IState state)
        {
            //nullable 대비
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }



        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}

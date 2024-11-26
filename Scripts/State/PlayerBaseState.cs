using Miku.Player;
using UnityEngine;

namespace Miku.State
{
    public abstract class PlayerBaseState : IState
    {
        protected readonly PlayerController _player;
        protected readonly Animator _animator;

        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");

        protected const float crossFadeDuration = 0.1f;

        protected PlayerBaseState(PlayerController player, Animator animator)
        {
            this._player = player;
            this._animator = animator;
        }

        public virtual void FixedUpdate()
        {
            //noop
        }

        public virtual void OnEnter()
        {
            //noop
        }

        public virtual void OnExit()
        {
            //noop
        }

        public virtual void Update()
        {
            //noop
        }
    }
}

using Miku.Player;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

namespace Miku.State.PlayerOwnStates
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController player;
        protected readonly Animator animator;
        protected readonly int stateEnum;

        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        protected static readonly int FallingHash = Animator.StringToHash("Falling");
        protected static readonly int SpeedHash = Animator.StringToHash("Speed");
        protected static readonly int MenuEmotion1Hash = Animator.StringToHash("MenuEmotion1");

        protected const float crossFadeDuration = 0.05f;

        protected BaseState(PlayerController player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
        }

        //감정표현 전용으로 하나 만들어봄, 어떤 메뉴 or 버튼이 눌렸는지 판단
        protected BaseState(PlayerController player, Animator animator, int stateEnum)
        {
            this.player = player;
            this.animator = animator;
            this.stateEnum = stateEnum;
        }

        public virtual void OnEnter()
        {
            // noop
        }

        public virtual void Update()
        {
            // noop
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnExit()
        {
            // noop
        }
    }

    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter()
        {
            //Debug.Log("[Locomotion State] 진입");
            animator.CrossFade(LocomotionHash, crossFadeDuration);
            
        }
        public override void Update()
        {
            
            float speed = player.HandleMovement();
            animator.SetFloat(SpeedHash, speed);
            // 튀어오름 방지
            if (player.VerticalVelocity < 0.0f)
            {
                player.VerticalVelocity = -2f;
            }
        }
        
    }

    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base (player, animator) { }
        public override void OnEnter()
        {
            //Debug.Log("[Jump State] 진입");
            animator.Play(JumpHash);
        }

    }
    public class FallingState : BaseState
    {
        public FallingState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter()
        {
            //Debug.Log("[Falling State] 진입");
            animator.CrossFade(FallingHash, crossFadeDuration);
        }
        public override void Update()
        {
            // 최대 속력 이하로 더 빠르게 낙하
            if (player.VerticalVelocity < player.TeminalVecticalVelocity)
            {
                player.VerticalVelocity += player.Gravity * Time.deltaTime;
            }
        }
    }

    public class EmotionState : BaseState
    {
        public EmotionState(PlayerController player, Animator animator, int stateEnum) : base(player, animator, stateEnum) { }
        public override void OnEnter()
        {
            Managers.Instance.Game.Player.PlayerController.SetSpeedZero();
            Debug.Log(stateEnum + "온스테이트 상태 enum 확인");
            if(stateEnum == 1)
            {
                animator.Play(MenuEmotion1Hash);
                Debug.Log("점프 애니메이터 실행");
            }         
        }
        public override void Update()
        {
            if (Managers.Instance.Game.Player.PlayerController.GetSpeed() != 0 || !player.Grounded)
            {
                player.IsExpressEmotion = false;
            }
        }
    }
}

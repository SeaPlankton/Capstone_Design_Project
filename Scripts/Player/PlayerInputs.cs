using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Miku.Player
{
    /// <summary>
    /// 인풋 이벤트 리스너 클래스
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputs : MonoBehaviour
    {

        // Character Input Events
        public event Action<Vector2> Move;
        //public event UnityAction<Vector2> Look = delegate { };
        public event Action<bool> Jump;

        [Header("Character Input Values")]
        public Vector2 MoveValue;
        public Vector2 LookValue;
        public bool JumpValue;
        public bool SprintValue;

        [Header("Mouse Cursor Settings")]
        public bool CursorLocked = true;
        public bool CursorInputForLook = true;

        private PlayerInput _playerInput;

        public Vector2 MLookValue;
        public Text text;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }


        public void EnablePlayerActions() => _playerInput.ActivateInput();

        public void DisablePlayerActions() => _playerInput.DeactivateInput();

        public void StartTouchPrimary(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:                  
                    Debug.Log("터치컨택트 ");
                    text.text = "터치컨택트 ";
                    break;
                case InputActionPhase.Canceled:
                    Debug.Log("터치캔슬? ");
                    break;
            }
            //float value = context.ReadValue<float>();
            //Debug.Log("터치컨택트 ");
            //text.text = value.ToString();
        } 

        public void OnTouchPosition(InputAction.CallbackContext context)
        {
            
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
            Move?.Invoke(context.ReadValue<Vector2>());  
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (CursorInputForLook)
            {
                LookInput(context.ReadValue<Vector2>());
                //Look.Invoke(value.Get<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    JumpInput(true);
                    Jump?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    JumpInput(false);
                    break;
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    SprintInput(true);
                    Debug.Log("스프린트 눌림");
                    break;
                case InputActionPhase.Canceled:
                    SprintInput(false);
                    break;
            }
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            MoveValue = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            LookValue = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            JumpValue = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            SprintValue = newSprintState;
        }
        // TODO : 마우스 조정을 어떻게 이벤트로 관리할 것인가?
/*
#if (UNITY_STANDALONE_WIN)
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(CursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
#endif     
   */     

        public void DoJump() => Jump?.Invoke(true);
    }
}

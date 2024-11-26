using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    // Character Input Events
    public event Action<Vector2> Move;
    //public event UnityAction<Vector2> Look = delegate { };
    public event Action<bool> Jump;
    public event Action<bool> TargetBoss;
    public event Action<bool> SpawnBoss;

    [Header("Character Input Values")]
    public Vector2 MoveValue;
    public Vector2 LookValue;
    public float CameraSensitivity = 1f;
    public bool JumpValue;
    public float MouseScrollYValue;

    [HideInInspector]
    public bool TouchCameraLock = false;
    [HideInInspector]
    public Vector2 TouchCameraPosition;

    [Header("Mouse Cursor Settings")]
    public bool CursorLocked = true;
    public bool CursorInputForLook = true;

    [HideInInspector]
    public PlayerControls playerControls;
    private Camera mainCamera;

    //ESC에 쿨다움 적용 위함
    private bool _isESCCool = true;
    private bool _isLockKeyboard = false;
    private void Awake()
    {
        SetCursorState();
        Managers.Instance.Game.OnGridSceneLoaded += Init;
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Init()
    {
        Managers.Instance.Game.InputManager = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    private void Start()
    {
#if (UNITY_STANDALONE_WIN) || (UNITY_EDITOR)
        playerControls.PC.Move.performed += ctx => OnMove(ctx);
        playerControls.PC.Look.started += ctx => OnLook(ctx);
        playerControls.PC.Jump.performed += ctx => OnJump(ctx);
        playerControls.PC.CursurLock.performed += ctx => OnCursurLock(ctx);
        playerControls.PC.MouseScrollY.performed += ctx => OnMouseScrollY(ctx);
        playerControls.PC.ExpressEmotion.performed += ctx => OnExpressEmotion(ctx);
        playerControls.PC.TargetBoss.performed += ctx => OnTargetBoss(ctx);
        playerControls.PC.SpawnBoss.performed += ctx => OnSpawnBoss(ctx);

        playerControls.PC.Move.canceled += ctx => OnEndMove(ctx);
        playerControls.PC.Look.canceled += ctx => OnEndLook(ctx);
        playerControls.PC.Jump.canceled += ctx => OnEndJump(ctx);

        CameraSensitivity = 1f;
#endif
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
        if (Managers.Instance.Game.UIController.IsMenuPanelOn == false) //메뉴가 꺼진 경우에만 할당- > 게임 상태이므로 감정표현중일때 움직이면 locomation상태로 감
        {
            Managers.Instance.Game.Player.PlayerController.IsExpressEmotion = false; //감정표현 상태 변경하려고 추가 
        }


        if (!Managers.Instance.Game.TimeController.TimeStop)
        {
            MoveInput(context.ReadValue<Vector2>());
            Move?.Invoke(context.ReadValue<Vector2>());
        }      
    }

    public void OnEndMove(InputAction.CallbackContext context)
    {
        MoveValue = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
#if (UNITY_STANDALONE_WIN) || (UNITY_EDITOR)
        if (!Managers.Instance.Game.TimeController.TimeStop)
        {
            LookInput(context.ReadValue<Vector2>());
        }
#endif
    }

    public void OnEndLook(InputAction.CallbackContext context)
    {      
        LookInput(context.ReadValue<Vector2>());    
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
        if (Managers.Instance.Game.UIController.GetCurrentUIState() == UIController.UIStates.None)
        {
            JumpInput(true);
            Jump?.Invoke(true);
        }        
    }

    public void OnEndJump(InputAction.CallbackContext context)
    {
        JumpInput(false);
    }

    public void OnTargetBoss(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
        TargetBoss?.Invoke(true);
    }
    public void OnSpawnBoss(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
        SpawnBoss?.Invoke(true);
    }

    public void OnCursurLock(InputAction.CallbackContext context)
    {
        if (_isESCCool)
        {
            Managers.Instance.Game.UIController.UpdateCurrentUI();
            Invoke("CheckESCTime", 0.4f);
        }
        _isESCCool = false;
    }

    //ESC 쿨타임 시간 체크
    private void CheckESCTime()
    {
        _isESCCool = true;
    }

    public void OnMouseScrollY(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
        MouseScrollYValue = context.ReadValue<float>() / 600f * -1f;
        Managers.Instance.Game.CinemachineController.ScrollMainCamera(MouseScrollYValue);
    }

    public void OnExpressEmotion(InputAction.CallbackContext context)
    {
        if (_isLockKeyboard) return;
        if (Managers.Instance.Game.UIController.GetCurrentUIState() == UIController.UIStates.None)
        {
            ExpressEmotionInput(true, int.Parse(context.control.name));
        }
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        MoveValue = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        LookValue = newLookDirection * CameraSensitivity;
        //LookValue = new Vector2(newLookDirection.x + CameraSensitivity, newLookDirection.y + CameraSensitivity);
    }

    public void ChangeSensitivity(float sensitivity)
    {
        CameraSensitivity = sensitivity * 2;
    }

    public void JumpInput(bool newJumpState)
    {
        JumpValue = newJumpState;
    }

    public void ExpressEmotionInput(bool newExpressEmotionState, int keyNum)
    {
        Debug.Log("전달받은 context :   " + keyNum);

        //할당안된 keyNum입력 되면 무시하려고 변수 선언됨 by. 최성훈
        int inputEmotionNum = Managers.Instance.Game.EmotionPresenter.GetInputEmotionNumValue(keyNum);
        
        if (Managers.Instance.Game.Player.PlayerController.Grounded && inputEmotionNum != -1)
        {
            Managers.Instance.Game.Player.PlayerController.IsExpressEmotion = newExpressEmotionState;
            Managers.Instance.Game.Player.PlayerController.PlayExpressEmotion(keyNum);
        }

    }

    public void LockKeyBoard()
    {
        _isLockKeyboard = true;
        JumpValue = false;
        MoveValue = Vector2.zero;
    }

    public void UnlockKeyBoard()
    {
        _isLockKeyboard = false;
    }

    #region Utilities
    private Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }
    #endregion

    // TODO : 마우스 조정을 어떻게 이벤트로 관리할 것인가?
    
#if (UNITY_STANDALONE_WIN) || (UNITY_EDITOR)
    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus ? CursorLockMode.Confined : CursorLockMode.None;
        SetCursorState();
    }

    public void SetCursorState()
    {
        //Cursor.lockState = newState ? CursorLockMode.locked : CursorLockMode.None;
        if (Managers.Instance.Game.UIController.GetCurrentUIState() == UIController.UIStates.None)
        {
            Cursor.visible = false;
        } else
        {
            Cursor.visible = true;
        }
    }
            
#endif
    
}

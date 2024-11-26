using Miku.Utils;
using Miku.State;
using System.Collections.Generic;
using UnityEngine;
using Timer = Miku.Utils.Timer;
using Miku.State.PlayerOwnStates;


[RequireComponent(typeof(CharacterController), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Player Player;

    [Header("Reference")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _mainCamera;
    //private PlayerInputs _inputs;
    private CharacterController _controller;

    [Space(10)]
    [Header("Movement Settings")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public bool IsPossibleMoving = true;
    public bool IsShootBoss = false;

    [Space(10)]
    [Header("Jump Settings")]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The Duration the jump animation play")]
    public float JumpDuration = 0.5f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    // TODO : 땅 판정 부분은 따로 분리할 지 말지 정해야 함.
    [Space(10)]
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.07f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Space(10)]
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;


    [Space(10)]
    public bool IsExpressEmotion = false;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private Vector3 _targetDirection;
    [Space(10)]
    [Header("Debug")]
    // 플레이어 수직 속력
    public float VerticalVelocity;
    // 플레이어 수직 제한 속력
    public float TeminalVecticalVelocity = 53.0f;

    // position
    //public Vector3 LastFixedPosition;
    //public Vector3 DesiredFixedPosition;

    // Timer
    List<Timer> timers;
    CountdownTimer jumpTimer;
    CountdownTimer jumpCooldownTimer;

    // statemachine
    StateMachine stateMachine;


    private const float _threshold = 0.01f;
    private const bool IsCurrentDeviceMouse = true;

    private void Awake()
    {
        // reference
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        Player = GetComponent<Player>();
        _controller = GetComponent<CharacterController>();
        //_inputs = GetComponent<PlayerInputs>();

        // setup
        SetupStateMachine();
        SetupTimers();
    }
    private void Start()
    {
        Managers.Instance.Game.InputManager.Jump += OnJump;
        Managers.Instance.Game.InputManager.TargetBoss += OnTargetBoss;
    }
    private void OnEnable()
    {

    }

    private void Update()
    {
        GroundedCheck();
        stateMachine.Update();
        HandleTimers();
        _controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime) +
                        new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    // TODO : https://velog.io/@nagi0101/Unity-%EC%99%84%EB%B2%BD%ED%95%9C-CharacterController-%EA%B8%B0%EB%B0%98-Player%EB%A5%BC-%EC%9C%84%ED%95%B4
    // FixedUpdate()는 여전히 오브젝트의 새로운 위치를 계산한다.
    // 다른 점이 있다면, 계산한 위치를 적용하는 것은 Update()가 맡는다는 것이다.
    // Update() 함수는 마지막 FixedUpdate()가 실행된 뒤로 경과한 시간에 따라,
    // 이전 위치값과 새로운 위치값을 보간하여 오브젝트를 이동시킨다.
    // 이런 방법을 통해 일관된 물리 계산을 기기 사양에 따라 부드러운 프레임으로 보여줄 수 있다.
    private void LateUpdate()
    {
        CameraRotation();
    }



    private void SetupStateMachine()
    {
        // #1. Instantiate State Machine
        stateMachine = new StateMachine();

        // #2. Declare States
        var locomotionState = new LocomotionState(this, _animator);
        var jumpState = new JumpState(this, _animator);
        var fallingState = new FallingState(this, _animator);

        var emotionState = new EmotionState(this, _animator, 0);

        // #3. Define transitions

        // 점프 애니메이션이 재생되었을 때
        At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
        Any(fallingState, new FuncPredicate(() => !jumpTimer.IsRunning && !Grounded));

        At(locomotionState, emotionState, new FuncPredicate(() => IsExpressEmotion == true && Grounded));
        Any(locomotionState, new FuncPredicate(() => !jumpTimer.IsRunning && Grounded && IsExpressEmotion == false));
        //Any(locomotionState, new FuncPredicate(() => !jumpTimer.IsRunning && Grounded)); << 감정표현 추가되면서 위에걸로 수정됨 by. 최성훈

        /*
        //메뉴1 버튼 눌렸을 때 
        At(locomotionState, emotionState, new FuncPredicate(() => Managers.Instance.Game.UIController.currentEmotionStates == UIController.UIEmotionStates.Menu1));
        //메인메뉴로 돌아갈 때
        Any(locomotionState, new FuncPredicate(() => Managers.Instance.Game.UIController.currentEmotionStates == UIController.UIEmotionStates.None));
        */

        // #4. Set Initial State
        stateMachine.SetState(locomotionState);
    }



    private void SetupTimers()
    {

        // Setup timers
        jumpTimer = new CountdownTimer(JumpDuration);
        jumpCooldownTimer = new CountdownTimer(JumpTimeout);
        // the square root of H * -2 * G = how much velocity needed to reach desired height
        jumpTimer.OnTimerStart +=
            () => VerticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();


        timers = new(2) { jumpTimer, jumpCooldownTimer };
    }

    private void HandleTimers()
    {
        foreach (var timer in timers)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    //뇌피셜 아닐수도 있음
    //현재 상태에서, 변경할 상태로, 특정 상태가 충족되었을 시 상태 변경
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    //변경할 상태로, 특정 상태가 충족되었을 시 상태 변경
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    public void OnJump(bool isJump)
    {
        if (isJump && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && Grounded)
        {
            jumpTimer.Start();
        }
        else if (!isJump && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }
    }

    public void OnTargetBoss(bool isTarget)
    {
        IsShootBoss = !IsShootBoss;
        if (!Managers.Instance.Game.Boss.gameObject.activeSelf)
        {
            Debug.Log("보스가 활성화되지 않음");
            IsShootBoss = false;
        }

    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Debug.DrawLine(
            start: new Vector3(spherePosition.x - GroundedRadius, spherePosition.y, spherePosition.z),
            end: new Vector3(spherePosition.x + GroundedRadius, spherePosition.y, spherePosition.z),
            color: Color.magenta);
        Debug.DrawLine(
            start: new Vector3(spherePosition.x, spherePosition.y - GroundedRadius, spherePosition.z),
            end: new Vector3(spherePosition.x, spherePosition.y + GroundedRadius, spherePosition.z),
            color: Color.magenta);
        // 트리거 무시
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }
    private void CameraRotation()
    {
        //print("LookValue : " + _inputs.LookValue);
        //print("LookValue.sqrMagnitude : " + _inputs.LookValue.sqrMagnitude);

        // if there is an input and camera position is not fixed
        if (Managers.Instance.Game.InputManager.LookValue.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += Managers.Instance.Game.InputManager.LookValue.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += Managers.Instance.Game.InputManager.LookValue.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    public float HandleMovement()
    {
        // #1. 목표 스피드
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (Managers.Instance.Game.InputManager.MoveValue == Vector2.zero) targetSpeed = 0.0f;

        // #2. 목표 스피드에 감속 or 가속 작업
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // #3. 전진 방향 설정
        // normalise input direction
        Vector3 inputDirection = new Vector3(Managers.Instance.Game.InputManager.MoveValue.x, 0.0f, Managers.Instance.Game.InputManager.MoveValue.y).normalized;
        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (Managers.Instance.Game.InputManager.MoveValue != Vector2.zero)
        {
            // 카메라 앵글 각도 + 인풋 각도
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // #4. 애니메이션 파라미터 값 설정 및 리턴
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        return _animationBlend;
    }


    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    //캐릭터의 이동속도 증가 함수
    public void PlayerSpeedUp(float percent)
    {
        Debug.Log("원래 이동속도 " + MoveSpeed);
        MoveSpeed = MoveSpeed + (MoveSpeed * (percent / 100f));
        Debug.Log("변경 이동속도 " + MoveSpeed);
    }

    //감정표현 애니메이션 재생
    public void PlayExpressEmotion(int num)
    {
        Managers.Instance.Game.EmotionPresenter.PlayExpressEmotion(num);
    }

    public void SetSpeedZero()
    {
        _speed = 0.0f;
    }
    public float GetSpeed()
    {
        return _speed;
    }
}


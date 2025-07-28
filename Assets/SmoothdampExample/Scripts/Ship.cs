using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    [Header("References")]
    public Transform VisualModel = null;
    public Transform CameraAnchor = null;
    public Transform ShipCam = null;

    [Header("Ship Stats")]
    public float MaxSpeed = 30;
    public float StrafeSpeed = 10;
    public float TurnRate = 180;
    public float FakeRoll = 30;
    public float CameraOffsetting = 0.1f;
    public float CameraLookahead = 0.1f;

    [Header("Smoothing Toggles")]
    public bool InvertPitch = false;
    public bool EnableVelocitySmoothing = false;
    public bool EnableTurningSmoothing = false;
    public bool EnableCamMovement = false;
    public bool EnableCamOffsetSmoothing = false;
    public bool EnableCamTurnSmoothing = false;

    [Header("Smoothing Speeds")]
    public float VelocitySmoothSpeed = 2;
    public float TurnSmoothSpeed = 6;
    public float CamOffsetSmoothSpeed = 6;
    public float CamLookaheadSmoothSpeed = 6;

    private InputAction _moveAction = null;
    private InputAction _stickAction = null;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _localAngularVelocity = Vector3.zero;
    private Vector3 _camOffset = Vector3.zero;
    private Vector3 _camLookahead = Vector3.zero;

    public UnityEvent<Ship> OnSmoothingUpdated = null;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _stickAction = InputSystem.actions.FindAction("Stick");

        InputSystem.actions.FindAction("SmoothingVelocity").performed += ToggleVelocitySmoothing;
        InputSystem.actions.FindAction("SmoothingTurning").performed += ToggleTurningSmoothing;
        InputSystem.actions.FindAction("InvertPitch").performed += ToggleInvertPitch;
        InputSystem.actions.FindAction("EnableCamMovement").performed += ToggleCamMovement;
        InputSystem.actions.FindAction("SmoothingCamOffset").performed += ToggleCamOffsetSmoothing;
        InputSystem.actions.FindAction("SmoothingCamRotation").performed += ToggleCamRotationSmoothing;
    }

    private void ToggleCamMovement(InputAction.CallbackContext context)
    {
        EnableCamMovement = !EnableCamMovement;
        OnSmoothingUpdated?.Invoke(this);
    }

    private void ToggleCamRotationSmoothing(InputAction.CallbackContext context)
    {
        EnableCamTurnSmoothing = !EnableCamTurnSmoothing;
        OnSmoothingUpdated?.Invoke(this);
    }

    private void ToggleCamOffsetSmoothing(InputAction.CallbackContext context)
    {
        EnableCamOffsetSmoothing = !EnableCamOffsetSmoothing;
        OnSmoothingUpdated?.Invoke(this);
    }

    private void ToggleInvertPitch(InputAction.CallbackContext context)
    {
        InvertPitch = !InvertPitch;
        OnSmoothingUpdated?.Invoke(this);
    }

    private void ToggleTurningSmoothing(InputAction.CallbackContext context)
    {
        EnableTurningSmoothing = !EnableTurningSmoothing;
        OnSmoothingUpdated?.Invoke(this);
    }

    private void ToggleVelocitySmoothing(InputAction.CallbackContext obj)
    {
        EnableVelocitySmoothing = !EnableVelocitySmoothing;
        OnSmoothingUpdated.Invoke(this);
    }

    private void Update()
    {
        // Movement

        var move = _moveAction.ReadValue<Vector2>();

        // Ugly hack for WebGL because it's incompatible with gamepads in the new Input System
        move += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        move = Vector2.ClampMagnitude(move, 1);

        var targetLocalVelocity = new Vector3(move.x * StrafeSpeed, 0, move.y * MaxSpeed);
        var targetWorldVelocity = transform.TransformDirection(targetLocalVelocity);

        if (EnableVelocitySmoothing)
            _velocity = SmoothDamp.Move(_velocity, targetWorldVelocity, VelocitySmoothSpeed, Time.deltaTime);
        else
            _velocity = targetWorldVelocity;

        transform.position += _velocity * Time.deltaTime;

        // Rotation

        var stick = _stickAction.ReadValue<Vector2>();

        // Ugly hack for WebGL because it's incompatible with gamepads in the new Input System
        stick += new Vector2(Input.GetAxis("StickHorizontal"), Input.GetAxis("StickVertical"));
        stick = Vector2.ClampMagnitude(stick, 1);

        if (InvertPitch == false)
            stick.y *= -1;

        var targetLocalAngularVelocity = new Vector3(stick.y, stick.x, 0) * TurnRate;
        if (EnableTurningSmoothing)
            _localAngularVelocity = SmoothDamp.Move(_localAngularVelocity, targetLocalAngularVelocity, TurnSmoothSpeed, Time.deltaTime);
        else
            _localAngularVelocity = targetLocalAngularVelocity;

        transform.Rotate(_localAngularVelocity * Time.deltaTime, Space.Self);

        var speedBlend = Mathf.InverseLerp(0, MaxSpeed, _velocity.magnitude);
        VisualModel.transform.localEulerAngles = new Vector3(0, 0, -_localAngularVelocity.y * speedBlend * FakeRoll * Mathf.Deg2Rad);

        // Camera Movement

        if (EnableCamMovement)
        {
            var targetCamOffset = -_velocity * CameraOffsetting;
            if (EnableCamOffsetSmoothing)
                _camOffset = SmoothDamp.Move(_camOffset, targetCamOffset, CamOffsetSmoothSpeed, Time.deltaTime);
            else
                _camOffset = targetCamOffset;
            ShipCam.transform.position = CameraAnchor.position + _camOffset;

            var targetCamLookahead = _localAngularVelocity * CameraLookahead;
            if (EnableCamTurnSmoothing)
                _camLookahead = SmoothDamp.Move(_camLookahead, targetCamLookahead, CamLookaheadSmoothSpeed, Time.deltaTime);
            else
                _camLookahead = targetCamLookahead;
            ShipCam.transform.localEulerAngles = _camLookahead;
        }
        else
        {
            ShipCam.SetLocalPositionAndRotation(CameraAnchor.localPosition, CameraAnchor.localRotation);
        }
    }
}

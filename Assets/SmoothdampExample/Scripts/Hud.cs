using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Ship PlayerShip = null;
    public Text SmoothingText = null;
    public CanvasGroup CanvasGroup = null;

    private bool _isVisible = true;
    private StringBuilder _builder = new();

    private void Start()
    {
        PlayerShip.OnSmoothingUpdated.AddListener(UpdateHud);
        UpdateHud(PlayerShip);
    }

    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            _isVisible = !_isVisible;
            CanvasGroup.alpha = _isVisible ? 1 : 0;
        }
    }

    private void UpdateHud(Ship ship)
    {
        _builder.Clear();
        _builder.AppendLine($"Invert Pitch: {PlayerShip.InvertPitch}");
        _builder.AppendLine();

        _builder.AppendLine($"Velocity Smoothing: {PlayerShip.EnableVelocitySmoothing}");
        _builder.AppendLine($"Turn Smoothing: {PlayerShip.EnableTurningSmoothing}");
        _builder.AppendLine();

        _builder.AppendLine($"Camera Movement: {PlayerShip.EnableCamMovement}");
        _builder.AppendLine($"Camera Offset Smoothing: {PlayerShip.EnableCamOffsetSmoothing}");
        _builder.AppendLine($"Camera Lookahead Smoothing: {PlayerShip.EnableCamTurnSmoothing}");
        SmoothingText.text = _builder.ToString();
    }
}

using UnityEngine;

[RequireComponent(typeof(BowShootController))]
public class BowAimController : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private bool invertHorizontal = true;
    [SerializeField] private bool invertVertical = true;

    [Header("Angle Restrictions")]
    [SerializeField] private float minAngle = -60f;
    [SerializeField] private float maxAngle = 60f;

    private BowShootController shootController;
    private Vector2 direction;

    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        shootController = GetComponent<BowShootController>();
    }

    private void Update()
    {
        if (invertHorizontal)
        {
            horizontalInput = -joystick.Horizontal;
        }
        else
        {
            horizontalInput = joystick.Horizontal;
        }

        if (invertVertical)
        {
            verticalInput = -joystick.Vertical;
        }
        else
        {
            verticalInput = joystick.Vertical;
        }

        Vector2 joystickInput = new Vector2(horizontalInput, verticalInput);
        float joystickMagnitude = joystickInput.magnitude;

        if (joystickMagnitude > 0.1f)
        {
            direction = joystickInput.normalized;
            shootController.SetAiming(true, joystickMagnitude);
        }
        else if (shootController.IsAiming)
        {
            shootController.Shoot();
            shootController.SetAiming(false, 0f);
        }
    }
}
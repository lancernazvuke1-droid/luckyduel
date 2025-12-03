using UnityEngine;

public class GunFollowCrosshair : MonoBehaviour
{
    public Transform gunPivot;
    public Joystick2D joystick;
    public float maxTilt = 20f;
    public float smooth = 10f;

    private float baseAngle;

    void Start()
    {
        baseAngle = gunPivot.rotation.eulerAngles.z;
    }

    void Update()
    {
        if (gunPivot == null || joystick == null)
            return;

        float x = joystick.Direction.x;

        // инвертировали знак
        float targetAngle = baseAngle - x * maxTilt;

        float currentAngle = gunPivot.rotation.eulerAngles.z;
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * smooth);

        gunPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
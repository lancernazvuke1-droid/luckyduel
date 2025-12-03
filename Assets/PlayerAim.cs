using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Joystick2D joystick;   // твой джойстик
    public Transform gunPivot;    // объект, который крутится (точка поворота пистолета)
    public Transform crosshair;   // прицел (объект со спрайтом)
    public float moveSpeed = 5f;  // скорость движения прицела
    public float deadZone = 0.1f; // чтобы джойстик в нуле не дрожал

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector2 input = joystick.Direction;

        // 1. Двигаем прицел свободно по экрану
        if (input.magnitude > deadZone)
        {
            Vector3 delta = new Vector3(input.x, input.y, 0f) * moveSpeed * Time.deltaTime;
            crosshair.position += delta;

            ClampCrosshairInsideScreen();
        }

        // 2. Поворачиваем пистолет в сторону прицела
        Vector3 dir = crosshair.position - gunPivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void ClampCrosshairInsideScreen()
    {
        // ограничиваем прицел границами экрана
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        Vector3 pos = crosshair.position;
        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);
        crosshair.position = pos;
    }
}
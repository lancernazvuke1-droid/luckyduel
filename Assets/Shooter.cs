using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.2f;

    public Transform crosshair; // прицел

    private float _lastShotTime;

    public void Fire()
    {
        if (Time.time - _lastShotTime < fireCooldown)
            return;

        // спрашиваем LevelManager, можно ли потратить патрон
        if (LevelManager.Instance == null || !LevelManager.Instance.TrySpendAmmo())
            return;

        _lastShotTime = Time.time;

        if (bulletPrefab == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 dir;

            if (crosshair != null)
            {
                // направление к прицелу
                dir = ((Vector2)crosshair.position - (Vector2)firePoint.position).normalized;
            }
            else
            {
                dir = firePoint.right;
            }

            rb.velocity = dir * bulletSpeed;
        }

        // 🔊 Звук выстрела
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayShot();
        }
    }
}

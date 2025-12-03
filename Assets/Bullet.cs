using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            TargetLife target = collision.GetComponent<TargetLife>();
            if (target != null)
            {
                target.Hit(); // Сообщаем мишени, что попали
            }

            Destroy(gameObject); // удалить пулю
        }
    }

}

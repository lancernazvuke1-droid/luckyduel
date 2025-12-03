using UnityEngine;

public class TargetLife : MonoBehaviour
{
    public float lifeTime = 2f;
    private bool _wasHit = false;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Hit()
    {
        if (_wasHit) return;

        _wasHit = true;

        // 🔊 Звук попадания
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayHit();
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnTargetHit();
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!_wasHit)
        {
            // промах — можно в будущем штрафовать, если захочешь
        }
    }
}

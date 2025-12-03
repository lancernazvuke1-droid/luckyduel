using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;

    public Transform spawnMinPoint;
    public Transform spawnMaxPoint;

    public float spawnInterval = 2f;

    private float timer;

    void Update()
    {
        // если игры ещё нет или уровень не активен — выходим
        if (LevelManager.Instance == null || !LevelManager.Instance.IsLevelActive)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnTarget();
            timer = 0f;
        }
    }

    void SpawnTarget()
    {
        if (targetPrefab == null || spawnMinPoint == null || spawnMaxPoint == null)
        {
            Debug.LogWarning("TargetSpawner: не заданы prefab или точки спавна");
            return;
        }

        float minX = Mathf.Min(spawnMinPoint.position.x, spawnMaxPoint.position.x);
        float maxX = Mathf.Max(spawnMinPoint.position.x, spawnMaxPoint.position.x);
        float minY = Mathf.Min(spawnMinPoint.position.y, spawnMaxPoint.position.y);
        float maxY = Mathf.Max(spawnMinPoint.position.y, spawnMaxPoint.position.y);

        Vector2 pos = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        Instantiate(targetPrefab, pos, Quaternion.identity);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySpawn();
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnMinPoint == null || spawnMaxPoint == null)
            return;

        float minX = Mathf.Min(spawnMinPoint.position.x, spawnMaxPoint.position.x);
        float maxX = Mathf.Max(spawnMinPoint.position.x, spawnMaxPoint.position.x);
        float minY = Mathf.Min(spawnMinPoint.position.y, spawnMaxPoint.position.y);
        float maxY = Mathf.Max(spawnMinPoint.position.y, spawnMaxPoint.position.y);

        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }
}

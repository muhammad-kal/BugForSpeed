using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string enemyLayer = "Enemy";
    [SerializeField] private string playerLayer = "Player";

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab belum diset!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Set tag
        enemy.tag = enemyTag;

        // Set layer
        int layerIndex = LayerMask.NameToLayer(enemyLayer);
        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{enemyLayer}' tidak ditemukan!");
            return;
        }
        enemy.layer = layerIndex;

        // Exclude collision dengan layer player
        int playerLayerIndex = LayerMask.NameToLayer(playerLayer);
        if (playerLayerIndex != -1)
        {
            Physics2D.IgnoreLayerCollision(layerIndex, playerLayerIndex, true);
        }

        // Jika prefab punya anak, set layer juga
        foreach (Transform child in enemy.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layerIndex;
        }
    }
}

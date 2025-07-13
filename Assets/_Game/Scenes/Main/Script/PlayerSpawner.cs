using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string playerLayer = "Player";
    [SerializeField] private string enemyLayer = "Enemy";

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab belum diset!");
            return;
        }

        GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        // Set tag
        player.tag = playerTag;

        // Set layer
        int layerIndex = LayerMask.NameToLayer(playerLayer);
        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{playerLayer}' tidak ditemukan! Pastikan layer sudah ditambahkan di Tags and Layers.");
            return;
        }
        player.layer = layerIndex;

        // Exclude collision antara Player dan Enemy
        int enemyLayerIndex = LayerMask.NameToLayer(enemyLayer);
        if (enemyLayerIndex != -1)
        {
            Physics2D.IgnoreLayerCollision(layerIndex, enemyLayerIndex, true);
        }

        // Jika prefab punya anak, set layer semua anak juga
        foreach (Transform child in player.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layerIndex;
        }
    }
}

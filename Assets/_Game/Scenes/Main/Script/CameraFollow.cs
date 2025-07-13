using UnityEngine;

public class CameraFollowByTag : MonoBehaviour
{
    [Header("Tag target untuk dikunci")]
    public string targetTag = "Player";

    [Header("Offset dan Smooth")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    public float smoothSpeed = 5f;

    private Transform target;

    void Start()
    {
        GameObject found = GameObject.FindGameObjectWithTag(targetTag);
        if (found != null)
        {
            target = found.transform;
        }
        else
        {
            Debug.LogWarning("Tidak ditemukan objek dengan tag: " + targetTag);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPos.x, smoothedPos.y, offset.z);  // Tetap jaga Z kamera
    }
}

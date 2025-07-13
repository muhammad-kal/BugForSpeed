using UnityEngine;

public class SpinEffect : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -360f; // derajat per detik
    [SerializeField] private float duration = 0.5f; // waktu aktif dalam detik

    private float timer;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject); // Hapus objek setelah waktu habis
        }
    }
}

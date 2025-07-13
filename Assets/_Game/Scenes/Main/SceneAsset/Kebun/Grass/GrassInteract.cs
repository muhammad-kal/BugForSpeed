using UnityEngine;

public class GrassInteract : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private GameObject blurPanelPrefab;
    [SerializeField] private GameObject insectSpritePrefab;
    [SerializeField] private GameObject skillCheckPrefab;

    [Header("UI Root")]
    [SerializeField] private Transform canvasUIRoot;

    [Header("Net Offset")]
    [SerializeField] private Vector2 offset = new Vector2(0, 0.5f);

    private bool skillCheckRunning = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Rumput diklik!");

                // Munculkan sprite jaring
                GameObject net = Instantiate(netPrefab, transform.position + (Vector3)offset, Quaternion.identity);
                if (!net.TryGetComponent<SpinEffect>(out _))
                {
                    net.AddComponent<SpinEffect>();
                }

                // Munculkan blur UI full screen
                if (blurPanelPrefab && canvasUIRoot)
                {
                    Instantiate(blurPanelPrefab, canvasUIRoot);
                }

                // Munculkan sprite serangga di tengah layar
                if (insectSpritePrefab && canvasUIRoot)
                {
                    Debug.Log("asd");
                    GameObject insect = Instantiate(insectSpritePrefab, canvasUIRoot);
                    insect.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // posisi tengah
                }

                // Jalankan skill check
                RunSkillCheck(gameObject);
            }
        }
    }

    private void RunSkillCheck(GameObject enemy)
    {
        if (skillCheckRunning || skillCheckPrefab == null || canvasUIRoot == null)
            return;

        skillCheckRunning = true;

        GameObject go = Instantiate(skillCheckPrefab, canvasUIRoot);
        SkillCheck sc = go.GetComponent<SkillCheck>();

        sc.OnFinished = result =>
        {
            skillCheckRunning = false;

            if (result)
            {
                Debug.Log("Skill-check berhasil!");
                // Tambahkan aksi seperti menangkap musuh, animasi, dll.
                // Contoh: StartCoroutine(CatchEffect(enemy));
            }
            else
            {
                Debug.Log("Skill-check gagal");
            }
        };
    }
}

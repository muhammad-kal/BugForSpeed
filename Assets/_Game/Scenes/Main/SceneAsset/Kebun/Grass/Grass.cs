using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Grass : MonoBehaviour
{
    [Header("Popup Sprite")]
    [SerializeField] private Sprite popupSprite;

    [Header("Skill-Check Prefab & UI Root")]
    [SerializeField] private GameObject skillCheckPrefab; // prefab berisi UI SkillCheck
    [SerializeField] private Transform uiRoot;            // Canvas / Panel untuk UI

    [Header("Skill Logic")]
    [SerializeField] private Vector2 floatingOffset = Vector2.up * 0.5f;

    [Header("Skill Check Status")]
    [SerializeField] private bool sudahCheck;

    private Camera mainCam;
    private GameObject popupImageGO;     // image yang muncul di tengah
    private bool skillCheckRunning;

    private void Start()
    {
        mainCam = Camera.main;

        // Buat Image popup satu kali (disembunyikan di awal)
        if (popupSprite != null)
        {
            popupImageGO = new GameObject("CenterPopup", typeof(RectTransform), typeof(Image));
            popupImageGO.transform.SetParent(uiRoot,false);

            var img = popupImageGO.GetComponent<Image>();
            img.sprite = popupSprite;
            img.preserveAspect = true;

            var rt = img.rectTransform;
            rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.one * 0.5f;
            rt.anchoredPosition = Vector2.zero;

            float max = Mathf.Min(Screen.width, Screen.height) * 0.6f;
            rt.sizeDelta = new Vector2(max, max);

            popupImageGO.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !skillCheckRunning)
        {
            Vector2 mouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouse, Vector2.zero);

            if (hit && hit.collider.gameObject == gameObject)
            {
                // Tampilkan sprite & jalankan skill-check
                if (popupImageGO) popupImageGO.SetActive(true);
                RunSkillCheck();
            }
        }
    }

    /* ───────── Skill-Check Workflow ───────── */

    private void RunSkillCheck()
    {
        skillCheckRunning = true;

        // Buat instance skill-check UI
        GameObject go = Instantiate(skillCheckPrefab, null);
        SkillCheck sc = go.GetComponent<SkillCheck>();

        sc.OnFinished = result =>
        {
            // Hapus UI skill-check
            //Destroy(go);
            // Sembunyikan sprite popup
            if (popupImageGO) popupImageGO.SetActive(false);

            skillCheckRunning = false;

            if (result)
            {
                // popupImageGO.SetActive(true);
                Debug.Log("Skill-check BERHASIL ✔");
            }
            else
                Debug.Log("Skill-check GAGAL ✖");

            sudahCheck = true;
        };
    }

    private void MunculSerangga()
    {
        popupImageGO = new GameObject("CenterPopup", typeof(RectTransform), typeof(Image));
        popupImageGO.transform.SetParent(uiRoot, false);

        var img = popupImageGO.GetComponent<Image>();
        img.sprite = popupSprite;
        img.preserveAspect = true;

        var rt = img.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.one * 0.5f;
        rt.anchoredPosition = Vector2.zero;

        float max = Mathf.Min(Screen.width, Screen.height) * 0.6f;
        rt.sizeDelta = new Vector2(max, max);

        popupImageGO.SetActive(true);
    }
}

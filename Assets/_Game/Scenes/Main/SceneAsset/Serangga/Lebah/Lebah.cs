using System.Collections;
using UnityEngine;

public class Lebah : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Vector2 offset = new Vector2(0.5f, 0.5f);
    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private float cooldownDuration = 2f;
    [SerializeField] private KeyCode skillKey = KeyCode.F;

    [Header("Skill-Check Prefab")]
    [SerializeField] private GameObject skillCheckPrefab;
    [SerializeField] private Transform uiRoot;          // Parent Canvas

    private GameObject floatingSprite;
    private float lastSkillTime = -Mathf.Infinity;

    private bool skillCheckRunning = false;             // ← penanda baru

    /* ─────────────────────────────── */

    private void Start()
    {
        floatingSprite = new GameObject("FloatingSprite");
        var sr = floatingSprite.AddComponent<SpriteRenderer>();
        sr.sprite = playerSprite;
        sr.sortingOrder = 999;
        floatingSprite.SetActive(false);
    }

    private void Update()
    {
        // syarat: 1) tombol ditekan, 2) cooldown lewat, 3) tidak ada skill-check aktif
        if (Input.GetKeyDown(skillKey) &&
            Time.time >= lastSkillTime + cooldownDuration &&
            !skillCheckRunning)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag(enemyTag);
            if (enemy != null)
            {
                RunSkillCheck(enemy);
                lastSkillTime = Time.time;      // cooldown mulai dihitung
            }
        }
    }

    /* ───────── Skill-Check Flow ───────── */

    private void RunSkillCheck(GameObject enemy)
    {
        skillCheckRunning = true;               // kunci penggunaan skill

        GameObject go = Instantiate(skillCheckPrefab, uiRoot);
        SkillCheck sc = go.GetComponent<SkillCheck>();

        sc.OnFinished = result =>
        {
            skillCheckRunning = false;          // buka kunci begitu selesai

            if (result)
                StartCoroutine(StunEnemy(enemy));
            else
                Debug.Log("Skill-check gagal, musuh tidak distun");
        };
    }

    /* ───────── Stun Enemy ───────── */

    private IEnumerator StunEnemy(GameObject enemy)
    {
        if (enemy == null) yield break;

        Rigidbody2D  rb     = enemy.GetComponent<Rigidbody2D>();
        MonoBehaviour mover = enemy.GetComponent<MonoBehaviour>(); // ganti dgn script gerak spesifik
        Vector2 savedVel    = rb ? rb.velocity : Vector2.zero;

        if (rb)    rb.velocity = Vector2.zero;
        if (mover) mover.enabled = false;

        floatingSprite.transform.position = (Vector2)enemy.transform.position + offset;
        floatingSprite.SetActive(true);

        yield return new WaitForSeconds(stunDuration);

        if (rb)    rb.velocity = savedVel;
        if (mover) mover.enabled = true;
        floatingSprite.SetActive(false);
    }
}

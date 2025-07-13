using System.Collections;
using UnityEngine;

public class Lebah : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private string TagLawan = "Enemy";
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Vector2 offset = new Vector2(0.5f, 0.5f);
    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private float cooldownDuration = 2f;
    [SerializeField] private KeyCode skillKey = KeyCode.F;

    [Header("Skill-Check Prefab (untuk Player)")]
    [SerializeField] private GameObject skillCheckPrefab;
    [SerializeField] private Transform uiRoot;

    private GameObject floatingSprite;
    private float lastSkillTime = -Mathf.Infinity;
    private bool skillCheckRunning = false;

    private void Start()
    {
        // Siapkan sprite animasi depan musuh saat stun
        floatingSprite = new GameObject("FloatingSprite");
        var sr = floatingSprite.AddComponent<SpriteRenderer>();
        sr.sprite = playerSprite;
        sr.sortingOrder = 999;
        floatingSprite.SetActive(false);

        // Jika objek ini bertag Enemy, langsung serang otomatis (tanpa skill-check)
        if (CompareTag("Enemy"))
        {
            float randomDelay = Random.Range(0.5f, 2.0f);
            Invoke(nameof(AutoAttack), randomDelay);
        }
    }

    private void Update()
    {
        // Jika ini musuh (Enemy), tidak perlu kontrol manual
        if (CompareTag("Enemy")) return;

        // Kontrol pemain
        if (Input.GetKeyDown(skillKey) &&
            Time.time >= lastSkillTime + cooldownDuration &&
            !skillCheckRunning)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag(TagLawan);
            if (enemy != null)
            {
                RunSkillCheck(enemy);
                lastSkillTime = Time.time;
            }
        }
    }

    /* ───────── Auto Attack (untuk Enemy) ───────── */

    private void AutoAttack()
    {
        GameObject target = GameObject.FindGameObjectWithTag(TagLawan);
        if (target != null)
        {
            StartCoroutine(StunEnemy(target));
        }
    }

    /* ───────── Skill-Check (untuk Player) ───────── */

    private void RunSkillCheck(GameObject enemy)
    {
        skillCheckRunning = true;

        GameObject go = Instantiate(skillCheckPrefab, uiRoot);
        SkillCheck sc = go.GetComponent<SkillCheck>();

        sc.OnFinished = result =>
        {
            skillCheckRunning = false;

            if (result)
                StartCoroutine(StunEnemy(enemy));
            else
                Debug.Log("Skill-check gagal, musuh tidak distun");
        };
    }

    /* ───────── Stun Logic ───────── */

    private IEnumerator StunEnemy(GameObject enemy)
    {
        if (enemy == null) yield break;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        MonoBehaviour mover = enemy.GetComponent<MonoBehaviour>(); // Ganti jika ada skrip gerak spesifik
        Vector2 savedVel = rb ? rb.velocity : Vector2.zero;

        if (rb) rb.velocity = Vector2.zero;
        if (mover) mover.enabled = false;

        floatingSprite.transform.position = (Vector2)enemy.transform.position + offset;
        floatingSprite.SetActive(true);

        yield return new WaitForSeconds(stunDuration);

        if (rb) rb.velocity = savedVel;
        if (mover) mover.enabled = true;
        floatingSprite.SetActive(false);
    }
}

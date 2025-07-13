using System.Collections;
using UnityEngine;

public class KumbangTanduk : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private string TagLawan = "Enemy";
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Vector2 offset = new Vector2(0.5f, 0.5f);
    [SerializeField] private float cooldownDuration = 2f;
    [SerializeField] private KeyCode skillKey = KeyCode.F;

    [Header("Skill-Check Prefab (untuk Player)")]
    [SerializeField] private GameObject skillCheckPrefab;
    [SerializeField] private Transform uiRoot;

    [Header("Self Power")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float deceleration = 35f;
    [SerializeField] private float jumpForce = 600f;

    private PlayerController pc;

    private GameObject floatingSprite;
    private float lastSkillTime = -Mathf.Infinity;
    private bool skillCheckRunning = false;

    private void Start()
    {
        changestats();

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
            StartCoroutine(Skill(target));
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
                StartCoroutine(Skill(enemy));
            else
                Debug.Log("Skill-check gagal, musuh tidak distun");
        };
    }

    /* ───────── Skill Logic ───────── */

    private IEnumerator Skill(GameObject enemy)
    {
    if (enemy == null) yield break;

    Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
    if (rb == null) yield break;

    // Tempatkan di depan musuh
    floatingSprite.transform.position = (Vector2)enemy.transform.position + offset;

    // Balik horizontal (mirror)
    Vector3 scale = floatingSprite.transform.localScale;
    scale.x = -Mathf.Abs(scale.x); // pastikan selalu negatif (terbalik)
    floatingSprite.transform.localScale = scale;
    floatingSprite.SetActive(true);

    // Simpan kecepatan awal
    Vector2 originalVelocity = rb.velocity;

    // Dorong ke kiri selama 1 detik
    float timer = 0f;
    while (timer < 1f)
    {
        rb.velocity = new Vector2(-3f, rb.velocity.y);  // Kecepatan ke kiri
        timer += Time.deltaTime;
        yield return null;
    }

    // Kembalikan kontrol ke AI
    rb.velocity = originalVelocity;

    // Sembunyikan sprite
    floatingSprite.SetActive(false);
    }

    private void changestats()
    {
        pc = GetComponent<PlayerController>();
        pc.maxSpeed = maxSpeed;
        pc.acceleration = acceleration;
        pc.deceleration = deceleration;
        pc.jumpForce = jumpForce;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;                // ← Action<T>

public class SkillCheck : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] RectTransform pointer;
    [SerializeField] RectTransform successZone;
    [SerializeField] RectTransform barBackground;

    [Header("Settings")]
    [SerializeField] float minSpeed = 350f;
    [SerializeField] float maxSpeed = 650f;
    [SerializeField] Vector2 successWidthRange = new Vector2(80, 160);
    [SerializeField] int maxFails = 3;

    // ← callback eksternal: true = sukses, false = gagal
    public Action<bool> OnFinished;

    /* ---------- INTERNAL ---------- */
    float speed;
    bool goingRight, active;
    int failCount;

    void OnEnable()
    {
        failCount = 0;
        StartRound();
    }

    void Update()
    {
    if (!active) return;
    MovePointer();

    if (Input.GetKeyDown(KeyCode.F))  // ← Ganti Space ke F
        Evaluate();
    }


    /* ---------- CORE ---------- */

    void StartRound()
    {
        active     = true;
        speed      = UnityEngine.Random.Range(minSpeed, maxSpeed);
        goingRight = UnityEngine.Random.value > .5f;

        float half = barBackground.rect.width * .5f;
        pointer.anchoredPosition = new Vector2(goingRight ? -half : half, 0);

        float zoneW  = UnityEngine.Random.Range(successWidthRange.x, successWidthRange.y);
        successZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, zoneW);

        float maxC   = half - zoneW * .5f;
        successZone.anchoredPosition = new Vector2(UnityEngine.Random.Range(-maxC, maxC), 0);
    }

    void MovePointer()
    {
        float move = speed * Time.deltaTime * (goingRight ? 1 : -1);
        pointer.anchoredPosition += new Vector2(move, 0);

        float half = barBackground.rect.width * .5f;
        if (pointer.anchoredPosition.x >=  half) { pointer.anchoredPosition = new Vector2( half, 0); goingRight = false; }
        if (pointer.anchoredPosition.x <= -half) { pointer.anchoredPosition = new Vector2(-half, 0); goingRight = true;  }
    }

    void Evaluate()
    {
        active = false;

        bool success = Overlap(pointer, successZone);
        if (success)
            Finish(true);
        else
        {
            failCount++;
            if (failCount >= maxFails) Finish(false);
            else           Invoke(nameof(StartRound), 1f); // percobaan berikut
        }
    }

    bool Overlap(RectTransform a, RectTransform b)
    {
        float aL = a.anchoredPosition.x - a.rect.width * .5f;
        float aR = a.anchoredPosition.x + a.rect.width * .5f;
        float bL = b.anchoredPosition.x - b.rect.width * .5f;
        float bR = b.anchoredPosition.x + b.rect.width * .5f;
        return aR >= bL && aL <= bR;
    }

    void Finish(bool success)
    {
        OnFinished?.Invoke(success);  // ← kirim hasil
        Destroy(gameObject);          // auto-hapus prefab instance
    }
}

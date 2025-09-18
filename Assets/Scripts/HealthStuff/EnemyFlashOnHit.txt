using UnityEngine;

public class EnemyFlashOnHit : MonoBehaviour
{
    public Renderer enemyRenderer; // Assign the MeshRenderer in the Inspector
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private Color originalColor;
    private Coroutine flashRoutine;

    void Start()
    {
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInChildren<Renderer>();
        }

        originalColor = enemyRenderer.material.color;
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private System.Collections.IEnumerator FlashCoroutine()
    {
        enemyRenderer.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material.color = originalColor;
    }
}
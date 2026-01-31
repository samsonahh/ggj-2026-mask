using UnityEngine;
using TMPro;

public class DamagePopUpController : MonoBehaviour
{
    private TMP_Text text;

    public float floatSpeed = 1.5f;
    public float lifetime = 0.8f;

    private float timer;

    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();

        if (text == null)
        {
            enabled = false;
        }
    }

    public void SetText(string value)
    {
        text.text = value;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (Camera.main != null) transform.forward = Camera.main.transform.forward;

        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);

        Color c = text.color;
        c.a = alpha;

        text.color = c;

        if (timer >= lifetime) Destroy(gameObject);
    }
}
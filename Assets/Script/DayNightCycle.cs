using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;

    public Color morningColor = new Color(1f, 0.5f, 0.25f);
    public Color noonColor = Color.white;
    public Color eveningColor = new Color(1f, 0.5f, 0.25f);
    public Color nightColor = new Color(0.1f, 0.1f, 0.35f);

    public float dayDuration = StatsManager.Instance.tick * 24; // Duration of a day in seconds

    public float time;

    void Start()
    {
        if (directionalLight == null)
        {
            Debug.LogError("Directional light is not assigned!");
        }

        dayDuration = StatsManager.Instance.tick * 24;
        time = StatsManager.Instance.hour;
    }

    void Update()
    {
        // Update the time
        time += Time.deltaTime / (dayDuration / 24f);

        // Loop the time value
        if (time >= 24f)
        {
            time -= 24f;
        }

        // Update the light color
        UpdateLightColor(time);
    }

    void UpdateLightColor(float time)
    {
        Color currentColor;

        if (time < 6f) // Morning (00:00 to 06:00)
        {
            float t = time / 6f;
            currentColor = Color.Lerp(nightColor, morningColor, t);
        }
        else if (time < 12f) // Noon (06:00 to 12:00)
        {
            float t = (time - 6f) / 6f;
            currentColor = Color.Lerp(morningColor, noonColor, t);
        }
        else if (time < 18f) // Evening (12:00 to 18:00)
        {
            float t = (time - 12f) / 6f;
            currentColor = Color.Lerp(noonColor, eveningColor, t);
        }
        else // Night (18:00 to 00:00)
        {
            float t = (time - 18f) / 6f;
            currentColor = Color.Lerp(eveningColor, nightColor, t);
        }

        directionalLight.color = currentColor;
    }
}
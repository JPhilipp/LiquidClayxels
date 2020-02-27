using UnityEngine;
using TMPro;

public class TextColorTweener : MonoBehaviour
{
    // Changes the color of a 3D Text Mesh Pro text over time.

    bool isRunning = false;
    
    Color startColor  = new Color();
    Color targetColor = new Color();
    
    float durationSeconds = 1f;
    float startTime = 0f;
    float endTime   = 0f;
        
    TextMeshPro textMesh = null;
    
    public void Animate(Color startColor, Color targetColor, float seconds = 1f, float delaySeconds = 0f)
    {
        isRunning = true;

        durationSeconds = seconds;
        startTime = Time.time + delaySeconds;
        endTime   = startTime + durationSeconds;

        textMesh = GetComponent<TextMeshPro>();
        textMesh.overrideColorTags = true;

        this.startColor  = startColor;
        this.targetColor = targetColor;
        
        SetColor(this.startColor);
    }

    void Update()
    {
        if (isRunning && Time.time >= startTime)
        {
            float secondsLeft = (float) endTime - Time.time;
            float fraction = 1f - secondsLeft / durationSeconds;
            
            Color color = Color.Lerp(startColor, targetColor, fraction);
            SetColor(color);

            if (secondsLeft <= 0f)
            {
                isRunning = false;
            }
        }
    }
    
    void SetColor(Color color)
    {
        textMesh.color = color;
        textMesh.faceColor = color;
    }

}

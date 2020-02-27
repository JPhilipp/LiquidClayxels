using System;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    // Smoothly animates a transform to a target
    // position/ scale/ rotation/ color over time.

    public bool isRunning { get; private set; } = false;
    
    Vector3  startPosition  = Vector3.zero;
    Vector3? targetPosition = null;
    
    Vector3  startScale     = Vector3.zero;
    Vector3? targetScale    = null;
    
    Vector3  startRotation  = Vector3.zero;
    Vector3? targetRotation = null;
    
    Color?   startColor     = null;
    Color?   targetColor    = null;
    
    string colorName = null;

    TweenType tweenType = TweenType.None;
    float durationSeconds = 0.2f;
    float endTime = 0f;
    bool destroyTweenerWhenDone = false;
    
    Action callbackWhenDone = null;
    
    Material material;

    public void Animate(Vector3? position = null, Vector3? rotation = null, Vector3? scale = null, Color? color = null, float? seconds = null, TweenType tweenType = TweenType.EaseInOut, bool reachTargetInstantly = false, string colorName = null, Action callbackWhenDone = null, bool destroyTweenerWhenDone = false, NullableVector3 partialPosition = null, NullableVector3 partialRotation = null, NullableVector3 partialScale = null)
    {
        if (!this) { return; }
        
        if (seconds == null) { seconds = 0.5f; }

        isRunning = true;
        this.tweenType = tweenType;
        durationSeconds = (float) seconds;
        endTime = Time.time + durationSeconds;
        this.callbackWhenDone = callbackWhenDone;
        this.destroyTweenerWhenDone = destroyTweenerWhenDone;

        if (position == null && partialPosition != null)
        {
            position = new Vector3
            (
                partialPosition.x != null ? (float) partialPosition.x : transform.localPosition.x,
                partialPosition.y != null ? (float) partialPosition.y : transform.localPosition.y,
                partialPosition.z != null ? (float) partialPosition.z : transform.localPosition.z
            );
        }
        if (rotation == null && partialRotation != null)
        {
            rotation = new Vector3
            (
                partialRotation.x != null ? (float) partialRotation.x : transform.localEulerAngles.x,
                partialRotation.y != null ? (float) partialRotation.y : transform.localEulerAngles.y,
                partialRotation.z != null ? (float) partialRotation.z : transform.localEulerAngles.z
            );
        }
        if (scale == null && partialScale != null)
        {
            scale = new Vector3(
                partialScale.x != null ? (float) partialScale.x : transform.localScale.x,
                partialScale.y != null ? (float) partialScale.y : transform.localScale.y,
                partialScale.z != null ? (float) partialScale.z : transform.localScale.z
            );
        }
        
        startPosition = transform.localPosition;
        targetPosition = position;
        
        startRotation = transform.localEulerAngles;
        targetRotation = rotation;

        startScale = transform.localScale;
        targetScale = scale;
        
        startColor  = null;
        targetColor = color;
        this.colorName = colorName != null ? colorName : "_Color";
        
        if (targetColor != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            material = renderer.material;
            startColor = material.GetColor(this.colorName);
        }
        
        if (reachTargetInstantly) { ReachTargetInstantly(); }
    }

    public void ReachTargetInstantly()
    {
        if (isRunning)
        {
            AnimateToFraction(1f);
            isRunning = false;
        }
    }

    void Update()
    {
        if (isRunning)
        {
            float secondsLeft = (float) endTime - Time.time;
            float fraction = 1f - secondsLeft / durationSeconds;
            fraction = TweenValue(fraction);
            
            AnimateToFraction(fraction);
            
            if (secondsLeft <= 0f)
            {
                AnimateToFraction(1f);
            
                isRunning = false;
                if (callbackWhenDone != null)
                {
                    callbackWhenDone();
                }
                if (destroyTweenerWhenDone)
                {
                    Destroy(this);
                }
            }
        }
    }
    
    void AnimateToFraction(float fraction) {
        if (targetPosition != null)
        {
            transform.localPosition = Vector3.Lerp(
                startPosition, (Vector3) targetPosition, fraction
            );
        }

        if (targetRotation != null)
        {
            transform.localEulerAngles = LerpRotation(
                startRotation, (Vector3) targetRotation, fraction
            );
        }

        if (targetScale != null)
        {
            transform.localScale = Vector3.Lerp(
                startScale, (Vector3) targetScale, fraction
            );
        }
        
        if (targetColor != null)
        {
            Color color = Color.Lerp( (Color) startColor, (Color) targetColor, fraction );
            material.SetColor(colorName, color);
        }
    }

    Vector3 LerpRotation(Vector3 startRotation, Vector3 endRotation, float fraction)
    {
        float xLerp = Mathf.LerpAngle(startRotation.x, endRotation.x, fraction);
        float yLerp = Mathf.LerpAngle(startRotation.y, endRotation.y, fraction);
        float zLerp = Mathf.LerpAngle(startRotation.z, endRotation.z, fraction);
        return new Vector3(xLerp, yLerp, zLerp);
    }
    
    float TweenValue(float v)
    {
        const float min = 0f, max = 1f;
        switch (tweenType)
        {
            case TweenType.EaseIn:    v = Mathfx.EaseIn   (min, max, v); break;
            case TweenType.EaseOut:   v = Mathfx.EaseOut  (min, max, v); break;
            case TweenType.EaseInOut: v = Mathfx.EaseInOut(min, max, v); break;
        }
        return v;
    }

}

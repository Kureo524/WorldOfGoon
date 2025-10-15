using System;
using Unity.Collections;
using UnityEngine;

public class Link : MonoBehaviour {
    public LineRenderer linkRenderer;
    
    public PointStateMachine startGoon;
    public PointStateMachine endGoon;
    
    public int segments;

    public float maxLength;

    public void InitializeValues(PointStateMachine firstGoon, PointStateMachine secondGoon) {
        startGoon = firstGoon;
        endGoon = secondGoon;
        
        maxLength = startGoon.dragRadius;
        
        linkRenderer.startColor = startGoon.linkColor;
        linkRenderer.endColor = endGoon.linkColor;
    }

    public void UpdatePosition() {
        linkRenderer.positionCount = segments;
        
        for (int i = 0; i < segments; i++) {
            float t = (float)i / (segments - 1);
            Vector3 pos = Vector3.Lerp(startGoon.transform.position, endGoon.transform.position, t);
            
            linkRenderer.SetPosition(i, pos);
        }

        UpdateLineWidth(startGoon.transform.position, endGoon.transform.position);
    }

    [Space]
    [SerializeField] private float baseWidth;
    [SerializeField] private float centerInfluence;
    [SerializeField] private float sideInfluence;
    
    public void UpdateLineWidth(Vector3 start, Vector3 end)
    {
        float length = Vector3.Distance(start, end);
        float tension = Mathf.InverseLerp(0f, maxLength, length);

        float endWidth = Mathf.Lerp(baseWidth, baseWidth * sideInfluence, tension);
        float centerWidth = Mathf.Lerp(baseWidth, baseWidth * centerInfluence, tension);

        Keyframe k0 = new Keyframe(0f, endWidth);
        Keyframe k1 = new Keyframe(0.5f, centerWidth);
        Keyframe k2 = new Keyframe(1f, endWidth);
        
        AnimationCurve curve = new AnimationCurve(k0, k1, k2);

        linkRenderer.widthCurve = curve;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer line;
 
    private const float activeTime = 0.2f;
    private float timer = 0f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        line.enabled = false;
    }

    public void DrawLine(Transform from, Transform to)
    {
        line.enabled = true;
        line.SetPosition(0, from.position);
        line.SetPosition(1, to.position);
    }

    void Update()
    {
        if(line.enabled)
        {
            timer += Time.deltaTime;
            if(timer >= activeTime)
            {
                timer = 0f;
                line.ResetBounds();
                line.enabled = false;
            }
        }
    }
}

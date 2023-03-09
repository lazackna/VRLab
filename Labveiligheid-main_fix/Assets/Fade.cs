using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    private void Update()
    {
        float oldOpacity;
        float newOpacity;

        foreach (var renderer in renderers)
        {
            oldOpacity = renderer.material.GetFloat("_Opacity");

            if (oldOpacity < 1.0f)
            {
                newOpacity = oldOpacity;
                oldOpacity += 0.005f;

                renderer.material.SetFloat("_Opacity", oldOpacity);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class ChangeScene : MonoBehaviour
{
    public UniversalRendererData rendererData;
    RenderObjects renderObjects;
    public int layer;
    public int matlayer;
    public int defaultmatlayer;
    public Material material;
    public Material otherMaterial;

    public float timer = 0;
    private void Start()
    {
        renderObjects = rendererData.rendererFeatures[rendererData.rendererFeatures.Count - 1] as RenderObjects;
        renderObjects.settings.stencilSettings.stencilReference = 1;
        material.SetInt("_StencilID", defaultmatlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && renderObjects.settings.stencilSettings.stencilReference != layer && timer > 0.5f)
        {
            timer = 0;
            Debug.Log(renderObjects.settings.stencilSettings.stencilReference + "to" + layer);
            renderObjects.settings.stencilSettings.stencilReference = layer;
            material.SetInt("_StencilID", matlayer);
            otherMaterial.SetInt("_StencilID", defaultmatlayer);
            renderObjects.Create();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}

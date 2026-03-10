using UnityEngine;

public class RenderBliter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RenderTexture sourceTexture, destinationTexture;
    public Material materialToApply;
    public void ApplyMaterialsAndSave()
    {
        materialToApply.SetTexture("_MainTex", sourceTexture);
        Graphics.Blit(sourceTexture, destinationTexture, materialToApply);
    }
    void Start()
    {
        ApplyMaterialsAndSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

/// <summary>
/// URP Renderer Feature for the Obra Dinn dither effect.
/// Add this via your URP Renderer asset → Add Renderer Feature → ObraDinnRendererFeature.
/// </summary>
public class ObraDinnRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material ditherMat;
        public Material thresholdMat;
        public RenderPassEvent injectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public Settings settings = new Settings();

    ObraDinnPass _pass;

    public override void Create()
    {
        _pass = new ObraDinnPass(settings);
        _pass.renderPassEvent = settings.injectionPoint;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.ditherMat == null || settings.thresholdMat == null) return;
        if (renderingData.cameraData.cameraType == CameraType.SceneView) return;

        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
    }
}

/// <summary>
/// Render pass using the RenderGraph API (Unity 6 / URP 17+).
/// Uses transient (graph-managed) textures for intermediates to avoid
/// RTHandle import issues that cause black screens.
///
/// Pipeline:
///   1. Copy camera color → transient srcCopy  (so we can read while writing back)
///   2. ditherMat    : srcCopy → largeRT  (1640x940)
///   3. thresholdMat : largeRT → mainRT   (820x470)
///   4. plain blit   : mainRT  → camera color
/// </summary>
class ObraDinnPass : ScriptableRenderPass
{
    static readonly int s_BL = Shader.PropertyToID("_BL");
    static readonly int s_TL = Shader.PropertyToID("_TL");
    static readonly int s_TR = Shader.PropertyToID("_TR");
    static readonly int s_BR = Shader.PropertyToID("_BR");

    const int LARGE_W = 1640, LARGE_H = 940;
    const int MAIN_W  = 820,  MAIN_H  = 470;

    ObraDinnRendererFeature.Settings _settings;

    // ── Pass data ─────────────────────────────────────────────────────────
    class PassData
    {
        public TextureHandle src;
        public TextureHandle dst;
        public Material mat;
    }

    public ObraDinnPass(ObraDinnRendererFeature.Settings settings)
    {
        _settings = settings;
        profilingSampler = new ProfilingSampler("ObraDinn Dither");
    }

    // ── RenderGraph entry point ───────────────────────────────────────────
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        var resourceData = frameData.Get<UniversalResourceData>();
        var cameraData   = frameData.Get<UniversalCameraData>();

        if (resourceData.isActiveTargetBackBuffer) return;

        SetFrustumCorners(cameraData.camera);

        TextureHandle cameraColor = resourceData.activeColorTexture;

        // Grab the camera descriptor to base our transient texture formats on
        var camDesc = cameraData.cameraTargetDescriptor;
        camDesc.depthBufferBits = 0;
        camDesc.msaaSamples = 1;

        // Transient descriptors — graph allocates and frees these automatically
        var srcCopyDesc = camDesc; // same size/format as camera

        var largeDesc = new RenderTextureDescriptor(LARGE_W, LARGE_H, RenderTextureFormat.ARGB32, 0) { msaaSamples = 1 };
        var mainDesc  = new RenderTextureDescriptor(MAIN_W,  MAIN_H,  RenderTextureFormat.ARGB32, 0) { msaaSamples = 1 };

        TextureHandle srcCopy  = UniversalRenderer.CreateRenderGraphTexture(renderGraph, srcCopyDesc, "_ObraDinnSrcCopy", false, FilterMode.Bilinear);
        TextureHandle largeRT  = UniversalRenderer.CreateRenderGraphTexture(renderGraph, largeDesc,  "_ObraDinnLarge",   false, FilterMode.Bilinear);
        TextureHandle mainRT   = UniversalRenderer.CreateRenderGraphTexture(renderGraph, mainDesc,   "_ObraDinnMain",    false, FilterMode.Bilinear);

        // ── Pass 0: copy camera → srcCopy (plain, no material) ───────────
        AddBlitPass(renderGraph, cameraColor, srcCopy, null, "ObraDinn CopySrc");

        // ── Pass 1: dither srcCopy → largeRT ─────────────────────────────
        AddBlitPass(renderGraph, srcCopy, largeRT, _settings.ditherMat, "ObraDinn Dither");

        // ── Pass 2: threshold largeRT → mainRT ───────────────────────────
        AddBlitPass(renderGraph, largeRT, mainRT, _settings.thresholdMat, "ObraDinn Threshold");

        // ── Pass 3: copy mainRT back → camera color ───────────────────────
        AddBlitPass(renderGraph, mainRT, cameraColor, null, "ObraDinn Composite");
    }

    void AddBlitPass(RenderGraph renderGraph, TextureHandle src, TextureHandle dst, Material mat, string name)
    {
        using var builder = renderGraph.AddRasterRenderPass<PassData>(name, out var passData, profilingSampler);

        passData.src = src;
        passData.dst = dst;
        passData.mat = mat;

        builder.UseTexture(src, AccessFlags.Read);
        builder.SetRenderAttachment(dst, 0, AccessFlags.Write);
        builder.AllowPassCulling(false);

        builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
        {
            if (data.mat != null)
                Blitter.BlitTexture(ctx.cmd, data.src, new Vector4(1, 1, 0, 0), data.mat, 0);
            else
                Blitter.BlitTexture(ctx.cmd, data.src, new Vector4(1, 1, 0, 0), 0, false);
        });
    }

    // ── Helpers ───────────────────────────────────────────────────────────
    void SetFrustumCorners(Camera cam)
    {
        Vector3[] corners = new Vector3[4];
        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, corners);
        for (int i = 0; i < 4; i++)
        {
            corners[i] = cam.transform.TransformVector(corners[i]);
            corners[i].Normalize();
        }
        _settings.ditherMat.SetVector(s_BL, corners[0]);
        _settings.ditherMat.SetVector(s_TL, corners[1]);
        _settings.ditherMat.SetVector(s_TR, corners[2]);
        _settings.ditherMat.SetVector(s_BR, corners[3]);
    }

    public void Dispose() { } // No persistent handles to release — graph owns all RTs now
}
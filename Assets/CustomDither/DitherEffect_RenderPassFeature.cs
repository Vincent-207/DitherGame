using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitherEffect_RenderPassFeature : ScriptableRendererFeature
{
    class RippleEffect_RenderPass : ScriptableRenderPass
    {
        static readonly int TempTargetId = Shader.PropertyToID("_Temp_RippleBlit"); // You can name this anything you want

        public RenderTargetIdentifier source;
        public RTHandle destination;
        public int blitShaderPassIndex;
        public FilterMode filterMode;

        private RTHandle m_TemporaryColorTexture;
        
        public RippleEffect_RenderPass(RenderPassEvent renderPassEvent, FilterMode filterMode, int blitShaderPassIndex)
        {
            RTHandles.Initialize(Screen.width, Screen.height);
            this.renderPassEvent = renderPassEvent;
            this.filterMode = filterMode;
            this.blitShaderPassIndex = blitShaderPassIndex;
            m_TemporaryColorTexture = RTHandles.Alloc("_TemporaryColorTexture");// You can name this anything you want
            // m_TemporaryColorTexture.Init
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            //  && GameplayManager.ins?.RipplePostProcessor.Amount > RipplePostProcessor.LOWEST_AMOUNT_VALUE
            if (Application.isPlaying)
            {
                RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDesc.depthBufferBits = 0;

                // Can't read and write to same color target, use a TemporaryRT
                // destination == RenderTargetHandle.CameraTarget
                if (true)
                {
                    
                    cmd.GetTemporaryRT(Shader.PropertyToID("_TemporaryColorTexture"), opaqueDesc, filterMode);
                    Blit(cmd, source, m_TemporaryColorTexture., GameplayManager.ins.RipplePostProcessor.RippleMaterial, blitShaderPassIndex);
                    Blit(cmd, m_TemporaryColorTexture.Identifier(), source);
                }

            }
            // execution
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        throw new System.NotImplementedException();
    }

    public override void Create()
    {
        throw new System.NotImplementedException();
    }

    
}

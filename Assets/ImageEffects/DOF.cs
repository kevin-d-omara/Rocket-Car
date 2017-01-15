using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class DOF : MonoBehaviour
{

    Material DOFBlurMat;
    [Range(0.0f,10.0f)]
    public float blurRadius;

    [Range(0.0f, 50.0f)]
    public float nearPlane=0;


    [Range(0.0f, 100.0f)]
    public float farPlane=100;

    [Range(0.0f, 30.0f)]
    public float apeture;

    public float focalDistance;

    public bool visualize;

    bool toggle = true;

    Vector2[] kernel;
    ComputeBuffer kernelBuffer;

    const float root2over2 = 0.70710678118f;
    const float cos225 = 0.92387953251f;
    const float sin225 = 0.38268343236f;
    const float cos675 = 0.38268343236f;
    const float sin675 = 0.92387953251f;

    Vector2[] radii = { new Vector2(0, 1), new Vector2(root2over2, root2over2), new Vector2(1, 0), new Vector2(root2over2, -root2over2), new Vector2(0, -1), new Vector2(-root2over2, -root2over2), new Vector2(-1, 0), new Vector2(-root2over2, root2over2),
                         new Vector2(cos225,sin225), new Vector2(cos225, -sin225), new Vector2(-cos225,sin225), new Vector2(-cos225,-sin225), new Vector2(cos675,sin675), new Vector2(cos675,-sin675), new Vector2(-cos675,sin675), new Vector2(-cos675, -sin675)};

    void OnEnable()
    {

        Camera.main.depthTextureMode |= DepthTextureMode.Depth; 

        //Load the motion blur shader into a material
        DOFBlurMat = new Material(Shader.Find("Hidden/DiskBlur"));

        kernel = new Vector2[64];

        int kernelCounter = 0;
        
        //create kernel
        for(int i=0; i< 16; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                kernel[kernelCounter] = Vector2.Lerp(new Vector2(0, 0), radii[i], (1.0f / 5.0f) * ((float)j + 1.0f));

                kernelCounter++;
            }
        }

        kernelBuffer = new ComputeBuffer(64, 8);

        kernelBuffer.SetData(kernel);

    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        RenderTexture originalImage = RenderTexture.GetTemporary(Screen.width, Screen.height);
        RenderTexture downScale = RenderTexture.GetTemporary(Screen.width>>1, Screen.height>>1);
        RenderTexture pass0 = RenderTexture.GetTemporary(Screen.width , Screen.height );
        Graphics.Blit(src, downScale);
        Graphics.Blit(src, originalImage);

        if (toggle)
        {
            Graphics.Blit(downScale, pass0, DOFBlurMat, 0);
            DOFBlurMat.SetTexture("_OriginalFrame", originalImage);
            Graphics.Blit(pass0, dest, DOFBlurMat, 1);
        }

        RenderTexture.ReleaseTemporary(downScale);
        RenderTexture.ReleaseTemporary(pass0);
        RenderTexture.ReleaseTemporary(originalImage);
    }

    // Update is called once per frame
    void Update()
    {

        //Update Uniforms
        DOFBlurMat.SetFloat("_BlurRadius", blurRadius);
        DOFBlurMat.SetBuffer("_Kernel", kernelBuffer);
        DOFBlurMat.SetVector("_ClipPlanes", new Vector4(Camera.main.nearClipPlane, Camera.main.farClipPlane, nearPlane,farPlane));
        DOFBlurMat.SetFloat("_Aperture", apeture);
        DOFBlurMat.SetFloat("_Visualize", visualize ? 0:1);
        DOFBlurMat.SetFloat("_FocalDistance", focalDistance);
        //toggle effect
       // if (Input.GetKeyDown(KeyCode.T)) toggle = !toggle;
    }

    void onReaderObject()
    {
        if (!Application.isPlaying)
            Update();
    }

    void OnDisable()
    {
        if(null != kernelBuffer)
            kernelBuffer.Dispose();
        kernelBuffer = null;
    }



}

using UnityEngine;
using System.Collections;

public class MotionBlur : MonoBehaviour
{

    Material motionBlurMat;
    [Range(0,1)]
    public float velocityScale;
    public int sampleSize;
    bool toggle = true;
    void OnEnable()
    {
        Camera.main.depthTextureMode =  DepthTextureMode.MotionVectors;

        //Load the motion blur shader into a material
        motionBlurMat = new Material(Shader.Find("Hidden/MotionBlur"));

    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(toggle)
        //Blit Effect to screen
        Graphics.Blit(src, dest, motionBlurMat);


    }
     
    // Update is called once per frame
    void Update()
    {

        //Update Uniforms
        motionBlurMat.SetFloat("_velocityScale", velocityScale);
        motionBlurMat.SetInt("_SampleSize", sampleSize);
        if (Input.GetKeyDown(KeyCode.T)) toggle = !toggle; 
    }

    void OnDisable()
    {
     
    }



}

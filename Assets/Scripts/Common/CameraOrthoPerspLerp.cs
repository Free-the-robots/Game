using UnityEngine;
using System.Collections;

public class CameraOrthoPerspLerp : MonoBehaviour
{
    private Matrix4x4 ortho,perspective;
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 50f;

    private Matrix4x4 current;
    private Matrix4x4 target;
    public bool flip = true;
    public float time = 1f;
    private float t = 0f;
    private bool animating = false;

    private void Awake()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        if (GetComponent<Camera>().targetTexture != null)
            aspect = GetComponent<Camera>().targetTexture.width / (float)GetComponent<Camera>().targetTexture.height;

        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
    }

    private void OnEnable()
    {
        current = GetComponent<Camera>().projectionMatrix;

        if (flip)
            target = ortho;
        else
            target = perspective;

        t = 0f;
        animating = true;
    }

    public void enable()
    {
        if(enabled)
        {
            animating = false;
            flip = !flip;
            GetComponent<Camera>().projectionMatrix = target;
            OnEnable();
        }
        enabled = true;
    }

    void Update()
    {
        if (animating)
        {
            t += Time.deltaTime;
            GetComponent<Camera>().projectionMatrix = MatrixLerp(current, target, t / time);

            if (t >= time)
            {
                flip = !flip;
                GetComponent<Camera>().projectionMatrix = target;
                t = 0f;
                enabled = false;
            }
        }
    }

    private Matrix4x4 MatrixLerp(Matrix4x4 fromMat, Matrix4x4 toMat, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(fromMat[i], toMat[i], time);
        return ret;
    }
}
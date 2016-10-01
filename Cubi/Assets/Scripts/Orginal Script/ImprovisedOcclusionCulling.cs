using UnityEngine;
using System.Collections;

public class ImprovisedOcclusionCulling : MonoBehaviour
{

    public bool makeRaysVisible = false;

    public int defaultFarPlane = 100;
    public int minDistance = 20;
    public int maxDistance = 200;
    public int farPlaneBuffer = 10;
    public int rateOfReceding = 16;

    private Camera camera;
    private float[] rayGridY = new float[] {0f, 0.01f, 0.02f, 0.03f, 0.04f, 0.05f, 0.05999999f, 0.06999999f, 0.07999999f, 0.08999999f, 0.09999999f, 0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f, 0.19f, 0.2f, 0.21f, 0.22f, 0.23f, 0.24f, 0.25f, 0.26f, 0.27f, 0.28f, 0.29f, 0.3f, 0.31f, 0.32f, 0.33f, 0.3399999f, 0.3499999f, 0.3599999f, 0.3699999f, 0.3799999f, 0.3899999f, 0.3999999f, 0.4099999f, 0.4199999f, 0.4299999f, 0.4399998f, 0.4499998f, 0.4599998f, 0.4699998f, 0.4799998f, 0.4899998f, 0.4999998f, 0.5099998f, 0.5199998f, 0.5299998f, 0.5399998f, 0.5499998f, 0.5599998f, 0.5699998f, 0.5799997f, 0.5899997f, 0.5999997f, 0.6099997f, 0.6199997f, 0.6299997f, 0.6399997f, 0.6499997f, 0.6599997f, 0.6699997f, 0.6799996f, 0.6899996f, 0.6999996f, 0.7099996f, 0.7199996f, 0.7299996f, 0.7399996f, 0.7499996f, 0.7599996f, 0.7699996f, 0.7799996f, 0.7899995f, 0.7999995f, 0.8099995f, 0.8199995f, 0.8299995f, 0.8399995f, 0.8499995f, 0.8599995f, 0.8699995f, 0.8799995f, 0.8899994f, 0.8999994f, 0.9099994f, 0.9199994f, 0.9299994f, 0.9399994f, 0.9499994f, 0.9599994f, 0.9699994f, 0.9799994f, 0.9899994f, 0.9999993f, };
    private float[] rayGridX = new float[] { 0.00f, 0.01f, 0.06f, 0.11f, 0.16f, 0.21f, 0.26f, 0.31f, 0.33f , 0.36f, 0.39f , 0.41f, 0.43f, 0.45f, 0.47f, 0.48f, 0.49f, 0.50f, 0.51f, 0.52f, 0.53f, 0.55f, 0.57f, 0.59f, 0.61f, 0.64f, 0.69f, 0.74f, 0.79f, 0.84f, 0.89f, 0.94f, 0.99f, 1.00f };
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera.farClipPlane = defaultFarPlane;

        StartCoroutine(AdjustFarPlane());
    }

    IEnumerator AdjustFarPlane()
    {
        while (true)
        {
            int farPlane = (int)camera.farClipPlane + farPlaneBuffer;
            int distance = minDistance;
            bool ExtendFarPlane = false;

            foreach (float y in rayGridY)
            {
                foreach (float x in rayGridX)
                {
                    int tempDist = CastOcclusionRay(x, y);
                    if (tempDist >= farPlane)
                    {
                        distance = tempDist;
                        ExtendFarPlane = true;
                        goto SHIFT_FAR_PLANE;
                    }
                }

                yield return 0;
            }

        SHIFT_FAR_PLANE:
            // Far plane should extend instantly, but recede slowly.
            if (ExtendFarPlane == false)
            {
                camera.farClipPlane -= rateOfReceding;
                if (camera.farClipPlane < minDistance) camera.farClipPlane = minDistance;
            }
            else
            {
                camera.farClipPlane = distance;
            }
        }
    }

    int CastOcclusionRay(float graphX, float graphY)
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(graphX, graphY, 0));

        if (makeRaysVisible == true) Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.distance < maxDistance)
        {
            hit.transform.GetComponent<BlockCulling>().updateBlock(true);
            return (int)hit.distance + farPlaneBuffer;
        }
        else
        {
            if(hit.transform!= null)
            {
            hit.transform.GetComponent<BlockCulling>().updateBlock(false);
            }
            return (int)maxDistance;
        }
    }
}
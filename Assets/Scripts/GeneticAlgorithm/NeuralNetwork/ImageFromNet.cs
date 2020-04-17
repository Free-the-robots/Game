using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

namespace NN
{
    public class ImageFromNet
    {
        public static void DrawLine(Texture2D tex, Vector2 p1, Vector2 p2, Color32 col, int radius = 3)
        {
            Vector2 t = p1;
            float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
            float ctr = 0;

            // RGBA32 texture format data layout exactly matches Color32 struct
            Unity.Collections.NativeArray<Color32> data = tex.GetRawTextureData<Color32>();
            data[(int)(tex.width * (0.5f + tex.height / 2))] = col;
            while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
            {
                t = Vector2.Lerp(p1, p2, ctr);
                ctr += frac;

                for(int i = -radius; i < radius; ++i)
                {
                    for (int j = -radius; j < radius; ++j)
                    {
                        int x = (int)(t.x) + i;
                        int y = (int)(t.y) + j;
                        float distSquared = Mathf.Sqrt(i * i + j * j);
                        if (distSquared <= radius)
                        {
                            if (x > 0 && x < tex.width && y > 0 && y < tex.height)
                            {
                                //int alpha = (255 - (int)(distSquared / radius * 255));
                                //col.a = (byte)alpha;

                                data[x + y * tex.width] = col;
                            }
                        }
                    }
                }
            }
        }

        public static List<Vector3> simulateNet(NN.Net net)
        {
            float dt = Time.fixedDeltaTime;
            List<Vector3> path = new List<Vector3>(100);
            path.Add(new Vector3(0f, 0f, 0f));
            List<float> inputs = new List<float>() { 0f, 0f, 0f, 1f };
            for (int i = 0; i < 50; ++i)
            {
                List<float> res = net.evaluate(inputs);
                Vector3 v = new Vector3(50*res[1], 0f, 50 * res[0]);

                path.Add(path[path.Count - 1] + v * dt);

                inputs[0] = path[path.Count - 1].z;
                inputs[1] = path[path.Count - 1].x;
                inputs[2] = Vector3.Distance(Vector3.zero, path[path.Count - 1]);
            }
            return path;
        }

        public static Texture2D imageFromNet(NN.Net net)
        {
            List<Vector3> path = simulateNet(net);

            Texture2D imageRes = new Texture2D(128, 128, TextureFormat.Alpha8, false);
            //imageRes.alphaIsTransparency = true;

            float scale = 5.0f;
            Vector2 zeroPoint = new Vector2(64, 64);
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 path1 = new Vector2(path[i].x, path[i].z) * scale;
                Vector2 path2 = new Vector2(path[i+1].x, path[i+1].z) * scale;

                Color32 color = Color.HSVToRGB((float)i / path.Count, 1f, 1f);
                DrawLine(imageRes, zeroPoint + path1,
                    zeroPoint + path2,
                    color,
                    6);
            }

            imageRes.Apply();

            //File.WriteAllBytes(Application.dataPath + Path.DirectorySeparatorChar + "AvatarData.png", imageRes.EncodeToPNG());

            return imageRes;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Projectile", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        public List<Texture2D> skin = new List<Texture2D>();
        public Texture2DArray textureArray = null;
        public float frequencyImage = 2f;
        //public Texture2D skin = null;
        public float velocity = 1f;
        public float frequency = 1f;
        public float lifeTime = 1f;
        public int damage = 0;

        public ProjectilesSpecials.SPECIAL special = ProjectilesSpecials.SPECIAL.NONE;

        public void createTextureArray(Material material)
        {
            if(textureArray == null && skin.Count > 0)
            {

                textureArray = new Texture2DArray(skin[0].width, skin[0].height, skin.Count, skin[0].format, false);

                for (int i = 0; i < skin.Count; i++)
                    Graphics.CopyTexture(skin[i], 0, 0, textureArray, i, 0);
            }
            material.SetTexture("_Textures", textureArray);
        }
    }
}

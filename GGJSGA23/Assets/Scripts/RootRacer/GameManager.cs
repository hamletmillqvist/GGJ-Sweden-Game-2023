using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootRacer
{
    public class GameManager : MonoBehaviour
    {
        private Material worldMaterial;
        public MeshRenderer worldMeshRenderer;
        private float ypos = 0;
        public float currentSpeed = 0.5f;
        public int shaderPropID;
        public static Camera MainCamera;

        void Start()
        {
            MainCamera = FindObjectOfType<Camera>();
            worldMaterial = worldMeshRenderer.material;
            shaderPropID = worldMaterial.shader.GetPropertyNameId(worldMaterial.shader.FindPropertyIndex("_Position"));
        }

        void Update()
        {
            ScrollWorld(Time.deltaTime);
        }
        private void ScrollWorld(float deltaTime)
        {
            ypos -= deltaTime * currentSpeed;
            worldMaterial.SetVector(shaderPropID, new Vector2(0, ypos));
        }
    } 
}

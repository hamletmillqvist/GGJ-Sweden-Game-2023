using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RootRacer
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI depthTM;
        private Material worldMaterial;
        public MeshRenderer worldMeshRenderer;
        private float ypos = 0;
        public float currentSpeed = 0.5f;
        private int shaderPropID;
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
            UpdateDepthText(-ypos);
        }

        private void UpdateDepthText(float ypos)
        {
            depthTM.text = ypos.ToString("0.#m");
        }

        private void ScrollWorld(float deltaTime)
        {
            ypos -= deltaTime * currentSpeed;
            worldMaterial.SetVector(shaderPropID, new Vector2(0, ypos));
        }
    } 
}

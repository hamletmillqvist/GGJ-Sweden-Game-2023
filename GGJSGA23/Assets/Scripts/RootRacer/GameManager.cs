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
        // Start is called before the first frame update
        void Start()
        {
            worldMaterial = worldMeshRenderer.material;
        }

        // Update is called once per frame
        void Update()
        {
            ScrollWorld(Time.deltaTime);
        }
        private void ScrollWorld(float deltaTime)
        {
            ypos -= deltaTime * currentSpeed;
            worldMaterial.SetVector("_Position", new Vector2(0, ypos));
        }
    } 
}

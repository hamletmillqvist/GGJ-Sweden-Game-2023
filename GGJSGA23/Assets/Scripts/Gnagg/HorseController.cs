using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gnagg
{
    public class HorseController : MonoBehaviour
    {
        public Transform meshHolder;
        public Transform mountPoint;
        public float moveSpeed;
        [Header("Jump")]
        public float minJumpHeight;
        public float maxJumpHeight;
        public float timeToJumpApex;
        private Vector3 velocity;

        private Vector3 gravityMax;
        private Vector3 gravityMin;
        private Vector2 movementInput;
        private CharacterController characterController;
        private Vector3 gravity;
        public void Mount(Transform player)
        {
            player.parent = mountPoint;
            player.localPosition = Vector3.zero;
        }
        void Start()
        {
            if (meshHolder == null)
            {
                meshHolder = transform.GetChild(0);
            }
            characterController = GetComponent<CharacterController>();
            gravityMax = new Vector3(0, (-2 * maxJumpHeight) / (timeToJumpApex * timeToJumpApex), 0);
            gravityMin = new Vector3(0, (-2 * minJumpHeight) / (timeToJumpApex * timeToJumpApex), 0);
            gravity = gravityMax;
        }

        private void Update()
        {
            movementInput = Vector2.zero;
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            if (movementInput.y > 0 && velocity.y > 0)
            {
                gravity = gravityMin;
            }
            else
            {
                gravity = gravityMax;
            }
            if (characterController.isGrounded)
            {
                if (velocity.y < 0)
                {
                    velocity.y = 0;
                }
                if (movementInput.y > 0)
                {
                    Jump();                    
                }
            }
            else
            {
                velocity += gravity * Time.deltaTime;
            }
            if (movementInput.x != 0)
            {
                characterController.Move(new Vector3(movementInput.x * moveSpeed * Time.deltaTime, 0, 0));
                meshHolder.localScale = (movementInput.x > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            }

            characterController.Move(velocity * Time.deltaTime);

        }

        private void Jump()
        {
            velocity += (new Vector3(0, Mathf.Sqrt(gravity.y * -3.0f * maxJumpHeight), 0));
        }
        //private void OnGUI()
        //{
        //    GUI.Label(new Rect(10,10,300,20),$"Gravity: {gravity}");
        //}
    }
}

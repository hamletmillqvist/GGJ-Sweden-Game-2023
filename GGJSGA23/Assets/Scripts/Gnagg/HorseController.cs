using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gnagg
{
    public class HorseController : MonoBehaviour
    {
        public float moveSpeed;
        [Header("Jump")]
        public float minJumpHeight;
        public float maxJumpHeight;
        public float timeToJumpApex;
        private Vector3 velocity;

        private Vector3 gravityMax;
        private Vector3 gravityMin;
        private Vector2 movementInput;
        private Rigidbody rb;
        private CharacterController characterController;
        private Vector3 gravity;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            characterController = GetComponent<CharacterController>();
            gravityMax = new Vector3(0, (-2 * maxJumpHeight) / (timeToJumpApex * timeToJumpApex), 0);
            gravityMin = new Vector3(0, (-2 * minJumpHeight) / (timeToJumpApex * timeToJumpApex), 0);
            gravity = gravityMax;
            Debug.Log($"min {gravityMin}, max {gravityMax}");
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
            //if (velocity.y <= 0)
            //{
            //    gravity = gravityMax;
            //}
            //else
            //{
            //    gravity = gravityMin;
            //}
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
            }

            characterController.Move(velocity * Time.deltaTime);

        }

        private void Jump()
        {
            velocity += (new Vector3(0, Mathf.Sqrt(gravity.y * -3.0f * maxJumpHeight), 0));
        }
        private void OnGUI()
        {
            GUI.Label(new Rect(10,10,300,20),$"Gravity: {gravity}");
        }
    }
}

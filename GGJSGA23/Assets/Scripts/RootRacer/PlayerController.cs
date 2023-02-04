using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootRacer
{
    public class PlayerController : MonoBehaviour
    {
        public KeyCode moveLeft;
        public KeyCode moveRight;
        public Color playerColor;
        public float horizontalMoveSpeed;
        public float downSpeed = 0;
        public float boostReduceAmount;

        private Animator headAnimator;
        private new Camera camera;
        private Vector2 screenSize;
        private GameManager gameManager;
        public float baseEatAnimationSpeed = 3;
        private float invertTimer = 0;
        private bool invertControlls = false;
        [Header("Powerups etc")]
        public float invertTime = 5;
        private Vector3 startPosition;
        private void Awake()
        {
            startPosition = transform.position;            
        }
        void Start()
        {
            camera = FindObjectOfType<Camera>();
            GetComponentInChildren<SpriteRenderer>().material.color = playerColor;
            gameManager = FindObjectOfType<GameManager>();
            downSpeed = gameManager.GetTargetSpeed();
            headAnimator = GetComponentInChildren<Animator>();
        }
        public void ResetPlayer()
        {
            transform.position = startPosition;
            downSpeed = gameManager.GetTargetSpeed();
        }
        //public void SetDownSpeed(float speed)
        //{
        //    this.downSpeed = speed;
        //}
        [ContextMenu("Stun Player")]
        public void StunPlayer()
        {
            downSpeed = gameManager.GetTargetSpeed() * 100;
        }
        [ContextMenu("Speed Player")]
        public void SpeedUp(float amount)
        {
            downSpeed -= amount;
        }
        [ContextMenu("Invert Controlls")]
        public void InvertControlls()
        {
            invertTimer = invertTime;
            invertControlls = true;
        }
        private void NormalizeDownSpeed(float deltaTime)
        {
            float targetSpeed = gameManager.GetTargetSpeed();
            if (downSpeed == targetSpeed) return;
            if (downSpeed < targetSpeed)
            {
                downSpeed += boostReduceAmount * deltaTime;
                if (downSpeed > targetSpeed)
                {
                    downSpeed = targetSpeed;
                }
            }
            else if (downSpeed > targetSpeed)
            {
                downSpeed -= boostReduceAmount * deltaTime;
                if (downSpeed < targetSpeed)
                {
                    downSpeed = targetSpeed;
                }
            }
        }
        void Update()
        {
            if (gameManager.IsPaused) return;
            float deltaTime = Time.deltaTime;
            if (invertControlls)
            {
                invertTimer -= deltaTime;
                if (invertTimer <= 0)
                {
                    invertControlls = false;
                }
            }
            float downSpeed = gameManager.GetTargetSpeed();
            float aMulti = (downSpeed + baseEatAnimationSpeed) / baseEatAnimationSpeed;
            headAnimator.SetFloat("AnimationMultiplier", aMulti);
            ControllHorizontalPosition(deltaTime);
            float deltaY = ControlVerticalPosition(deltaTime);

            NormalizeDownSpeed(deltaTime);
            if (IsOutsideOfScreen())
            {
                gameManager.GameOver(this);
            }
        }



        private float ControlVerticalPosition(float deltaTime)
        {
            float deltaY = downSpeed - gameManager.GetTargetSpeed();
            if (deltaY == 0) return 0;
            transform.position += new Vector3(0, deltaY * deltaTime, 0);
            return deltaY;
        }

        private void ControllHorizontalPosition(float deltaTime)
        {
            float xMove = 0;
            if (Input.GetKey(moveLeft))
            {
                xMove -= invertControlls ? -1 : 1;
            }
            if (Input.GetKey(moveRight))
            {
                xMove += invertControlls ? -1 : 1;
            }

            transform.position += new Vector3(horizontalMoveSpeed * xMove * deltaTime, 0, 0);
            Vector3 screenPoint = camera.WorldToScreenPoint(transform.position);

            screenPoint.x = Mathf.Clamp(screenPoint.x, 0, Screen.width);
            transform.position = camera.ScreenToWorldPoint(screenPoint);
        }

        private bool IsOutsideOfScreen()
        {
            Vector3 screenPoint = camera.WorldToScreenPoint(transform.position);
            return screenPoint.y > Screen.height;
        }
    }
}

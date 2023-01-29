using System;
using UnityEngine;

namespace Gnagg {
	public class SpeechBubbleBehaviour : MonoBehaviour {

		[SerializeField] private Transform playerTransform;
		[SerializeField] private float triggerDistance = 5f;
		[SerializeField] private string[] speechLines = Array.Empty<string>();

		private Transform backgroundTransform;
		private Transform textTransform;

		private void Awake() {
			if (playerTransform == null) {
				FindPlayerTransform();
			}

			backgroundTransform = transform.Find("Background");
			var scale = backgroundTransform.localScale;
			scale.x = 5;
			scale.y = 5;
			backgroundTransform.localScale = scale;

			textTransform = transform.Find("Text");
		}

		void Update() {
			var isPlayerClose = GetIsPlayerClose();
			backgroundTransform.gameObject.SetActive(isPlayerClose);
			textTransform.gameObject.SetActive(isPlayerClose);
		}

		private void FindPlayerTransform() {
			var playerController = FindObjectOfType<PlayerController>();

			if (playerController == null) {
				throw new NullReferenceException("No player transform could be found!");
			}

			playerTransform = playerController.transform;
		}

		private bool GetIsPlayerClose() {
			var thisPosition = transform.position;
			var playerPosition = playerTransform.position;
			var distance2D = Vector2.Distance(thisPosition, playerPosition);
			return distance2D <= triggerDistance;
		}
	}
}
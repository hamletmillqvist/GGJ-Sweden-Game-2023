using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootRacer.Behaviours
{
    public class SpeedPickupBehaviour : BaseSpawnedItemBehaviour
    {
        [SerializeField]private float speedUpAmount;
        public override void TriggerEffect(PlayerController playerController)
        {
            playerController.SpeedUp(speedUpAmount);
            Destroy(gameObject);
        }
    } 
}

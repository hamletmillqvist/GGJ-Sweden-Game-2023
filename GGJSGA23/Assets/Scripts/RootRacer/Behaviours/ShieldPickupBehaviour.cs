namespace RootRacer.Behaviours
{
    public class ShieldPickupBehaviour : BaseSpawnedItemBehaviour
    {
        public override void TriggerEffect(PlayerController playerController)
        {
            playerController.Shield();
            Destroy(gameObject);
        }
    } 
}

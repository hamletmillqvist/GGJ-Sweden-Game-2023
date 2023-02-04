namespace RootRacer.Behaviours
{
    public class SizeUpPickupBehaviour : BaseSpawnedItemBehaviour
    {
        public override void TriggerEffect(PlayerController playerController)
        {
            foreach (var player in GameManager.Players)
            {
                if (player == playerController)
                {
                    continue;
                }
                player.SizeUp();
            }
            Destroy(gameObject);
        }
    } 
}

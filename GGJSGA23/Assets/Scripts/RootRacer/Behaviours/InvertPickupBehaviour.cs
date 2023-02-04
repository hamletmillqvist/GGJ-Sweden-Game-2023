namespace RootRacer.Behaviours
{
    public class InvertPickupBehaviour : BaseSpawnedItemBehaviour
{
        public override void TriggerEffect(PlayerController playerController)
        {
            foreach (var player in GameManager.Players)
            {
                if (player == playerController)
                {
                    continue;
                }
                player.InvertControls();
            }
            Destroy(gameObject);
        }
    } 
}

namespace RootRacer.Behaviours
{
	public class ShieldPickupBehaviour : BaseSpawnedItemBehaviour
	{
		public override void TriggerEffect(PlayerController playerController)
		{
			base.TriggerEffect(playerController);
			playerController.Shield();
			Destroy(gameObject);
		}
	}
}
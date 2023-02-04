namespace RootRacer.Behaviours
{
	public class RockBehaviour : BaseSpawnedItemBehaviour
	{
		public override void TriggerEffect(PlayerController playerController)
		{
			base.TriggerEffect(playerController);
			playerController.StunPlayer();
			Destroy(gameObject);
		}
	}
}
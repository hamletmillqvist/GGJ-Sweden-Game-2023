namespace RootRacer.Behaviours
{
	public class RockBehaviour : BaseSpawnedItemBehaviour
	{
		public override void TriggerEffect(PlayerController playerController)
		{
			playerController.StunPlayer();
			Destroy(gameObject);
		}
	}
}
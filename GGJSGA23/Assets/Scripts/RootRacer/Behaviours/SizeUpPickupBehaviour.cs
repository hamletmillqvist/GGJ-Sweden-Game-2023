namespace RootRacer.Behaviours
{
	public class SizeUpPickupBehaviour : BaseSpawnedItemBehaviour
	{
		public override void TriggerEffect(PlayerController playerController)
		{
			base.TriggerEffect(playerController);

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
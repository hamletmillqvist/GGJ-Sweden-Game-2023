namespace RootRacer.Behaviours
{
	public class InvertPickupBehaviour : BaseSpawnedItemBehaviour
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

				player.InvertControls();
			}

			Destroy(gameObject);
		}
	}
}
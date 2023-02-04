using UnityEngine;

namespace RootRacer.Utils
{
	public static class OrthographicCameraPositionUtil
	{
		public static CameraBorderResult GetCameraCorners(Camera camera)
		{
			var topRight = camera.ScreenToWorldPoint(new Vector3(camera.scaledPixelWidth, camera.scaledPixelHeight));
			var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0));

			return new CameraBorderResult
			{
				Top = topRight.y,
				Right = topRight.x,
				Bottom = bottomLeft.y,
				Left = bottomLeft.x,
			};
		}
	}

	public struct CameraBorderResult
	{
		public float Left { get; set; }
		public float Right { get; set; }
		public float Bottom { get; set; }
		public float Top { get; set; }

		public Vector2 BottomLeft => new(Left, Bottom);
		public Vector2 BottomRight => new(Right, Bottom);
		public Vector2 TopLeft => new(Left, Top);
		public Vector2 TopRight => new(Right, Top);
	}
}
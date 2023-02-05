using System;
using Sonity;
using UnityEngine;

[CreateAssetMenu()]
public class DepthMusicSO : ScriptableObject
{
	public DepthMusic[] gameDepthMusic;

	public int SetDepthMusic(int currentIndex, float depth)
	{
		var selectedIndex = currentIndex;

		for (var i = currentIndex; i < gameDepthMusic.Length; i++)
		{
			var depthMusic = gameDepthMusic[i];
			if (depth > depthMusic.depth)
			{
				selectedIndex = i;
			}
		}

		if (selectedIndex != currentIndex)
		{
			gameDepthMusic[currentIndex].music.Stop2D();
			gameDepthMusic[selectedIndex].music.Play2D();
			currentIndex = selectedIndex;
		}

		return currentIndex;
	}
}

[Serializable]
public class DepthMusic
{
	public SoundEvent music;
	public float depth;
}
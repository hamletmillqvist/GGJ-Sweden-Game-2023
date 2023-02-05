using Sonity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class DepthMusicSO : ScriptableObject
{
    public DepthMusic[] gameDepthMusic;
    public int SetDepthMusic(int currentIndex, float depth)
    {
        //DepthMusic deepestSelectedMusic = gameDepthMusic[currentlyPlayingDepthMusic];
        int selectedIndex = currentIndex;

        for (int i = currentIndex; i < gameDepthMusic.Length; i++)
        {
            DepthMusic depthMusic = gameDepthMusic[i];
            if (depth > depthMusic.depth)
            {
                //deepestSelectedMusic = depthMusic;
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
[System.Serializable]
public class DepthMusic
{
    public SoundEvent music;
    public float depth;
}

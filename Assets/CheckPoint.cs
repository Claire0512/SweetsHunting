using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    private static Transform lastCheckpoint;
    public AudioClip checkpointCollectAudio;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastCheckpoint = transform;
            
            AudioSource.PlayClipAtPoint(checkpointCollectAudio, transform.position);
        }
    }

    public static Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpoint ? lastCheckpoint.position : new Vector3(3.48f, -0.5396699f, 1);
    }
}

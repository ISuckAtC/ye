using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController sounds;
    public AudioClip GunFire, Hurt, Death, LevelComplete, LevelStart, LevelFail, Fart, Tackle, ShootGround, ShootEnemy, Raid, Slam;
    // Start is called before the first frame update

    void Awake()
    {
        sounds = this;
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }
}

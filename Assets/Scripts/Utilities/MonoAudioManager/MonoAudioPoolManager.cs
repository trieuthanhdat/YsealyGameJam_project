//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class MonoAudioPoolManager : MonoSingleton<MonoAudioPoolManager>
//{
//    [SerializeField] int poolSize = 8;

//    private Queue<MonoAudioPlayer> pool;

//    public void CreateQueue()
//    {
//        pool = new Queue<MonoAudioPlayer>(poolSize);
        
//    }
//    public void AddToQueue(MonoAudioPlayer audioPlayerPrefab)
//    {
//        if (pool.Count >= poolSize) return ;

//        for (int i = 0; i < poolSize; i++)
//        {
//            MonoAudioPlayer player = Instantiate(audioPlayerPrefab, transform);
//            player.gameObject.SetActive(false);
//            pool.Enqueue(player);
//        }
//    }
//    public bool IsFullQueue()
//    {
//        if (pool.Count >= poolSize) return true;

//        return false;
//    }

//    public MonoAudioPlayer GetFromPool(Sound sound, MonoAudioPlayer audioPlayerPrefab)
//    {
//        MonoAudioPlayer player = null;
//        foreach (MonoAudioPlayer p in pool)
//        {
//            if (!p.gameObject.activeSelf && p.sound == sound)
//            {
//                player = p;
//                break;
//            }
//        }

//        if (player != null)
//        {
//            player.gameObject.SetActive(true);
//        }else
//        {
//            player.sound = sound;
//            player.fadeInTimer = sound.fadeInTimer;
//            player.fadeOutTimer = sound.fadeOutTimer;

//            //SETUP SOUND
//            sound.audioSrc.clip = sound.clip;
//            sound.audioSrc.playOnAwake = sound.playOnAwake;
//            sound.audioSrc.volume = sound.volume;
//            sound.audioSrc.pitch = sound.pitch;

//        }
        
//        return player;
//    }

//    public void RemoveFromPool(MonoAudioPlayer player)
//    {
//        player.gameObject.SetActive(false);
//    }


//}

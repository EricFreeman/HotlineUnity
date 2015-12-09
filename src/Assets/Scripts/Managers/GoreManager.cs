using System.Collections.Generic;
using Assets.Scripts.Context;
using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts.Managers
{
    public class GoreManager : MonoBehaviour, IListener<SpawnBloodMessage>
    {
        public GameObject SimpleObject;
        public Sprite BloodPool;
        public Sprite BloodSplatter;

        public List<Sprite> Gibs;

        void Start()
        {
            this.Register<SpawnBloodMessage>();
        }

        void OnDestroy()
        {
            this.UnRegister<SpawnBloodMessage>();
        }


        public void Handle(SpawnBloodMessage message)
        {
            // Blood Pool
            for (var i = 0; i < message.Amount; i++)
            {
                var blood = Instantiate(SimpleObject);
                var spriteRenderer = blood.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = BloodSplatter;
                spriteRenderer.sortingOrder = LevelContext.BloodLayer;
                blood.transform.position = message.Position;
            }

            // Blood Bullet Path

            // Gibs!
        }
    }
}
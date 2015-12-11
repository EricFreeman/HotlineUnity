using System.Collections.Generic;
using Assets.Scripts.Context;
using Assets.Scripts.Messages;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts.Managers
{
    public class GoreManager : MonoBehaviour, IListener<SpawnBloodMessage>
    {
        public GameObject SimpleObject;
        public List<Sprite> BloodPool;
        public List<Sprite> BloodSplatter;

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
            var amount = Mathf.Min(3, Random.Range(message.Damage, message.Damage * 3));

            // Blood Pool
            for (var i = 0; i < amount; i++)
            {
                var blood = Instantiate(SimpleObject);
                var spriteRenderer = blood.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = BloodPool.Random();
                spriteRenderer.sortingOrder = LevelContext.BloodLayer;
                blood.transform.position = message.Position.ApplyFunction(() => Random.Range(-message.Force, message.Force) / 45f);

                var opacity = Random.Range(.5f, 1f);
                spriteRenderer.color = new Color(255, 255, 255, opacity);
            }

            // Blood Bullet Path
            for (var i = 0; i < amount; i++)
            {
                var blood = Instantiate(SimpleObject);
                var spriteRenderer = blood.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = BloodSplatter.Random();
                spriteRenderer.sortingOrder = LevelContext.BloodLayer;
                blood.transform.position = message.Position + (message.Direction * (i + message.Force / 15) / 3).ApplyFunction(() => Random.Range(-message.Force, message.Force) / 45f);
                spriteRenderer.transform.forward = message.Direction + new Vector3(0, Random.Range(-10f, 10f), 0);

                var opacity = Random.Range(.5f, 1f);
                spriteRenderer.color = new Color(255, 255, 255, opacity);
            }

            // Gibs!
        }
    }
}
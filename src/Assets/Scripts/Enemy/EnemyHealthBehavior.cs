using System.Collections.Generic;
using Assets.Scripts.Context;
using Assets.Scripts.People;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyHealthBehavior : MonoBehaviour, IDamageBehavior
    {
        public GameObject SimpleObject;
        public List<Sprite> DeadSprite;

        void Update()
        {

        }

        public void OnHit()
        {
        }

        public void OnDeath()
        {
            var body = Instantiate(SimpleObject);
            body.transform.position = transform.position;
            body.transform.rotation = transform.rotation;

            var spriteRenderer = body.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = DeadSprite.Random();
            spriteRenderer.sortingOrder = LevelContext.BloodLayer;

            Destroy(gameObject);
        }
    }
}
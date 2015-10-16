using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Gun : MonoBehaviour
    {
        public int Ammunition;
        public GameObject BulletGameObject;

        public float ShotDelay = .1f;
        private float _lastShot;
        
        public void Fire()
        {
            if (CanFire())
            {
                var bullet = Instantiate(BulletGameObject);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<Bullet>().Damage = Random.Range(1, 5);

                _lastShot = Time.fixedTime;

                Ammunition--;
            }
        }

        private bool CanFire()
        {
            var ammunitionIsNotEmpty = Ammunition > 0;
            var itHasBeenLongEnoughSinceTheLastShot = Time.fixedTime - _lastShot > ShotDelay;
            
            return ammunitionIsNotEmpty && itHasBeenLongEnoughSinceTheLastShot;
        }
    }
}
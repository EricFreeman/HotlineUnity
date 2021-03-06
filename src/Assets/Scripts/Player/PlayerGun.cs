﻿using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerGun : MonoBehaviour
    {
        public Gun _gun;

        void Start()
        {
            _gun = GetComponent<Gun>();
        }

        void Update ()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _gun.Fire();
            }
        }
    }
}
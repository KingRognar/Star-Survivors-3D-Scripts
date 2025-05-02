using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Stats_Scr : MonoBehaviour
{
    public static Player_Stats_Scr instance;

    //private int stat_1 = 1;


    //TODO: нужно определится с Base Damage и всяким таким или немного поменять систему

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static class Ship
    {
        public static int maxHp = 10;
        public static int curHp = 10;
        public static int armor = 0;
        public static float ExpCollectorRadius = 5f;
        public static float damageMultiplier
        {
            get { return damageMultiplier_; }
            set
            {
                damageMultiplier_ = value;

                Machinegun.damage = (int)(Machinegun.damage * damageMultiplier_);
                //Debug.Log(Machinegun.bulletDamage + " | " + damageMultiplier_);
            }

        }
        private static float damageMultiplier_ = 1f;
        public static float firerateMultiplier
        {
            get { return firerateMultiplier_; }
            set
            {
                firerateMultiplier_ = value;
                Machinegun.spawnDelay *= firerateMultiplier_;
                CircleBots.bulletSpawnDelay *= firerateMultiplier_;
            }
        }
        private static float firerateMultiplier_ = 1f;
    }

    public static class Machinegun
    {
        public static float spawnDelay = 0.3f; //0.5f
        public static float spawnDelayMod = 1f;
        public static float spreadAngle = 5f;
        public static int damage = 15; //5
        public static float projectileScale = 1f;
        public static int projectileCount = 1;
    }
    public static class CircleBots
    {
        public static float bulletSpawnDelay = 2f;
        public static int bulletDamage = 5;
        public static int botsCount = 3;
        public static float rotationSpeed = 100f;
        public static float circlingDistance = 1f;
        //public Vector3 circleCenter;
        //public bool allBotsIsFiring;
    }
    public static class ChainLightningGun
    {
        public static float spawnDelay = 0.5f;
        public static int baseDamage = 3;
        public static float damageMultiplier = 1f;
        public static int chainsCount = 4;
        public static float chainDistance = 1.5f;
    }
    public static class Grinder
    {
        public static float spawnDelay = 1f;
        public static int damage = 10;
        public static float projectileScale = 1f;
        public static int projectileCount = 1;
        public static int collisionsNum = 3;
    }
    public static class RocketLauncher
    {
        public static float spawnDelay = 0.8f;
        public static int damage = 10;
        public static float projectileScale = 1f;
        public static int projectileCount = 1;
        public static float projectileSpeed = 10f;
    }
    public static class LazerDrones
    {
        public static int damage = 1;
        public static float timeBetweenTicks = 0.5f;
    }
}










using System;
using UnityEngine;
using System.Collections;

public class BuildView :MonoBehaviour {

    public EntityController.Faction faction;

    public BaseUnit.UnitType spawnUnitType;

    public BaraksUnitConstructor[] baraksUnitConstructor;

    public GameObject spawnPosition;

#if UNITY_EDITOR
    [ReadOnly]
#endif

    public int buildLevel;

    [SerializeField]
    public int BuildLevel {
        get {
            return buildLevel; }
    }

    [Serializable]
    public class BaraksUnitConstructor {

        [SerializeField]
        public int upgradeCost;
        [SerializeField]
        public float trainingTime;
        [SerializeField]
        public int hp;
        [SerializeField]
        [Range( 0, 1 )]
        public float armor;
        [SerializeField]
        public float attack;
        [SerializeField]
        public float attackSpeed;
        [SerializeField]
        public float speed;
        [SerializeField]
        public float attackRange;
        [SerializeField]
        public int gold;
        [SerializeField]
        public int xp;

    }

    public BaseUnit.UnitCharacteristics UpgradeBuilding () {
        ++buildLevel;
        return GetUnit();
    }

    public BaseUnit.UnitCharacteristics GetUnit () {
        BaseUnit.UnitCharacteristics unitCharacteristics = new BaseUnit.UnitCharacteristics();

        unitCharacteristics.hp = baraksUnitConstructor[buildLevel].hp;
        unitCharacteristics.armor = baraksUnitConstructor[buildLevel].armor;
        unitCharacteristics.attack = baraksUnitConstructor[buildLevel].attack;
        unitCharacteristics.attackSpeed = baraksUnitConstructor[buildLevel].attackSpeed;
        unitCharacteristics.speed = baraksUnitConstructor[buildLevel].speed;
        unitCharacteristics.attackRange = baraksUnitConstructor[buildLevel].attackRange;

        return unitCharacteristics;

    }

}

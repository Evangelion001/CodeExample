using UnityEngine;
using System;

[Serializable]
public class Spell {
    [SerializeField]
    public float damage;
    [SerializeField]
    public float healing;
    [SerializeField]
    public float attackRange;
    [SerializeField]
    public float aoeRadius;
    [SerializeField]
    public bool needTarget;
    [SerializeField]
    public Effect effect;
    [SerializeField]
    public GameObject visualPrefab;
}

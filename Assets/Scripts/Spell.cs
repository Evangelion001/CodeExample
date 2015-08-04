using UnityEngine;
using System;

[Serializable]
public class Spell {
    [SerializeField]
    public float damage = 0;
    [SerializeField]
    public float healing = 0;
    [SerializeField]
    public float attackRange = 0;
    [SerializeField]
    public float aoeRadius = 0;
    [SerializeField]
    public bool needTarget = false;
    [SerializeField]
    public TimeEffect effect = null;
    [SerializeField]
    public GameObject visualPrefab = null;
    [SerializeField]
    public float cdTime = 0;
    [SerializeField]
    public bool cd = false;
}

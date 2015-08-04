using UnityEngine;
using System.Collections.Generic;

public class EffectsArea : MonoBehaviour {

    void Start () {
        InvokeRepeating( "Healing",1,1 );
    }

    public EntityController.Faction faction;

    private List<UnitViewPresenter> units = new List<UnitViewPresenter>();

    void OnTriggerEnter ( Collider other ) {
        Debug.Log( "other: " + other.name );
        if ( other.GetComponent<UnitViewPresenter>() && other.GetComponent<UnitViewPresenter>().faction == faction ) {
            Influence influence = new Influence();
            influence.timeEffect = EfffectOn( 99999 );

            units.Add( other.GetComponent<UnitViewPresenter>() );

            other.GetComponent<UnitViewPresenter>().GetDamage( influence );
        }
    }

    void OnTriggerExit ( Collider other ) {
        if ( other.GetComponent<UnitViewPresenter>() && other.GetComponent<UnitViewPresenter>().faction == faction ) {
            Influence influence = new Influence();
            influence.timeEffect = EfffectOn( 1 );

            units.Remove( other.GetComponent<UnitViewPresenter>() );

            other.GetComponent<UnitViewPresenter>().GetDamage( influence );
        }
    }


    private void Healing () {
        foreach ( var key in units ) {
            Influence influence = new Influence();
            influence.healing = 5;
            key.GetDamage( influence );
        }
    }

    private TimeEffect EfffectOn (int time) {
        GameObject immortalityEffect = (GameObject)Resources.Load( "Prefabs/Particles/Immortality" );

        if ( time == 1 ) {
            immortalityEffect = null;
        }

        EffectsController effectsController = new EffectsController();

        TimeEffect effect = new TimeEffect( effectsController );
        effect.id = 99;
        effect.name = "Immortality";
        effect.visualPrefab = immortalityEffect;
        effect.duration = time;

        effect.characteristicsModifiers.armor = 1;
        effect.characteristicsModifiers.attack = 1;
        effect.characteristicsModifiers.attackRange = 1;
        effect.characteristicsModifiers.attackSpeed = 1;
        effect.characteristicsModifiers.speed = 1;
        effect.characteristicsModifiers.armor = 1;
        effect.characteristicsModifiers.hp = 1;

        return effect;

    }
}

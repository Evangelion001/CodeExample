using UnityEngine;

public class UnitViewPresenter : MonoBehaviour {

    public GameObject selectCircle;
    public GameObject selectTarget;
    public NavMeshAgent navMeshAgent;
    public BaseUnitBehaviour baseUnitBehaviour;
    public BaseUnit.UnitType unitType;
    public Animation _animation;
    public EntityController.Faction faction;

    private BaseUnitController.SelectUnit selectUnit;
    private BaseUnit.DamageDelegate damageDelegate;
    private Player player;

    private GameObject effectParticle;

    public void SetEffectParticle (GameObject effectParticle ) {
        GameObject temp = Instantiate( effectParticle );
        this.effectParticle = temp;
        this.effectParticle.transform.position = Vector3.zero;
        this.effectParticle.transform.SetParent( transform, false );
        this.effectParticle.GetComponent<ParticleSystem>().Play();
    }

    public void DeleteEffectParticle () {
        if ( effectParticle != null ) {
            Destroy( effectParticle );
        }
    }

    public void AddSelectDelegate ( BaseUnitController.SelectUnit selectUnit ) {
        this.selectUnit = selectUnit;
    }

    public void AddDamageDelegate ( BaseUnit.DamageDelegate damageDelegate ) {
        this.damageDelegate = damageDelegate;
    }

    public void Select () {
        selectUnit();
    }

    public void ShowCircle () {
        selectCircle.SetActive( true );
    }

    public void ShowTarget () {
        selectTarget.SetActive( true );
    }

    public void HideCircle () {
        selectCircle.SetActive( false );
    }

    public void HideTarget () {
        selectTarget.SetActive( false );
    }

    public void GetDamage (Influence influence ) {
        damageDelegate( influence );
    }
    //FIXME Remove 
    public void GetPlayer (Player player) {
        this.player = player;
    }
    //FIXME Remove 
    public void GetGold (int gold) {
        //FIXME rename method
        player.Gold = gold;
    }

}

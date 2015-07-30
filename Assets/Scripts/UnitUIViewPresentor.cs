using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitUIViewPresentor : MonoBehaviour {

    [SerializeField]
    private GameObject unitPanel;
    [SerializeField]
    private Text hpValue;
    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private Text type;
    [SerializeField]
    private Text armor;
    [SerializeField]
    private Text attack;
    [SerializeField]
    private Text attackSpeed;
    [SerializeField]
    private Text attackRange;
    [SerializeField]
    private Text speed;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private GameObject levelPanel;
    [SerializeField]
    private Text level;

    public void SetHp (int currentHp, int maxHp) {

        hpValue.text = currentHp.ToString() + "/" + maxHp;
        
    }

    public string Type {
        set {
            type.text = value.ToString();
        }
    }

    public float Armor {
        set {
            armor.text = value.ToString();
        }
    }

    public int Attack {
        set {
            attack.text = value.ToString();
        }
    }

    public float AttackSpeed {
        set {
            attackSpeed.text = value.ToString();
        }
    }

    public float AttackRange {
        set {
            attackRange.text = value.ToString();
        }
    }

    public float Speed {
        set {
            speed.text = value.ToString();
        }
    }

    public GameObject UnitPanel {
        get {
            return unitPanel;
        }
    }

    public Sprite Icon {
        set {
            icon.sprite = value;
        }
    }

    public GameObject LevelPanel {
        get {
            return levelPanel;
        }
    }

    public int Level {
        set {
            level.text = value.ToString();
        }
    }
}

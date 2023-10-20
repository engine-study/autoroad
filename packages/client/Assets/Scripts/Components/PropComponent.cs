using UnityEngine;
using mudworld;
using mud;

public abstract class PropComponent : ValueComponent {

    [Header("Prop")]
    public SPAnimationProp propPrefab;

    protected override void PostInit() {
        base.PostInit();

        propPrefab.gameObject.SetActive(false);

        PlayerMUD player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.Animator.SetDefaultProp(propPrefab);
        player.Animator.ToggleProp(true, propPrefab);

    }

    protected override StatType SetStat(IMudTable update){ return StatType.None; }
    protected override float SetValue(IMudTable update) {return 1;}
    protected override string SetString(IMudTable update){ return "";}

}

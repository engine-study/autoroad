using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class StickComponent : MUDComponent {


    public SPAnimationProp propPrefab;

    protected override void PostInit() {
        base.PostInit();

        PlayerMUD player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.Animator.defaultPropPrefab = propPrefab;
        player.Animator.ToggleProp(true, propPrefab);

    }
    protected override IMudTable GetTable() {return new StickTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        StickTable table = update as StickTable;
    }

}

using UnityEngine;
using DefaultNamespace;
using mud.Client;

public abstract class PropComponent : MUDComponent {

    [Header("Prop")]
    public SPAnimationProp propPrefab;

    protected override void PostInit() {
        base.PostInit();

        propPrefab.gameObject.SetActive(false);

        PlayerMUD player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.Animator.SetDefaultProp(propPrefab);
        player.Animator.ToggleProp(true, propPrefab);

    }

}

using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class RobeComponent : MUDComponent {

    [Header("Robe")]
    public GameObject body;


    protected override IMudTable GetTable() {return new RobeTable();}

    protected override void PostInit() {
        base.PostInit();

        Cosmetic robeCosmetic = Entity.GetMUDComponent<PlayerComponent>().PlayerScript.bodyCosmetic;
        robeCosmetic.SetNewBody(body);
        
    }
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        RobeTable table = update as RobeTable;

    }

}

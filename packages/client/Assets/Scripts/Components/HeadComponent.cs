using UnityEngine;
using mudworld;
using mud;

public class HeadComponent : MUDComponent {

    [Header("Head")]
    public GameObject body;


    protected override IMudTable GetTable() {return new HeadTable();}

    protected override void PostInit() {
        base.PostInit();

        Cosmetic head = Entity.GetMUDComponent<PlayerComponent>().PlayerScript.headCosmetic;
        head.SetNewBody(body);
        
    }
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        HeadTable table = update as HeadTable;

    }

}

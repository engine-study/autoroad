using UnityEngine;
using DefaultNamespace;
using mud;

public class GameEventComponent : MUDComponent {
    
    public string GameEvent {get{return eventName;}}
    private string eventName;

    protected override IMudTable GetTable() {return new GameEventTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        GameEventTable table = update as GameEventTable;

        eventName = (string)table.eventType;

    }

}

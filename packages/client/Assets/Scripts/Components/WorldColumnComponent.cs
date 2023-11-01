using mud;
using mudworld;

public class WorldColumnComponent : MUDComponent
{
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        Entity.SetName("World Column");
    }

}

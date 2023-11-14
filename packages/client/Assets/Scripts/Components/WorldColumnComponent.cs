using mud;
using mudworld;

public class WorldColumnComponent : MUDComponent
{
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {
        Entity.SetName("World Column");
    }

}

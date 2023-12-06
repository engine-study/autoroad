using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
public class WeightUI : SPWindowParent
{
    public static WeightUI Instance;

    [Header("Weight UI")]
    [SerializeField] MoveTypeUI [] moves;
    [SerializeField] List<Mover> activeMovers;

    public override void Init() {
        if(HasInit) return;
        base.Init();

        Instance = this;
        ToggleWindowClose();

    }

    protected override void Destroy() {
        base.Destroy();
        Instance = null;
    }

    public void ToggleWeights(bool toggle, List<Mover> movers) {

        activeMovers = movers;

        ToggleWindow(toggle);

        if(toggle) {
            for(int i = 0; i < moves.Length; i++) {
                if(i < movers.Count) {
                    moves[i].SetMove(movers[i].moveType, movers[i].weight, movers[i].cannotMove);
                    moves[i].Position.SetFollow(movers[i].target);
                } else {
                    moves[i].ToggleWindowClose();
                }
            }
        }


    }

}

[System.Serializable]
public class Mover {
    public bool cannotMove;
    public Transform target;
    public int weight;
    public MoveType moveType;
}

using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class BoulderComponent : MUDComponent {

 

    [Header("Boulder")]
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject heavy;

    [Header("Debug")]
    [SerializeField] private bool isBigRock;
    [SerializeField] private WeightComponent weight;

    protected override IMudTable GetTable() {return new BoulderTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        // BoulderTable table = update as BoulderTable;
    }

    protected override void PostInit() {
        base.PostInit();

        weight = Entity.GetMUDComponent<WeightComponent>();
        isBigRock = weight.Weight > 3;

        normal.SetActive(!isBigRock);
        heavy.SetActive(isBigRock);

    }

}

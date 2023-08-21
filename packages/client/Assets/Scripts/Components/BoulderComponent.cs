using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class BoulderComponent : MUDComponent {

 

    [Header("Boulder")]
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject heavy;
    [SerializeField] private GameObject pillar;

    [Header("Debug")]
    [SerializeField] private WeightComponent weight;

    protected override IMudTable GetTable() {return new BoulderTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        // BoulderTable table = update as BoulderTable;
    }

    protected override void PostInit() {
        base.PostInit();

        weight = Entity.GetMUDComponent<WeightComponent>();

        normal.SetActive(weight.Weight < 5);
        heavy.SetActive(weight.Weight == 5);
        pillar.SetActive(weight.Weight > 5);

    }

}
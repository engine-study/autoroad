using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUI : WindowEntity
{
    [Header("Stat")]
    public TMPro.TextMeshProUGUI textField;

    public void SetValue(string newStat) {
        textField.text = newStat;
    }
}

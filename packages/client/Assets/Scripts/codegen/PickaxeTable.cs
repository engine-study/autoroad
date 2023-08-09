/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud.Client;
using mud.Network.schemas;
using mud.Unity;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PickaxeTableUpdate : TypedRecordUpdate<Tuple<PickaxeTable?, PickaxeTable?>> { }

    public class PickaxeTable : IMudTable
    {
        public readonly static TableId ID = new("", "Pickaxe");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(PickaxeTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(PickaxeTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (bool)functionParameters[0];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var valueValue = (bool)table["value"];

            value = valueValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            PickaxeTableUpdate update = (PickaxeTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new PickaxeTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<PickaxeTable?, PickaxeTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            PickaxeTable? current = null;
            PickaxeTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new PickaxeTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new PickaxeTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new PickaxeTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new PickaxeTable { value = null, };
                }
            }

            return new Tuple<PickaxeTable?, PickaxeTable?>(current, previous);
        }
    }
}

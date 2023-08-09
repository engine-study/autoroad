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
    public class DamageTableUpdate : TypedRecordUpdate<Tuple<DamageTable?, DamageTable?>> { }

    public class DamageTable : IMudTable
    {
        public readonly static TableId ID = new("", "Damage");

        public override TableId GetTableId()
        {
            return ID;
        }

        public long? value;

        public override Type TableType()
        {
            return typeof(DamageTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(DamageTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (long)(int)functionParameters[0];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var valueValue = (long)table["value"];

            value = valueValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            DamageTableUpdate update = (DamageTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new DamageTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<DamageTable?, DamageTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            DamageTable? current = null;
            DamageTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new DamageTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (long)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new DamageTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new DamageTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (long)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new DamageTable { value = null, };
                }
            }

            return new Tuple<DamageTable?, DamageTable?>(current, previous);
        }
    }
}

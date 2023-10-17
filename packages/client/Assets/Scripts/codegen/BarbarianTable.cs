/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using mud.Network.schemas;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class BarbarianTableUpdate
        : TypedRecordUpdate<Tuple<BarbarianTable?, BarbarianTable?>> { }

    public class BarbarianTable : IMudTable
    {
        public readonly static TableId ID = new("", "Barbarian");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(BarbarianTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(BarbarianTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            BarbarianTable other = (BarbarianTable)obj;

            if (other == null)
            {
                return false;
            }
            if (value != other.value)
            {
                return false;
            }
            return true;
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
            BarbarianTableUpdate update = (BarbarianTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new BarbarianTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<BarbarianTable?, BarbarianTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            BarbarianTable? current = null;
            BarbarianTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new BarbarianTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new BarbarianTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new BarbarianTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new BarbarianTable { value = null, };
                }
            }

            return new Tuple<BarbarianTable?, BarbarianTable?>(current, previous);
        }
    }
}

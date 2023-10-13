/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using mud.Network.schemas;
using mud.Unity;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class BonesTableUpdate : TypedRecordUpdate<Tuple<BonesTable?, BonesTable?>> { }

    public class BonesTable : IMudTable
    {
        public readonly static TableId ID = new("", "Bones");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(BonesTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(BonesTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            BonesTable other = (BonesTable)obj;

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
            BonesTableUpdate update = (BonesTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new BonesTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<BonesTable?, BonesTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            BonesTable? current = null;
            BonesTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new BonesTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new BonesTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new BonesTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new BonesTable { value = null, };
                }
            }

            return new Tuple<BonesTable?, BonesTable?>(current, previous);
        }
    }
}

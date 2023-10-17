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
    public class SeekerTableUpdate : TypedRecordUpdate<Tuple<SeekerTable?, SeekerTable?>> { }

    public class SeekerTable : IMudTable
    {
        public readonly static TableId ID = new("", "Seeker");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(SeekerTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(SeekerTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            SeekerTable other = (SeekerTable)obj;

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
            value = (ulong)(int)functionParameters[0];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var valueValue = (ulong)table["value"];
            value = valueValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            SeekerTableUpdate update = (SeekerTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new SeekerTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<SeekerTable?, SeekerTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            SeekerTable? current = null;
            SeekerTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new SeekerTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new SeekerTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new SeekerTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new SeekerTable { value = null, };
                }
            }

            return new Tuple<SeekerTable?, SeekerTable?>(current, previous);
        }
    }
}

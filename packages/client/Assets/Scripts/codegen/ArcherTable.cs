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
    public class ArcherTableUpdate : TypedRecordUpdate<Tuple<ArcherTable?, ArcherTable?>> { }

    public class ArcherTable : IMudTable
    {
        public readonly static TableId ID = new("", "Archer");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(ArcherTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ArcherTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            ArcherTable other = (ArcherTable)obj;

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
            ArcherTableUpdate update = (ArcherTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new ArcherTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<ArcherTable?, ArcherTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            ArcherTable? current = null;
            ArcherTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new ArcherTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new ArcherTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new ArcherTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new ArcherTable { value = null, };
                }
            }

            return new Tuple<ArcherTable?, ArcherTable?>(current, previous);
        }
    }
}

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
    public class BootsTableUpdate : TypedRecordUpdate<Tuple<BootsTable?, BootsTable?>> { }

    public class BootsTable : IMudTable
    {
        public readonly static TableId ID = new("", "Boots");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(BootsTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(BootsTableUpdate);
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
            BootsTableUpdate update = (BootsTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new BootsTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<BootsTable?, BootsTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            BootsTable? current = null;
            BootsTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new BootsTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new BootsTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new BootsTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new BootsTable { value = null, };
                }
            }

            return new Tuple<BootsTable?, BootsTable?>(current, previous);
        }
    }
}

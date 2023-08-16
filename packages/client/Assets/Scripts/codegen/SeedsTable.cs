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
    public class SeedsTableUpdate : TypedRecordUpdate<Tuple<SeedsTable?, SeedsTable?>> { }

    public class SeedsTable : IMudTable
    {
        public readonly static TableId ID = new("", "Seeds");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(SeedsTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(SeedsTableUpdate);
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
            SeedsTableUpdate update = (SeedsTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new SeedsTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<SeedsTable?, SeedsTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            SeedsTable? current = null;
            SeedsTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new SeedsTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new SeedsTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new SeedsTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new SeedsTable { value = null, };
                }
            }

            return new Tuple<SeedsTable?, SeedsTable?>(current, previous);
        }
    }
}

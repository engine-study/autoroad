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
    public class AggroTableUpdate : TypedRecordUpdate<Tuple<AggroTable?, AggroTable?>> { }

    public class AggroTable : IMudTable
    {
        public readonly static TableId ID = new("", "Aggro");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(AggroTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(AggroTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            AggroTable other = (AggroTable)obj;

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
            AggroTableUpdate update = (AggroTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new AggroTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<AggroTable?, AggroTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            AggroTable? current = null;
            AggroTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new AggroTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new AggroTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new AggroTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new AggroTable { value = null, };
                }
            }

            return new Tuple<AggroTable?, AggroTable?>(current, previous);
        }
    }
}

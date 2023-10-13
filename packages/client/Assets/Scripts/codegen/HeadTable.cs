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
    public class HeadTableUpdate : TypedRecordUpdate<Tuple<HeadTable?, HeadTable?>> { }

    public class HeadTable : IMudTable
    {
        public readonly static TableId ID = new("", "Head");

        public override TableId GetTableId()
        {
            return ID;
        }

        public long? value;

        public override Type TableType()
        {
            return typeof(HeadTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(HeadTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            HeadTable other = (HeadTable)obj;

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
            HeadTableUpdate update = (HeadTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new HeadTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<HeadTable?, HeadTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            HeadTable? current = null;
            HeadTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new HeadTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (long)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new HeadTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new HeadTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (long)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new HeadTable { value = null, };
                }
            }

            return new Tuple<HeadTable?, HeadTable?>(current, previous);
        }
    }
}

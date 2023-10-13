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
    public class TreeTableUpdate : TypedRecordUpdate<Tuple<TreeTable?, TreeTable?>> { }

    public class TreeTable : IMudTable
    {
        public readonly static TableId ID = new("", "Tree");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(TreeTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(TreeTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            TreeTable other = (TreeTable)obj;

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
            TreeTableUpdate update = (TreeTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new TreeTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<TreeTable?, TreeTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            TreeTable? current = null;
            TreeTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new TreeTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new TreeTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new TreeTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new TreeTable { value = null, };
                }
            }

            return new Tuple<TreeTable?, TreeTable?>(current, previous);
        }
    }
}

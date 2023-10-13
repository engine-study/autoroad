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
    public class BootsTableUpdate : TypedRecordUpdate<Tuple<BootsTable?, BootsTable?>> { }

    public class BootsTable : IMudTable
    {
        public readonly static TableId ID = new("", "Boots");

        public override TableId GetTableId()
        {
            return ID;
        }

        public long? minMove;
        public long? maxMove;

        public override Type TableType()
        {
            return typeof(BootsTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(BootsTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            BootsTable other = (BootsTable)obj;

            if (other == null)
            {
                return false;
            }
            if (minMove != other.minMove)
            {
                return false;
            }
            if (maxMove != other.maxMove)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            minMove = (long)(int)functionParameters[0];

            maxMove = (long)(int)functionParameters[1];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var minMoveValue = (long)table["minMove"];
            minMove = minMoveValue;
            var maxMoveValue = (long)table["maxMove"];
            maxMove = maxMoveValue;
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
                        minMove = value.Item1.TryGetValue("minMove", out var minMoveVal)
                            ? (long)minMoveVal
                            : default,
                        maxMove = value.Item1.TryGetValue("maxMove", out var maxMoveVal)
                            ? (long)maxMoveVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new BootsTable { minMove = null, maxMove = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new BootsTable
                    {
                        minMove = value.Item2.TryGetValue("minMove", out var minMoveVal)
                            ? (long)minMoveVal
                            : default,
                        maxMove = value.Item2.TryGetValue("maxMove", out var maxMoveVal)
                            ? (long)maxMoveVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new BootsTable { minMove = null, maxMove = null, };
                }
            }

            return new Tuple<BootsTable?, BootsTable?>(current, previous);
        }
    }
}

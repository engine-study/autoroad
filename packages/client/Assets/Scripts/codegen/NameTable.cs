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
    public class NameTableUpdate : TypedRecordUpdate<Tuple<NameTable?, NameTable?>> { }

    public class NameTable : IMudTable
    {
        public readonly static TableId ID = new("", "Name");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? named;
        public ulong? first;
        public ulong? middle;
        public ulong? last;

        public override Type TableType()
        {
            return typeof(NameTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(NameTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            NameTable other = (NameTable)obj;

            if (other == null)
            {
                return false;
            }
            if (named != other.named)
            {
                return false;
            }
            if (first != other.first)
            {
                return false;
            }
            if (middle != other.middle)
            {
                return false;
            }
            if (last != other.last)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            named = (bool)functionParameters[0];

            first = (ulong)(int)functionParameters[1];

            middle = (ulong)(int)functionParameters[2];

            last = (ulong)(int)functionParameters[3];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var namedValue = (bool)table["named"];
            named = namedValue;
            var firstValue = (ulong)table["first"];
            first = firstValue;
            var middleValue = (ulong)table["middle"];
            middle = middleValue;
            var lastValue = (ulong)table["last"];
            last = lastValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            NameTableUpdate update = (NameTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new NameTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<NameTable?, NameTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            NameTable? current = null;
            NameTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new NameTable
                    {
                        named = value.Item1.TryGetValue("named", out var namedVal)
                            ? (bool)namedVal
                            : default,
                        first = value.Item1.TryGetValue("first", out var firstVal)
                            ? (ulong)firstVal
                            : default,
                        middle = value.Item1.TryGetValue("middle", out var middleVal)
                            ? (ulong)middleVal
                            : default,
                        last = value.Item1.TryGetValue("last", out var lastVal)
                            ? (ulong)lastVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new NameTable
                    {
                        named = null,
                        first = null,
                        middle = null,
                        last = null,
                    };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new NameTable
                    {
                        named = value.Item2.TryGetValue("named", out var namedVal)
                            ? (bool)namedVal
                            : default,
                        first = value.Item2.TryGetValue("first", out var firstVal)
                            ? (ulong)firstVal
                            : default,
                        middle = value.Item2.TryGetValue("middle", out var middleVal)
                            ? (ulong)middleVal
                            : default,
                        last = value.Item2.TryGetValue("last", out var lastVal)
                            ? (ulong)lastVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new NameTable
                    {
                        named = null,
                        first = null,
                        middle = null,
                        last = null,
                    };
                }
            }

            return new Tuple<NameTable?, NameTable?>(current, previous);
        }
    }
}

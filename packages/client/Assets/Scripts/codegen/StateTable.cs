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
    public class StateTableUpdate : TypedRecordUpdate<Tuple<StateTable?, StateTable?>> { }

    public class StateTable : IMudTable
    {
        public readonly static TableId ID = new("", "State");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? state;
        public long? x;
        public long? y;

        public override Type TableType()
        {
            return typeof(StateTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(StateTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            state = (ulong)(int)functionParameters[0];

            x = (long)(int)functionParameters[1];

            y = (long)(int)functionParameters[2];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var stateValue = (ulong)table["state"];
            state = stateValue;
            var xValue = (long)table["x"];
            x = xValue;
            var yValue = (long)table["y"];
            y = yValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            StateTableUpdate update = (StateTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new StateTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<StateTable?, StateTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            StateTable? current = null;
            StateTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new StateTable
                    {
                        state = value.Item1.TryGetValue("state", out var stateVal)
                            ? (ulong)stateVal
                            : default,
                        x = value.Item1.TryGetValue("x", out var xVal) ? (long)xVal : default,
                        y = value.Item1.TryGetValue("y", out var yVal) ? (long)yVal : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new StateTable
                    {
                        state = null,
                        x = null,
                        y = null,
                    };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new StateTable
                    {
                        state = value.Item2.TryGetValue("state", out var stateVal)
                            ? (ulong)stateVal
                            : default,
                        x = value.Item2.TryGetValue("x", out var xVal) ? (long)xVal : default,
                        y = value.Item2.TryGetValue("y", out var yVal) ? (long)yVal : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new StateTable
                    {
                        state = null,
                        x = null,
                        y = null,
                    };
                }
            }

            return new Tuple<StateTable?, StateTable?>(current, previous);
        }
    }
}

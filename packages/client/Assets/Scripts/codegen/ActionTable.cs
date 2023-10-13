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
    public class ActionTableUpdate : TypedRecordUpdate<Tuple<ActionTable?, ActionTable?>> { }

    public class ActionTable : IMudTable
    {
        public readonly static TableId ID = new("", "Action");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? action;
        public long? x;
        public long? y;
        public string? target;

        public override Type TableType()
        {
            return typeof(ActionTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ActionTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            ActionTable other = (ActionTable)obj;

            if (other == null)
            {
                return false;
            }
            if (action != other.action)
            {
                return false;
            }
            if (x != other.x)
            {
                return false;
            }
            if (y != other.y)
            {
                return false;
            }
            if (target != other.target)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            action = (ulong)(int)functionParameters[0];

            x = (long)(int)functionParameters[1];

            y = (long)(int)functionParameters[2];

            target = (string)functionParameters[3];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var actionValue = (ulong)table["action"];
            action = actionValue;
            var xValue = (long)table["x"];
            x = xValue;
            var yValue = (long)table["y"];
            y = yValue;
            var targetValue = (string)table["target"];
            target = targetValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            ActionTableUpdate update = (ActionTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new ActionTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<ActionTable?, ActionTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            ActionTable? current = null;
            ActionTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new ActionTable
                    {
                        action = value.Item1.TryGetValue("action", out var actionVal)
                            ? (ulong)actionVal
                            : default,
                        x = value.Item1.TryGetValue("x", out var xVal) ? (long)xVal : default,
                        y = value.Item1.TryGetValue("y", out var yVal) ? (long)yVal : default,
                        target = value.Item1.TryGetValue("target", out var targetVal)
                            ? (string)targetVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new ActionTable
                    {
                        action = null,
                        x = null,
                        y = null,
                        target = null,
                    };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new ActionTable
                    {
                        action = value.Item2.TryGetValue("action", out var actionVal)
                            ? (ulong)actionVal
                            : default,
                        x = value.Item2.TryGetValue("x", out var xVal) ? (long)xVal : default,
                        y = value.Item2.TryGetValue("y", out var yVal) ? (long)yVal : default,
                        target = value.Item2.TryGetValue("target", out var targetVal)
                            ? (string)targetVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new ActionTable
                    {
                        action = null,
                        x = null,
                        y = null,
                        target = null,
                    };
                }
            }

            return new Tuple<ActionTable?, ActionTable?>(current, previous);
        }
    }
}

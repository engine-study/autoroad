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
    public class RoadConfigTableUpdate
        : TypedRecordUpdate<Tuple<RoadConfigTable?, RoadConfigTable?>> { }

    public class RoadConfigTable : IMudTable
    {
        public readonly static TableId ID = new("", "RoadConfig");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? width;
        public ulong? height;
        public long? left;
        public long? right;

        public override Type TableType()
        {
            return typeof(RoadConfigTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(RoadConfigTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            width = (ulong)(int)functionParameters[0];

            height = (ulong)(int)functionParameters[1];

            left = (long)(int)functionParameters[2];

            right = (long)(int)functionParameters[3];
        }

        public override bool SetValues(IEnumerable<Property> result)
        {
            var hasValues = false;
            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "width":
                        var widthValue = (ulong)value;
                        width = widthValue;
                        hasValues = true;
                        break;
                    case "height":
                        var heightValue = (ulong)value;
                        height = heightValue;
                        hasValues = true;
                        break;
                    case "left":
                        var leftValue = (long)value;
                        left = leftValue;
                        hasValues = true;
                        break;
                    case "right":
                        var rightValue = (long)value;
                        right = rightValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues;
        }

        public override IMudTable GetTableValue(string key)
        {
            var query = new Query()
                .Find("?value", "?attribute")
                .Where(TableId.ToString(), key, "?attribute", "?value");
            var result = NetworkManager.Instance.ds.Query(query);
            var roadConfigTable = new RoadConfigTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "width":
                        var widthValue = (ulong)value;
                        roadConfigTable.width = widthValue;
                        hasValues = true;
                        break;
                    case "height":
                        var heightValue = (ulong)value;
                        roadConfigTable.height = heightValue;
                        hasValues = true;
                        break;
                    case "left":
                        var leftValue = (long)value;
                        roadConfigTable.left = leftValue;
                        hasValues = true;
                        break;
                    case "right":
                        var rightValue = (long)value;
                        roadConfigTable.right = rightValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? roadConfigTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            RoadConfigTableUpdate update = (RoadConfigTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new RoadConfigTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<RoadConfigTable?, RoadConfigTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            RoadConfigTable? current = null;
            RoadConfigTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new RoadConfigTable
                    {
                        width = value.Item1.TryGetValue("width", out var widthVal)
                            ? (ulong)widthVal
                            : default,
                        height = value.Item1.TryGetValue("height", out var heightVal)
                            ? (ulong)heightVal
                            : default,
                        left = value.Item1.TryGetValue("left", out var leftVal)
                            ? (long)leftVal
                            : default,
                        right = value.Item1.TryGetValue("right", out var rightVal)
                            ? (long)rightVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new RoadConfigTable
                    {
                        width = null,
                        height = null,
                        left = null,
                        right = null,
                    };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new RoadConfigTable
                    {
                        width = value.Item2.TryGetValue("width", out var widthVal)
                            ? (ulong)widthVal
                            : default,
                        height = value.Item2.TryGetValue("height", out var heightVal)
                            ? (ulong)heightVal
                            : default,
                        left = value.Item2.TryGetValue("left", out var leftVal)
                            ? (long)leftVal
                            : default,
                        right = value.Item2.TryGetValue("right", out var rightVal)
                            ? (long)rightVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new RoadConfigTable
                    {
                        width = null,
                        height = null,
                        left = null,
                        right = null,
                    };
                }
            }

            return new Tuple<RoadConfigTable?, RoadConfigTable?>(current, previous);
        }
    }
}

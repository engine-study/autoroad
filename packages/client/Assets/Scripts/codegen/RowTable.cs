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
    public class RowTableUpdate : TypedRecordUpdate<Tuple<RowTable?, RowTable?>> { }

    public class RowTable : IMudTable
    {
        public readonly static TableId ID = new("", "Row");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? segments;

        public override Type TableType()
        {
            return typeof(RowTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(RowTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            segments = (ulong)(int)functionParameters[0];
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
                    case "segments":
                        var segmentsValue = (ulong)value;
                        segments = segmentsValue;
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
            var rowTable = new RowTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "segments":
                        var segmentsValue = (ulong)value;
                        rowTable.segments = segmentsValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? rowTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            RowTableUpdate update = (RowTableUpdate)tableUpdate;

            var currentValue = update?.TypedValue.Item1;
            if (currentValue == null)
            {
                Debug.LogError("No value on RowTable update");
            }

            return currentValue;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new RowTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<RowTable?, RowTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            RowTable? current = null;
            RowTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new RowTable
                    {
                        segments = value.Item1.TryGetValue("segments", out var segmentsVal)
                            ? (ulong)segmentsVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new RowTable { segments = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new RowTable
                    {
                        segments = value.Item2.TryGetValue("segments", out var segmentsVal)
                            ? (ulong)segmentsVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new RowTable { segments = null, };
                }
            }

            return new Tuple<RowTable?, RowTable?>(current, previous);
        }
    }
}

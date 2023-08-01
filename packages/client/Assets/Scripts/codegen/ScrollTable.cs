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
    public class ScrollTableUpdate : TypedRecordUpdate<Tuple<ScrollTable?, ScrollTable?>> { }

    public class ScrollTable : IMudTable
    {
        public readonly static TableId ID = new("", "Scroll");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(ScrollTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ScrollTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (ulong)(int)functionParameters[0];
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
                    case "value":
                        var valueValue = (ulong)value;
                        value = valueValue;
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
            var scrollTable = new ScrollTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "value":
                        var valueValue = (ulong)value;
                        scrollTable.value = valueValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? scrollTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            ScrollTableUpdate update = (ScrollTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new ScrollTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<ScrollTable?, ScrollTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            ScrollTable? current = null;
            ScrollTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new ScrollTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new ScrollTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new ScrollTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new ScrollTable { value = null, };
                }
            }

            return new Tuple<ScrollTable?, ScrollTable?>(current, previous);
        }
    }
}

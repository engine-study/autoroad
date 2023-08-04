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
    public class ActiveTableUpdate : TypedRecordUpdate<Tuple<ActiveTable?, ActiveTable?>> { }

    public class ActiveTable : IMudTable
    {
        public readonly static TableId ID = new("", "Active");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(ActiveTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ActiveTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (bool)functionParameters[0];
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
                        var valueValue = (bool)value;
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
            var activeTable = new ActiveTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "value":
                        var valueValue = (bool)value;
                        activeTable.value = valueValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? activeTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            ActiveTableUpdate update = (ActiveTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new ActiveTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<ActiveTable?, ActiveTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            ActiveTable? current = null;
            ActiveTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new ActiveTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new ActiveTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new ActiveTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new ActiveTable { value = null, };
                }
            }

            return new Tuple<ActiveTable?, ActiveTable?>(current, previous);
        }
    }
}
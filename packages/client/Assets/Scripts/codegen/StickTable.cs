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
    public class StickTableUpdate : TypedRecordUpdate<Tuple<StickTable?, StickTable?>> { }

    public class StickTable : IMudTable
    {
        public readonly static TableId ID = new("", "Stick");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(StickTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(StickTableUpdate);
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
            var stickTable = new StickTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "value":
                        var valueValue = (bool)value;
                        stickTable.value = valueValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? stickTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            StickTableUpdate update = (StickTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new StickTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<StickTable?, StickTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            StickTable? current = null;
            StickTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new StickTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new StickTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new StickTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new StickTable { value = null, };
                }
            }

            return new Tuple<StickTable?, StickTable?>(current, previous);
        }
    }
}

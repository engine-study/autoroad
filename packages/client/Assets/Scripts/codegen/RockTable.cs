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
    public class RockTableUpdate : TypedRecordUpdate<Tuple<RockTable?, RockTable?>> { }

    public class RockTable : IMudTable
    {
        public readonly static TableId ID = new("", "Rock");

        public override TableId GetTableId()
        {
            return ID;
        }

        public ulong? value;

        public override Type TableType()
        {
            return typeof(RockTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(RockTableUpdate);
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
            var rockTable = new RockTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "value":
                        var valueValue = (ulong)value;
                        rockTable.value = valueValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? rockTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            RockTableUpdate update = (RockTableUpdate)tableUpdate;

            var currentValue = update?.TypedValue.Item1;
            if (currentValue == null)
            {
                Debug.LogError("No value on RockTable update");
            }

            return currentValue;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new RockTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<RockTable?, RockTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            RockTable? current = null;
            RockTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new RockTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new RockTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new RockTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (ulong)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new RockTable { value = null, };
                }
            }

            return new Tuple<RockTable?, RockTable?>(current, previous);
        }
    }
}
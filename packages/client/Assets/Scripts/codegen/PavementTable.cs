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
    public class PavementTableUpdate : TypedRecordUpdate<Tuple<PavementTable?, PavementTable?>> { }

    public class PavementTable : IMudTable
    {
        public readonly static TableId ID = new("", "Pavement");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(PavementTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(PavementTableUpdate);
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
            var pavementTable = new PavementTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "value":
                        var valueValue = (bool)value;
                        pavementTable.value = valueValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? pavementTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            PavementTableUpdate update = (PavementTableUpdate)tableUpdate;

            var currentValue = update?.TypedValue.Item1;
            if (currentValue == null)
            {
                Debug.LogError("No value on PavementTable update");
            }

            return currentValue;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new PavementTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<PavementTable?, PavementTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            PavementTable? current = null;
            PavementTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new PavementTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new PavementTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new PavementTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new PavementTable { value = null, };
                }
            }

            return new Tuple<PavementTable?, PavementTable?>(current, previous);
        }
    }
}

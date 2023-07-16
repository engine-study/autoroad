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
    public class CarryingTableUpdate : TypedRecordUpdate<Tuple<CarryingTable?, CarryingTable?>> { }

    public class CarryingTable : IMudTable
    {
        public readonly static TableId ID = new("", "Carrying");

        public override TableId GetTableId()
        {
            return ID;
        }

        public string? value;

        public override Type TableType()
        {
            return typeof(CarryingTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(CarryingTableUpdate);
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (string)functionParameters[0];
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
                        var valueValue = (string)value;
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
            var carryingTable = new CarryingTable();
            var hasValues = false;

            foreach (var record in result)
            {
                var attribute = record["attribute"].ToString();
                var value = record["value"];

                switch (attribute)
                {
                    case "value":
                        var valueValue = (string)value;
                        carryingTable.value = valueValue;
                        hasValues = true;
                        break;
                }
            }

            return hasValues ? carryingTable : null;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            CarryingTableUpdate update = (CarryingTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new CarryingTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<CarryingTable?, CarryingTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            CarryingTable? current = null;
            CarryingTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new CarryingTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (string)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new CarryingTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new CarryingTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (string)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new CarryingTable { value = null, };
                }
            }

            return new Tuple<CarryingTable?, CarryingTable?>(current, previous);
        }
    }
}

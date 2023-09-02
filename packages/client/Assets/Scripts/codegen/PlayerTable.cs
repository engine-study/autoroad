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
    public class PlayerTableUpdate : TypedRecordUpdate<Tuple<PlayerTable?, PlayerTable?>> { }

    public class PlayerTable : IMudTable
    {
        public readonly static TableId ID = new("", "Player");

        public override TableId GetTableId()
        {
            return ID;
        }

        public bool? value;

        public override Type TableType()
        {
            return typeof(PlayerTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(PlayerTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            PlayerTable other = (PlayerTable)obj;

            if (other == null)
            {
                return false;
            }
            if (value != other.value)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (bool)functionParameters[0];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var valueValue = (bool)table["value"];
            value = valueValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            PlayerTableUpdate update = (PlayerTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new PlayerTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<PlayerTable?, PlayerTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            PlayerTable? current = null;
            PlayerTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new PlayerTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new PlayerTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new PlayerTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (bool)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new PlayerTable { value = null, };
                }
            }

            return new Tuple<PlayerTable?, PlayerTable?>(current, previous);
        }
    }
}

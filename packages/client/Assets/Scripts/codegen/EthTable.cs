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
    public class EthTableUpdate : TypedRecordUpdate<Tuple<EthTable?, EthTable?>> { }

    public class EthTable : IMudTable
    {
        public readonly static TableId ID = new("", "Eth");

        public override TableId GetTableId()
        {
            return ID;
        }

        public System.Numerics.BigInteger? value;

        public override Type TableType()
        {
            return typeof(EthTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(EthTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            EthTable other = (EthTable)obj;

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
            value = (System.Numerics.BigInteger)functionParameters[0];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var valueValue = (System.Numerics.BigInteger)table["value"];
            value = valueValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            EthTableUpdate update = (EthTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new EthTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<EthTable?, EthTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            EthTable? current = null;
            EthTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new EthTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (System.Numerics.BigInteger)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new EthTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new EthTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (System.Numerics.BigInteger)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new EthTable { value = null, };
                }
            }

            return new Tuple<EthTable?, EthTable?>(current, previous);
        }
    }
}
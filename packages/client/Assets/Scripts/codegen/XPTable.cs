/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using mud.Network.schemas;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class XPTableUpdate : TypedRecordUpdate<Tuple<XPTable?, XPTable?>> { }

    public class XPTable : IMudTable
    {
        public readonly static TableId ID = new("", "XP");

        public override TableId GetTableId()
        {
            return ID;
        }

        public System.Numerics.BigInteger? value;

        public override Type TableType()
        {
            return typeof(XPTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(XPTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            XPTable other = (XPTable)obj;

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
            XPTableUpdate update = (XPTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new XPTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<XPTable?, XPTable?> MapUpdates(Tuple<Property?, Property?> value)
        {
            XPTable? current = null;
            XPTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new XPTable
                    {
                        value = value.Item1.TryGetValue("value", out var valueVal)
                            ? (System.Numerics.BigInteger)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new XPTable { value = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new XPTable
                    {
                        value = value.Item2.TryGetValue("value", out var valueVal)
                            ? (System.Numerics.BigInteger)valueVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new XPTable { value = null, };
                }
            }

            return new Tuple<XPTable?, XPTable?>(current, previous);
        }
    }
}

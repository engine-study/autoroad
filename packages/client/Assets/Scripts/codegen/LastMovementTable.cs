/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using System.Linq;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    [System.Serializable]
    public class LastMovementTable : MUDTable
    {
        public class LastMovementTableUpdate : RecordUpdate
        {
            public System.Numerics.BigInteger? Value;
            public System.Numerics.BigInteger? PreviousValue;
        }

        public readonly static string ID = "LastMovement";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public System.Numerics.BigInteger? Value;

        public override Type TableType()
        {
            return typeof(LastMovementTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(LastMovementTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            LastMovementTable other = (LastMovementTable)obj;

            if (other == null)
            {
                return false;
            }
            if (Value != other.Value)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Value = (System.Numerics.BigInteger)functionParameters[0];
        }

        public static IObservable<RecordUpdate> GetLastMovementTableUpdates()
        {
            LastMovementTable mudTable = new LastMovementTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Value = (System.Numerics.BigInteger)property["value"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            System.Numerics.BigInteger? currentValueTyped = null;
            System.Numerics.BigInteger? previousValueTyped = null;

            if (currentValue != null && currentValue.ContainsKey("value"))
            {
                currentValueTyped = (System.Numerics.BigInteger)currentValue["value"];
            }

            if (previousValue != null && previousValue.ContainsKey("value"))
            {
                previousValueTyped = (System.Numerics.BigInteger)previousValue["value"];
            }

            return new LastMovementTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Value = currentValueTyped,
                PreviousValue = previousValueTyped,
            };
        }
    }
}
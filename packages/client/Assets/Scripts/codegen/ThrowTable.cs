/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using System.Linq;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class ThrowTable : MUDTable
    {
        public class ThrowTableUpdate : RecordUpdate
        {
            public uint? Value;
            public uint? PreviousValue;
        }

        public readonly static string ID = "Throw";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public uint? Value;

        public override Type TableType()
        {
            return typeof(ThrowTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ThrowTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            ThrowTable other = (ThrowTable)obj;

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
            Value = (uint)functionParameters[0];
        }

        public static IObservable<RecordUpdate> GetThrowTableUpdates()
        {
            ThrowTable mudTable = new ThrowTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Value = (uint)property["value"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            uint? currentValueTyped = null;
            uint? previousValueTyped = null;

            if (currentValue != null && currentValue.ContainsKey("value"))
            {
                currentValueTyped = (uint)currentValue["value"];
            }

            if (previousValue != null && previousValue.ContainsKey("value"))
            {
                previousValueTyped = (uint)previousValue["value"];
            }

            return new ThrowTableUpdate
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

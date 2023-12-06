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
    public class WorldColumnTable : MUDTable
    {
        public class WorldColumnTableUpdate : RecordUpdate
        {
            public bool? Value;
            public bool? PreviousValue;
        }

        public readonly static string ID = "WorldColumn";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public bool? Value;

        public override Type TableType()
        {
            return typeof(WorldColumnTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(WorldColumnTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            WorldColumnTable other = (WorldColumnTable)obj;

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
            Value = (bool)functionParameters[0];
        }

        public static IObservable<RecordUpdate> GetWorldColumnTableUpdates()
        {
            WorldColumnTable mudTable = new WorldColumnTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Value = (bool)property["value"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            bool? currentValueTyped = null;
            bool? previousValueTyped = null;

            if (currentValue != null && currentValue.ContainsKey("value"))
            {
                currentValueTyped = (bool)currentValue["value"];
            }

            if (previousValue != null && previousValue.ContainsKey("value"))
            {
                previousValueTyped = (bool)previousValue["value"];
            }

            return new WorldColumnTableUpdate
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

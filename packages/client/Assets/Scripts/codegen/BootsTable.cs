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
    public class BootsTable : MUDTable
    {
        public class BootsTableUpdate : RecordUpdate
        {
            public int? MinMove;
            public int? PreviousMinMove;
            public int? MaxMove;
            public int? PreviousMaxMove;
        }

        public readonly static string ID = "Boots";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public int? MinMove;
        public int? MaxMove;

        public override Type TableType()
        {
            return typeof(BootsTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(BootsTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            BootsTable other = (BootsTable)obj;

            if (other == null)
            {
                return false;
            }
            if (MinMove != other.MinMove)
            {
                return false;
            }
            if (MaxMove != other.MaxMove)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            MinMove = (int)functionParameters[0];

            MaxMove = (int)functionParameters[1];
        }

        public static IObservable<RecordUpdate> GetBootsTableUpdates()
        {
            BootsTable mudTable = new BootsTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            MinMove = (int)property["minMove"];
            MaxMove = (int)property["maxMove"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            int? currentMinMoveTyped = null;
            int? previousMinMoveTyped = null;

            if (currentValue != null && currentValue.ContainsKey("minmove"))
            {
                currentMinMoveTyped = (int)currentValue["minmove"];
            }

            if (previousValue != null && previousValue.ContainsKey("minmove"))
            {
                previousMinMoveTyped = (int)previousValue["minmove"];
            }
            int? currentMaxMoveTyped = null;
            int? previousMaxMoveTyped = null;

            if (currentValue != null && currentValue.ContainsKey("maxmove"))
            {
                currentMaxMoveTyped = (int)currentValue["maxmove"];
            }

            if (previousValue != null && previousValue.ContainsKey("maxmove"))
            {
                previousMaxMoveTyped = (int)previousValue["maxmove"];
            }

            return new BootsTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                MinMove = currentMinMoveTyped,
                PreviousMinMove = previousMinMoveTyped,
                MaxMove = currentMaxMoveTyped,
                PreviousMaxMove = previousMaxMoveTyped,
            };
        }
    }
}

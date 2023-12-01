/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using System.Linq;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class RobeTable : MUDTable
    {
        public class RobeTableUpdate : RecordUpdate
        {
            public uint? Index;
            public uint? PreviousIndex;
            public bool[]? Owned;
            public bool[]? PreviousOwned;
        }

        public readonly static string ID = "Robe";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public uint? Index;
        public bool[]? Owned;

        public override Type TableType()
        {
            return typeof(RobeTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(RobeTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            RobeTable other = (RobeTable)obj;

            if (other == null)
            {
                return false;
            }
            if (Index != other.Index)
            {
                return false;
            }
            if (Owned != other.Owned)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Index = (uint)functionParameters[0];

            Owned = (bool[])functionParameters[1];
        }

        public static IObservable<RecordUpdate> GetRobeTableUpdates()
        {
            RobeTable mudTable = new RobeTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Index = (uint)property["index"];
            Owned = ((object[])property["owned"]).Cast<bool>().ToArray();
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            uint? currentIndexTyped = null;
            uint? previousIndexTyped = null;

            if (currentValue != null && currentValue.ContainsKey("index"))
            {
                currentIndexTyped = (uint)currentValue["index"];
            }

            if (previousValue != null && previousValue.ContainsKey("index"))
            {
                previousIndexTyped = (uint)previousValue["index"];
            }
            bool[]? currentOwnedTyped = null;
            bool[]? previousOwnedTyped = null;

            if (currentValue != null && currentValue.ContainsKey("owned"))
            {
                currentOwnedTyped = ((object[])currentValue["owned"]).Cast<bool>().ToArray();
            }

            if (previousValue != null && previousValue.ContainsKey("owned"))
            {
                previousOwnedTyped = ((object[])previousValue["owned"]).Cast<bool>().ToArray();
            }

            return new RobeTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Index = currentIndexTyped,
                PreviousIndex = previousIndexTyped,
                Owned = currentOwnedTyped,
                PreviousOwned = previousOwnedTyped,
            };
        }
    }
}

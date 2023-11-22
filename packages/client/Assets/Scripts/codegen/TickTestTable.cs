/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class TickTestTable : MUDTable
    {
        public class TickTestTableUpdate : RecordUpdate
        {
            public System.Numerics.BigInteger? LastBlock;
            public System.Numerics.BigInteger? PreviousLastBlock;
            public string? Entities;
            public string? PreviousEntities;
        }

        public readonly static string ID = "TickTest";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public System.Numerics.BigInteger? LastBlock;
        public string? Entities;

        public override Type TableType()
        {
            return typeof(TickTestTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(TickTestTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            TickTestTable other = (TickTestTable)obj;

            if (other == null)
            {
                return false;
            }
            if (LastBlock != other.LastBlock)
            {
                return false;
            }
            if (Entities != other.Entities)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            LastBlock = (System.Numerics.BigInteger)functionParameters[0];

            Entities = (string)functionParameters[1];
        }

        public static IObservable<RecordUpdate> GetTickTestTableUpdates()
        {
            TickTestTable mudTable = new TickTestTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            LastBlock = (System.Numerics.BigInteger)property["lastBlock"];
            Entities = (string)property["entities"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            System.Numerics.BigInteger? currentLastBlockTyped = null;
            System.Numerics.BigInteger? previousLastBlockTyped = null;

            if (currentValue != null && currentValue.ContainsKey("lastblock"))
            {
                currentLastBlockTyped = (System.Numerics.BigInteger)currentValue["lastblock"];
            }

            if (previousValue != null && previousValue.ContainsKey("lastblock"))
            {
                previousLastBlockTyped = (System.Numerics.BigInteger)previousValue["lastblock"];
            }
            string? currentEntitiesTyped = null;
            string? previousEntitiesTyped = null;

            if (currentValue != null && currentValue.ContainsKey("entities"))
            {
                currentEntitiesTyped = (string)currentValue["entities"];
            }

            if (previousValue != null && previousValue.ContainsKey("entities"))
            {
                previousEntitiesTyped = (string)previousValue["entities"];
            }

            return new TickTestTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                LastBlock = currentLastBlockTyped,
                PreviousLastBlock = previousLastBlockTyped,
                Entities = currentEntitiesTyped,
                PreviousEntities = previousEntitiesTyped,
            };
        }
    }
}

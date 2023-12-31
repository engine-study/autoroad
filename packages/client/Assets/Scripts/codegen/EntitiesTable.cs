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
    public class EntitiesTable : MUDTable
    {
        public class EntitiesTableUpdate : RecordUpdate
        {
            public string[]? Entities;
            public string[]? PreviousEntities;
        }

        public readonly static string ID = "Entities";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public string[]? Entities;

        public override Type TableType()
        {
            return typeof(EntitiesTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(EntitiesTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            EntitiesTable other = (EntitiesTable)obj;

            if (other == null)
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
            Entities = (string[])functionParameters[0];
        }

        public static IObservable<RecordUpdate> GetEntitiesTableUpdates()
        {
            EntitiesTable mudTable = new EntitiesTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Entities = ((object[])property["entities"]).Cast<string>().ToArray();
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            string[]? currentEntitiesTyped = null;
            string[]? previousEntitiesTyped = null;

            if (currentValue != null && currentValue.ContainsKey("entities"))
            {
                currentEntitiesTyped = ((object[])currentValue["entities"])
                    .Cast<string>()
                    .ToArray();
            }

            if (previousValue != null && previousValue.ContainsKey("entities"))
            {
                previousEntitiesTyped = ((object[])previousValue["entities"])
                    .Cast<string>()
                    .ToArray();
            }

            return new EntitiesTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Entities = currentEntitiesTyped,
                PreviousEntities = previousEntitiesTyped,
            };
        }
    }
}

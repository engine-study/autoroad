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
    public class GameStateTable : MUDTable
    {
        public class GameStateTableUpdate : RecordUpdate
        {
            public int? Miles;
            public int? PreviousMiles;
            public int? Unused;
            public int? PreviousUnused;
        }

        public readonly static string ID = "GameState";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public int? Miles;
        public int? Unused;

        public override Type TableType()
        {
            return typeof(GameStateTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(GameStateTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            GameStateTable other = (GameStateTable)obj;

            if (other == null)
            {
                return false;
            }
            if (Miles != other.Miles)
            {
                return false;
            }
            if (Unused != other.Unused)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Miles = (int)functionParameters[0];

            Unused = (int)functionParameters[1];
        }

        public static IObservable<RecordUpdate> GetGameStateTableUpdates()
        {
            GameStateTable mudTable = new GameStateTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Miles = (int)property["miles"];
            Unused = (int)property["unused"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            int? currentMilesTyped = null;
            int? previousMilesTyped = null;

            if (currentValue != null && currentValue.ContainsKey("miles"))
            {
                currentMilesTyped = (int)currentValue["miles"];
            }

            if (previousValue != null && previousValue.ContainsKey("miles"))
            {
                previousMilesTyped = (int)previousValue["miles"];
            }
            int? currentUnusedTyped = null;
            int? previousUnusedTyped = null;

            if (currentValue != null && currentValue.ContainsKey("unused"))
            {
                currentUnusedTyped = (int)currentValue["unused"];
            }

            if (previousValue != null && previousValue.ContainsKey("unused"))
            {
                previousUnusedTyped = (int)previousValue["unused"];
            }

            return new GameStateTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Miles = currentMilesTyped,
                PreviousMiles = previousMilesTyped,
                Unused = currentUnusedTyped,
                PreviousUnused = previousUnusedTyped,
            };
        }
    }
}

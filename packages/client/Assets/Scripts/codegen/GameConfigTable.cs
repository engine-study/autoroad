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
    public class GameConfigTable : MUDTable
    {
        public class GameConfigTableUpdate : RecordUpdate
        {
            public bool? Debug;
            public bool? PreviousDebug;
            public bool? DummyPlayers;
            public bool? PreviousDummyPlayers;
        }

        public readonly static string ID = "GameConfig";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public bool? Debug;
        public bool? DummyPlayers;

        public override Type TableType()
        {
            return typeof(GameConfigTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(GameConfigTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            GameConfigTable other = (GameConfigTable)obj;

            if (other == null)
            {
                return false;
            }
            if (Debug != other.Debug)
            {
                return false;
            }
            if (DummyPlayers != other.DummyPlayers)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Debug = (bool)functionParameters[0];

            DummyPlayers = (bool)functionParameters[1];
        }

        public static IObservable<RecordUpdate> GetGameConfigTableUpdates()
        {
            GameConfigTable mudTable = new GameConfigTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Debug = (bool)property["debug"];
            DummyPlayers = (bool)property["dummyPlayers"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            bool? currentDebugTyped = null;
            bool? previousDebugTyped = null;

            if (currentValue != null && currentValue.ContainsKey("debug"))
            {
                currentDebugTyped = (bool)currentValue["debug"];
            }

            if (previousValue != null && previousValue.ContainsKey("debug"))
            {
                previousDebugTyped = (bool)previousValue["debug"];
            }
            bool? currentDummyPlayersTyped = null;
            bool? previousDummyPlayersTyped = null;

            if (currentValue != null && currentValue.ContainsKey("dummyplayers"))
            {
                currentDummyPlayersTyped = (bool)currentValue["dummyplayers"];
            }

            if (previousValue != null && previousValue.ContainsKey("dummyplayers"))
            {
                previousDummyPlayersTyped = (bool)previousValue["dummyplayers"];
            }

            return new GameConfigTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Debug = currentDebugTyped,
                PreviousDebug = previousDebugTyped,
                DummyPlayers = currentDummyPlayersTyped,
                PreviousDummyPlayers = previousDummyPlayersTyped,
            };
        }
    }
}

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
    public class MapConfigTable : MUDTable
    {
        public class MapConfigTableUpdate : RecordUpdate
        {
            public int? PlayWidth;
            public int? PreviousPlayWidth;
            public int? PlayHeight;
            public int? PreviousPlayHeight;
            public int? PlaySpawnWidth;
            public int? PreviousPlaySpawnWidth;
        }

        public readonly static string ID = "MapConfig";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public int? PlayWidth;
        public int? PlayHeight;
        public int? PlaySpawnWidth;

        public override Type TableType()
        {
            return typeof(MapConfigTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(MapConfigTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            MapConfigTable other = (MapConfigTable)obj;

            if (other == null)
            {
                return false;
            }
            if (PlayWidth != other.PlayWidth)
            {
                return false;
            }
            if (PlayHeight != other.PlayHeight)
            {
                return false;
            }
            if (PlaySpawnWidth != other.PlaySpawnWidth)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            PlayWidth = (int)functionParameters[0];

            PlayHeight = (int)functionParameters[1];

            PlaySpawnWidth = (int)functionParameters[2];
        }

        public static IObservable<RecordUpdate> GetMapConfigTableUpdates()
        {
            MapConfigTable mudTable = new MapConfigTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            PlayWidth = (int)property["playWidth"];
            PlayHeight = (int)property["playHeight"];
            PlaySpawnWidth = (int)property["playSpawnWidth"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            int? currentPlayWidthTyped = null;
            int? previousPlayWidthTyped = null;

            if (currentValue != null && currentValue.ContainsKey("playwidth"))
            {
                currentPlayWidthTyped = (int)currentValue["playwidth"];
            }

            if (previousValue != null && previousValue.ContainsKey("playwidth"))
            {
                previousPlayWidthTyped = (int)previousValue["playwidth"];
            }
            int? currentPlayHeightTyped = null;
            int? previousPlayHeightTyped = null;

            if (currentValue != null && currentValue.ContainsKey("playheight"))
            {
                currentPlayHeightTyped = (int)currentValue["playheight"];
            }

            if (previousValue != null && previousValue.ContainsKey("playheight"))
            {
                previousPlayHeightTyped = (int)previousValue["playheight"];
            }
            int? currentPlaySpawnWidthTyped = null;
            int? previousPlaySpawnWidthTyped = null;

            if (currentValue != null && currentValue.ContainsKey("playspawnwidth"))
            {
                currentPlaySpawnWidthTyped = (int)currentValue["playspawnwidth"];
            }

            if (previousValue != null && previousValue.ContainsKey("playspawnwidth"))
            {
                previousPlaySpawnWidthTyped = (int)previousValue["playspawnwidth"];
            }

            return new MapConfigTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                PlayWidth = currentPlayWidthTyped,
                PreviousPlayWidth = previousPlayWidthTyped,
                PlayHeight = currentPlayHeightTyped,
                PreviousPlayHeight = previousPlayHeightTyped,
                PlaySpawnWidth = currentPlaySpawnWidthTyped,
                PreviousPlaySpawnWidth = previousPlaySpawnWidthTyped,
            };
        }
    }
}

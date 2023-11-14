/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class EntitiesTable : MUDTable
    {
        public class EntitiesTableUpdate : RecordUpdate
        {
            public string[]? Width;
            public string[]? PreviousWidth;
            public string[]? Height;
            public string[]? PreviousHeight;
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

        public string[]? Width;
        public string[]? Height;

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
            if (Width != other.Width)
            {
                return false;
            }
            if (Height != other.Height)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Width = (string[])functionParameters[0];

            Height = (string[])functionParameters[1];
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
            Width = (string[])property["width"];
            Height = (string[])property["height"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            string[]? currentWidthTyped = null;
            string[]? previousWidthTyped = null;

            if (currentValue != null && currentValue.ContainsKey("width"))
            {
                currentWidthTyped = (string[])currentValue["width"];
            }

            if (previousValue != null && previousValue.ContainsKey("width"))
            {
                previousWidthTyped = (string[])previousValue["width"];
            }
            string[]? currentHeightTyped = null;
            string[]? previousHeightTyped = null;

            if (currentValue != null && currentValue.ContainsKey("height"))
            {
                currentHeightTyped = (string[])currentValue["height"];
            }

            if (previousValue != null && previousValue.ContainsKey("height"))
            {
                previousHeightTyped = (string[])previousValue["height"];
            }

            return new EntitiesTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Width = currentWidthTyped,
                PreviousWidth = previousWidthTyped,
                Height = currentHeightTyped,
                PreviousHeight = previousHeightTyped,
            };
        }
    }
}

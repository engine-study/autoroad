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
    public class BoundsTable : MUDTable
    {
        public class BoundsTableUpdate : RecordUpdate
        {
            public int? Left;
            public int? PreviousLeft;
            public int? Right;
            public int? PreviousRight;
            public int? Up;
            public int? PreviousUp;
            public int? Down;
            public int? PreviousDown;
        }

        public readonly static string ID = "Bounds";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public int? Left;
        public int? Right;
        public int? Up;
        public int? Down;

        public override Type TableType()
        {
            return typeof(BoundsTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(BoundsTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            BoundsTable other = (BoundsTable)obj;

            if (other == null)
            {
                return false;
            }
            if (Left != other.Left)
            {
                return false;
            }
            if (Right != other.Right)
            {
                return false;
            }
            if (Up != other.Up)
            {
                return false;
            }
            if (Down != other.Down)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Left = (int)functionParameters[0];

            Right = (int)functionParameters[1];

            Up = (int)functionParameters[2];

            Down = (int)functionParameters[3];
        }

        public static IObservable<RecordUpdate> GetBoundsTableUpdates()
        {
            BoundsTable mudTable = new BoundsTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Left = (int)property["left"];
            Right = (int)property["right"];
            Up = (int)property["up"];
            Down = (int)property["down"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            int? currentLeftTyped = null;
            int? previousLeftTyped = null;

            if (currentValue != null && currentValue.ContainsKey("left"))
            {
                currentLeftTyped = (int)currentValue["left"];
            }

            if (previousValue != null && previousValue.ContainsKey("left"))
            {
                previousLeftTyped = (int)previousValue["left"];
            }
            int? currentRightTyped = null;
            int? previousRightTyped = null;

            if (currentValue != null && currentValue.ContainsKey("right"))
            {
                currentRightTyped = (int)currentValue["right"];
            }

            if (previousValue != null && previousValue.ContainsKey("right"))
            {
                previousRightTyped = (int)previousValue["right"];
            }
            int? currentUpTyped = null;
            int? previousUpTyped = null;

            if (currentValue != null && currentValue.ContainsKey("up"))
            {
                currentUpTyped = (int)currentValue["up"];
            }

            if (previousValue != null && previousValue.ContainsKey("up"))
            {
                previousUpTyped = (int)previousValue["up"];
            }
            int? currentDownTyped = null;
            int? previousDownTyped = null;

            if (currentValue != null && currentValue.ContainsKey("down"))
            {
                currentDownTyped = (int)currentValue["down"];
            }

            if (previousValue != null && previousValue.ContainsKey("down"))
            {
                previousDownTyped = (int)previousValue["down"];
            }

            return new BoundsTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Left = currentLeftTyped,
                PreviousLeft = previousLeftTyped,
                Right = currentRightTyped,
                PreviousRight = previousRightTyped,
                Up = currentUpTyped,
                PreviousUp = previousUpTyped,
                Down = currentDownTyped,
                PreviousDown = previousDownTyped,
            };
        }
    }
}

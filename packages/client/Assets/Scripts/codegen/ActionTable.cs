/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class ActionTable : MUDTable
    {
        public class ActionTableUpdate : RecordUpdate
        {
            public int? Action;
            public int? PreviousAction;
            public int? X;
            public int? PreviousX;
            public int? Y;
            public int? PreviousY;
            public string? Target;
            public string? PreviousTarget;
        }

        public readonly static string ID = "Action";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public int? Action;
        public int? X;
        public int? Y;
        public string? Target;

        public override Type TableType()
        {
            return typeof(ActionTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ActionTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            ActionTable other = (ActionTable)obj;

            if (other == null)
            {
                return false;
            }
            if (Action != other.Action)
            {
                return false;
            }
            if (X != other.X)
            {
                return false;
            }
            if (Y != other.Y)
            {
                return false;
            }
            if (Target != other.Target)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            Action = (int)functionParameters[0];

            X = (int)functionParameters[1];

            Y = (int)functionParameters[2];

            Target = (string)functionParameters[3];
        }

        public static IObservable<RecordUpdate> GetActionTableUpdates()
        {
            ActionTable mudTable = new ActionTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            Action = (int)property["action"];
            X = (int)property["x"];
            Y = (int)property["y"];
            Target = (string)property["target"];
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            int? currentActionTyped = null;
            int? previousActionTyped = null;

            if (currentValue != null && currentValue.ContainsKey("action"))
            {
                currentActionTyped = (int)currentValue["action"];
            }

            if (previousValue != null && previousValue.ContainsKey("action"))
            {
                previousActionTyped = (int)previousValue["action"];
            }
            int? currentXTyped = null;
            int? previousXTyped = null;

            if (currentValue != null && currentValue.ContainsKey("x"))
            {
                currentXTyped = (int)currentValue["x"];
            }

            if (previousValue != null && previousValue.ContainsKey("x"))
            {
                previousXTyped = (int)previousValue["x"];
            }
            int? currentYTyped = null;
            int? previousYTyped = null;

            if (currentValue != null && currentValue.ContainsKey("y"))
            {
                currentYTyped = (int)currentValue["y"];
            }

            if (previousValue != null && previousValue.ContainsKey("y"))
            {
                previousYTyped = (int)previousValue["y"];
            }
            string? currentTargetTyped = null;
            string? previousTargetTyped = null;

            if (currentValue != null && currentValue.ContainsKey("target"))
            {
                currentTargetTyped = (string)currentValue["target"];
            }

            if (previousValue != null && previousValue.ContainsKey("target"))
            {
                previousTargetTyped = (string)previousValue["target"];
            }

            return new ActionTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentRecordValue = recordUpdate.CurrentRecordValue,
                PreviousRecordValue = recordUpdate.PreviousRecordValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                Action = currentActionTyped,
                PreviousAction = previousActionTyped,
                X = currentXTyped,
                PreviousX = previousXTyped,
                Y = currentYTyped,
                PreviousY = previousYTyped,
                Target = currentTargetTyped,
                PreviousTarget = previousTargetTyped,
            };
        }
    }
}

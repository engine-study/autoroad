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
    public class EnumTestTable : MUDTable
    {
        public class EnumTestTableUpdate : RecordUpdate
        {
            public uint? MinMove;
            public uint? PreviousMinMove;
            public uint[]? MaxMove;
            public uint[]? PreviousMaxMove;
            public int[]? IntSmall;
            public int[]? PreviousIntSmall;
            public System.Numerics.BigInteger[]? IntBig;
            public System.Numerics.BigInteger[]? PreviousIntBig;
            public System.Numerics.BigInteger[]? UintBig;
            public System.Numerics.BigInteger[]? PreviousUintBig;
        }

        public readonly static string ID = "EnumTest";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public uint? MinMove;
        public uint[]? MaxMove;
        public int[]? IntSmall;
        public System.Numerics.BigInteger[]? IntBig;
        public System.Numerics.BigInteger[]? UintBig;

        public override Type TableType()
        {
            return typeof(EnumTestTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(EnumTestTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            EnumTestTable other = (EnumTestTable)obj;

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
            if (IntSmall != other.IntSmall)
            {
                return false;
            }
            if (IntBig != other.IntBig)
            {
                return false;
            }
            if (UintBig != other.UintBig)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            MinMove = (uint)functionParameters[0];

            MaxMove = (uint[])functionParameters[1];

            IntSmall = (int[])functionParameters[2];

            IntBig = (System.Numerics.BigInteger[])functionParameters[3];

            UintBig = (System.Numerics.BigInteger[])functionParameters[4];
        }

        public static IObservable<RecordUpdate> GetEnumTestTableUpdates()
        {
            EnumTestTable mudTable = new EnumTestTable();

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override void PropertyToTable(Property property)
        {
            MinMove = (uint)property["minMove"];
            MaxMove = ((object[])property["maxMove"]).Cast<uint>().ToArray();
            IntSmall = ((object[])property["intSmall"]).Cast<int>().ToArray();
            IntBig = ((object[])property["intBig"]).Cast<System.Numerics.BigInteger>().ToArray();
            UintBig = ((object[])property["uintBig"]).Cast<System.Numerics.BigInteger>().ToArray();
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentRecordValue as Property;
            var previousValue = recordUpdate.PreviousRecordValue as Property;
            uint? currentMinMoveTyped = null;
            uint? previousMinMoveTyped = null;

            if (currentValue != null && currentValue.ContainsKey("minmove"))
            {
                currentMinMoveTyped = (uint)currentValue["minmove"];
            }

            if (previousValue != null && previousValue.ContainsKey("minmove"))
            {
                previousMinMoveTyped = (uint)previousValue["minmove"];
            }
            uint[]? currentMaxMoveTyped = null;
            uint[]? previousMaxMoveTyped = null;

            if (currentValue != null && currentValue.ContainsKey("maxmove"))
            {
                currentMaxMoveTyped = ((object[])currentValue["maxmove"]).Cast<uint>().ToArray();
            }

            if (previousValue != null && previousValue.ContainsKey("maxmove"))
            {
                previousMaxMoveTyped = ((object[])previousValue["maxmove"]).Cast<uint>().ToArray();
            }
            int[]? currentIntSmallTyped = null;
            int[]? previousIntSmallTyped = null;

            if (currentValue != null && currentValue.ContainsKey("intsmall"))
            {
                currentIntSmallTyped = ((object[])currentValue["intsmall"]).Cast<int>().ToArray();
            }

            if (previousValue != null && previousValue.ContainsKey("intsmall"))
            {
                previousIntSmallTyped = ((object[])previousValue["intsmall"]).Cast<int>().ToArray();
            }
            System.Numerics.BigInteger[]? currentIntBigTyped = null;
            System.Numerics.BigInteger[]? previousIntBigTyped = null;

            if (currentValue != null && currentValue.ContainsKey("intbig"))
            {
                currentIntBigTyped = ((object[])currentValue["intbig"])
                    .Cast<System.Numerics.BigInteger>()
                    .ToArray();
            }

            if (previousValue != null && previousValue.ContainsKey("intbig"))
            {
                previousIntBigTyped = ((object[])previousValue["intbig"])
                    .Cast<System.Numerics.BigInteger>()
                    .ToArray();
            }
            System.Numerics.BigInteger[]? currentUintBigTyped = null;
            System.Numerics.BigInteger[]? previousUintBigTyped = null;

            if (currentValue != null && currentValue.ContainsKey("uintbig"))
            {
                currentUintBigTyped = ((object[])currentValue["uintbig"])
                    .Cast<System.Numerics.BigInteger>()
                    .ToArray();
            }

            if (previousValue != null && previousValue.ContainsKey("uintbig"))
            {
                previousUintBigTyped = ((object[])previousValue["uintbig"])
                    .Cast<System.Numerics.BigInteger>()
                    .ToArray();
            }

            return new EnumTestTableUpdate
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
                IntSmall = currentIntSmallTyped,
                PreviousIntSmall = previousIntSmallTyped,
                IntBig = currentIntBigTyped,
                PreviousIntBig = previousIntBigTyped,
                UintBig = currentUintBigTyped,
                PreviousUintBig = previousUintBigTyped,
            };
        }
    }
}

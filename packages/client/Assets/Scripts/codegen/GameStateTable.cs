/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud.Client;
using mud.Network.schemas;
using mud.Unity;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameStateTableUpdate
        : TypedRecordUpdate<Tuple<GameStateTable?, GameStateTable?>> { }

    public class GameStateTable : IMudTable
    {
        public readonly static TableId ID = new("", "GameState");

        public override TableId GetTableId()
        {
            return ID;
        }

        public long? miles;
        public long? playerCount;

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
            if (miles != other.miles)
            {
                return false;
            }
            if (playerCount != other.playerCount)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            miles = (long)(int)functionParameters[0];

            playerCount = (long)(int)functionParameters[1];
        }

        public override void RecordToTable(Record record)
        {
            var table = record.value;
            //bool hasValues = false;

            var milesValue = (long)table["miles"];
            miles = milesValue;
            var playerCountValue = (long)table["playerCount"];
            playerCount = playerCountValue;
        }

        public override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
        {
            GameStateTableUpdate update = (GameStateTableUpdate)tableUpdate;
            return update?.TypedValue.Item1;
        }

        public override RecordUpdate CreateTypedRecord(RecordUpdate newUpdate)
        {
            return new GameStateTableUpdate
            {
                TableId = newUpdate.TableId,
                Key = newUpdate.Key,
                Value = newUpdate.Value,
                TypedValue = MapUpdates(newUpdate.Value)
            };
        }

        public static Tuple<GameStateTable?, GameStateTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            GameStateTable? current = null;
            GameStateTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new GameStateTable
                    {
                        miles = value.Item1.TryGetValue("miles", out var milesVal)
                            ? (long)milesVal
                            : default,
                        playerCount = value.Item1.TryGetValue("playerCount", out var playerCountVal)
                            ? (long)playerCountVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new GameStateTable { miles = null, playerCount = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new GameStateTable
                    {
                        miles = value.Item2.TryGetValue("miles", out var milesVal)
                            ? (long)milesVal
                            : default,
                        playerCount = value.Item2.TryGetValue("playerCount", out var playerCountVal)
                            ? (long)playerCountVal
                            : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new GameStateTable { miles = null, playerCount = null, };
                }
            }

            return new Tuple<GameStateTable?, GameStateTable?>(current, previous);
        }
    }
}

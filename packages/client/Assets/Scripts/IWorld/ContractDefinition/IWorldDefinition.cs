using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace IWorld.ContractDefinition
{


    public partial class IWorldDeployment : IWorldDeploymentBase
    {
        public IWorldDeployment() : base(BYTECODE) { }
        public IWorldDeployment(string byteCode) : base(byteCode) { }
    }

    public class IWorldDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IWorldDeploymentBase() : base(BYTECODE) { }
        public IWorldDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class ActionFunction : ActionFunctionBase { }

    [Function("action")]
    public class ActionFunctionBase : FunctionMessage
    {
        [Parameter("uint8", "newAction", 1)]
        public virtual byte NewAction { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class AddCoinsAdminFunction : AddCoinsAdminFunctionBase { }

    [Function("addCoinsAdmin")]
    public class AddCoinsAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "amount", 1)]
        public virtual int Amount { get; set; }
    }

    public partial class AddGemXPFunction : AddGemXPFunctionBase { }

    [Function("addGemXP")]
    public class AddGemXPFunctionBase : FunctionMessage
    {
        [Parameter("int32", "amount", 1)]
        public virtual int Amount { get; set; }
    }

    public partial class AddXPAdminFunction : AddXPAdminFunctionBase { }

    [Function("addXPAdmin")]
    public class AddXPAdminFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class BatchCallFunction : BatchCallFunctionBase { }

    [Function("batchCall", "bytes[]")]
    public class BatchCallFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "systemCalls", 1)]
        public virtual List<SystemCallData> SystemCalls { get; set; }
    }

    public partial class BatchCallFromFunction : BatchCallFromFunctionBase { }

    [Function("batchCallFrom", "bytes[]")]
    public class BatchCallFromFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "systemCalls", 1)]
        public virtual List<SystemCallFromData> SystemCalls { get; set; }
    }

    public partial class BuyFunction : BuyFunctionBase { }

    [Function("buy")]
    public class BuyFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "id", 1)]
        public virtual uint Id { get; set; }
        [Parameter("uint8", "payment", 2)]
        public virtual byte Payment { get; set; }
    }

    public partial class BuyItemFunction : BuyItemFunctionBase { }

    [Function("buyItem")]
    public class BuyItemFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "sender", 1)]
        public virtual byte[] Sender { get; set; }
        [Parameter("bytes32", "seller", 2)]
        public virtual byte[] Seller { get; set; }
        [Parameter("uint32", "id", 3)]
        public virtual uint Id { get; set; }
        [Parameter("uint8", "payment", 4)]
        public virtual byte Payment { get; set; }
    }

    public partial class CallFunction : CallFunctionBase { }

    [Function("call", "bytes")]
    public class CallFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "systemId", 1)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("bytes", "callData", 2)]
        public virtual byte[] CallData { get; set; }
    }

    public partial class CallFlingFunction : CallFlingFunctionBase { }

    [Function("callFling")]
    public class CallFlingFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "entity", 3)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "targetPos", 4)]
        public virtual PositionData TargetPos { get; set; }
        [Parameter("tuple", "entityPos", 5)]
        public virtual PositionData EntityPos { get; set; }
    }

    public partial class CallFromFunction : CallFromFunctionBase { }

    [Function("callFrom", "bytes")]
    public class CallFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "delegator", 1)]
        public virtual string Delegator { get; set; }
        [Parameter("bytes32", "systemId", 2)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("bytes", "callData", 3)]
        public virtual byte[] CallData { get; set; }
    }

    public partial class CanAggroEntityFunction : CanAggroEntityFunctionBase { }

    [Function("canAggroEntity", "bool")]
    public class CanAggroEntityFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "attacker", 1)]
        public virtual byte[] Attacker { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
    }

    public partial class CanBuyFunction : CanBuyFunctionBase { }

    [Function("canBuy", "bool")]
    public class CanBuyFunctionBase : FunctionMessage
    {
        [Parameter("int32", "price", 1)]
        public virtual int Price { get; set; }
        [Parameter("int32", "gems", 2)]
        public virtual int Gems { get; set; }
        [Parameter("int32", "eth", 3)]
        public virtual int Eth { get; set; }
    }

    public partial class ChopFunction : ChopFunctionBase { }

    [Function("chop")]
    public class ChopFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class ContemplateMileFunction : ContemplateMileFunctionBase { }

    [Function("contemplateMile")]
    public class ContemplateMileFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("int32", "mileNumber", 2)]
        public virtual int MileNumber { get; set; }
    }

    public partial class CreateItemMappingFunction : CreateItemMappingFunctionBase { }

    [Function("createItemMapping")]
    public class CreateItemMappingFunctionBase : FunctionMessage
    {

    }

    public partial class CreateMileFunction : CreateMileFunctionBase { }

    [Function("createMile")]
    public class CreateMileFunctionBase : FunctionMessage
    {

    }

    public partial class CreateMiliariumFunction : CreateMiliariumFunctionBase { }

    [Function("createMiliarium")]
    public class CreateMiliariumFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("int32", "width", 2)]
        public virtual int Width { get; set; }
        [Parameter("int32", "up", 3)]
        public virtual int Up { get; set; }
        [Parameter("int32", "down", 4)]
        public virtual int Down { get; set; }
    }

    public partial class CreatePuzzleOnMileFunction : CreatePuzzleOnMileFunctionBase { }

    [Function("createPuzzleOnMile")]
    public class CreatePuzzleOnMileFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
    }

    public partial class CreateStatuePuzzleFunction : CreateStatuePuzzleFunctionBase { }

    [Function("createStatuePuzzle")]
    public class CreateStatuePuzzleFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("int32", "width", 2)]
        public virtual int Width { get; set; }
        [Parameter("int32", "up", 3)]
        public virtual int Up { get; set; }
        [Parameter("int32", "down", 4)]
        public virtual int Down { get; set; }
    }

    public partial class CreateTickersFunction : CreateTickersFunctionBase { }

    [Function("createTickers")]
    public class CreateTickersFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("int32", "width", 2)]
        public virtual int Width { get; set; }
        [Parameter("int32", "up", 3)]
        public virtual int Up { get; set; }
        [Parameter("int32", "down", 4)]
        public virtual int Down { get; set; }
    }

    public partial class CreateWorldFunction : CreateWorldFunctionBase { }

    [Function("createWorld")]
    public class CreateWorldFunctionBase : FunctionMessage
    {

    }

    public partial class CreatorFunction : CreatorFunctionBase { }

    [Function("creator", "address")]
    public class CreatorFunctionBase : FunctionMessage
    {

    }

    public partial class DebugMileFunction : DebugMileFunctionBase { }

    [Function("debugMile")]
    public class DebugMileFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "credit", 1)]
        public virtual byte[] Credit { get; set; }
    }

    public partial class DeleteAdminFunction : DeleteAdminFunctionBase { }

    [Function("deleteAdmin")]
    public class DeleteAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
        [Parameter("int32", "layer", 3)]
        public virtual int Layer { get; set; }
    }

    public partial class DeleteRecordFunction : DeleteRecordFunctionBase { }

    [Function("deleteRecord")]
    public class DeleteRecordFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
    }

    public partial class DestroyFunction : DestroyFunctionBase { }

    [Function("destroy")]
    public class DestroyFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "attacker", 3)]
        public virtual byte[] Attacker { get; set; }
        [Parameter("tuple", "pos", 4)]
        public virtual PositionData Pos { get; set; }
    }

    public partial class DestroyPlayerAdminFunction : DestroyPlayerAdminFunctionBase { }

    [Function("destroyPlayerAdmin")]
    public class DestroyPlayerAdminFunctionBase : FunctionMessage
    {

    }

    public partial class DoAggroFunction : DoAggroFunctionBase { }

    [Function("doAggro")]
    public class DoAggroFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "attacker", 3)]
        public virtual byte[] Attacker { get; set; }
        [Parameter("tuple", "targetPos", 4)]
        public virtual PositionData TargetPos { get; set; }
        [Parameter("tuple", "attackerPos", 5)]
        public virtual PositionData AttackerPos { get; set; }
    }

    public partial class DoArrowFunction : DoArrowFunctionBase { }

    [Function("doArrow")]
    public class DoArrowFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "attacker", 3)]
        public virtual byte[] Attacker { get; set; }
        [Parameter("tuple", "targetPos", 4)]
        public virtual PositionData TargetPos { get; set; }
        [Parameter("tuple", "attackerPos", 5)]
        public virtual PositionData AttackerPos { get; set; }
    }

    public partial class DoFlingFunction : DoFlingFunctionBase { }

    [Function("doFling")]
    public class DoFlingFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("tuple", "startPos", 3)]
        public virtual PositionData StartPos { get; set; }
        [Parameter("tuple", "endPos", 4)]
        public virtual PositionData EndPos { get; set; }
    }

    public partial class DoSeekFunction : DoSeekFunctionBase { }

    [Function("doSeek")]
    public class DoSeekFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "seek", 3)]
        public virtual byte[] Seek { get; set; }
        [Parameter("tuple", "targetPos", 4)]
        public virtual PositionData TargetPos { get; set; }
        [Parameter("tuple", "seekerPos", 5)]
        public virtual PositionData SeekerPos { get; set; }
    }

    public partial class DoWanderFunction : DoWanderFunctionBase { }

    [Function("doWander")]
    public class DoWanderFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "entityPos", 3)]
        public virtual PositionData EntityPos { get; set; }
    }

    public partial class FinishMileFunction : FinishMileFunctionBase { }

    [Function("finishMile")]
    public class FinishMileFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "chunk", 2)]
        public virtual byte[] Chunk { get; set; }
        [Parameter("int32", "currentMile", 3)]
        public virtual int CurrentMile { get; set; }
        [Parameter("uint32", "pieces", 4)]
        public virtual uint Pieces { get; set; }
    }

    public partial class FinishMileAdminFunction : FinishMileAdminFunctionBase { }

    [Function("finishMileAdmin")]
    public class FinishMileAdminFunctionBase : FunctionMessage
    {

    }

    public partial class FishFunction : FishFunctionBase { }

    [Function("fish")]
    public class FishFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class GetDynamicFieldFunction : GetDynamicFieldFunctionBase { }

    [Function("getDynamicField", "bytes")]
    public class GetDynamicFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
    }

    public partial class GetDynamicFieldLengthFunction : GetDynamicFieldLengthFunctionBase { }

    [Function("getDynamicFieldLength", "uint256")]
    public class GetDynamicFieldLengthFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
    }

    public partial class GetDynamicFieldSliceFunction : GetDynamicFieldSliceFunctionBase { }

    [Function("getDynamicFieldSlice", "bytes")]
    public class GetDynamicFieldSliceFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
        [Parameter("uint256", "start", 4)]
        public virtual BigInteger Start { get; set; }
        [Parameter("uint256", "end", 5)]
        public virtual BigInteger End { get; set; }
    }

    public partial class GetField1Function : GetField1FunctionBase { }

    [Function("getField", "bytes")]
    public class GetField1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
        [Parameter("bytes32", "fieldLayout", 4)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class GetFieldFunction : GetFieldFunctionBase { }

    [Function("getField", "bytes")]
    public class GetFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
    }

    public partial class GetFieldLayoutFunction : GetFieldLayoutFunctionBase { }

    [Function("getFieldLayout", "bytes32")]
    public class GetFieldLayoutFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
    }

    public partial class GetFieldLength1Function : GetFieldLength1FunctionBase { }

    [Function("getFieldLength", "uint256")]
    public class GetFieldLength1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
        [Parameter("bytes32", "fieldLayout", 4)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class GetFieldLengthFunction : GetFieldLengthFunctionBase { }

    [Function("getFieldLength", "uint256")]
    public class GetFieldLengthFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
    }

    public partial class GetKeySchemaFunction : GetKeySchemaFunctionBase { }

    [Function("getKeySchema", "bytes32")]
    public class GetKeySchemaFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
    }

    public partial class GetRecord1Function : GetRecord1FunctionBase { }

    [Function("getRecord", typeof(GetRecord1OutputDTO))]
    public class GetRecord1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("bytes32", "fieldLayout", 3)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class GetRecordFunction : GetRecordFunctionBase { }

    [Function("getRecord", typeof(GetRecordOutputDTO))]
    public class GetRecordFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
    }

    public partial class GetStaticFieldFunction : GetStaticFieldFunctionBase { }

    [Function("getStaticField", "bytes32")]
    public class GetStaticFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
        [Parameter("bytes32", "fieldLayout", 4)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class GetValueSchemaFunction : GetValueSchemaFunctionBase { }

    [Function("getValueSchema", "bytes32")]
    public class GetValueSchemaFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
    }

    public partial class GiveCoinsFunction : GiveCoinsFunctionBase { }

    [Function("giveCoins")]
    public class GiveCoinsFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "amount", 2)]
        public virtual int Amount { get; set; }
    }

    public partial class GiveGemFunction : GiveGemFunctionBase { }

    [Function("giveGem")]
    public class GiveGemFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "amount", 2)]
        public virtual int Amount { get; set; }
    }

    public partial class GiveKillRewardFunction : GiveKillRewardFunctionBase { }

    [Function("giveKillReward")]
    public class GiveKillRewardFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
    }

    public partial class GivePuzzleRewardFunction : GivePuzzleRewardFunctionBase { }

    [Function("givePuzzleReward")]
    public class GivePuzzleRewardFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
    }

    public partial class GiveRoadFilledRewardFunction : GiveRoadFilledRewardFunctionBase { }

    [Function("giveRoadFilledReward")]
    public class GiveRoadFilledRewardFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
    }

    public partial class GiveRoadLotteryFunction : GiveRoadLotteryFunctionBase { }

    [Function("giveRoadLottery")]
    public class GiveRoadLotteryFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "road", 1)]
        public virtual byte[] Road { get; set; }
    }

    public partial class GiveRoadShoveledRewardFunction : GiveRoadShoveledRewardFunctionBase { }

    [Function("giveRoadShoveledReward")]
    public class GiveRoadShoveledRewardFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
    }

    public partial class GiveXPFunction : GiveXPFunctionBase { }

    [Function("giveXP")]
    public class GiveXPFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class GrantAccessFunction : GrantAccessFunctionBase { }

    [Function("grantAccess")]
    public class GrantAccessFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "resourceId", 1)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("address", "grantee", 2)]
        public virtual string Grantee { get; set; }
    }

    public partial class HandleMoveTypeFunction : HandleMoveTypeFunctionBase { }

    [Function("handleMoveType", "bool")]
    public class HandleMoveTypeFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "to", 3)]
        public virtual PositionData To { get; set; }
        [Parameter("bytes32", "atDest", 4)]
        public virtual byte[] AtDest { get; set; }
        [Parameter("uint8", "moveTypeAtDest", 5)]
        public virtual byte MoveTypeAtDest { get; set; }
        [Parameter("uint8", "actionType", 6)]
        public virtual byte ActionType { get; set; }
    }

    public partial class HelpSummonFunction : HelpSummonFunctionBase { }

    [Function("helpSummon")]
    public class HelpSummonFunctionBase : FunctionMessage
    {

    }

    public partial class InitializeFunction : InitializeFunctionBase { }

    [Function("initialize")]
    public class InitializeFunctionBase : FunctionMessage
    {
        [Parameter("address", "coreModule", 1)]
        public virtual string CoreModule { get; set; }
    }

    public partial class InstallModuleFunction : InstallModuleFunctionBase { }

    [Function("installModule")]
    public class InstallModuleFunctionBase : FunctionMessage
    {
        [Parameter("address", "module", 1)]
        public virtual string Module { get; set; }
        [Parameter("bytes", "args", 2)]
        public virtual byte[] Args { get; set; }
    }

    public partial class InstallRootModuleFunction : InstallRootModuleFunctionBase { }

    [Function("installRootModule")]
    public class InstallRootModuleFunctionBase : FunctionMessage
    {
        [Parameter("address", "module", 1)]
        public virtual string Module { get; set; }
        [Parameter("bytes", "args", 2)]
        public virtual byte[] Args { get; set; }
    }

    public partial class IsAdminFunction : IsAdminFunctionBase { }

    [Function("isAdmin", "bool")]
    public class IsAdminFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
    }

    public partial class KillFunction : KillFunctionBase { }

    [Function("kill")]
    public class KillFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "attacker", 3)]
        public virtual byte[] Attacker { get; set; }
        [Parameter("tuple", "pos", 4)]
        public virtual PositionData Pos { get; set; }
    }

    public partial class KillPlayerAdminFunction : KillPlayerAdminFunctionBase { }

    [Function("killPlayerAdmin")]
    public class KillPlayerAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class KillRewardsFunction : KillRewardsFunctionBase { }

    [Function("killRewards")]
    public class KillRewardsFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "attacker", 3)]
        public virtual byte[] Attacker { get; set; }
    }

    public partial class ManifestFunction : ManifestFunctionBase { }

    [Function("manifest", "bool")]
    public class ManifestFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "item", 1)]
        public virtual uint Item { get; set; }
    }

    public partial class MegaSummonFunction : MegaSummonFunctionBase { }

    [Function("megaSummon")]
    public class MegaSummonFunctionBase : FunctionMessage
    {

    }

    public partial class MeleeFunction : MeleeFunctionBase { }

    [Function("melee")]
    public class MeleeFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class MineFunction : MineFunctionBase { }

    [Function("mine")]
    public class MineFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class MoveOrPushFunction : MoveOrPushFunctionBase { }

    [Function("moveOrPush")]
    public class MoveOrPushFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "player", 2)]
        public virtual byte[] Player { get; set; }
        [Parameter("tuple", "startPos", 3)]
        public virtual PositionData StartPos { get; set; }
        [Parameter("tuple", "vector", 4)]
        public virtual PositionData Vector { get; set; }
        [Parameter("int32", "distance", 5)]
        public virtual int Distance { get; set; }
    }

    public partial class MoveSimpleFunction : MoveSimpleFunctionBase { }

    [Function("moveSimple")]
    public class MoveSimpleFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class MoveSimpleDistanceFunction : MoveSimpleDistanceFunctionBase { }

    [Function("moveSimpleDistance")]
    public class MoveSimpleDistanceFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
        [Parameter("int32", "distance", 4)]
        public virtual int Distance { get; set; }
    }

    public partial class MoveToFunction : MoveToFunctionBase { }

    [Function("moveTo")]
    public class MoveToFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "from", 3)]
        public virtual PositionData From { get; set; }
        [Parameter("tuple", "to", 4)]
        public virtual PositionData To { get; set; }
        [Parameter("bytes32[]", "atDest", 5)]
        public virtual List<byte[]> AtDest { get; set; }
        [Parameter("uint8", "actionType", 6)]
        public virtual byte ActionType { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name")]
    public class NameFunctionBase : FunctionMessage
    {
        [Parameter("uint32", "firstName", 1)]
        public virtual uint FirstName { get; set; }
        [Parameter("uint32", "middleName", 2)]
        public virtual uint MiddleName { get; set; }
        [Parameter("uint32", "lastName", 3)]
        public virtual uint LastName { get; set; }
    }

    public partial class NeumanNeighborhoodFunction : NeumanNeighborhoodFunctionBase { }

    [Function("neumanNeighborhood", typeof(NeumanNeighborhoodOutputDTO))]
    public class NeumanNeighborhoodFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "center", 1)]
        public virtual PositionData Center { get; set; }
        [Parameter("int32", "distance", 2)]
        public virtual int Distance { get; set; }
    }

    public partial class NeumanNeighborhoodOuterFunction : NeumanNeighborhoodOuterFunctionBase { }

    [Function("neumanNeighborhoodOuter", typeof(NeumanNeighborhoodOuterOutputDTO))]
    public class NeumanNeighborhoodOuterFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "center", 1)]
        public virtual PositionData Center { get; set; }
        [Parameter("int32", "distance", 2)]
        public virtual int Distance { get; set; }
    }

    public partial class PlantFunction : PlantFunctionBase { }

    [Function("plant")]
    public class PlantFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class PopFromDynamicFieldFunction : PopFromDynamicFieldFunctionBase { }

    [Function("popFromDynamicField")]
    public class PopFromDynamicFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
        [Parameter("uint256", "byteLengthToPop", 4)]
        public virtual BigInteger ByteLengthToPop { get; set; }
    }

    public partial class PushFunction : PushFunctionBase { }

    [Function("push")]
    public class PushFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class PushToDynamicFieldFunction : PushToDynamicFieldFunctionBase { }

    [Function("pushToDynamicField")]
    public class PushToDynamicFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
        [Parameter("bytes", "dataToPush", 4)]
        public virtual byte[] DataToPush { get; set; }
    }

    public partial class RegisterDelegationFunction : RegisterDelegationFunctionBase { }

    [Function("registerDelegation")]
    public class RegisterDelegationFunctionBase : FunctionMessage
    {
        [Parameter("address", "delegatee", 1)]
        public virtual string Delegatee { get; set; }
        [Parameter("bytes32", "delegationControlId", 2)]
        public virtual byte[] DelegationControlId { get; set; }
        [Parameter("bytes", "initCallData", 3)]
        public virtual byte[] InitCallData { get; set; }
    }

    public partial class RegisterFunctionSelectorFunction : RegisterFunctionSelectorFunctionBase { }

    [Function("registerFunctionSelector", "bytes4")]
    public class RegisterFunctionSelectorFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "systemId", 1)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("string", "systemFunctionSignature", 2)]
        public virtual string SystemFunctionSignature { get; set; }
    }

    public partial class RegisterNamespaceFunction : RegisterNamespaceFunctionBase { }

    [Function("registerNamespace")]
    public class RegisterNamespaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "namespaceId", 1)]
        public virtual byte[] NamespaceId { get; set; }
    }

    public partial class RegisterNamespaceDelegationFunction : RegisterNamespaceDelegationFunctionBase { }

    [Function("registerNamespaceDelegation")]
    public class RegisterNamespaceDelegationFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "namespaceId", 1)]
        public virtual byte[] NamespaceId { get; set; }
        [Parameter("bytes32", "delegationControlId", 2)]
        public virtual byte[] DelegationControlId { get; set; }
        [Parameter("bytes", "initCallData", 3)]
        public virtual byte[] InitCallData { get; set; }
    }

    public partial class RegisterRootFunctionSelectorFunction : RegisterRootFunctionSelectorFunctionBase { }

    [Function("registerRootFunctionSelector", "bytes4")]
    public class RegisterRootFunctionSelectorFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "systemId", 1)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("string", "worldFunctionSignature", 2)]
        public virtual string WorldFunctionSignature { get; set; }
        [Parameter("bytes4", "systemFunctionSelector", 3)]
        public virtual byte[] SystemFunctionSelector { get; set; }
    }

    public partial class RegisterStoreHookFunction : RegisterStoreHookFunctionBase { }

    [Function("registerStoreHook")]
    public class RegisterStoreHookFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("address", "hookAddress", 2)]
        public virtual string HookAddress { get; set; }
        [Parameter("uint8", "enabledHooksBitmap", 3)]
        public virtual byte EnabledHooksBitmap { get; set; }
    }

    public partial class RegisterSystemFunction : RegisterSystemFunctionBase { }

    [Function("registerSystem")]
    public class RegisterSystemFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "systemId", 1)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("address", "system", 2)]
        public virtual string System { get; set; }
        [Parameter("bool", "publicAccess", 3)]
        public virtual bool PublicAccess { get; set; }
    }

    public partial class RegisterSystemHookFunction : RegisterSystemHookFunctionBase { }

    [Function("registerSystemHook")]
    public class RegisterSystemHookFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "systemId", 1)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("address", "hookAddress", 2)]
        public virtual string HookAddress { get; set; }
        [Parameter("uint8", "enabledHooksBitmap", 3)]
        public virtual byte EnabledHooksBitmap { get; set; }
    }

    public partial class RegisterTableFunction : RegisterTableFunctionBase { }

    [Function("registerTable")]
    public class RegisterTableFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32", "fieldLayout", 2)]
        public virtual byte[] FieldLayout { get; set; }
        [Parameter("bytes32", "keySchema", 3)]
        public virtual byte[] KeySchema { get; set; }
        [Parameter("bytes32", "valueSchema", 4)]
        public virtual byte[] ValueSchema { get; set; }
        [Parameter("string[]", "keyNames", 5)]
        public virtual List<string> KeyNames { get; set; }
        [Parameter("string[]", "fieldNames", 6)]
        public virtual List<string> FieldNames { get; set; }
    }

    public partial class ResetPlayerFunction : ResetPlayerFunctionBase { }

    [Function("resetPlayer")]
    public class ResetPlayerFunctionBase : FunctionMessage
    {

    }

    public partial class RevokeAccessFunction : RevokeAccessFunctionBase { }

    [Function("revokeAccess")]
    public class RevokeAccessFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "resourceId", 1)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("address", "grantee", 2)]
        public virtual string Grantee { get; set; }
    }

    public partial class SendCoinsFunction : SendCoinsFunctionBase { }

    [Function("sendCoins")]
    public class SendCoinsFunctionBase : FunctionMessage
    {
        [Parameter("int32", "amount", 1)]
        public virtual int Amount { get; set; }
    }

    public partial class SetDynamicFieldFunction : SetDynamicFieldFunctionBase { }

    [Function("setDynamicField")]
    public class SetDynamicFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SetFieldFunction : SetFieldFunctionBase { }

    [Function("setField")]
    public class SetFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SetField1Function : SetField1FunctionBase { }

    [Function("setField")]
    public class SetField1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
        [Parameter("bytes32", "fieldLayout", 5)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class SetPositionFunction : SetPositionFunctionBase { }

    [Function("setPosition")]
    public class SetPositionFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "pos", 3)]
        public virtual PositionData Pos { get; set; }
        [Parameter("uint8", "action", 4)]
        public virtual byte Action { get; set; }
    }

    public partial class SetRecordFunction : SetRecordFunctionBase { }

    [Function("setRecord")]
    public class SetRecordFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("bytes", "staticData", 3)]
        public virtual byte[] StaticData { get; set; }
        [Parameter("bytes32", "encodedLengths", 4)]
        public virtual byte[] EncodedLengths { get; set; }
        [Parameter("bytes", "dynamicData", 5)]
        public virtual byte[] DynamicData { get; set; }
    }

    public partial class SetStaticFieldFunction : SetStaticFieldFunctionBase { }

    [Function("setStaticField")]
    public class SetStaticFieldFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "fieldIndex", 3)]
        public virtual byte FieldIndex { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
        [Parameter("bytes32", "fieldLayout", 5)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class ShovelFunction : ShovelFunctionBase { }

    [Function("shovel")]
    public class ShovelFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnFunction : SpawnFunctionBase { }

    [Function("spawn")]
    public class SpawnFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnEmptyRoadFunction : SpawnEmptyRoadFunctionBase { }

    [Function("spawnEmptyRoad")]
    public class SpawnEmptyRoadFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnFinishedRoadFunction : SpawnFinishedRoadFunctionBase { }

    [Function("spawnFinishedRoad")]
    public class SpawnFinishedRoadFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "state", 4)]
        public virtual byte State { get; set; }
    }

    public partial class SpawnFinishedRoadAdminFunction : SpawnFinishedRoadAdminFunctionBase { }

    [Function("spawnFinishedRoadAdmin")]
    public class SpawnFinishedRoadAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnFloraFunction : SpawnFloraFunctionBase { }

    [Function("spawnFlora")]
    public class SpawnFloraFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("int32", "x", 3)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 4)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "floraType", 5)]
        public virtual byte FloraType { get; set; }
    }

    public partial class SpawnFloraRandomFunction : SpawnFloraRandomFunctionBase { }

    [Function("spawnFloraRandom")]
    public class SpawnFloraRandomFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("int32", "x", 3)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 4)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnMileAdminFunction : SpawnMileAdminFunctionBase { }

    [Function("spawnMileAdmin")]
    public class SpawnMileAdminFunctionBase : FunctionMessage
    {

    }

    public partial class SpawnNPCFunction : SpawnNPCFunctionBase { }

    [Function("spawnNPC")]
    public class SpawnNPCFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "spawner", 1)]
        public virtual byte[] Spawner { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "npcType", 4)]
        public virtual byte NpcType { get; set; }
    }

    public partial class SpawnNPCAdminFunction : SpawnNPCAdminFunctionBase { }

    [Function("spawnNPCAdmin")]
    public class SpawnNPCAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "npcType", 3)]
        public virtual byte NpcType { get; set; }
    }

    public partial class SpawnPlayerFunction : SpawnPlayerFunctionBase { }

    [Function("spawnPlayer")]
    public class SpawnPlayerFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "entity", 1)]
        public virtual byte[] Entity { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
        [Parameter("bool", "isBot", 4)]
        public virtual bool IsBot { get; set; }
    }

    public partial class SpawnPlayerNPCFunction : SpawnPlayerNPCFunctionBase { }

    [Function("spawnPlayerNPC")]
    public class SpawnPlayerNPCFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "entity", 1)]
        public virtual byte[] Entity { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnPuzzleAdminFunction : SpawnPuzzleAdminFunctionBase { }

    [Function("spawnPuzzleAdmin")]
    public class SpawnPuzzleAdminFunctionBase : FunctionMessage
    {

    }

    public partial class SpawnRoadFromPushFunction : SpawnRoadFromPushFunctionBase { }

    [Function("spawnRoadFromPush")]
    public class SpawnRoadFromPushFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "pushed", 2)]
        public virtual byte[] Pushed { get; set; }
        [Parameter("bytes32", "road", 3)]
        public virtual byte[] Road { get; set; }
        [Parameter("tuple", "pos", 4)]
        public virtual PositionData Pos { get; set; }
    }

    public partial class SpawnShoveledRoadFunction : SpawnShoveledRoadFunctionBase { }

    [Function("spawnShoveledRoad")]
    public class SpawnShoveledRoadFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnShoveledRoadAdminFunction : SpawnShoveledRoadAdminFunctionBase { }

    [Function("spawnShoveledRoadAdmin")]
    public class SpawnShoveledRoadAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class SpawnTerrainFunction : SpawnTerrainFunctionBase { }

    [Function("spawnTerrain")]
    public class SpawnTerrainFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "tType", 4)]
        public virtual byte TType { get; set; }
    }

    public partial class SpawnTerrainAdminFunction : SpawnTerrainAdminFunctionBase { }

    [Function("spawnTerrainAdmin")]
    public class SpawnTerrainAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "terrainType", 3)]
        public virtual byte TerrainType { get; set; }
    }

    public partial class SpawnTickerFunction : SpawnTickerFunctionBase { }

    [Function("spawnTicker")]
    public class SpawnTickerFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "entity", 1)]
        public virtual byte[] Entity { get; set; }
    }

    public partial class SpliceDynamicDataFunction : SpliceDynamicDataFunctionBase { }

    [Function("spliceDynamicData")]
    public class SpliceDynamicDataFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint8", "dynamicFieldIndex", 3)]
        public virtual byte DynamicFieldIndex { get; set; }
        [Parameter("uint40", "startWithinField", 4)]
        public virtual ulong StartWithinField { get; set; }
        [Parameter("uint40", "deleteCount", 5)]
        public virtual ulong DeleteCount { get; set; }
        [Parameter("bytes", "data", 6)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SpliceStaticDataFunction : SpliceStaticDataFunctionBase { }

    [Function("spliceStaticData")]
    public class SpliceStaticDataFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2)]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint48", "start", 3)]
        public virtual ulong Start { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
    }

    public partial class StickFunction : StickFunctionBase { }

    [Function("stick")]
    public class StickFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class StoreVersionFunction : StoreVersionFunctionBase { }

    [Function("storeVersion", "bytes32")]
    public class StoreVersionFunctionBase : FunctionMessage
    {

    }

    public partial class SummonMileFunction : SummonMileFunctionBase { }

    [Function("summonMile")]
    public class SummonMileFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bool", "summonAll", 2)]
        public virtual bool SummonAll { get; set; }
    }

    public partial class SummonRowFunction : SummonRowFunctionBase { }

    [Function("summonRow", "int32")]
    public class SummonRowFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("int32", "left", 2)]
        public virtual int Left { get; set; }
        [Parameter("int32", "right", 3)]
        public virtual int Right { get; set; }
        [Parameter("uint256", "difficulty", 4)]
        public virtual BigInteger Difficulty { get; set; }
    }

    public partial class SupFunction : SupFunctionBase { }

    [Function("sup")]
    public class SupFunctionBase : FunctionMessage
    {

    }

    public partial class SwapScrollFunction : SwapScrollFunctionBase { }

    [Function("swapScroll")]
    public class SwapScrollFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class TeleportFunction : TeleportFunctionBase { }

    [Function("teleport")]
    public class TeleportFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
        [Parameter("uint8", "actionType", 4)]
        public virtual byte ActionType { get; set; }
    }

    public partial class TeleportAdminFunction : TeleportAdminFunctionBase { }

    [Function("teleportAdmin")]
    public class TeleportAdminFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class TeleportScrollFunction : TeleportScrollFunctionBase { }

    [Function("teleportScroll")]
    public class TeleportScrollFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "player", 1)]
        public virtual byte[] Player { get; set; }
        [Parameter("int32", "x", 2)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 3)]
        public virtual int Y { get; set; }
    }

    public partial class TickActionFunction : TickActionFunctionBase { }

    [Function("tickAction")]
    public class TickActionFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "entityPos", 3)]
        public virtual PositionData EntityPos { get; set; }
    }

    public partial class TickBehaviourFunction : TickBehaviourFunctionBase { }

    [Function("tickBehaviour")]
    public class TickBehaviourFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "target", 2)]
        public virtual byte[] Target { get; set; }
        [Parameter("bytes32", "entity", 3)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "targetPos", 4)]
        public virtual PositionData TargetPos { get; set; }
        [Parameter("tuple", "entityPos", 5)]
        public virtual PositionData EntityPos { get; set; }
    }

    public partial class TickEntityFunction : TickEntityFunctionBase { }

    [Function("tickEntity")]
    public class TickEntityFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
    }

    public partial class TransferBalanceToAddressFunction : TransferBalanceToAddressFunctionBase { }

    [Function("transferBalanceToAddress")]
    public class TransferBalanceToAddressFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "fromNamespaceId", 1)]
        public virtual byte[] FromNamespaceId { get; set; }
        [Parameter("address", "toAddress", 2)]
        public virtual string ToAddress { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferBalanceToNamespaceFunction : TransferBalanceToNamespaceFunctionBase { }

    [Function("transferBalanceToNamespace")]
    public class TransferBalanceToNamespaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "fromNamespaceId", 1)]
        public virtual byte[] FromNamespaceId { get; set; }
        [Parameter("bytes32", "toNamespaceId", 2)]
        public virtual byte[] ToNamespaceId { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "namespaceId", 1)]
        public virtual byte[] NamespaceId { get; set; }
        [Parameter("address", "newOwner", 2)]
        public virtual string NewOwner { get; set; }
    }

    public partial class TriggerEntitiesFunction : TriggerEntitiesFunctionBase { }

    [Function("triggerEntities")]
    public class TriggerEntitiesFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "player", 2)]
        public virtual byte[] Player { get; set; }
        [Parameter("tuple", "pos", 3)]
        public virtual PositionData Pos { get; set; }
    }

    public partial class TriggerPuzzlesFunction : TriggerPuzzlesFunctionBase { }

    [Function("triggerPuzzles")]
    public class TriggerPuzzlesFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
        [Parameter("bytes32", "entity", 2)]
        public virtual byte[] Entity { get; set; }
        [Parameter("tuple", "pos", 3)]
        public virtual PositionData Pos { get; set; }
    }

    public partial class TriggerTicksFunction : TriggerTicksFunctionBase { }

    [Function("triggerTicks")]
    public class TriggerTicksFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedby", 1)]
        public virtual byte[] Causedby { get; set; }
    }

    public partial class UnregisterStoreHookFunction : UnregisterStoreHookFunctionBase { }

    [Function("unregisterStoreHook")]
    public class UnregisterStoreHookFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("address", "hookAddress", 2)]
        public virtual string HookAddress { get; set; }
    }

    public partial class UnregisterSystemHookFunction : UnregisterSystemHookFunctionBase { }

    [Function("unregisterSystemHook")]
    public class UnregisterSystemHookFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "systemId", 1)]
        public virtual byte[] SystemId { get; set; }
        [Parameter("address", "hookAddress", 2)]
        public virtual string HookAddress { get; set; }
    }

    public partial class UpdateChunkFunction : UpdateChunkFunctionBase { }

    [Function("updateChunk")]
    public class UpdateChunkFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "causedBy", 1)]
        public virtual byte[] CausedBy { get; set; }
    }

    public partial class WalkFunction : WalkFunctionBase { }

    [Function("walk")]
    public class WalkFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
        [Parameter("int32", "distance", 3)]
        public virtual int Distance { get; set; }
    }

    public partial class WaterFunction : WaterFunctionBase { }

    [Function("water")]
    public class WaterFunctionBase : FunctionMessage
    {
        [Parameter("int32", "x", 1)]
        public virtual int X { get; set; }
        [Parameter("int32", "y", 2)]
        public virtual int Y { get; set; }
    }

    public partial class WorldVersionFunction : WorldVersionFunctionBase { }

    [Function("worldVersion", "bytes32")]
    public class WorldVersionFunctionBase : FunctionMessage
    {

    }

    public partial class HelloStoreEventDTO : HelloStoreEventDTOBase { }

    [Event("HelloStore")]
    public class HelloStoreEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "storeVersion", 1, true )]
        public virtual byte[] StoreVersion { get; set; }
    }

    public partial class HelloWorldEventDTO : HelloWorldEventDTOBase { }

    [Event("HelloWorld")]
    public class HelloWorldEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "worldVersion", 1, true )]
        public virtual byte[] WorldVersion { get; set; }
    }

    public partial class StoreDeleterecordEventDTO : StoreDeleterecordEventDTOBase { }

    [Event("Store_DeleteRecord")]
    public class StoreDeleterecordEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "tableId", 1, true )]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2, false )]
        public virtual List<byte[]> KeyTuple { get; set; }
    }

    public partial class StoreSetrecordEventDTO : StoreSetrecordEventDTOBase { }

    [Event("Store_SetRecord")]
    public class StoreSetrecordEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "tableId", 1, true )]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2, false )]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("bytes", "staticData", 3, false )]
        public virtual byte[] StaticData { get; set; }
        [Parameter("bytes32", "encodedLengths", 4, false )]
        public virtual byte[] EncodedLengths { get; set; }
        [Parameter("bytes", "dynamicData", 5, false )]
        public virtual byte[] DynamicData { get; set; }
    }

    public partial class StoreSplicedynamicdataEventDTO : StoreSplicedynamicdataEventDTOBase { }

    [Event("Store_SpliceDynamicData")]
    public class StoreSplicedynamicdataEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "tableId", 1, true )]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2, false )]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint48", "start", 3, false )]
        public virtual ulong Start { get; set; }
        [Parameter("uint40", "deleteCount", 4, false )]
        public virtual ulong DeleteCount { get; set; }
        [Parameter("bytes32", "encodedLengths", 5, false )]
        public virtual byte[] EncodedLengths { get; set; }
        [Parameter("bytes", "data", 6, false )]
        public virtual byte[] Data { get; set; }
    }

    public partial class StoreSplicestaticdataEventDTO : StoreSplicestaticdataEventDTOBase { }

    [Event("Store_SpliceStaticData")]
    public class StoreSplicestaticdataEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "tableId", 1, true )]
        public virtual byte[] TableId { get; set; }
        [Parameter("bytes32[]", "keyTuple", 2, false )]
        public virtual List<byte[]> KeyTuple { get; set; }
        [Parameter("uint48", "start", 3, false )]
        public virtual ulong Start { get; set; }
        [Parameter("bytes", "data", 4, false )]
        public virtual byte[] Data { get; set; }
    }

    public partial class StoreIndexoutofboundsError : StoreIndexoutofboundsErrorBase { }

    [Error("Store_IndexOutOfBounds")]
    public class StoreIndexoutofboundsErrorBase : IErrorDTO
    {
        [Parameter("uint256", "length", 1)]
        public virtual BigInteger Length { get; set; }
        [Parameter("uint256", "accessedIndex", 2)]
        public virtual BigInteger AccessedIndex { get; set; }
    }

    public partial class StoreInvaliddynamicdatalengthError : StoreInvaliddynamicdatalengthErrorBase { }

    [Error("Store_InvalidDynamicDataLength")]
    public class StoreInvaliddynamicdatalengthErrorBase : IErrorDTO
    {
        [Parameter("uint256", "expected", 1)]
        public virtual BigInteger Expected { get; set; }
        [Parameter("uint256", "received", 2)]
        public virtual BigInteger Received { get; set; }
    }

    public partial class StoreInvalidfieldnameslengthError : StoreInvalidfieldnameslengthErrorBase { }

    [Error("Store_InvalidFieldNamesLength")]
    public class StoreInvalidfieldnameslengthErrorBase : IErrorDTO
    {
        [Parameter("uint256", "expected", 1)]
        public virtual BigInteger Expected { get; set; }
        [Parameter("uint256", "received", 2)]
        public virtual BigInteger Received { get; set; }
    }

    public partial class StoreInvalidkeynameslengthError : StoreInvalidkeynameslengthErrorBase { }

    [Error("Store_InvalidKeyNamesLength")]
    public class StoreInvalidkeynameslengthErrorBase : IErrorDTO
    {
        [Parameter("uint256", "expected", 1)]
        public virtual BigInteger Expected { get; set; }
        [Parameter("uint256", "received", 2)]
        public virtual BigInteger Received { get; set; }
    }

    public partial class StoreInvalidresourcetypeError : StoreInvalidresourcetypeErrorBase { }

    [Error("Store_InvalidResourceType")]
    public class StoreInvalidresourcetypeErrorBase : IErrorDTO
    {
        [Parameter("bytes2", "expected", 1)]
        public virtual byte[] Expected { get; set; }
        [Parameter("bytes32", "resourceId", 2)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("string", "resourceIdString", 3)]
        public virtual string ResourceIdString { get; set; }
    }

    public partial class StoreInvalidspliceError : StoreInvalidspliceErrorBase { }

    [Error("Store_InvalidSplice")]
    public class StoreInvalidspliceErrorBase : IErrorDTO
    {
        [Parameter("uint40", "startWithinField", 1)]
        public virtual ulong StartWithinField { get; set; }
        [Parameter("uint40", "deleteCount", 2)]
        public virtual ulong DeleteCount { get; set; }
        [Parameter("uint40", "fieldLength", 3)]
        public virtual ulong FieldLength { get; set; }
    }

    public partial class StoreInvalidvalueschemalengthError : StoreInvalidvalueschemalengthErrorBase { }

    [Error("Store_InvalidValueSchemaLength")]
    public class StoreInvalidvalueschemalengthErrorBase : IErrorDTO
    {
        [Parameter("uint256", "expected", 1)]
        public virtual BigInteger Expected { get; set; }
        [Parameter("uint256", "received", 2)]
        public virtual BigInteger Received { get; set; }
    }

    public partial class StoreTablealreadyexistsError : StoreTablealreadyexistsErrorBase { }

    [Error("Store_TableAlreadyExists")]
    public class StoreTablealreadyexistsErrorBase : IErrorDTO
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("string", "tableIdString", 2)]
        public virtual string TableIdString { get; set; }
    }

    public partial class StoreTablenotfoundError : StoreTablenotfoundErrorBase { }

    [Error("Store_TableNotFound")]
    public class StoreTablenotfoundErrorBase : IErrorDTO
    {
        [Parameter("bytes32", "tableId", 1)]
        public virtual byte[] TableId { get; set; }
        [Parameter("string", "tableIdString", 2)]
        public virtual string TableIdString { get; set; }
    }

    public partial class WorldAccessdeniedError : WorldAccessdeniedErrorBase { }

    [Error("World_AccessDenied")]
    public class WorldAccessdeniedErrorBase : IErrorDTO
    {
        [Parameter("string", "resource", 1)]
        public virtual string Resource { get; set; }
        [Parameter("address", "caller", 2)]
        public virtual string Caller { get; set; }
    }

    public partial class WorldAlreadyinitializedError : WorldAlreadyinitializedErrorBase { }
    [Error("World_AlreadyInitialized")]
    public class WorldAlreadyinitializedErrorBase : IErrorDTO
    {
    }

    public partial class WorldCallbacknotallowedError : WorldCallbacknotallowedErrorBase { }

    [Error("World_CallbackNotAllowed")]
    public class WorldCallbacknotallowedErrorBase : IErrorDTO
    {
        [Parameter("bytes4", "functionSelector", 1)]
        public virtual byte[] FunctionSelector { get; set; }
    }

    public partial class WorldDelegationnotfoundError : WorldDelegationnotfoundErrorBase { }

    [Error("World_DelegationNotFound")]
    public class WorldDelegationnotfoundErrorBase : IErrorDTO
    {
        [Parameter("address", "delegator", 1)]
        public virtual string Delegator { get; set; }
        [Parameter("address", "delegatee", 2)]
        public virtual string Delegatee { get; set; }
    }

    public partial class WorldFunctionselectoralreadyexistsError : WorldFunctionselectoralreadyexistsErrorBase { }

    [Error("World_FunctionSelectorAlreadyExists")]
    public class WorldFunctionselectoralreadyexistsErrorBase : IErrorDTO
    {
        [Parameter("bytes4", "functionSelector", 1)]
        public virtual byte[] FunctionSelector { get; set; }
    }

    public partial class WorldFunctionselectornotfoundError : WorldFunctionselectornotfoundErrorBase { }

    [Error("World_FunctionSelectorNotFound")]
    public class WorldFunctionselectornotfoundErrorBase : IErrorDTO
    {
        [Parameter("bytes4", "functionSelector", 1)]
        public virtual byte[] FunctionSelector { get; set; }
    }

    public partial class WorldInsufficientbalanceError : WorldInsufficientbalanceErrorBase { }

    [Error("World_InsufficientBalance")]
    public class WorldInsufficientbalanceErrorBase : IErrorDTO
    {
        [Parameter("uint256", "balance", 1)]
        public virtual BigInteger Balance { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class WorldInterfacenotsupportedError : WorldInterfacenotsupportedErrorBase { }

    [Error("World_InterfaceNotSupported")]
    public class WorldInterfacenotsupportedErrorBase : IErrorDTO
    {
        [Parameter("address", "contractAddress", 1)]
        public virtual string ContractAddress { get; set; }
        [Parameter("bytes4", "interfaceId", 2)]
        public virtual byte[] InterfaceId { get; set; }
    }

    public partial class WorldInvalidresourceidError : WorldInvalidresourceidErrorBase { }

    [Error("World_InvalidResourceId")]
    public class WorldInvalidresourceidErrorBase : IErrorDTO
    {
        [Parameter("bytes32", "resourceId", 1)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("string", "resourceIdString", 2)]
        public virtual string ResourceIdString { get; set; }
    }

    public partial class WorldInvalidresourcetypeError : WorldInvalidresourcetypeErrorBase { }

    [Error("World_InvalidResourceType")]
    public class WorldInvalidresourcetypeErrorBase : IErrorDTO
    {
        [Parameter("bytes2", "expected", 1)]
        public virtual byte[] Expected { get; set; }
        [Parameter("bytes32", "resourceId", 2)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("string", "resourceIdString", 3)]
        public virtual string ResourceIdString { get; set; }
    }

    public partial class WorldResourcealreadyexistsError : WorldResourcealreadyexistsErrorBase { }

    [Error("World_ResourceAlreadyExists")]
    public class WorldResourcealreadyexistsErrorBase : IErrorDTO
    {
        [Parameter("bytes32", "resourceId", 1)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("string", "resourceIdString", 2)]
        public virtual string ResourceIdString { get; set; }
    }

    public partial class WorldResourcenotfoundError : WorldResourcenotfoundErrorBase { }

    [Error("World_ResourceNotFound")]
    public class WorldResourcenotfoundErrorBase : IErrorDTO
    {
        [Parameter("bytes32", "resourceId", 1)]
        public virtual byte[] ResourceId { get; set; }
        [Parameter("string", "resourceIdString", 2)]
        public virtual string ResourceIdString { get; set; }
    }

    public partial class WorldSystemalreadyexistsError : WorldSystemalreadyexistsErrorBase { }

    [Error("World_SystemAlreadyExists")]
    public class WorldSystemalreadyexistsErrorBase : IErrorDTO
    {
        [Parameter("address", "system", 1)]
        public virtual string System { get; set; }
    }

    public partial class WorldUnlimiteddelegationnotallowedError : WorldUnlimiteddelegationnotallowedErrorBase { }
    [Error("World_UnlimitedDelegationNotAllowed")]
    public class WorldUnlimiteddelegationnotallowedErrorBase : IErrorDTO
    {
    }













































    public partial class CreatorOutputDTO : CreatorOutputDTOBase { }

    [FunctionOutput]
    public class CreatorOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }



























    public partial class GetDynamicFieldOutputDTO : GetDynamicFieldOutputDTOBase { }

    [FunctionOutput]
    public class GetDynamicFieldOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class GetDynamicFieldLengthOutputDTO : GetDynamicFieldLengthOutputDTOBase { }

    [FunctionOutput]
    public class GetDynamicFieldLengthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetDynamicFieldSliceOutputDTO : GetDynamicFieldSliceOutputDTOBase { }

    [FunctionOutput]
    public class GetDynamicFieldSliceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "data", 1)]
        public virtual byte[] Data { get; set; }
    }

    public partial class GetField1OutputDTO : GetField1OutputDTOBase { }

    [FunctionOutput]
    public class GetField1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "data", 1)]
        public virtual byte[] Data { get; set; }
    }

    public partial class GetFieldOutputDTO : GetFieldOutputDTOBase { }

    [FunctionOutput]
    public class GetFieldOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "data", 1)]
        public virtual byte[] Data { get; set; }
    }

    public partial class GetFieldLayoutOutputDTO : GetFieldLayoutOutputDTOBase { }

    [FunctionOutput]
    public class GetFieldLayoutOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "fieldLayout", 1)]
        public virtual byte[] FieldLayout { get; set; }
    }

    public partial class GetFieldLength1OutputDTO : GetFieldLength1OutputDTOBase { }

    [FunctionOutput]
    public class GetFieldLength1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetFieldLengthOutputDTO : GetFieldLengthOutputDTOBase { }

    [FunctionOutput]
    public class GetFieldLengthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetKeySchemaOutputDTO : GetKeySchemaOutputDTOBase { }

    [FunctionOutput]
    public class GetKeySchemaOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "keySchema", 1)]
        public virtual byte[] KeySchema { get; set; }
    }

    public partial class GetRecord1OutputDTO : GetRecord1OutputDTOBase { }

    [FunctionOutput]
    public class GetRecord1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "staticData", 1)]
        public virtual byte[] StaticData { get; set; }
        [Parameter("bytes32", "encodedLengths", 2)]
        public virtual byte[] EncodedLengths { get; set; }
        [Parameter("bytes", "dynamicData", 3)]
        public virtual byte[] DynamicData { get; set; }
    }

    public partial class GetRecordOutputDTO : GetRecordOutputDTOBase { }

    [FunctionOutput]
    public class GetRecordOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "staticData", 1)]
        public virtual byte[] StaticData { get; set; }
        [Parameter("bytes32", "encodedLengths", 2)]
        public virtual byte[] EncodedLengths { get; set; }
        [Parameter("bytes", "dynamicData", 3)]
        public virtual byte[] DynamicData { get; set; }
    }

    public partial class GetStaticFieldOutputDTO : GetStaticFieldOutputDTOBase { }

    [FunctionOutput]
    public class GetStaticFieldOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class GetValueSchemaOutputDTO : GetValueSchemaOutputDTOBase { }

    [FunctionOutput]
    public class GetValueSchemaOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "valueSchema", 1)]
        public virtual byte[] ValueSchema { get; set; }
    }





























    public partial class IsAdminOutputDTO : IsAdminOutputDTOBase { }

    [FunctionOutput]
    public class IsAdminOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

























    public partial class NeumanNeighborhoodOutputDTO : NeumanNeighborhoodOutputDTOBase { }

    [FunctionOutput]
    public class NeumanNeighborhoodOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<PositionData> ReturnValue1 { get; set; }
    }

    public partial class NeumanNeighborhoodOuterOutputDTO : NeumanNeighborhoodOuterOutputDTOBase { }

    [FunctionOutput]
    public class NeumanNeighborhoodOuterOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<PositionData> ReturnValue1 { get; set; }
    }

























































































    public partial class StoreVersionOutputDTO : StoreVersionOutputDTOBase { }

    [FunctionOutput]
    public class StoreVersionOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "version", 1)]
        public virtual byte[] Version { get; set; }
    }











































    public partial class WorldVersionOutputDTO : WorldVersionOutputDTOBase { }

    [FunctionOutput]
    public class WorldVersionOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }
}

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using IWorld.ContractDefinition;

namespace IWorld.Service
{
    public partial class IWorldService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IWorldDeployment iWorldDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IWorldDeployment>().SendRequestAndWaitForReceiptAsync(iWorldDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IWorldDeployment iWorldDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IWorldDeployment>().SendRequestAsync(iWorldDeployment);
        }

        public static async Task<IWorldService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IWorldDeployment iWorldDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iWorldDeployment, cancellationTokenSource);
            return new IWorldService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IWorldService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public IWorldService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> ActionRequestAsync(ActionFunction actionFunction)
        {
             return ContractHandler.SendRequestAsync(actionFunction);
        }

        public Task<TransactionReceipt> ActionRequestAndWaitForReceiptAsync(ActionFunction actionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(actionFunction, cancellationToken);
        }

        public Task<string> ActionRequestAsync(byte newAction, int x, int y)
        {
            var actionFunction = new ActionFunction();
                actionFunction.NewAction = newAction;
                actionFunction.X = x;
                actionFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(actionFunction);
        }

        public Task<TransactionReceipt> ActionRequestAndWaitForReceiptAsync(byte newAction, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var actionFunction = new ActionFunction();
                actionFunction.NewAction = newAction;
                actionFunction.X = x;
                actionFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(actionFunction, cancellationToken);
        }

        public Task<string> AddCoinsAdminRequestAsync(AddCoinsAdminFunction addCoinsAdminFunction)
        {
             return ContractHandler.SendRequestAsync(addCoinsAdminFunction);
        }

        public Task<TransactionReceipt> AddCoinsAdminRequestAndWaitForReceiptAsync(AddCoinsAdminFunction addCoinsAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addCoinsAdminFunction, cancellationToken);
        }

        public Task<string> AddCoinsAdminRequestAsync(int amount)
        {
            var addCoinsAdminFunction = new AddCoinsAdminFunction();
                addCoinsAdminFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(addCoinsAdminFunction);
        }

        public Task<TransactionReceipt> AddCoinsAdminRequestAndWaitForReceiptAsync(int amount, CancellationTokenSource cancellationToken = null)
        {
            var addCoinsAdminFunction = new AddCoinsAdminFunction();
                addCoinsAdminFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addCoinsAdminFunction, cancellationToken);
        }

        public Task<string> AddGemXPRequestAsync(AddGemXPFunction addGemXPFunction)
        {
             return ContractHandler.SendRequestAsync(addGemXPFunction);
        }

        public Task<TransactionReceipt> AddGemXPRequestAndWaitForReceiptAsync(AddGemXPFunction addGemXPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addGemXPFunction, cancellationToken);
        }

        public Task<string> AddGemXPRequestAsync(uint amount)
        {
            var addGemXPFunction = new AddGemXPFunction();
                addGemXPFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(addGemXPFunction);
        }

        public Task<TransactionReceipt> AddGemXPRequestAndWaitForReceiptAsync(uint amount, CancellationTokenSource cancellationToken = null)
        {
            var addGemXPFunction = new AddGemXPFunction();
                addGemXPFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addGemXPFunction, cancellationToken);
        }

        public Task<string> AddXPAdminRequestAsync(AddXPAdminFunction addXPAdminFunction)
        {
             return ContractHandler.SendRequestAsync(addXPAdminFunction);
        }

        public Task<TransactionReceipt> AddXPAdminRequestAndWaitForReceiptAsync(AddXPAdminFunction addXPAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addXPAdminFunction, cancellationToken);
        }

        public Task<string> AddXPAdminRequestAsync(BigInteger amount)
        {
            var addXPAdminFunction = new AddXPAdminFunction();
                addXPAdminFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(addXPAdminFunction);
        }

        public Task<TransactionReceipt> AddXPAdminRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var addXPAdminFunction = new AddXPAdminFunction();
                addXPAdminFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addXPAdminFunction, cancellationToken);
        }

        public Task<string> AggroRequestAsync(AggroFunction aggroFunction)
        {
             return ContractHandler.SendRequestAsync(aggroFunction);
        }

        public Task<TransactionReceipt> AggroRequestAndWaitForReceiptAsync(AggroFunction aggroFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggroFunction, cancellationToken);
        }

        public Task<string> AggroRequestAsync(byte[] causedBy, byte[] target, byte[] attacker, PositionData targetPos, PositionData attackerPos)
        {
            var aggroFunction = new AggroFunction();
                aggroFunction.CausedBy = causedBy;
                aggroFunction.Target = target;
                aggroFunction.Attacker = attacker;
                aggroFunction.TargetPos = targetPos;
                aggroFunction.AttackerPos = attackerPos;
            
             return ContractHandler.SendRequestAsync(aggroFunction);
        }

        public Task<TransactionReceipt> AggroRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] target, byte[] attacker, PositionData targetPos, PositionData attackerPos, CancellationTokenSource cancellationToken = null)
        {
            var aggroFunction = new AggroFunction();
                aggroFunction.CausedBy = causedBy;
                aggroFunction.Target = target;
                aggroFunction.Attacker = attacker;
                aggroFunction.TargetPos = targetPos;
                aggroFunction.AttackerPos = attackerPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggroFunction, cancellationToken);
        }

        public Task<string> BuyCosmeticRequestAsync(BuyCosmeticFunction buyCosmeticFunction)
        {
             return ContractHandler.SendRequestAsync(buyCosmeticFunction);
        }

        public Task<TransactionReceipt> BuyCosmeticRequestAndWaitForReceiptAsync(BuyCosmeticFunction buyCosmeticFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCosmeticFunction, cancellationToken);
        }

        public Task<string> BuyCosmeticRequestAsync(uint id)
        {
            var buyCosmeticFunction = new BuyCosmeticFunction();
                buyCosmeticFunction.Id = id;
            
             return ContractHandler.SendRequestAsync(buyCosmeticFunction);
        }

        public Task<TransactionReceipt> BuyCosmeticRequestAndWaitForReceiptAsync(uint id, CancellationTokenSource cancellationToken = null)
        {
            var buyCosmeticFunction = new BuyCosmeticFunction();
                buyCosmeticFunction.Id = id;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCosmeticFunction, cancellationToken);
        }

        public Task<string> CallRequestAsync(CallFunction callFunction)
        {
             return ContractHandler.SendRequestAsync(callFunction);
        }

        public Task<TransactionReceipt> CallRequestAndWaitForReceiptAsync(CallFunction callFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFunction, cancellationToken);
        }

        public Task<string> CallRequestAsync(byte[] @namespace, byte[] name, byte[] funcSelectorAndArgs)
        {
            var callFunction = new CallFunction();
                callFunction.Namespace = @namespace;
                callFunction.Name = name;
                callFunction.FuncSelectorAndArgs = funcSelectorAndArgs;
            
             return ContractHandler.SendRequestAsync(callFunction);
        }

        public Task<TransactionReceipt> CallRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, byte[] funcSelectorAndArgs, CancellationTokenSource cancellationToken = null)
        {
            var callFunction = new CallFunction();
                callFunction.Namespace = @namespace;
                callFunction.Name = name;
                callFunction.FuncSelectorAndArgs = funcSelectorAndArgs;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFunction, cancellationToken);
        }

        public Task<string> CanDoStuffRequestAsync(CanDoStuffFunction canDoStuffFunction)
        {
             return ContractHandler.SendRequestAsync(canDoStuffFunction);
        }

        public Task<TransactionReceipt> CanDoStuffRequestAndWaitForReceiptAsync(CanDoStuffFunction canDoStuffFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canDoStuffFunction, cancellationToken);
        }

        public Task<string> CanDoStuffRequestAsync(byte[] player)
        {
            var canDoStuffFunction = new CanDoStuffFunction();
                canDoStuffFunction.Player = player;
            
             return ContractHandler.SendRequestAsync(canDoStuffFunction);
        }

        public Task<TransactionReceipt> CanDoStuffRequestAndWaitForReceiptAsync(byte[] player, CancellationTokenSource cancellationToken = null)
        {
            var canDoStuffFunction = new CanDoStuffFunction();
                canDoStuffFunction.Player = player;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canDoStuffFunction, cancellationToken);
        }

        public Task<string> CanInteractRequestAsync(CanInteractFunction canInteractFunction)
        {
             return ContractHandler.SendRequestAsync(canInteractFunction);
        }

        public Task<TransactionReceipt> CanInteractRequestAndWaitForReceiptAsync(CanInteractFunction canInteractFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canInteractFunction, cancellationToken);
        }

        public Task<string> CanInteractRequestAsync(byte[] player, int x, int y, List<byte[]> entities, BigInteger distance)
        {
            var canInteractFunction = new CanInteractFunction();
                canInteractFunction.Player = player;
                canInteractFunction.X = x;
                canInteractFunction.Y = y;
                canInteractFunction.Entities = entities;
                canInteractFunction.Distance = distance;
            
             return ContractHandler.SendRequestAsync(canInteractFunction);
        }

        public Task<TransactionReceipt> CanInteractRequestAndWaitForReceiptAsync(byte[] player, int x, int y, List<byte[]> entities, BigInteger distance, CancellationTokenSource cancellationToken = null)
        {
            var canInteractFunction = new CanInteractFunction();
                canInteractFunction.Player = player;
                canInteractFunction.X = x;
                canInteractFunction.Y = y;
                canInteractFunction.Entities = entities;
                canInteractFunction.Distance = distance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canInteractFunction, cancellationToken);
        }

        public Task<string> CanInteractEmptyRequestAsync(CanInteractEmptyFunction canInteractEmptyFunction)
        {
             return ContractHandler.SendRequestAsync(canInteractEmptyFunction);
        }

        public Task<TransactionReceipt> CanInteractEmptyRequestAndWaitForReceiptAsync(CanInteractEmptyFunction canInteractEmptyFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canInteractEmptyFunction, cancellationToken);
        }

        public Task<string> CanInteractEmptyRequestAsync(byte[] player, int x, int y, List<byte[]> entities, BigInteger distance)
        {
            var canInteractEmptyFunction = new CanInteractEmptyFunction();
                canInteractEmptyFunction.Player = player;
                canInteractEmptyFunction.X = x;
                canInteractEmptyFunction.Y = y;
                canInteractEmptyFunction.Entities = entities;
                canInteractEmptyFunction.Distance = distance;
            
             return ContractHandler.SendRequestAsync(canInteractEmptyFunction);
        }

        public Task<TransactionReceipt> CanInteractEmptyRequestAndWaitForReceiptAsync(byte[] player, int x, int y, List<byte[]> entities, BigInteger distance, CancellationTokenSource cancellationToken = null)
        {
            var canInteractEmptyFunction = new CanInteractEmptyFunction();
                canInteractEmptyFunction.Player = player;
                canInteractEmptyFunction.X = x;
                canInteractEmptyFunction.Y = y;
                canInteractEmptyFunction.Entities = entities;
                canInteractEmptyFunction.Distance = distance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canInteractEmptyFunction, cancellationToken);
        }

        public Task<bool> CanPlaceOnQueryAsync(CanPlaceOnFunction canPlaceOnFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CanPlaceOnFunction, bool>(canPlaceOnFunction, blockParameter);
        }

        
        public Task<bool> CanPlaceOnQueryAsync(List<byte[]> at, BlockParameter blockParameter = null)
        {
            var canPlaceOnFunction = new CanPlaceOnFunction();
                canPlaceOnFunction.At = at;
            
            return ContractHandler.QueryAsync<CanPlaceOnFunction, bool>(canPlaceOnFunction, blockParameter);
        }

        public Task<bool> CanWalkOnQueryAsync(CanWalkOnFunction canWalkOnFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CanWalkOnFunction, bool>(canWalkOnFunction, blockParameter);
        }

        
        public Task<bool> CanWalkOnQueryAsync(List<byte[]> at, BlockParameter blockParameter = null)
        {
            var canWalkOnFunction = new CanWalkOnFunction();
                canWalkOnFunction.At = at;
            
            return ContractHandler.QueryAsync<CanWalkOnFunction, bool>(canWalkOnFunction, blockParameter);
        }

        public Task<string> ChopRequestAsync(ChopFunction chopFunction)
        {
             return ContractHandler.SendRequestAsync(chopFunction);
        }

        public Task<TransactionReceipt> ChopRequestAndWaitForReceiptAsync(ChopFunction chopFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(chopFunction, cancellationToken);
        }

        public Task<string> ChopRequestAsync(byte[] player, int x, int y)
        {
            var chopFunction = new ChopFunction();
                chopFunction.Player = player;
                chopFunction.X = x;
                chopFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(chopFunction);
        }

        public Task<TransactionReceipt> ChopRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var chopFunction = new ChopFunction();
                chopFunction.Player = player;
                chopFunction.X = x;
                chopFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(chopFunction, cancellationToken);
        }

        public Task<string> ContemplateMileRequestAsync(ContemplateMileFunction contemplateMileFunction)
        {
             return ContractHandler.SendRequestAsync(contemplateMileFunction);
        }

        public Task<TransactionReceipt> ContemplateMileRequestAndWaitForReceiptAsync(ContemplateMileFunction contemplateMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(contemplateMileFunction, cancellationToken);
        }

        public Task<string> ContemplateMileRequestAsync(int mileNumber)
        {
            var contemplateMileFunction = new ContemplateMileFunction();
                contemplateMileFunction.MileNumber = mileNumber;
            
             return ContractHandler.SendRequestAsync(contemplateMileFunction);
        }

        public Task<TransactionReceipt> ContemplateMileRequestAndWaitForReceiptAsync(int mileNumber, CancellationTokenSource cancellationToken = null)
        {
            var contemplateMileFunction = new ContemplateMileFunction();
                contemplateMileFunction.MileNumber = mileNumber;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(contemplateMileFunction, cancellationToken);
        }

        public Task<string> CreateEntitiesRequestAsync(CreateEntitiesFunction createEntitiesFunction)
        {
             return ContractHandler.SendRequestAsync(createEntitiesFunction);
        }

        public Task<TransactionReceipt> CreateEntitiesRequestAndWaitForReceiptAsync(CreateEntitiesFunction createEntitiesFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createEntitiesFunction, cancellationToken);
        }

        public Task<string> CreateEntitiesRequestAsync(byte[] chunkEntity, int playWidth, uint playHeight)
        {
            var createEntitiesFunction = new CreateEntitiesFunction();
                createEntitiesFunction.ChunkEntity = chunkEntity;
                createEntitiesFunction.PlayWidth = playWidth;
                createEntitiesFunction.PlayHeight = playHeight;
            
             return ContractHandler.SendRequestAsync(createEntitiesFunction);
        }

        public Task<TransactionReceipt> CreateEntitiesRequestAndWaitForReceiptAsync(byte[] chunkEntity, int playWidth, uint playHeight, CancellationTokenSource cancellationToken = null)
        {
            var createEntitiesFunction = new CreateEntitiesFunction();
                createEntitiesFunction.ChunkEntity = chunkEntity;
                createEntitiesFunction.PlayWidth = playWidth;
                createEntitiesFunction.PlayHeight = playHeight;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createEntitiesFunction, cancellationToken);
        }

        public Task<string> CreateMileRequestAsync(CreateMileFunction createMileFunction)
        {
             return ContractHandler.SendRequestAsync(createMileFunction);
        }

        public Task<string> CreateMileRequestAsync()
        {
             return ContractHandler.SendRequestAsync<CreateMileFunction>();
        }

        public Task<TransactionReceipt> CreateMileRequestAndWaitForReceiptAsync(CreateMileFunction createMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMileFunction, cancellationToken);
        }

        public Task<TransactionReceipt> CreateMileRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<CreateMileFunction>(null, cancellationToken);
        }

        public Task<string> CreateMiliariumRequestAsync(CreateMiliariumFunction createMiliariumFunction)
        {
             return ContractHandler.SendRequestAsync(createMiliariumFunction);
        }

        public Task<TransactionReceipt> CreateMiliariumRequestAndWaitForReceiptAsync(CreateMiliariumFunction createMiliariumFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMiliariumFunction, cancellationToken);
        }

        public Task<string> CreateMiliariumRequestAsync(byte[] causedBy, int width, int up, int down, int roadSide)
        {
            var createMiliariumFunction = new CreateMiliariumFunction();
                createMiliariumFunction.CausedBy = causedBy;
                createMiliariumFunction.Width = width;
                createMiliariumFunction.Up = up;
                createMiliariumFunction.Down = down;
                createMiliariumFunction.RoadSide = roadSide;
            
             return ContractHandler.SendRequestAsync(createMiliariumFunction);
        }

        public Task<TransactionReceipt> CreateMiliariumRequestAndWaitForReceiptAsync(byte[] causedBy, int width, int up, int down, int roadSide, CancellationTokenSource cancellationToken = null)
        {
            var createMiliariumFunction = new CreateMiliariumFunction();
                createMiliariumFunction.CausedBy = causedBy;
                createMiliariumFunction.Width = width;
                createMiliariumFunction.Up = up;
                createMiliariumFunction.Down = down;
                createMiliariumFunction.RoadSide = roadSide;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMiliariumFunction, cancellationToken);
        }

        public Task<string> CreatePuzzleOnMileRequestAsync(CreatePuzzleOnMileFunction createPuzzleOnMileFunction)
        {
             return ContractHandler.SendRequestAsync(createPuzzleOnMileFunction);
        }

        public Task<TransactionReceipt> CreatePuzzleOnMileRequestAndWaitForReceiptAsync(CreatePuzzleOnMileFunction createPuzzleOnMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createPuzzleOnMileFunction, cancellationToken);
        }

        public Task<string> CreatePuzzleOnMileRequestAsync(byte[] causedBy)
        {
            var createPuzzleOnMileFunction = new CreatePuzzleOnMileFunction();
                createPuzzleOnMileFunction.CausedBy = causedBy;
            
             return ContractHandler.SendRequestAsync(createPuzzleOnMileFunction);
        }

        public Task<TransactionReceipt> CreatePuzzleOnMileRequestAndWaitForReceiptAsync(byte[] causedBy, CancellationTokenSource cancellationToken = null)
        {
            var createPuzzleOnMileFunction = new CreatePuzzleOnMileFunction();
                createPuzzleOnMileFunction.CausedBy = causedBy;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createPuzzleOnMileFunction, cancellationToken);
        }

        public Task<string> CreateRandomPuzzleRequestAsync(CreateRandomPuzzleFunction createRandomPuzzleFunction)
        {
             return ContractHandler.SendRequestAsync(createRandomPuzzleFunction);
        }

        public Task<TransactionReceipt> CreateRandomPuzzleRequestAndWaitForReceiptAsync(CreateRandomPuzzleFunction createRandomPuzzleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createRandomPuzzleFunction, cancellationToken);
        }

        public Task<string> CreateRandomPuzzleRequestAsync(byte[] causedBy, int playWidth, int up, int down)
        {
            var createRandomPuzzleFunction = new CreateRandomPuzzleFunction();
                createRandomPuzzleFunction.CausedBy = causedBy;
                createRandomPuzzleFunction.PlayWidth = playWidth;
                createRandomPuzzleFunction.Up = up;
                createRandomPuzzleFunction.Down = down;
            
             return ContractHandler.SendRequestAsync(createRandomPuzzleFunction);
        }

        public Task<TransactionReceipt> CreateRandomPuzzleRequestAndWaitForReceiptAsync(byte[] causedBy, int playWidth, int up, int down, CancellationTokenSource cancellationToken = null)
        {
            var createRandomPuzzleFunction = new CreateRandomPuzzleFunction();
                createRandomPuzzleFunction.CausedBy = causedBy;
                createRandomPuzzleFunction.PlayWidth = playWidth;
                createRandomPuzzleFunction.Up = up;
                createRandomPuzzleFunction.Down = down;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createRandomPuzzleFunction, cancellationToken);
        }

        public Task<string> CreateTerrainRequestAsync(CreateTerrainFunction createTerrainFunction)
        {
             return ContractHandler.SendRequestAsync(createTerrainFunction);
        }

        public Task<TransactionReceipt> CreateTerrainRequestAndWaitForReceiptAsync(CreateTerrainFunction createTerrainFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTerrainFunction, cancellationToken);
        }

        public Task<string> CreateTerrainRequestAsync(byte[] causedBy, int width, int up, int down)
        {
            var createTerrainFunction = new CreateTerrainFunction();
                createTerrainFunction.CausedBy = causedBy;
                createTerrainFunction.Width = width;
                createTerrainFunction.Up = up;
                createTerrainFunction.Down = down;
            
             return ContractHandler.SendRequestAsync(createTerrainFunction);
        }

        public Task<TransactionReceipt> CreateTerrainRequestAndWaitForReceiptAsync(byte[] causedBy, int width, int up, int down, CancellationTokenSource cancellationToken = null)
        {
            var createTerrainFunction = new CreateTerrainFunction();
                createTerrainFunction.CausedBy = causedBy;
                createTerrainFunction.Width = width;
                createTerrainFunction.Up = up;
                createTerrainFunction.Down = down;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTerrainFunction, cancellationToken);
        }

        public Task<string> CreateWorldRequestAsync(CreateWorldFunction createWorldFunction)
        {
             return ContractHandler.SendRequestAsync(createWorldFunction);
        }

        public Task<TransactionReceipt> CreateWorldRequestAndWaitForReceiptAsync(CreateWorldFunction createWorldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createWorldFunction, cancellationToken);
        }

        public Task<string> CreateWorldRequestAsync(string worldAddress)
        {
            var createWorldFunction = new CreateWorldFunction();
                createWorldFunction.WorldAddress = worldAddress;
            
             return ContractHandler.SendRequestAsync(createWorldFunction);
        }

        public Task<TransactionReceipt> CreateWorldRequestAndWaitForReceiptAsync(string worldAddress, CancellationTokenSource cancellationToken = null)
        {
            var createWorldFunction = new CreateWorldFunction();
                createWorldFunction.WorldAddress = worldAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createWorldFunction, cancellationToken);
        }

        public Task<string> DebugMileRequestAsync(DebugMileFunction debugMileFunction)
        {
             return ContractHandler.SendRequestAsync(debugMileFunction);
        }

        public Task<TransactionReceipt> DebugMileRequestAndWaitForReceiptAsync(DebugMileFunction debugMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(debugMileFunction, cancellationToken);
        }

        public Task<string> DebugMileRequestAsync(byte[] credit)
        {
            var debugMileFunction = new DebugMileFunction();
                debugMileFunction.Credit = credit;
            
             return ContractHandler.SendRequestAsync(debugMileFunction);
        }

        public Task<TransactionReceipt> DebugMileRequestAndWaitForReceiptAsync(byte[] credit, CancellationTokenSource cancellationToken = null)
        {
            var debugMileFunction = new DebugMileFunction();
                debugMileFunction.Credit = credit;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(debugMileFunction, cancellationToken);
        }

        public Task<string> DeleteAdminRequestAsync(DeleteAdminFunction deleteAdminFunction)
        {
             return ContractHandler.SendRequestAsync(deleteAdminFunction);
        }

        public Task<TransactionReceipt> DeleteAdminRequestAndWaitForReceiptAsync(DeleteAdminFunction deleteAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteAdminFunction, cancellationToken);
        }

        public Task<string> DeleteAdminRequestAsync(int x, int y, int layer)
        {
            var deleteAdminFunction = new DeleteAdminFunction();
                deleteAdminFunction.X = x;
                deleteAdminFunction.Y = y;
                deleteAdminFunction.Layer = layer;
            
             return ContractHandler.SendRequestAsync(deleteAdminFunction);
        }

        public Task<TransactionReceipt> DeleteAdminRequestAndWaitForReceiptAsync(int x, int y, int layer, CancellationTokenSource cancellationToken = null)
        {
            var deleteAdminFunction = new DeleteAdminFunction();
                deleteAdminFunction.X = x;
                deleteAdminFunction.Y = y;
                deleteAdminFunction.Layer = layer;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteAdminFunction, cancellationToken);
        }

        public Task<string> DeleteAtRequestAsync(DeleteAtFunction deleteAtFunction)
        {
             return ContractHandler.SendRequestAsync(deleteAtFunction);
        }

        public Task<TransactionReceipt> DeleteAtRequestAndWaitForReceiptAsync(DeleteAtFunction deleteAtFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteAtFunction, cancellationToken);
        }

        public Task<string> DeleteAtRequestAsync(int x, int y, int layer)
        {
            var deleteAtFunction = new DeleteAtFunction();
                deleteAtFunction.X = x;
                deleteAtFunction.Y = y;
                deleteAtFunction.Layer = layer;
            
             return ContractHandler.SendRequestAsync(deleteAtFunction);
        }

        public Task<TransactionReceipt> DeleteAtRequestAndWaitForReceiptAsync(int x, int y, int layer, CancellationTokenSource cancellationToken = null)
        {
            var deleteAtFunction = new DeleteAtFunction();
                deleteAtFunction.X = x;
                deleteAtFunction.Y = y;
                deleteAtFunction.Layer = layer;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteAtFunction, cancellationToken);
        }

        public Task<string> DeleteRecordRequestAsync(DeleteRecordFunction deleteRecordFunction)
        {
             return ContractHandler.SendRequestAsync(deleteRecordFunction);
        }

        public Task<TransactionReceipt> DeleteRecordRequestAndWaitForReceiptAsync(DeleteRecordFunction deleteRecordFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteRecordFunction, cancellationToken);
        }

        public Task<string> DeleteRecordRequestAsync(byte[] table, List<byte[]> key)
        {
            var deleteRecordFunction = new DeleteRecordFunction();
                deleteRecordFunction.Table = table;
                deleteRecordFunction.Key = key;
            
             return ContractHandler.SendRequestAsync(deleteRecordFunction);
        }

        public Task<TransactionReceipt> DeleteRecordRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, CancellationTokenSource cancellationToken = null)
        {
            var deleteRecordFunction = new DeleteRecordFunction();
                deleteRecordFunction.Table = table;
                deleteRecordFunction.Key = key;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteRecordFunction, cancellationToken);
        }

        public Task<string> DeleteRecordRequestAsync(DeleteRecord1Function deleteRecord1Function)
        {
             return ContractHandler.SendRequestAsync(deleteRecord1Function);
        }

        public Task<TransactionReceipt> DeleteRecordRequestAndWaitForReceiptAsync(DeleteRecord1Function deleteRecord1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteRecord1Function, cancellationToken);
        }

        public Task<string> DeleteRecordRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key)
        {
            var deleteRecord1Function = new DeleteRecord1Function();
                deleteRecord1Function.Namespace = @namespace;
                deleteRecord1Function.Name = name;
                deleteRecord1Function.Key = key;
            
             return ContractHandler.SendRequestAsync(deleteRecord1Function);
        }

        public Task<TransactionReceipt> DeleteRecordRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, CancellationTokenSource cancellationToken = null)
        {
            var deleteRecord1Function = new DeleteRecord1Function();
                deleteRecord1Function.Namespace = @namespace;
                deleteRecord1Function.Name = name;
                deleteRecord1Function.Key = key;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteRecord1Function, cancellationToken);
        }

        public Task<string> DestroyPlayerAdminRequestAsync(DestroyPlayerAdminFunction destroyPlayerAdminFunction)
        {
             return ContractHandler.SendRequestAsync(destroyPlayerAdminFunction);
        }

        public Task<string> DestroyPlayerAdminRequestAsync()
        {
             return ContractHandler.SendRequestAsync<DestroyPlayerAdminFunction>();
        }

        public Task<TransactionReceipt> DestroyPlayerAdminRequestAndWaitForReceiptAsync(DestroyPlayerAdminFunction destroyPlayerAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(destroyPlayerAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> DestroyPlayerAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<DestroyPlayerAdminFunction>(null, cancellationToken);
        }

        public Task<string> EmitEphemeralRecordRequestAsync(EmitEphemeralRecord1Function emitEphemeralRecord1Function)
        {
             return ContractHandler.SendRequestAsync(emitEphemeralRecord1Function);
        }

        public Task<TransactionReceipt> EmitEphemeralRecordRequestAndWaitForReceiptAsync(EmitEphemeralRecord1Function emitEphemeralRecord1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(emitEphemeralRecord1Function, cancellationToken);
        }

        public Task<string> EmitEphemeralRecordRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte[] data)
        {
            var emitEphemeralRecord1Function = new EmitEphemeralRecord1Function();
                emitEphemeralRecord1Function.Namespace = @namespace;
                emitEphemeralRecord1Function.Name = name;
                emitEphemeralRecord1Function.Key = key;
                emitEphemeralRecord1Function.Data = data;
            
             return ContractHandler.SendRequestAsync(emitEphemeralRecord1Function);
        }

        public Task<TransactionReceipt> EmitEphemeralRecordRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var emitEphemeralRecord1Function = new EmitEphemeralRecord1Function();
                emitEphemeralRecord1Function.Namespace = @namespace;
                emitEphemeralRecord1Function.Name = name;
                emitEphemeralRecord1Function.Key = key;
                emitEphemeralRecord1Function.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(emitEphemeralRecord1Function, cancellationToken);
        }

        public Task<string> EmitEphemeralRecordRequestAsync(EmitEphemeralRecordFunction emitEphemeralRecordFunction)
        {
             return ContractHandler.SendRequestAsync(emitEphemeralRecordFunction);
        }

        public Task<TransactionReceipt> EmitEphemeralRecordRequestAndWaitForReceiptAsync(EmitEphemeralRecordFunction emitEphemeralRecordFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(emitEphemeralRecordFunction, cancellationToken);
        }

        public Task<string> EmitEphemeralRecordRequestAsync(byte[] table, List<byte[]> key, byte[] data)
        {
            var emitEphemeralRecordFunction = new EmitEphemeralRecordFunction();
                emitEphemeralRecordFunction.Table = table;
                emitEphemeralRecordFunction.Key = key;
                emitEphemeralRecordFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(emitEphemeralRecordFunction);
        }

        public Task<TransactionReceipt> EmitEphemeralRecordRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var emitEphemeralRecordFunction = new EmitEphemeralRecordFunction();
                emitEphemeralRecordFunction.Table = table;
                emitEphemeralRecordFunction.Key = key;
                emitEphemeralRecordFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(emitEphemeralRecordFunction, cancellationToken);
        }

        public Task<string> FindEmptyPositionInAreaRequestAsync(FindEmptyPositionInAreaFunction findEmptyPositionInAreaFunction)
        {
             return ContractHandler.SendRequestAsync(findEmptyPositionInAreaFunction);
        }

        public Task<TransactionReceipt> FindEmptyPositionInAreaRequestAndWaitForReceiptAsync(FindEmptyPositionInAreaFunction findEmptyPositionInAreaFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(findEmptyPositionInAreaFunction, cancellationToken);
        }

        public Task<string> FindEmptyPositionInAreaRequestAsync(byte[] entity, int width, int up, int down, int roadSide)
        {
            var findEmptyPositionInAreaFunction = new FindEmptyPositionInAreaFunction();
                findEmptyPositionInAreaFunction.Entity = entity;
                findEmptyPositionInAreaFunction.Width = width;
                findEmptyPositionInAreaFunction.Up = up;
                findEmptyPositionInAreaFunction.Down = down;
                findEmptyPositionInAreaFunction.RoadSide = roadSide;
            
             return ContractHandler.SendRequestAsync(findEmptyPositionInAreaFunction);
        }

        public Task<TransactionReceipt> FindEmptyPositionInAreaRequestAndWaitForReceiptAsync(byte[] entity, int width, int up, int down, int roadSide, CancellationTokenSource cancellationToken = null)
        {
            var findEmptyPositionInAreaFunction = new FindEmptyPositionInAreaFunction();
                findEmptyPositionInAreaFunction.Entity = entity;
                findEmptyPositionInAreaFunction.Width = width;
                findEmptyPositionInAreaFunction.Up = up;
                findEmptyPositionInAreaFunction.Down = down;
                findEmptyPositionInAreaFunction.RoadSide = roadSide;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(findEmptyPositionInAreaFunction, cancellationToken);
        }

        public Task<string> FinishMileRequestAsync(FinishMileFunction finishMileFunction)
        {
             return ContractHandler.SendRequestAsync(finishMileFunction);
        }

        public Task<TransactionReceipt> FinishMileRequestAndWaitForReceiptAsync(FinishMileFunction finishMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finishMileFunction, cancellationToken);
        }

        public Task<string> FinishMileRequestAsync(byte[] chunk, int currentMile, uint pieces)
        {
            var finishMileFunction = new FinishMileFunction();
                finishMileFunction.Chunk = chunk;
                finishMileFunction.CurrentMile = currentMile;
                finishMileFunction.Pieces = pieces;
            
             return ContractHandler.SendRequestAsync(finishMileFunction);
        }

        public Task<TransactionReceipt> FinishMileRequestAndWaitForReceiptAsync(byte[] chunk, int currentMile, uint pieces, CancellationTokenSource cancellationToken = null)
        {
            var finishMileFunction = new FinishMileFunction();
                finishMileFunction.Chunk = chunk;
                finishMileFunction.CurrentMile = currentMile;
                finishMileFunction.Pieces = pieces;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finishMileFunction, cancellationToken);
        }

        public Task<string> FinishMileAdminRequestAsync(FinishMileAdminFunction finishMileAdminFunction)
        {
             return ContractHandler.SendRequestAsync(finishMileAdminFunction);
        }

        public Task<string> FinishMileAdminRequestAsync()
        {
             return ContractHandler.SendRequestAsync<FinishMileAdminFunction>();
        }

        public Task<TransactionReceipt> FinishMileAdminRequestAndWaitForReceiptAsync(FinishMileAdminFunction finishMileAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finishMileAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> FinishMileAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<FinishMileAdminFunction>(null, cancellationToken);
        }

        public Task<string> FishRequestAsync(FishFunction fishFunction)
        {
             return ContractHandler.SendRequestAsync(fishFunction);
        }

        public Task<TransactionReceipt> FishRequestAndWaitForReceiptAsync(FishFunction fishFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fishFunction, cancellationToken);
        }

        public Task<string> FishRequestAsync(byte[] player, int x, int y)
        {
            var fishFunction = new FishFunction();
                fishFunction.Player = player;
                fishFunction.X = x;
                fishFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(fishFunction);
        }

        public Task<TransactionReceipt> FishRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var fishFunction = new FishFunction();
                fishFunction.Player = player;
                fishFunction.X = x;
                fishFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fishFunction, cancellationToken);
        }

        public Task<string> GetCarriageEntityRequestAsync(GetCarriageEntityFunction getCarriageEntityFunction)
        {
             return ContractHandler.SendRequestAsync(getCarriageEntityFunction);
        }

        public Task<string> GetCarriageEntityRequestAsync()
        {
             return ContractHandler.SendRequestAsync<GetCarriageEntityFunction>();
        }

        public Task<TransactionReceipt> GetCarriageEntityRequestAndWaitForReceiptAsync(GetCarriageEntityFunction getCarriageEntityFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getCarriageEntityFunction, cancellationToken);
        }

        public Task<TransactionReceipt> GetCarriageEntityRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<GetCarriageEntityFunction>(null, cancellationToken);
        }

        public Task<string> GetChunkEntityRequestAsync(GetChunkEntityFunction getChunkEntityFunction)
        {
             return ContractHandler.SendRequestAsync(getChunkEntityFunction);
        }

        public Task<TransactionReceipt> GetChunkEntityRequestAndWaitForReceiptAsync(GetChunkEntityFunction getChunkEntityFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getChunkEntityFunction, cancellationToken);
        }

        public Task<string> GetChunkEntityRequestAsync(int mile)
        {
            var getChunkEntityFunction = new GetChunkEntityFunction();
                getChunkEntityFunction.Mile = mile;
            
             return ContractHandler.SendRequestAsync(getChunkEntityFunction);
        }

        public Task<TransactionReceipt> GetChunkEntityRequestAndWaitForReceiptAsync(int mile, CancellationTokenSource cancellationToken = null)
        {
            var getChunkEntityFunction = new GetChunkEntityFunction();
                getChunkEntityFunction.Mile = mile;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getChunkEntityFunction, cancellationToken);
        }

        public Task<byte[]> GetFieldQueryAsync(GetFieldFunction getFieldFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldFunction, byte[]>(getFieldFunction, blockParameter);
        }

        
        public Task<byte[]> GetFieldQueryAsync(byte[] table, List<byte[]> key, byte schemaIndex, BlockParameter blockParameter = null)
        {
            var getFieldFunction = new GetFieldFunction();
                getFieldFunction.Table = table;
                getFieldFunction.Key = key;
                getFieldFunction.SchemaIndex = schemaIndex;
            
            return ContractHandler.QueryAsync<GetFieldFunction, byte[]>(getFieldFunction, blockParameter);
        }

        public Task<BigInteger> GetFieldLengthQueryAsync(GetFieldLengthFunction getFieldLengthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldLengthFunction, BigInteger>(getFieldLengthFunction, blockParameter);
        }

        
        public Task<BigInteger> GetFieldLengthQueryAsync(byte[] table, List<byte[]> key, byte schemaIndex, byte[] schema, BlockParameter blockParameter = null)
        {
            var getFieldLengthFunction = new GetFieldLengthFunction();
                getFieldLengthFunction.Table = table;
                getFieldLengthFunction.Key = key;
                getFieldLengthFunction.SchemaIndex = schemaIndex;
                getFieldLengthFunction.Schema = schema;
            
            return ContractHandler.QueryAsync<GetFieldLengthFunction, BigInteger>(getFieldLengthFunction, blockParameter);
        }

        public Task<byte[]> GetFieldSliceQueryAsync(GetFieldSliceFunction getFieldSliceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldSliceFunction, byte[]>(getFieldSliceFunction, blockParameter);
        }

        
        public Task<byte[]> GetFieldSliceQueryAsync(byte[] table, List<byte[]> key, byte schemaIndex, byte[] schema, BigInteger start, BigInteger end, BlockParameter blockParameter = null)
        {
            var getFieldSliceFunction = new GetFieldSliceFunction();
                getFieldSliceFunction.Table = table;
                getFieldSliceFunction.Key = key;
                getFieldSliceFunction.SchemaIndex = schemaIndex;
                getFieldSliceFunction.Schema = schema;
                getFieldSliceFunction.Start = start;
                getFieldSliceFunction.End = end;
            
            return ContractHandler.QueryAsync<GetFieldSliceFunction, byte[]>(getFieldSliceFunction, blockParameter);
        }

        public Task<byte[]> GetKeySchemaQueryAsync(GetKeySchemaFunction getKeySchemaFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeySchemaFunction, byte[]>(getKeySchemaFunction, blockParameter);
        }

        
        public Task<byte[]> GetKeySchemaQueryAsync(byte[] table, BlockParameter blockParameter = null)
        {
            var getKeySchemaFunction = new GetKeySchemaFunction();
                getKeySchemaFunction.Table = table;
            
            return ContractHandler.QueryAsync<GetKeySchemaFunction, byte[]>(getKeySchemaFunction, blockParameter);
        }

        public Task<GetRandomPositionNotRoadOutputDTO> GetRandomPositionNotRoadQueryAsync(GetRandomPositionNotRoadFunction getRandomPositionNotRoadFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetRandomPositionNotRoadFunction, GetRandomPositionNotRoadOutputDTO>(getRandomPositionNotRoadFunction, blockParameter);
        }

        public Task<GetRandomPositionNotRoadOutputDTO> GetRandomPositionNotRoadQueryAsync(byte[] causedBy, int width, int up, int down, int roadSide, BigInteger seed, BlockParameter blockParameter = null)
        {
            var getRandomPositionNotRoadFunction = new GetRandomPositionNotRoadFunction();
                getRandomPositionNotRoadFunction.CausedBy = causedBy;
                getRandomPositionNotRoadFunction.Width = width;
                getRandomPositionNotRoadFunction.Up = up;
                getRandomPositionNotRoadFunction.Down = down;
                getRandomPositionNotRoadFunction.RoadSide = roadSide;
                getRandomPositionNotRoadFunction.Seed = seed;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetRandomPositionNotRoadFunction, GetRandomPositionNotRoadOutputDTO>(getRandomPositionNotRoadFunction, blockParameter);
        }

        public Task<byte[]> GetRecordQueryAsync(GetRecord1Function getRecord1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRecord1Function, byte[]>(getRecord1Function, blockParameter);
        }

        
        public Task<byte[]> GetRecordQueryAsync(byte[] table, List<byte[]> key, byte[] schema, BlockParameter blockParameter = null)
        {
            var getRecord1Function = new GetRecord1Function();
                getRecord1Function.Table = table;
                getRecord1Function.Key = key;
                getRecord1Function.Schema = schema;
            
            return ContractHandler.QueryAsync<GetRecord1Function, byte[]>(getRecord1Function, blockParameter);
        }

        public Task<byte[]> GetRecordQueryAsync(GetRecordFunction getRecordFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRecordFunction, byte[]>(getRecordFunction, blockParameter);
        }

        
        public Task<byte[]> GetRecordQueryAsync(byte[] table, List<byte[]> key, BlockParameter blockParameter = null)
        {
            var getRecordFunction = new GetRecordFunction();
                getRecordFunction.Table = table;
                getRecordFunction.Key = key;
            
            return ContractHandler.QueryAsync<GetRecordFunction, byte[]>(getRecordFunction, blockParameter);
        }

        public Task<string> GetRoadEntityRequestAsync(GetRoadEntityFunction getRoadEntityFunction)
        {
             return ContractHandler.SendRequestAsync(getRoadEntityFunction);
        }

        public Task<TransactionReceipt> GetRoadEntityRequestAndWaitForReceiptAsync(GetRoadEntityFunction getRoadEntityFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getRoadEntityFunction, cancellationToken);
        }

        public Task<string> GetRoadEntityRequestAsync(int x, int y)
        {
            var getRoadEntityFunction = new GetRoadEntityFunction();
                getRoadEntityFunction.X = x;
                getRoadEntityFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(getRoadEntityFunction);
        }

        public Task<TransactionReceipt> GetRoadEntityRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var getRoadEntityFunction = new GetRoadEntityFunction();
                getRoadEntityFunction.X = x;
                getRoadEntityFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getRoadEntityFunction, cancellationToken);
        }

        public Task<byte[]> GetSchemaQueryAsync(GetSchemaFunction getSchemaFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetSchemaFunction, byte[]>(getSchemaFunction, blockParameter);
        }

        
        public Task<byte[]> GetSchemaQueryAsync(byte[] table, BlockParameter blockParameter = null)
        {
            var getSchemaFunction = new GetSchemaFunction();
                getSchemaFunction.Table = table;
            
            return ContractHandler.QueryAsync<GetSchemaFunction, byte[]>(getSchemaFunction, blockParameter);
        }

        public Task<string> GiveCoinsRequestAsync(GiveCoinsFunction giveCoinsFunction)
        {
             return ContractHandler.SendRequestAsync(giveCoinsFunction);
        }

        public Task<TransactionReceipt> GiveCoinsRequestAndWaitForReceiptAsync(GiveCoinsFunction giveCoinsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveCoinsFunction, cancellationToken);
        }

        public Task<string> GiveCoinsRequestAsync(byte[] player, int amount)
        {
            var giveCoinsFunction = new GiveCoinsFunction();
                giveCoinsFunction.Player = player;
                giveCoinsFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(giveCoinsFunction);
        }

        public Task<TransactionReceipt> GiveCoinsRequestAndWaitForReceiptAsync(byte[] player, int amount, CancellationTokenSource cancellationToken = null)
        {
            var giveCoinsFunction = new GiveCoinsFunction();
                giveCoinsFunction.Player = player;
                giveCoinsFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveCoinsFunction, cancellationToken);
        }

        public Task<string> GiveGemRequestAsync(GiveGemFunction giveGemFunction)
        {
             return ContractHandler.SendRequestAsync(giveGemFunction);
        }

        public Task<TransactionReceipt> GiveGemRequestAndWaitForReceiptAsync(GiveGemFunction giveGemFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveGemFunction, cancellationToken);
        }

        public Task<string> GiveGemRequestAsync(byte[] player, uint amount)
        {
            var giveGemFunction = new GiveGemFunction();
                giveGemFunction.Player = player;
                giveGemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(giveGemFunction);
        }

        public Task<TransactionReceipt> GiveGemRequestAndWaitForReceiptAsync(byte[] player, uint amount, CancellationTokenSource cancellationToken = null)
        {
            var giveGemFunction = new GiveGemFunction();
                giveGemFunction.Player = player;
                giveGemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveGemFunction, cancellationToken);
        }

        public Task<string> GiveKilledBarbarianRewardRequestAsync(GiveKilledBarbarianRewardFunction giveKilledBarbarianRewardFunction)
        {
             return ContractHandler.SendRequestAsync(giveKilledBarbarianRewardFunction);
        }

        public Task<TransactionReceipt> GiveKilledBarbarianRewardRequestAndWaitForReceiptAsync(GiveKilledBarbarianRewardFunction giveKilledBarbarianRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveKilledBarbarianRewardFunction, cancellationToken);
        }

        public Task<string> GiveKilledBarbarianRewardRequestAsync(byte[] player)
        {
            var giveKilledBarbarianRewardFunction = new GiveKilledBarbarianRewardFunction();
                giveKilledBarbarianRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAsync(giveKilledBarbarianRewardFunction);
        }

        public Task<TransactionReceipt> GiveKilledBarbarianRewardRequestAndWaitForReceiptAsync(byte[] player, CancellationTokenSource cancellationToken = null)
        {
            var giveKilledBarbarianRewardFunction = new GiveKilledBarbarianRewardFunction();
                giveKilledBarbarianRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveKilledBarbarianRewardFunction, cancellationToken);
        }

        public Task<string> GivePuzzleRewardRequestAsync(GivePuzzleRewardFunction givePuzzleRewardFunction)
        {
             return ContractHandler.SendRequestAsync(givePuzzleRewardFunction);
        }

        public Task<TransactionReceipt> GivePuzzleRewardRequestAndWaitForReceiptAsync(GivePuzzleRewardFunction givePuzzleRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(givePuzzleRewardFunction, cancellationToken);
        }

        public Task<string> GivePuzzleRewardRequestAsync(byte[] player)
        {
            var givePuzzleRewardFunction = new GivePuzzleRewardFunction();
                givePuzzleRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAsync(givePuzzleRewardFunction);
        }

        public Task<TransactionReceipt> GivePuzzleRewardRequestAndWaitForReceiptAsync(byte[] player, CancellationTokenSource cancellationToken = null)
        {
            var givePuzzleRewardFunction = new GivePuzzleRewardFunction();
                givePuzzleRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(givePuzzleRewardFunction, cancellationToken);
        }

        public Task<string> GiveRoadFilledRewardRequestAsync(GiveRoadFilledRewardFunction giveRoadFilledRewardFunction)
        {
             return ContractHandler.SendRequestAsync(giveRoadFilledRewardFunction);
        }

        public Task<TransactionReceipt> GiveRoadFilledRewardRequestAndWaitForReceiptAsync(GiveRoadFilledRewardFunction giveRoadFilledRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadFilledRewardFunction, cancellationToken);
        }

        public Task<string> GiveRoadFilledRewardRequestAsync(byte[] player)
        {
            var giveRoadFilledRewardFunction = new GiveRoadFilledRewardFunction();
                giveRoadFilledRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAsync(giveRoadFilledRewardFunction);
        }

        public Task<TransactionReceipt> GiveRoadFilledRewardRequestAndWaitForReceiptAsync(byte[] player, CancellationTokenSource cancellationToken = null)
        {
            var giveRoadFilledRewardFunction = new GiveRoadFilledRewardFunction();
                giveRoadFilledRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadFilledRewardFunction, cancellationToken);
        }

        public Task<string> GiveRoadRewardRequestAsync(GiveRoadRewardFunction giveRoadRewardFunction)
        {
             return ContractHandler.SendRequestAsync(giveRoadRewardFunction);
        }

        public Task<TransactionReceipt> GiveRoadRewardRequestAndWaitForReceiptAsync(GiveRoadRewardFunction giveRoadRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadRewardFunction, cancellationToken);
        }

        public Task<string> GiveRoadRewardRequestAsync(byte[] road)
        {
            var giveRoadRewardFunction = new GiveRoadRewardFunction();
                giveRoadRewardFunction.Road = road;
            
             return ContractHandler.SendRequestAsync(giveRoadRewardFunction);
        }

        public Task<TransactionReceipt> GiveRoadRewardRequestAndWaitForReceiptAsync(byte[] road, CancellationTokenSource cancellationToken = null)
        {
            var giveRoadRewardFunction = new GiveRoadRewardFunction();
                giveRoadRewardFunction.Road = road;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadRewardFunction, cancellationToken);
        }

        public Task<string> GiveRoadShoveledRewardRequestAsync(GiveRoadShoveledRewardFunction giveRoadShoveledRewardFunction)
        {
             return ContractHandler.SendRequestAsync(giveRoadShoveledRewardFunction);
        }

        public Task<TransactionReceipt> GiveRoadShoveledRewardRequestAndWaitForReceiptAsync(GiveRoadShoveledRewardFunction giveRoadShoveledRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadShoveledRewardFunction, cancellationToken);
        }

        public Task<string> GiveRoadShoveledRewardRequestAsync(byte[] player)
        {
            var giveRoadShoveledRewardFunction = new GiveRoadShoveledRewardFunction();
                giveRoadShoveledRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAsync(giveRoadShoveledRewardFunction);
        }

        public Task<TransactionReceipt> GiveRoadShoveledRewardRequestAndWaitForReceiptAsync(byte[] player, CancellationTokenSource cancellationToken = null)
        {
            var giveRoadShoveledRewardFunction = new GiveRoadShoveledRewardFunction();
                giveRoadShoveledRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadShoveledRewardFunction, cancellationToken);
        }

        public Task<string> GiveXPRequestAsync(GiveXPFunction giveXPFunction)
        {
             return ContractHandler.SendRequestAsync(giveXPFunction);
        }

        public Task<TransactionReceipt> GiveXPRequestAndWaitForReceiptAsync(GiveXPFunction giveXPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveXPFunction, cancellationToken);
        }

        public Task<string> GiveXPRequestAsync(byte[] player, BigInteger amount)
        {
            var giveXPFunction = new GiveXPFunction();
                giveXPFunction.Player = player;
                giveXPFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(giveXPFunction);
        }

        public Task<TransactionReceipt> GiveXPRequestAndWaitForReceiptAsync(byte[] player, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var giveXPFunction = new GiveXPFunction();
                giveXPFunction.Player = player;
                giveXPFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveXPFunction, cancellationToken);
        }

        public Task<string> GrantAccessRequestAsync(GrantAccessFunction grantAccessFunction)
        {
             return ContractHandler.SendRequestAsync(grantAccessFunction);
        }

        public Task<TransactionReceipt> GrantAccessRequestAndWaitForReceiptAsync(GrantAccessFunction grantAccessFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(grantAccessFunction, cancellationToken);
        }

        public Task<string> GrantAccessRequestAsync(byte[] @namespace, byte[] name, string grantee)
        {
            var grantAccessFunction = new GrantAccessFunction();
                grantAccessFunction.Namespace = @namespace;
                grantAccessFunction.Name = name;
                grantAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAsync(grantAccessFunction);
        }

        public Task<TransactionReceipt> GrantAccessRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string grantee, CancellationTokenSource cancellationToken = null)
        {
            var grantAccessFunction = new GrantAccessFunction();
                grantAccessFunction.Namespace = @namespace;
                grantAccessFunction.Name = name;
                grantAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(grantAccessFunction, cancellationToken);
        }

        public Task<string> InstallModuleRequestAsync(InstallModuleFunction installModuleFunction)
        {
             return ContractHandler.SendRequestAsync(installModuleFunction);
        }

        public Task<TransactionReceipt> InstallModuleRequestAndWaitForReceiptAsync(InstallModuleFunction installModuleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(installModuleFunction, cancellationToken);
        }

        public Task<string> InstallModuleRequestAsync(string module, byte[] args)
        {
            var installModuleFunction = new InstallModuleFunction();
                installModuleFunction.Module = module;
                installModuleFunction.Args = args;
            
             return ContractHandler.SendRequestAsync(installModuleFunction);
        }

        public Task<TransactionReceipt> InstallModuleRequestAndWaitForReceiptAsync(string module, byte[] args, CancellationTokenSource cancellationToken = null)
        {
            var installModuleFunction = new InstallModuleFunction();
                installModuleFunction.Module = module;
                installModuleFunction.Args = args;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(installModuleFunction, cancellationToken);
        }

        public Task<string> InstallRootModuleRequestAsync(InstallRootModuleFunction installRootModuleFunction)
        {
             return ContractHandler.SendRequestAsync(installRootModuleFunction);
        }

        public Task<TransactionReceipt> InstallRootModuleRequestAndWaitForReceiptAsync(InstallRootModuleFunction installRootModuleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(installRootModuleFunction, cancellationToken);
        }

        public Task<string> InstallRootModuleRequestAsync(string module, byte[] args)
        {
            var installRootModuleFunction = new InstallRootModuleFunction();
                installRootModuleFunction.Module = module;
                installRootModuleFunction.Args = args;
            
             return ContractHandler.SendRequestAsync(installRootModuleFunction);
        }

        public Task<TransactionReceipt> InstallRootModuleRequestAndWaitForReceiptAsync(string module, byte[] args, CancellationTokenSource cancellationToken = null)
        {
            var installRootModuleFunction = new InstallRootModuleFunction();
                installRootModuleFunction.Module = module;
                installRootModuleFunction.Args = args;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(installRootModuleFunction, cancellationToken);
        }

        public Task<bool> IsAdminQueryAsync(IsAdminFunction isAdminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsAdminFunction, bool>(isAdminFunction, blockParameter);
        }

        
        public Task<bool> IsAdminQueryAsync(byte[] player, BlockParameter blockParameter = null)
        {
            var isAdminFunction = new IsAdminFunction();
                isAdminFunction.Player = player;
            
            return ContractHandler.QueryAsync<IsAdminFunction, bool>(isAdminFunction, blockParameter);
        }



        public Task<string> KillRequestAsync(KillFunction killFunction)
        {
             return ContractHandler.SendRequestAsync(killFunction);
        }

        public Task<TransactionReceipt> KillRequestAndWaitForReceiptAsync(KillFunction killFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killFunction, cancellationToken);
        }

        public Task<string> KillRequestAsync(byte[] causedBy, byte[] target, byte[] attacker, PositionData pos)
        {
            var killFunction = new KillFunction();
                killFunction.CausedBy = causedBy;
                killFunction.Target = target;
                killFunction.Attacker = attacker;
                killFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(killFunction);
        }

        public Task<TransactionReceipt> KillRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] target, byte[] attacker, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var killFunction = new KillFunction();
                killFunction.CausedBy = causedBy;
                killFunction.Target = target;
                killFunction.Attacker = attacker;
                killFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killFunction, cancellationToken);
        }

        public Task<string> KillPlayerAdminRequestAsync(KillPlayerAdminFunction killPlayerAdminFunction)
        {
             return ContractHandler.SendRequestAsync(killPlayerAdminFunction);
        }

        public Task<TransactionReceipt> KillPlayerAdminRequestAndWaitForReceiptAsync(KillPlayerAdminFunction killPlayerAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killPlayerAdminFunction, cancellationToken);
        }

        public Task<string> KillPlayerAdminRequestAsync(int x, int y)
        {
            var killPlayerAdminFunction = new KillPlayerAdminFunction();
                killPlayerAdminFunction.X = x;
                killPlayerAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(killPlayerAdminFunction);
        }

        public Task<TransactionReceipt> KillPlayerAdminRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var killPlayerAdminFunction = new KillPlayerAdminFunction();
                killPlayerAdminFunction.X = x;
                killPlayerAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killPlayerAdminFunction, cancellationToken);
        }

        public Task<string> KillRewardsRequestAsync(KillRewardsFunction killRewardsFunction)
        {
             return ContractHandler.SendRequestAsync(killRewardsFunction);
        }

        public Task<TransactionReceipt> KillRewardsRequestAndWaitForReceiptAsync(KillRewardsFunction killRewardsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killRewardsFunction, cancellationToken);
        }

        public Task<string> KillRewardsRequestAsync(byte[] causedBy, byte[] target, byte[] attacker)
        {
            var killRewardsFunction = new KillRewardsFunction();
                killRewardsFunction.CausedBy = causedBy;
                killRewardsFunction.Target = target;
                killRewardsFunction.Attacker = attacker;
            
             return ContractHandler.SendRequestAsync(killRewardsFunction);
        }

        public Task<TransactionReceipt> KillRewardsRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] target, byte[] attacker, CancellationTokenSource cancellationToken = null)
        {
            var killRewardsFunction = new KillRewardsFunction();
                killRewardsFunction.CausedBy = causedBy;
                killRewardsFunction.Target = target;
                killRewardsFunction.Attacker = attacker;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killRewardsFunction, cancellationToken);
        }

        public Task<string> ManifestRequestAsync(ManifestFunction manifestFunction)
        {
             return ContractHandler.SendRequestAsync(manifestFunction);
        }

        public Task<TransactionReceipt> ManifestRequestAndWaitForReceiptAsync(ManifestFunction manifestFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(manifestFunction, cancellationToken);
        }

        public Task<string> ManifestRequestAsync(uint item)
        {
            var manifestFunction = new ManifestFunction();
                manifestFunction.Item = item;
            
             return ContractHandler.SendRequestAsync(manifestFunction);
        }

        public Task<TransactionReceipt> ManifestRequestAndWaitForReceiptAsync(uint item, CancellationTokenSource cancellationToken = null)
        {
            var manifestFunction = new ManifestFunction();
                manifestFunction.Item = item;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(manifestFunction, cancellationToken);
        }

        public Task<string> MeleeRequestAsync(MeleeFunction meleeFunction)
        {
             return ContractHandler.SendRequestAsync(meleeFunction);
        }

        public Task<TransactionReceipt> MeleeRequestAndWaitForReceiptAsync(MeleeFunction meleeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(meleeFunction, cancellationToken);
        }

        public Task<string> MeleeRequestAsync(byte[] player, int x, int y)
        {
            var meleeFunction = new MeleeFunction();
                meleeFunction.Player = player;
                meleeFunction.X = x;
                meleeFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(meleeFunction);
        }

        public Task<TransactionReceipt> MeleeRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var meleeFunction = new MeleeFunction();
                meleeFunction.Player = player;
                meleeFunction.X = x;
                meleeFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(meleeFunction, cancellationToken);
        }

        public Task<string> MineRequestAsync(MineFunction mineFunction)
        {
             return ContractHandler.SendRequestAsync(mineFunction);
        }

        public Task<TransactionReceipt> MineRequestAndWaitForReceiptAsync(MineFunction mineFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mineFunction, cancellationToken);
        }

        public Task<string> MineRequestAsync(byte[] player, int x, int y)
        {
            var mineFunction = new MineFunction();
                mineFunction.Player = player;
                mineFunction.X = x;
                mineFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(mineFunction);
        }

        public Task<TransactionReceipt> MineRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var mineFunction = new MineFunction();
                mineFunction.Player = player;
                mineFunction.X = x;
                mineFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mineFunction, cancellationToken);
        }

        public Task<string> MoveSimpleRequestAsync(MoveSimpleFunction moveSimpleFunction)
        {
             return ContractHandler.SendRequestAsync(moveSimpleFunction);
        }

        public Task<TransactionReceipt> MoveSimpleRequestAndWaitForReceiptAsync(MoveSimpleFunction moveSimpleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveSimpleFunction, cancellationToken);
        }

        public Task<string> MoveSimpleRequestAsync(byte[] player, int x, int y)
        {
            var moveSimpleFunction = new MoveSimpleFunction();
                moveSimpleFunction.Player = player;
                moveSimpleFunction.X = x;
                moveSimpleFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(moveSimpleFunction);
        }

        public Task<TransactionReceipt> MoveSimpleRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var moveSimpleFunction = new MoveSimpleFunction();
                moveSimpleFunction.Player = player;
                moveSimpleFunction.X = x;
                moveSimpleFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveSimpleFunction, cancellationToken);
        }

        public Task<string> MoveToRequestAsync(MoveToFunction moveToFunction)
        {
             return ContractHandler.SendRequestAsync(moveToFunction);
        }

        public Task<TransactionReceipt> MoveToRequestAndWaitForReceiptAsync(MoveToFunction moveToFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveToFunction, cancellationToken);
        }

        public Task<string> MoveToRequestAsync(byte[] causedBy, byte[] entity, PositionData from, PositionData to, List<byte[]> atDest, byte animation)
        {
            var moveToFunction = new MoveToFunction();
                moveToFunction.CausedBy = causedBy;
                moveToFunction.Entity = entity;
                moveToFunction.From = from;
                moveToFunction.To = to;
                moveToFunction.AtDest = atDest;
                moveToFunction.Animation = animation;
            
             return ContractHandler.SendRequestAsync(moveToFunction);
        }

        public Task<TransactionReceipt> MoveToRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData from, PositionData to, List<byte[]> atDest, byte animation, CancellationTokenSource cancellationToken = null)
        {
            var moveToFunction = new MoveToFunction();
                moveToFunction.CausedBy = causedBy;
                moveToFunction.Entity = entity;
                moveToFunction.From = from;
                moveToFunction.To = to;
                moveToFunction.AtDest = atDest;
                moveToFunction.Animation = animation;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveToFunction, cancellationToken);
        }

        public Task<string> NameRequestAsync(NameFunction nameFunction)
        {
             return ContractHandler.SendRequestAsync(nameFunction);
        }

        public Task<TransactionReceipt> NameRequestAndWaitForReceiptAsync(NameFunction nameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(nameFunction, cancellationToken);
        }

        public Task<string> NameRequestAsync(uint firstName, uint middleName, uint lastName)
        {
            var nameFunction = new NameFunction();
                nameFunction.FirstName = firstName;
                nameFunction.MiddleName = middleName;
                nameFunction.LastName = lastName;
            
             return ContractHandler.SendRequestAsync(nameFunction);
        }

        public Task<TransactionReceipt> NameRequestAndWaitForReceiptAsync(uint firstName, uint middleName, uint lastName, CancellationTokenSource cancellationToken = null)
        {
            var nameFunction = new NameFunction();
                nameFunction.FirstName = firstName;
                nameFunction.MiddleName = middleName;
                nameFunction.LastName = lastName;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(nameFunction, cancellationToken);
        }

        public Task<NeumanNeighborhoodOutputDTO> NeumanNeighborhoodQueryAsync(NeumanNeighborhoodFunction neumanNeighborhoodFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<NeumanNeighborhoodFunction, NeumanNeighborhoodOutputDTO>(neumanNeighborhoodFunction, blockParameter);
        }

        public Task<NeumanNeighborhoodOutputDTO> NeumanNeighborhoodQueryAsync(PositionData center, int distance, BlockParameter blockParameter = null)
        {
            var neumanNeighborhoodFunction = new NeumanNeighborhoodFunction();
                neumanNeighborhoodFunction.Center = center;
                neumanNeighborhoodFunction.Distance = distance;
            
            return ContractHandler.QueryDeserializingToObjectAsync<NeumanNeighborhoodFunction, NeumanNeighborhoodOutputDTO>(neumanNeighborhoodFunction, blockParameter);
        }

        public Task<NeumanNeighborhoodOuterOutputDTO> NeumanNeighborhoodOuterQueryAsync(NeumanNeighborhoodOuterFunction neumanNeighborhoodOuterFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<NeumanNeighborhoodOuterFunction, NeumanNeighborhoodOuterOutputDTO>(neumanNeighborhoodOuterFunction, blockParameter);
        }

        public Task<NeumanNeighborhoodOuterOutputDTO> NeumanNeighborhoodOuterQueryAsync(PositionData center, int distance, BlockParameter blockParameter = null)
        {
            var neumanNeighborhoodOuterFunction = new NeumanNeighborhoodOuterFunction();
                neumanNeighborhoodOuterFunction.Center = center;
                neumanNeighborhoodOuterFunction.Distance = distance;
            
            return ContractHandler.QueryDeserializingToObjectAsync<NeumanNeighborhoodOuterFunction, NeumanNeighborhoodOuterOutputDTO>(neumanNeighborhoodOuterFunction, blockParameter);
        }

        public Task<bool> OnMapQueryAsync(OnMapFunction onMapFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OnMapFunction, bool>(onMapFunction, blockParameter);
        }

        
        public Task<bool> OnMapQueryAsync(int x, int y, BlockParameter blockParameter = null)
        {
            var onMapFunction = new OnMapFunction();
                onMapFunction.X = x;
                onMapFunction.Y = y;
            
            return ContractHandler.QueryAsync<OnMapFunction, bool>(onMapFunction, blockParameter);
        }

        public Task<string> OnRoadRequestAsync(OnRoadFunction onRoadFunction)
        {
             return ContractHandler.SendRequestAsync(onRoadFunction);
        }

        public Task<TransactionReceipt> OnRoadRequestAndWaitForReceiptAsync(OnRoadFunction onRoadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(onRoadFunction, cancellationToken);
        }

        public Task<string> OnRoadRequestAsync(int x, int y)
        {
            var onRoadFunction = new OnRoadFunction();
                onRoadFunction.X = x;
                onRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(onRoadFunction);
        }

        public Task<TransactionReceipt> OnRoadRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var onRoadFunction = new OnRoadFunction();
                onRoadFunction.X = x;
                onRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(onRoadFunction, cancellationToken);
        }

        public Task<bool> OnSpawnQueryAsync(OnSpawnFunction onSpawnFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OnSpawnFunction, bool>(onSpawnFunction, blockParameter);
        }

        
        public Task<bool> OnSpawnQueryAsync(int x, int y, BlockParameter blockParameter = null)
        {
            var onSpawnFunction = new OnSpawnFunction();
                onSpawnFunction.X = x;
                onSpawnFunction.Y = y;
            
            return ContractHandler.QueryAsync<OnSpawnFunction, bool>(onSpawnFunction, blockParameter);
        }

        public Task<bool> OnWorldQueryAsync(OnWorldFunction onWorldFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OnWorldFunction, bool>(onWorldFunction, blockParameter);
        }

        
        public Task<bool> OnWorldQueryAsync(int x, int y, BlockParameter blockParameter = null)
        {
            var onWorldFunction = new OnWorldFunction();
                onWorldFunction.X = x;
                onWorldFunction.Y = y;
            
            return ContractHandler.QueryAsync<OnWorldFunction, bool>(onWorldFunction, blockParameter);
        }

        public Task<string> PlantRequestAsync(PlantFunction plantFunction)
        {
             return ContractHandler.SendRequestAsync(plantFunction);
        }

        public Task<TransactionReceipt> PlantRequestAndWaitForReceiptAsync(PlantFunction plantFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(plantFunction, cancellationToken);
        }

        public Task<string> PlantRequestAsync(byte[] player, int x, int y)
        {
            var plantFunction = new PlantFunction();
                plantFunction.Player = player;
                plantFunction.X = x;
                plantFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(plantFunction);
        }

        public Task<TransactionReceipt> PlantRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var plantFunction = new PlantFunction();
                plantFunction.Player = player;
                plantFunction.X = x;
                plantFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(plantFunction, cancellationToken);
        }

        public Task<string> PopFromFieldRequestAsync(PopFromField1Function popFromField1Function)
        {
             return ContractHandler.SendRequestAsync(popFromField1Function);
        }

        public Task<TransactionReceipt> PopFromFieldRequestAndWaitForReceiptAsync(PopFromField1Function popFromField1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(popFromField1Function, cancellationToken);
        }

        public Task<string> PopFromFieldRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, BigInteger byteLengthToPop)
        {
            var popFromField1Function = new PopFromField1Function();
                popFromField1Function.Namespace = @namespace;
                popFromField1Function.Name = name;
                popFromField1Function.Key = key;
                popFromField1Function.SchemaIndex = schemaIndex;
                popFromField1Function.ByteLengthToPop = byteLengthToPop;
            
             return ContractHandler.SendRequestAsync(popFromField1Function);
        }

        public Task<TransactionReceipt> PopFromFieldRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, BigInteger byteLengthToPop, CancellationTokenSource cancellationToken = null)
        {
            var popFromField1Function = new PopFromField1Function();
                popFromField1Function.Namespace = @namespace;
                popFromField1Function.Name = name;
                popFromField1Function.Key = key;
                popFromField1Function.SchemaIndex = schemaIndex;
                popFromField1Function.ByteLengthToPop = byteLengthToPop;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(popFromField1Function, cancellationToken);
        }

        public Task<string> PopFromFieldRequestAsync(PopFromFieldFunction popFromFieldFunction)
        {
             return ContractHandler.SendRequestAsync(popFromFieldFunction);
        }

        public Task<TransactionReceipt> PopFromFieldRequestAndWaitForReceiptAsync(PopFromFieldFunction popFromFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(popFromFieldFunction, cancellationToken);
        }

        public Task<string> PopFromFieldRequestAsync(byte[] table, List<byte[]> key, byte schemaIndex, BigInteger byteLengthToPop)
        {
            var popFromFieldFunction = new PopFromFieldFunction();
                popFromFieldFunction.Table = table;
                popFromFieldFunction.Key = key;
                popFromFieldFunction.SchemaIndex = schemaIndex;
                popFromFieldFunction.ByteLengthToPop = byteLengthToPop;
            
             return ContractHandler.SendRequestAsync(popFromFieldFunction);
        }

        public Task<TransactionReceipt> PopFromFieldRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, byte schemaIndex, BigInteger byteLengthToPop, CancellationTokenSource cancellationToken = null)
        {
            var popFromFieldFunction = new PopFromFieldFunction();
                popFromFieldFunction.Table = table;
                popFromFieldFunction.Key = key;
                popFromFieldFunction.SchemaIndex = schemaIndex;
                popFromFieldFunction.ByteLengthToPop = byteLengthToPop;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(popFromFieldFunction, cancellationToken);
        }

        public Task<string> PushRequestAsync(PushFunction pushFunction)
        {
             return ContractHandler.SendRequestAsync(pushFunction);
        }

        public Task<TransactionReceipt> PushRequestAndWaitForReceiptAsync(PushFunction pushFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushFunction, cancellationToken);
        }

        public Task<string> PushRequestAsync(byte[] player, int x, int y)
        {
            var pushFunction = new PushFunction();
                pushFunction.Player = player;
                pushFunction.X = x;
                pushFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(pushFunction);
        }

        public Task<TransactionReceipt> PushRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var pushFunction = new PushFunction();
                pushFunction.Player = player;
                pushFunction.X = x;
                pushFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushFunction, cancellationToken);
        }

        public Task<string> PushToFieldRequestAsync(PushToFieldFunction pushToFieldFunction)
        {
             return ContractHandler.SendRequestAsync(pushToFieldFunction);
        }

        public Task<TransactionReceipt> PushToFieldRequestAndWaitForReceiptAsync(PushToFieldFunction pushToFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushToFieldFunction, cancellationToken);
        }

        public Task<string> PushToFieldRequestAsync(byte[] table, List<byte[]> key, byte schemaIndex, byte[] dataToPush)
        {
            var pushToFieldFunction = new PushToFieldFunction();
                pushToFieldFunction.Table = table;
                pushToFieldFunction.Key = key;
                pushToFieldFunction.SchemaIndex = schemaIndex;
                pushToFieldFunction.DataToPush = dataToPush;
            
             return ContractHandler.SendRequestAsync(pushToFieldFunction);
        }

        public Task<TransactionReceipt> PushToFieldRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, byte schemaIndex, byte[] dataToPush, CancellationTokenSource cancellationToken = null)
        {
            var pushToFieldFunction = new PushToFieldFunction();
                pushToFieldFunction.Table = table;
                pushToFieldFunction.Key = key;
                pushToFieldFunction.SchemaIndex = schemaIndex;
                pushToFieldFunction.DataToPush = dataToPush;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushToFieldFunction, cancellationToken);
        }

        public Task<string> PushToFieldRequestAsync(PushToField1Function pushToField1Function)
        {
             return ContractHandler.SendRequestAsync(pushToField1Function);
        }

        public Task<TransactionReceipt> PushToFieldRequestAndWaitForReceiptAsync(PushToField1Function pushToField1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushToField1Function, cancellationToken);
        }

        public Task<string> PushToFieldRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, byte[] dataToPush)
        {
            var pushToField1Function = new PushToField1Function();
                pushToField1Function.Namespace = @namespace;
                pushToField1Function.Name = name;
                pushToField1Function.Key = key;
                pushToField1Function.SchemaIndex = schemaIndex;
                pushToField1Function.DataToPush = dataToPush;
            
             return ContractHandler.SendRequestAsync(pushToField1Function);
        }

        public Task<TransactionReceipt> PushToFieldRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, byte[] dataToPush, CancellationTokenSource cancellationToken = null)
        {
            var pushToField1Function = new PushToField1Function();
                pushToField1Function.Namespace = @namespace;
                pushToField1Function.Name = name;
                pushToField1Function.Key = key;
                pushToField1Function.SchemaIndex = schemaIndex;
                pushToField1Function.DataToPush = dataToPush;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushToField1Function, cancellationToken);
        }

        public Task<string> RegisterFunctionSelectorRequestAsync(RegisterFunctionSelectorFunction registerFunctionSelectorFunction)
        {
             return ContractHandler.SendRequestAsync(registerFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterFunctionSelectorRequestAndWaitForReceiptAsync(RegisterFunctionSelectorFunction registerFunctionSelectorFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterFunctionSelectorRequestAsync(byte[] @namespace, byte[] name, string systemFunctionName, string systemFunctionArguments)
        {
            var registerFunctionSelectorFunction = new RegisterFunctionSelectorFunction();
                registerFunctionSelectorFunction.Namespace = @namespace;
                registerFunctionSelectorFunction.Name = name;
                registerFunctionSelectorFunction.SystemFunctionName = systemFunctionName;
                registerFunctionSelectorFunction.SystemFunctionArguments = systemFunctionArguments;
            
             return ContractHandler.SendRequestAsync(registerFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterFunctionSelectorRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string systemFunctionName, string systemFunctionArguments, CancellationTokenSource cancellationToken = null)
        {
            var registerFunctionSelectorFunction = new RegisterFunctionSelectorFunction();
                registerFunctionSelectorFunction.Namespace = @namespace;
                registerFunctionSelectorFunction.Name = name;
                registerFunctionSelectorFunction.SystemFunctionName = systemFunctionName;
                registerFunctionSelectorFunction.SystemFunctionArguments = systemFunctionArguments;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterHookRequestAsync(RegisterHookFunction registerHookFunction)
        {
             return ContractHandler.SendRequestAsync(registerHookFunction);
        }

        public Task<TransactionReceipt> RegisterHookRequestAndWaitForReceiptAsync(RegisterHookFunction registerHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerHookFunction, cancellationToken);
        }

        public Task<string> RegisterHookRequestAsync(byte[] @namespace, byte[] name, string hook)
        {
            var registerHookFunction = new RegisterHookFunction();
                registerHookFunction.Namespace = @namespace;
                registerHookFunction.Name = name;
                registerHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAsync(registerHookFunction);
        }

        public Task<TransactionReceipt> RegisterHookRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string hook, CancellationTokenSource cancellationToken = null)
        {
            var registerHookFunction = new RegisterHookFunction();
                registerHookFunction.Namespace = @namespace;
                registerHookFunction.Name = name;
                registerHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerHookFunction, cancellationToken);
        }

        public Task<string> RegisterNamespaceRequestAsync(RegisterNamespaceFunction registerNamespaceFunction)
        {
             return ContractHandler.SendRequestAsync(registerNamespaceFunction);
        }

        public Task<TransactionReceipt> RegisterNamespaceRequestAndWaitForReceiptAsync(RegisterNamespaceFunction registerNamespaceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerNamespaceFunction, cancellationToken);
        }

        public Task<string> RegisterNamespaceRequestAsync(byte[] @namespace)
        {
            var registerNamespaceFunction = new RegisterNamespaceFunction();
                registerNamespaceFunction.Namespace = @namespace;
            
             return ContractHandler.SendRequestAsync(registerNamespaceFunction);
        }

        public Task<TransactionReceipt> RegisterNamespaceRequestAndWaitForReceiptAsync(byte[] @namespace, CancellationTokenSource cancellationToken = null)
        {
            var registerNamespaceFunction = new RegisterNamespaceFunction();
                registerNamespaceFunction.Namespace = @namespace;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerNamespaceFunction, cancellationToken);
        }

        public Task<string> RegisterRootFunctionSelectorRequestAsync(RegisterRootFunctionSelectorFunction registerRootFunctionSelectorFunction)
        {
             return ContractHandler.SendRequestAsync(registerRootFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterRootFunctionSelectorRequestAndWaitForReceiptAsync(RegisterRootFunctionSelectorFunction registerRootFunctionSelectorFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerRootFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterRootFunctionSelectorRequestAsync(byte[] @namespace, byte[] name, byte[] worldFunctionSelector, byte[] systemFunctionSelector)
        {
            var registerRootFunctionSelectorFunction = new RegisterRootFunctionSelectorFunction();
                registerRootFunctionSelectorFunction.Namespace = @namespace;
                registerRootFunctionSelectorFunction.Name = name;
                registerRootFunctionSelectorFunction.WorldFunctionSelector = worldFunctionSelector;
                registerRootFunctionSelectorFunction.SystemFunctionSelector = systemFunctionSelector;
            
             return ContractHandler.SendRequestAsync(registerRootFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterRootFunctionSelectorRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, byte[] worldFunctionSelector, byte[] systemFunctionSelector, CancellationTokenSource cancellationToken = null)
        {
            var registerRootFunctionSelectorFunction = new RegisterRootFunctionSelectorFunction();
                registerRootFunctionSelectorFunction.Namespace = @namespace;
                registerRootFunctionSelectorFunction.Name = name;
                registerRootFunctionSelectorFunction.WorldFunctionSelector = worldFunctionSelector;
                registerRootFunctionSelectorFunction.SystemFunctionSelector = systemFunctionSelector;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerRootFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterSchemaRequestAsync(RegisterSchemaFunction registerSchemaFunction)
        {
             return ContractHandler.SendRequestAsync(registerSchemaFunction);
        }

        public Task<TransactionReceipt> RegisterSchemaRequestAndWaitForReceiptAsync(RegisterSchemaFunction registerSchemaFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerSchemaFunction, cancellationToken);
        }

        public Task<string> RegisterSchemaRequestAsync(byte[] table, byte[] schema, byte[] keySchema)
        {
            var registerSchemaFunction = new RegisterSchemaFunction();
                registerSchemaFunction.Table = table;
                registerSchemaFunction.Schema = schema;
                registerSchemaFunction.KeySchema = keySchema;
            
             return ContractHandler.SendRequestAsync(registerSchemaFunction);
        }

        public Task<TransactionReceipt> RegisterSchemaRequestAndWaitForReceiptAsync(byte[] table, byte[] schema, byte[] keySchema, CancellationTokenSource cancellationToken = null)
        {
            var registerSchemaFunction = new RegisterSchemaFunction();
                registerSchemaFunction.Table = table;
                registerSchemaFunction.Schema = schema;
                registerSchemaFunction.KeySchema = keySchema;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerSchemaFunction, cancellationToken);
        }

        public Task<string> RegisterStoreHookRequestAsync(RegisterStoreHookFunction registerStoreHookFunction)
        {
             return ContractHandler.SendRequestAsync(registerStoreHookFunction);
        }

        public Task<TransactionReceipt> RegisterStoreHookRequestAndWaitForReceiptAsync(RegisterStoreHookFunction registerStoreHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerStoreHookFunction, cancellationToken);
        }

        public Task<string> RegisterStoreHookRequestAsync(byte[] table, string hook)
        {
            var registerStoreHookFunction = new RegisterStoreHookFunction();
                registerStoreHookFunction.Table = table;
                registerStoreHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAsync(registerStoreHookFunction);
        }

        public Task<TransactionReceipt> RegisterStoreHookRequestAndWaitForReceiptAsync(byte[] table, string hook, CancellationTokenSource cancellationToken = null)
        {
            var registerStoreHookFunction = new RegisterStoreHookFunction();
                registerStoreHookFunction.Table = table;
                registerStoreHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerStoreHookFunction, cancellationToken);
        }

        public Task<string> RegisterSystemRequestAsync(RegisterSystemFunction registerSystemFunction)
        {
             return ContractHandler.SendRequestAsync(registerSystemFunction);
        }

        public Task<TransactionReceipt> RegisterSystemRequestAndWaitForReceiptAsync(RegisterSystemFunction registerSystemFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerSystemFunction, cancellationToken);
        }

        public Task<string> RegisterSystemRequestAsync(byte[] @namespace, byte[] name, string system, bool publicAccess)
        {
            var registerSystemFunction = new RegisterSystemFunction();
                registerSystemFunction.Namespace = @namespace;
                registerSystemFunction.Name = name;
                registerSystemFunction.System = system;
                registerSystemFunction.PublicAccess = publicAccess;
            
             return ContractHandler.SendRequestAsync(registerSystemFunction);
        }

        public Task<TransactionReceipt> RegisterSystemRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string system, bool publicAccess, CancellationTokenSource cancellationToken = null)
        {
            var registerSystemFunction = new RegisterSystemFunction();
                registerSystemFunction.Namespace = @namespace;
                registerSystemFunction.Name = name;
                registerSystemFunction.System = system;
                registerSystemFunction.PublicAccess = publicAccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerSystemFunction, cancellationToken);
        }

        public Task<string> RegisterSystemHookRequestAsync(RegisterSystemHookFunction registerSystemHookFunction)
        {
             return ContractHandler.SendRequestAsync(registerSystemHookFunction);
        }

        public Task<TransactionReceipt> RegisterSystemHookRequestAndWaitForReceiptAsync(RegisterSystemHookFunction registerSystemHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerSystemHookFunction, cancellationToken);
        }

        public Task<string> RegisterSystemHookRequestAsync(byte[] @namespace, byte[] name, string hook)
        {
            var registerSystemHookFunction = new RegisterSystemHookFunction();
                registerSystemHookFunction.Namespace = @namespace;
                registerSystemHookFunction.Name = name;
                registerSystemHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAsync(registerSystemHookFunction);
        }

        public Task<TransactionReceipt> RegisterSystemHookRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string hook, CancellationTokenSource cancellationToken = null)
        {
            var registerSystemHookFunction = new RegisterSystemHookFunction();
                registerSystemHookFunction.Namespace = @namespace;
                registerSystemHookFunction.Name = name;
                registerSystemHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerSystemHookFunction, cancellationToken);
        }

        public Task<string> RegisterTableRequestAsync(RegisterTableFunction registerTableFunction)
        {
             return ContractHandler.SendRequestAsync(registerTableFunction);
        }

        public Task<TransactionReceipt> RegisterTableRequestAndWaitForReceiptAsync(RegisterTableFunction registerTableFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerTableFunction, cancellationToken);
        }

        public Task<string> RegisterTableRequestAsync(byte[] @namespace, byte[] name, byte[] valueSchema, byte[] keySchema)
        {
            var registerTableFunction = new RegisterTableFunction();
                registerTableFunction.Namespace = @namespace;
                registerTableFunction.Name = name;
                registerTableFunction.ValueSchema = valueSchema;
                registerTableFunction.KeySchema = keySchema;
            
             return ContractHandler.SendRequestAsync(registerTableFunction);
        }

        public Task<TransactionReceipt> RegisterTableRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, byte[] valueSchema, byte[] keySchema, CancellationTokenSource cancellationToken = null)
        {
            var registerTableFunction = new RegisterTableFunction();
                registerTableFunction.Namespace = @namespace;
                registerTableFunction.Name = name;
                registerTableFunction.ValueSchema = valueSchema;
                registerTableFunction.KeySchema = keySchema;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerTableFunction, cancellationToken);
        }

        public Task<string> RegisterTableHookRequestAsync(RegisterTableHookFunction registerTableHookFunction)
        {
             return ContractHandler.SendRequestAsync(registerTableHookFunction);
        }

        public Task<TransactionReceipt> RegisterTableHookRequestAndWaitForReceiptAsync(RegisterTableHookFunction registerTableHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerTableHookFunction, cancellationToken);
        }

        public Task<string> RegisterTableHookRequestAsync(byte[] @namespace, byte[] name, string hook)
        {
            var registerTableHookFunction = new RegisterTableHookFunction();
                registerTableHookFunction.Namespace = @namespace;
                registerTableHookFunction.Name = name;
                registerTableHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAsync(registerTableHookFunction);
        }

        public Task<TransactionReceipt> RegisterTableHookRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string hook, CancellationTokenSource cancellationToken = null)
        {
            var registerTableHookFunction = new RegisterTableHookFunction();
                registerTableHookFunction.Namespace = @namespace;
                registerTableHookFunction.Name = name;
                registerTableHookFunction.Hook = hook;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerTableHookFunction, cancellationToken);
        }



        public Task<string> RequireLegalMoveRequestAsync(RequireLegalMoveFunction requireLegalMoveFunction)
        {
             return ContractHandler.SendRequestAsync(requireLegalMoveFunction);
        }

        public Task<TransactionReceipt> RequireLegalMoveRequestAndWaitForReceiptAsync(RequireLegalMoveFunction requireLegalMoveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(requireLegalMoveFunction, cancellationToken);
        }

        public Task<string> RequireLegalMoveRequestAsync(byte[] player, PositionData from, PositionData to, BigInteger distance)
        {
            var requireLegalMoveFunction = new RequireLegalMoveFunction();
                requireLegalMoveFunction.Player = player;
                requireLegalMoveFunction.From = from;
                requireLegalMoveFunction.To = to;
                requireLegalMoveFunction.Distance = distance;
            
             return ContractHandler.SendRequestAsync(requireLegalMoveFunction);
        }

        public Task<TransactionReceipt> RequireLegalMoveRequestAndWaitForReceiptAsync(byte[] player, PositionData from, PositionData to, BigInteger distance, CancellationTokenSource cancellationToken = null)
        {
            var requireLegalMoveFunction = new RequireLegalMoveFunction();
                requireLegalMoveFunction.Player = player;
                requireLegalMoveFunction.From = from;
                requireLegalMoveFunction.To = to;
                requireLegalMoveFunction.Distance = distance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(requireLegalMoveFunction, cancellationToken);
        }





        public Task<bool> RequirePushableOrEmptyQueryAsync(RequirePushableOrEmptyFunction requirePushableOrEmptyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RequirePushableOrEmptyFunction, bool>(requirePushableOrEmptyFunction, blockParameter);
        }

        
        public Task<bool> RequirePushableOrEmptyQueryAsync(List<byte[]> at, BlockParameter blockParameter = null)
        {
            var requirePushableOrEmptyFunction = new RequirePushableOrEmptyFunction();
                requirePushableOrEmptyFunction.At = at;
            
            return ContractHandler.QueryAsync<RequirePushableOrEmptyFunction, bool>(requirePushableOrEmptyFunction, blockParameter);
        }

        public Task<string> ResetPlayerRequestAsync(ResetPlayerFunction resetPlayerFunction)
        {
             return ContractHandler.SendRequestAsync(resetPlayerFunction);
        }

        public Task<string> ResetPlayerRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ResetPlayerFunction>();
        }

        public Task<TransactionReceipt> ResetPlayerRequestAndWaitForReceiptAsync(ResetPlayerFunction resetPlayerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(resetPlayerFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ResetPlayerRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ResetPlayerFunction>(null, cancellationToken);
        }

        public Task<string> RevokeAccessRequestAsync(RevokeAccessFunction revokeAccessFunction)
        {
             return ContractHandler.SendRequestAsync(revokeAccessFunction);
        }

        public Task<TransactionReceipt> RevokeAccessRequestAndWaitForReceiptAsync(RevokeAccessFunction revokeAccessFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(revokeAccessFunction, cancellationToken);
        }

        public Task<string> RevokeAccessRequestAsync(byte[] @namespace, byte[] name, string grantee)
        {
            var revokeAccessFunction = new RevokeAccessFunction();
                revokeAccessFunction.Namespace = @namespace;
                revokeAccessFunction.Name = name;
                revokeAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAsync(revokeAccessFunction);
        }

        public Task<TransactionReceipt> RevokeAccessRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string grantee, CancellationTokenSource cancellationToken = null)
        {
            var revokeAccessFunction = new RevokeAccessFunction();
                revokeAccessFunction.Namespace = @namespace;
                revokeAccessFunction.Name = name;
                revokeAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(revokeAccessFunction, cancellationToken);
        }

        public Task<string> SeekRequestAsync(SeekFunction seekFunction)
        {
             return ContractHandler.SendRequestAsync(seekFunction);
        }

        public Task<TransactionReceipt> SeekRequestAndWaitForReceiptAsync(SeekFunction seekFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(seekFunction, cancellationToken);
        }

        public Task<string> SeekRequestAsync(byte[] causedBy, byte[] target, byte[] seeker, PositionData targetPos, PositionData seekerPos)
        {
            var seekFunction = new SeekFunction();
                seekFunction.CausedBy = causedBy;
                seekFunction.Target = target;
                seekFunction.Seeker = seeker;
                seekFunction.TargetPos = targetPos;
                seekFunction.SeekerPos = seekerPos;
            
             return ContractHandler.SendRequestAsync(seekFunction);
        }

        public Task<TransactionReceipt> SeekRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] target, byte[] seeker, PositionData targetPos, PositionData seekerPos, CancellationTokenSource cancellationToken = null)
        {
            var seekFunction = new SeekFunction();
                seekFunction.CausedBy = causedBy;
                seekFunction.Target = target;
                seekFunction.Seeker = seeker;
                seekFunction.TargetPos = targetPos;
                seekFunction.SeekerPos = seekerPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(seekFunction, cancellationToken);
        }

        public Task<string> SendCoinsRequestAsync(SendCoinsFunction sendCoinsFunction)
        {
             return ContractHandler.SendRequestAsync(sendCoinsFunction);
        }

        public Task<TransactionReceipt> SendCoinsRequestAndWaitForReceiptAsync(SendCoinsFunction sendCoinsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendCoinsFunction, cancellationToken);
        }

        public Task<string> SendCoinsRequestAsync(int amount)
        {
            var sendCoinsFunction = new SendCoinsFunction();
                sendCoinsFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(sendCoinsFunction);
        }

        public Task<TransactionReceipt> SendCoinsRequestAndWaitForReceiptAsync(int amount, CancellationTokenSource cancellationToken = null)
        {
            var sendCoinsFunction = new SendCoinsFunction();
                sendCoinsFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendCoinsFunction, cancellationToken);
        }

        public Task<string> SetActionRequestAsync(SetActionFunction setActionFunction)
        {
             return ContractHandler.SendRequestAsync(setActionFunction);
        }

        public Task<TransactionReceipt> SetActionRequestAndWaitForReceiptAsync(SetActionFunction setActionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setActionFunction, cancellationToken);
        }

        public Task<string> SetActionRequestAsync(byte[] player, byte newAction, int x, int y)
        {
            var setActionFunction = new SetActionFunction();
                setActionFunction.Player = player;
                setActionFunction.NewAction = newAction;
                setActionFunction.X = x;
                setActionFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(setActionFunction);
        }

        public Task<TransactionReceipt> SetActionRequestAndWaitForReceiptAsync(byte[] player, byte newAction, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var setActionFunction = new SetActionFunction();
                setActionFunction.Player = player;
                setActionFunction.NewAction = newAction;
                setActionFunction.X = x;
                setActionFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setActionFunction, cancellationToken);
        }

        public Task<string> SetFieldRequestAsync(SetFieldFunction setFieldFunction)
        {
             return ContractHandler.SendRequestAsync(setFieldFunction);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(SetFieldFunction setFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFieldFunction, cancellationToken);
        }

        public Task<string> SetFieldRequestAsync(byte[] table, List<byte[]> key, byte schemaIndex, byte[] data)
        {
            var setFieldFunction = new SetFieldFunction();
                setFieldFunction.Table = table;
                setFieldFunction.Key = key;
                setFieldFunction.SchemaIndex = schemaIndex;
                setFieldFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(setFieldFunction);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, byte schemaIndex, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var setFieldFunction = new SetFieldFunction();
                setFieldFunction.Table = table;
                setFieldFunction.Key = key;
                setFieldFunction.SchemaIndex = schemaIndex;
                setFieldFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFieldFunction, cancellationToken);
        }

        public Task<string> SetFieldRequestAsync(SetField1Function setField1Function)
        {
             return ContractHandler.SendRequestAsync(setField1Function);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(SetField1Function setField1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setField1Function, cancellationToken);
        }

        public Task<string> SetFieldRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, byte[] data)
        {
            var setField1Function = new SetField1Function();
                setField1Function.Namespace = @namespace;
                setField1Function.Name = name;
                setField1Function.Key = key;
                setField1Function.SchemaIndex = schemaIndex;
                setField1Function.Data = data;
            
             return ContractHandler.SendRequestAsync(setField1Function);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var setField1Function = new SetField1Function();
                setField1Function.Namespace = @namespace;
                setField1Function.Name = name;
                setField1Function.Key = key;
                setField1Function.SchemaIndex = schemaIndex;
                setField1Function.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setField1Function, cancellationToken);
        }

        public Task<string> SetMetadataRequestAsync(SetMetadata1Function setMetadata1Function)
        {
             return ContractHandler.SendRequestAsync(setMetadata1Function);
        }

        public Task<TransactionReceipt> SetMetadataRequestAndWaitForReceiptAsync(SetMetadata1Function setMetadata1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMetadata1Function, cancellationToken);
        }

        public Task<string> SetMetadataRequestAsync(byte[] @namespace, byte[] name, string tableName, List<string> fieldNames)
        {
            var setMetadata1Function = new SetMetadata1Function();
                setMetadata1Function.Namespace = @namespace;
                setMetadata1Function.Name = name;
                setMetadata1Function.TableName = tableName;
                setMetadata1Function.FieldNames = fieldNames;
            
             return ContractHandler.SendRequestAsync(setMetadata1Function);
        }

        public Task<TransactionReceipt> SetMetadataRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, string tableName, List<string> fieldNames, CancellationTokenSource cancellationToken = null)
        {
            var setMetadata1Function = new SetMetadata1Function();
                setMetadata1Function.Namespace = @namespace;
                setMetadata1Function.Name = name;
                setMetadata1Function.TableName = tableName;
                setMetadata1Function.FieldNames = fieldNames;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMetadata1Function, cancellationToken);
        }

        public Task<string> SetMetadataRequestAsync(SetMetadataFunction setMetadataFunction)
        {
             return ContractHandler.SendRequestAsync(setMetadataFunction);
        }

        public Task<TransactionReceipt> SetMetadataRequestAndWaitForReceiptAsync(SetMetadataFunction setMetadataFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMetadataFunction, cancellationToken);
        }

        public Task<string> SetMetadataRequestAsync(byte[] table, string tableName, List<string> fieldNames)
        {
            var setMetadataFunction = new SetMetadataFunction();
                setMetadataFunction.Table = table;
                setMetadataFunction.TableName = tableName;
                setMetadataFunction.FieldNames = fieldNames;
            
             return ContractHandler.SendRequestAsync(setMetadataFunction);
        }

        public Task<TransactionReceipt> SetMetadataRequestAndWaitForReceiptAsync(byte[] table, string tableName, List<string> fieldNames, CancellationTokenSource cancellationToken = null)
        {
            var setMetadataFunction = new SetMetadataFunction();
                setMetadataFunction.Table = table;
                setMetadataFunction.TableName = tableName;
                setMetadataFunction.FieldNames = fieldNames;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMetadataFunction, cancellationToken);
        }

        public Task<string> SetPositionRequestAsync(SetPositionFunction setPositionFunction)
        {
             return ContractHandler.SendRequestAsync(setPositionFunction);
        }

        public Task<TransactionReceipt> SetPositionRequestAndWaitForReceiptAsync(SetPositionFunction setPositionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionFunction, cancellationToken);
        }

        public Task<string> SetPositionRequestAsync(byte[] causedBy, byte[] entity, int x, int y, int layer, byte action)
        {
            var setPositionFunction = new SetPositionFunction();
                setPositionFunction.CausedBy = causedBy;
                setPositionFunction.Entity = entity;
                setPositionFunction.X = x;
                setPositionFunction.Y = y;
                setPositionFunction.Layer = layer;
                setPositionFunction.Action = action;
            
             return ContractHandler.SendRequestAsync(setPositionFunction);
        }

        public Task<TransactionReceipt> SetPositionRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, int x, int y, int layer, byte action, CancellationTokenSource cancellationToken = null)
        {
            var setPositionFunction = new SetPositionFunction();
                setPositionFunction.CausedBy = causedBy;
                setPositionFunction.Entity = entity;
                setPositionFunction.X = x;
                setPositionFunction.Y = y;
                setPositionFunction.Layer = layer;
                setPositionFunction.Action = action;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionFunction, cancellationToken);
        }

        public Task<string> SetPositionDataRequestAsync(SetPositionDataFunction setPositionDataFunction)
        {
             return ContractHandler.SendRequestAsync(setPositionDataFunction);
        }

        public Task<TransactionReceipt> SetPositionDataRequestAndWaitForReceiptAsync(SetPositionDataFunction setPositionDataFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionDataFunction, cancellationToken);
        }

        public Task<string> SetPositionDataRequestAsync(byte[] causedBy, byte[] entity, PositionData pos, byte action)
        {
            var setPositionDataFunction = new SetPositionDataFunction();
                setPositionDataFunction.CausedBy = causedBy;
                setPositionDataFunction.Entity = entity;
                setPositionDataFunction.Pos = pos;
                setPositionDataFunction.Action = action;
            
             return ContractHandler.SendRequestAsync(setPositionDataFunction);
        }

        public Task<TransactionReceipt> SetPositionDataRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData pos, byte action, CancellationTokenSource cancellationToken = null)
        {
            var setPositionDataFunction = new SetPositionDataFunction();
                setPositionDataFunction.CausedBy = causedBy;
                setPositionDataFunction.Entity = entity;
                setPositionDataFunction.Pos = pos;
                setPositionDataFunction.Action = action;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionDataFunction, cancellationToken);
        }

        public Task<string> SetPositionRawRequestAsync(SetPositionRawFunction setPositionRawFunction)
        {
             return ContractHandler.SendRequestAsync(setPositionRawFunction);
        }

        public Task<TransactionReceipt> SetPositionRawRequestAndWaitForReceiptAsync(SetPositionRawFunction setPositionRawFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionRawFunction, cancellationToken);
        }

        public Task<string> SetPositionRawRequestAsync(byte[] causedBy, byte[] entity, PositionData pos)
        {
            var setPositionRawFunction = new SetPositionRawFunction();
                setPositionRawFunction.CausedBy = causedBy;
                setPositionRawFunction.Entity = entity;
                setPositionRawFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(setPositionRawFunction);
        }

        public Task<TransactionReceipt> SetPositionRawRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var setPositionRawFunction = new SetPositionRawFunction();
                setPositionRawFunction.CausedBy = causedBy;
                setPositionRawFunction.Entity = entity;
                setPositionRawFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionRawFunction, cancellationToken);
        }

        public Task<string> SetRecordRequestAsync(SetRecord1Function setRecord1Function)
        {
             return ContractHandler.SendRequestAsync(setRecord1Function);
        }

        public Task<TransactionReceipt> SetRecordRequestAndWaitForReceiptAsync(SetRecord1Function setRecord1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRecord1Function, cancellationToken);
        }

        public Task<string> SetRecordRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte[] data)
        {
            var setRecord1Function = new SetRecord1Function();
                setRecord1Function.Namespace = @namespace;
                setRecord1Function.Name = name;
                setRecord1Function.Key = key;
                setRecord1Function.Data = data;
            
             return ContractHandler.SendRequestAsync(setRecord1Function);
        }

        public Task<TransactionReceipt> SetRecordRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var setRecord1Function = new SetRecord1Function();
                setRecord1Function.Namespace = @namespace;
                setRecord1Function.Name = name;
                setRecord1Function.Key = key;
                setRecord1Function.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRecord1Function, cancellationToken);
        }

        public Task<string> SetRecordRequestAsync(SetRecordFunction setRecordFunction)
        {
             return ContractHandler.SendRequestAsync(setRecordFunction);
        }

        public Task<TransactionReceipt> SetRecordRequestAndWaitForReceiptAsync(SetRecordFunction setRecordFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRecordFunction, cancellationToken);
        }

        public Task<string> SetRecordRequestAsync(byte[] table, List<byte[]> key, byte[] data)
        {
            var setRecordFunction = new SetRecordFunction();
                setRecordFunction.Table = table;
                setRecordFunction.Key = key;
                setRecordFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(setRecordFunction);
        }

        public Task<TransactionReceipt> SetRecordRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var setRecordFunction = new SetRecordFunction();
                setRecordFunction.Table = table;
                setRecordFunction.Key = key;
                setRecordFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRecordFunction, cancellationToken);
        }

        public Task<string> ShovelRequestAsync(ShovelFunction shovelFunction)
        {
             return ContractHandler.SendRequestAsync(shovelFunction);
        }

        public Task<TransactionReceipt> ShovelRequestAndWaitForReceiptAsync(ShovelFunction shovelFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(shovelFunction, cancellationToken);
        }

        public Task<string> ShovelRequestAsync(byte[] player, int x, int y)
        {
            var shovelFunction = new ShovelFunction();
                shovelFunction.Player = player;
                shovelFunction.X = x;
                shovelFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(shovelFunction);
        }

        public Task<TransactionReceipt> ShovelRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var shovelFunction = new ShovelFunction();
                shovelFunction.Player = player;
                shovelFunction.X = x;
                shovelFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(shovelFunction, cancellationToken);
        }

        public Task<string> SpawnRequestAsync(SpawnFunction spawnFunction)
        {
             return ContractHandler.SendRequestAsync(spawnFunction);
        }

        public Task<TransactionReceipt> SpawnRequestAndWaitForReceiptAsync(SpawnFunction spawnFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFunction, cancellationToken);
        }

        public Task<string> SpawnRequestAsync(int x, int y)
        {
            var spawnFunction = new SpawnFunction();
                spawnFunction.X = x;
                spawnFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnFunction);
        }

        public Task<TransactionReceipt> SpawnRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnFunction = new SpawnFunction();
                spawnFunction.X = x;
                spawnFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFunction, cancellationToken);
        }

        public Task<string> SpawnDebugRoadRequestAsync(SpawnDebugRoadFunction spawnDebugRoadFunction)
        {
             return ContractHandler.SendRequestAsync(spawnDebugRoadFunction);
        }

        public Task<TransactionReceipt> SpawnDebugRoadRequestAndWaitForReceiptAsync(SpawnDebugRoadFunction spawnDebugRoadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnDebugRoadFunction, cancellationToken);
        }

        public Task<string> SpawnDebugRoadRequestAsync(byte[] credit, int x, int y)
        {
            var spawnDebugRoadFunction = new SpawnDebugRoadFunction();
                spawnDebugRoadFunction.Credit = credit;
                spawnDebugRoadFunction.X = x;
                spawnDebugRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnDebugRoadFunction);
        }

        public Task<TransactionReceipt> SpawnDebugRoadRequestAndWaitForReceiptAsync(byte[] credit, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnDebugRoadFunction = new SpawnDebugRoadFunction();
                spawnDebugRoadFunction.Credit = credit;
                spawnDebugRoadFunction.X = x;
                spawnDebugRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnDebugRoadFunction, cancellationToken);
        }

        public Task<string> SpawnFinishedRoadAdminRequestAsync(SpawnFinishedRoadAdminFunction spawnFinishedRoadAdminFunction)
        {
             return ContractHandler.SendRequestAsync(spawnFinishedRoadAdminFunction);
        }

        public Task<TransactionReceipt> SpawnFinishedRoadAdminRequestAndWaitForReceiptAsync(SpawnFinishedRoadAdminFunction spawnFinishedRoadAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFinishedRoadAdminFunction, cancellationToken);
        }

        public Task<string> SpawnFinishedRoadAdminRequestAsync(int x, int y)
        {
            var spawnFinishedRoadAdminFunction = new SpawnFinishedRoadAdminFunction();
                spawnFinishedRoadAdminFunction.X = x;
                spawnFinishedRoadAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnFinishedRoadAdminFunction);
        }

        public Task<TransactionReceipt> SpawnFinishedRoadAdminRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnFinishedRoadAdminFunction = new SpawnFinishedRoadAdminFunction();
                spawnFinishedRoadAdminFunction.X = x;
                spawnFinishedRoadAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFinishedRoadAdminFunction, cancellationToken);
        }

        public Task<string> SpawnFloraRequestAsync(SpawnFloraFunction spawnFloraFunction)
        {
             return ContractHandler.SendRequestAsync(spawnFloraFunction);
        }

        public Task<TransactionReceipt> SpawnFloraRequestAndWaitForReceiptAsync(SpawnFloraFunction spawnFloraFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFloraFunction, cancellationToken);
        }

        public Task<string> SpawnFloraRequestAsync(byte[] player, byte[] entity, int x, int y)
        {
            var spawnFloraFunction = new SpawnFloraFunction();
                spawnFloraFunction.Player = player;
                spawnFloraFunction.Entity = entity;
                spawnFloraFunction.X = x;
                spawnFloraFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnFloraFunction);
        }

        public Task<TransactionReceipt> SpawnFloraRequestAndWaitForReceiptAsync(byte[] player, byte[] entity, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnFloraFunction = new SpawnFloraFunction();
                spawnFloraFunction.Player = player;
                spawnFloraFunction.Entity = entity;
                spawnFloraFunction.X = x;
                spawnFloraFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFloraFunction, cancellationToken);
        }

        public Task<string> SpawnMileAdminRequestAsync(SpawnMileAdminFunction spawnMileAdminFunction)
        {
             return ContractHandler.SendRequestAsync(spawnMileAdminFunction);
        }

        public Task<string> SpawnMileAdminRequestAsync()
        {
             return ContractHandler.SendRequestAsync<SpawnMileAdminFunction>();
        }

        public Task<TransactionReceipt> SpawnMileAdminRequestAndWaitForReceiptAsync(SpawnMileAdminFunction spawnMileAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnMileAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> SpawnMileAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<SpawnMileAdminFunction>(null, cancellationToken);
        }

        public Task<string> SpawnNPCRequestAsync(SpawnNPCFunction spawnNPCFunction)
        {
             return ContractHandler.SendRequestAsync(spawnNPCFunction);
        }

        public Task<TransactionReceipt> SpawnNPCRequestAndWaitForReceiptAsync(SpawnNPCFunction spawnNPCFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnNPCFunction, cancellationToken);
        }

        public Task<string> SpawnNPCRequestAsync(byte[] spawner, int x, int y, byte npcType)
        {
            var spawnNPCFunction = new SpawnNPCFunction();
                spawnNPCFunction.Spawner = spawner;
                spawnNPCFunction.X = x;
                spawnNPCFunction.Y = y;
                spawnNPCFunction.NpcType = npcType;
            
             return ContractHandler.SendRequestAsync(spawnNPCFunction);
        }

        public Task<TransactionReceipt> SpawnNPCRequestAndWaitForReceiptAsync(byte[] spawner, int x, int y, byte npcType, CancellationTokenSource cancellationToken = null)
        {
            var spawnNPCFunction = new SpawnNPCFunction();
                spawnNPCFunction.Spawner = spawner;
                spawnNPCFunction.X = x;
                spawnNPCFunction.Y = y;
                spawnNPCFunction.NpcType = npcType;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnNPCFunction, cancellationToken);
        }

        public Task<string> SpawnNPCAdminRequestAsync(SpawnNPCAdminFunction spawnNPCAdminFunction)
        {
             return ContractHandler.SendRequestAsync(spawnNPCAdminFunction);
        }

        public Task<TransactionReceipt> SpawnNPCAdminRequestAndWaitForReceiptAsync(SpawnNPCAdminFunction spawnNPCAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnNPCAdminFunction, cancellationToken);
        }

        public Task<string> SpawnNPCAdminRequestAsync(int x, int y, byte npcType)
        {
            var spawnNPCAdminFunction = new SpawnNPCAdminFunction();
                spawnNPCAdminFunction.X = x;
                spawnNPCAdminFunction.Y = y;
                spawnNPCAdminFunction.NpcType = npcType;
            
             return ContractHandler.SendRequestAsync(spawnNPCAdminFunction);
        }

        public Task<TransactionReceipt> SpawnNPCAdminRequestAndWaitForReceiptAsync(int x, int y, byte npcType, CancellationTokenSource cancellationToken = null)
        {
            var spawnNPCAdminFunction = new SpawnNPCAdminFunction();
                spawnNPCAdminFunction.X = x;
                spawnNPCAdminFunction.Y = y;
                spawnNPCAdminFunction.NpcType = npcType;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnNPCAdminFunction, cancellationToken);
        }

        public Task<string> SpawnPlayerRequestAsync(SpawnPlayerFunction spawnPlayerFunction)
        {
             return ContractHandler.SendRequestAsync(spawnPlayerFunction);
        }

        public Task<TransactionReceipt> SpawnPlayerRequestAndWaitForReceiptAsync(SpawnPlayerFunction spawnPlayerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnPlayerFunction, cancellationToken);
        }

        public Task<string> SpawnPlayerRequestAsync(byte[] entity, int x, int y, bool isBot)
        {
            var spawnPlayerFunction = new SpawnPlayerFunction();
                spawnPlayerFunction.Entity = entity;
                spawnPlayerFunction.X = x;
                spawnPlayerFunction.Y = y;
                spawnPlayerFunction.IsBot = isBot;
            
             return ContractHandler.SendRequestAsync(spawnPlayerFunction);
        }

        public Task<TransactionReceipt> SpawnPlayerRequestAndWaitForReceiptAsync(byte[] entity, int x, int y, bool isBot, CancellationTokenSource cancellationToken = null)
        {
            var spawnPlayerFunction = new SpawnPlayerFunction();
                spawnPlayerFunction.Entity = entity;
                spawnPlayerFunction.X = x;
                spawnPlayerFunction.Y = y;
                spawnPlayerFunction.IsBot = isBot;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnPlayerFunction, cancellationToken);
        }

        public Task<string> SpawnPlayerNPCRequestAsync(SpawnPlayerNPCFunction spawnPlayerNPCFunction)
        {
             return ContractHandler.SendRequestAsync(spawnPlayerNPCFunction);
        }

        public Task<TransactionReceipt> SpawnPlayerNPCRequestAndWaitForReceiptAsync(SpawnPlayerNPCFunction spawnPlayerNPCFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnPlayerNPCFunction, cancellationToken);
        }

        public Task<string> SpawnPlayerNPCRequestAsync(byte[] entity, int x, int y)
        {
            var spawnPlayerNPCFunction = new SpawnPlayerNPCFunction();
                spawnPlayerNPCFunction.Entity = entity;
                spawnPlayerNPCFunction.X = x;
                spawnPlayerNPCFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnPlayerNPCFunction);
        }

        public Task<TransactionReceipt> SpawnPlayerNPCRequestAndWaitForReceiptAsync(byte[] entity, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnPlayerNPCFunction = new SpawnPlayerNPCFunction();
                spawnPlayerNPCFunction.Entity = entity;
                spawnPlayerNPCFunction.X = x;
                spawnPlayerNPCFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnPlayerNPCFunction, cancellationToken);
        }

        public Task<string> SpawnPuzzleAdminRequestAsync(SpawnPuzzleAdminFunction spawnPuzzleAdminFunction)
        {
             return ContractHandler.SendRequestAsync(spawnPuzzleAdminFunction);
        }

        public Task<string> SpawnPuzzleAdminRequestAsync()
        {
             return ContractHandler.SendRequestAsync<SpawnPuzzleAdminFunction>();
        }

        public Task<TransactionReceipt> SpawnPuzzleAdminRequestAndWaitForReceiptAsync(SpawnPuzzleAdminFunction spawnPuzzleAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnPuzzleAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> SpawnPuzzleAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<SpawnPuzzleAdminFunction>(null, cancellationToken);
        }

        public Task<string> SpawnRoadRequestAsync(SpawnRoadFunction spawnRoadFunction)
        {
             return ContractHandler.SendRequestAsync(spawnRoadFunction);
        }

        public Task<TransactionReceipt> SpawnRoadRequestAndWaitForReceiptAsync(SpawnRoadFunction spawnRoadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnRoadFunction, cancellationToken);
        }

        public Task<string> SpawnRoadRequestAsync(byte[] player, int x, int y, byte state)
        {
            var spawnRoadFunction = new SpawnRoadFunction();
                spawnRoadFunction.Player = player;
                spawnRoadFunction.X = x;
                spawnRoadFunction.Y = y;
                spawnRoadFunction.State = state;
            
             return ContractHandler.SendRequestAsync(spawnRoadFunction);
        }

        public Task<TransactionReceipt> SpawnRoadRequestAndWaitForReceiptAsync(byte[] player, int x, int y, byte state, CancellationTokenSource cancellationToken = null)
        {
            var spawnRoadFunction = new SpawnRoadFunction();
                spawnRoadFunction.Player = player;
                spawnRoadFunction.X = x;
                spawnRoadFunction.Y = y;
                spawnRoadFunction.State = state;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnRoadFunction, cancellationToken);
        }

        public Task<string> SpawnRoadFromPlayerRequestAsync(SpawnRoadFromPlayerFunction spawnRoadFromPlayerFunction)
        {
             return ContractHandler.SendRequestAsync(spawnRoadFromPlayerFunction);
        }

        public Task<TransactionReceipt> SpawnRoadFromPlayerRequestAndWaitForReceiptAsync(SpawnRoadFromPlayerFunction spawnRoadFromPlayerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnRoadFromPlayerFunction, cancellationToken);
        }

        public Task<string> SpawnRoadFromPlayerRequestAsync(byte[] player, byte[] pushed, byte[] road, PositionData pos)
        {
            var spawnRoadFromPlayerFunction = new SpawnRoadFromPlayerFunction();
                spawnRoadFromPlayerFunction.Player = player;
                spawnRoadFromPlayerFunction.Pushed = pushed;
                spawnRoadFromPlayerFunction.Road = road;
                spawnRoadFromPlayerFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(spawnRoadFromPlayerFunction);
        }

        public Task<TransactionReceipt> SpawnRoadFromPlayerRequestAndWaitForReceiptAsync(byte[] player, byte[] pushed, byte[] road, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var spawnRoadFromPlayerFunction = new SpawnRoadFromPlayerFunction();
                spawnRoadFromPlayerFunction.Player = player;
                spawnRoadFromPlayerFunction.Pushed = pushed;
                spawnRoadFromPlayerFunction.Road = road;
                spawnRoadFromPlayerFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnRoadFromPlayerFunction, cancellationToken);
        }

        public Task<string> SpawnShoveledRoadRequestAsync(SpawnShoveledRoadFunction spawnShoveledRoadFunction)
        {
             return ContractHandler.SendRequestAsync(spawnShoveledRoadFunction);
        }

        public Task<TransactionReceipt> SpawnShoveledRoadRequestAndWaitForReceiptAsync(SpawnShoveledRoadFunction spawnShoveledRoadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnShoveledRoadFunction, cancellationToken);
        }

        public Task<string> SpawnShoveledRoadRequestAsync(byte[] player, int x, int y)
        {
            var spawnShoveledRoadFunction = new SpawnShoveledRoadFunction();
                spawnShoveledRoadFunction.Player = player;
                spawnShoveledRoadFunction.X = x;
                spawnShoveledRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnShoveledRoadFunction);
        }

        public Task<TransactionReceipt> SpawnShoveledRoadRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnShoveledRoadFunction = new SpawnShoveledRoadFunction();
                spawnShoveledRoadFunction.Player = player;
                spawnShoveledRoadFunction.X = x;
                spawnShoveledRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnShoveledRoadFunction, cancellationToken);
        }

        public Task<string> SpawnShoveledRoadAdminRequestAsync(SpawnShoveledRoadAdminFunction spawnShoveledRoadAdminFunction)
        {
             return ContractHandler.SendRequestAsync(spawnShoveledRoadAdminFunction);
        }

        public Task<TransactionReceipt> SpawnShoveledRoadAdminRequestAndWaitForReceiptAsync(SpawnShoveledRoadAdminFunction spawnShoveledRoadAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnShoveledRoadAdminFunction, cancellationToken);
        }

        public Task<string> SpawnShoveledRoadAdminRequestAsync(int x, int y)
        {
            var spawnShoveledRoadAdminFunction = new SpawnShoveledRoadAdminFunction();
                spawnShoveledRoadAdminFunction.X = x;
                spawnShoveledRoadAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnShoveledRoadAdminFunction);
        }

        public Task<TransactionReceipt> SpawnShoveledRoadAdminRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnShoveledRoadAdminFunction = new SpawnShoveledRoadAdminFunction();
                spawnShoveledRoadAdminFunction.X = x;
                spawnShoveledRoadAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnShoveledRoadAdminFunction, cancellationToken);
        }

        public Task<string> SpawnTerrainRequestAsync(SpawnTerrainFunction spawnTerrainFunction)
        {
             return ContractHandler.SendRequestAsync(spawnTerrainFunction);
        }

        public Task<TransactionReceipt> SpawnTerrainRequestAndWaitForReceiptAsync(SpawnTerrainFunction spawnTerrainFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnTerrainFunction, cancellationToken);
        }

        public Task<string> SpawnTerrainRequestAsync(byte[] player, int x, int y, byte tType)
        {
            var spawnTerrainFunction = new SpawnTerrainFunction();
                spawnTerrainFunction.Player = player;
                spawnTerrainFunction.X = x;
                spawnTerrainFunction.Y = y;
                spawnTerrainFunction.TType = tType;
            
             return ContractHandler.SendRequestAsync(spawnTerrainFunction);
        }

        public Task<TransactionReceipt> SpawnTerrainRequestAndWaitForReceiptAsync(byte[] player, int x, int y, byte tType, CancellationTokenSource cancellationToken = null)
        {
            var spawnTerrainFunction = new SpawnTerrainFunction();
                spawnTerrainFunction.Player = player;
                spawnTerrainFunction.X = x;
                spawnTerrainFunction.Y = y;
                spawnTerrainFunction.TType = tType;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnTerrainFunction, cancellationToken);
        }

        public Task<string> SpawnTerrainAdminRequestAsync(SpawnTerrainAdminFunction spawnTerrainAdminFunction)
        {
             return ContractHandler.SendRequestAsync(spawnTerrainAdminFunction);
        }

        public Task<TransactionReceipt> SpawnTerrainAdminRequestAndWaitForReceiptAsync(SpawnTerrainAdminFunction spawnTerrainAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnTerrainAdminFunction, cancellationToken);
        }

        public Task<string> SpawnTerrainAdminRequestAsync(int x, int y, byte terrainType)
        {
            var spawnTerrainAdminFunction = new SpawnTerrainAdminFunction();
                spawnTerrainAdminFunction.X = x;
                spawnTerrainAdminFunction.Y = y;
                spawnTerrainAdminFunction.TerrainType = terrainType;
            
             return ContractHandler.SendRequestAsync(spawnTerrainAdminFunction);
        }

        public Task<TransactionReceipt> SpawnTerrainAdminRequestAndWaitForReceiptAsync(int x, int y, byte terrainType, CancellationTokenSource cancellationToken = null)
        {
            var spawnTerrainAdminFunction = new SpawnTerrainAdminFunction();
                spawnTerrainAdminFunction.X = x;
                spawnTerrainAdminFunction.Y = y;
                spawnTerrainAdminFunction.TerrainType = terrainType;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnTerrainAdminFunction, cancellationToken);
        }

        public Task<string> StickRequestAsync(StickFunction stickFunction)
        {
             return ContractHandler.SendRequestAsync(stickFunction);
        }

        public Task<TransactionReceipt> StickRequestAndWaitForReceiptAsync(StickFunction stickFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stickFunction, cancellationToken);
        }

        public Task<string> StickRequestAsync(byte[] player, int x, int y)
        {
            var stickFunction = new StickFunction();
                stickFunction.Player = player;
                stickFunction.X = x;
                stickFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(stickFunction);
        }

        public Task<TransactionReceipt> StickRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var stickFunction = new StickFunction();
                stickFunction.Player = player;
                stickFunction.X = x;
                stickFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stickFunction, cancellationToken);
        }

        public Task<string> SummonMapRequestAsync(SummonMapFunction summonMapFunction)
        {
             return ContractHandler.SendRequestAsync(summonMapFunction);
        }

        public Task<string> SummonMapRequestAsync()
        {
             return ContractHandler.SendRequestAsync<SummonMapFunction>();
        }

        public Task<TransactionReceipt> SummonMapRequestAndWaitForReceiptAsync(SummonMapFunction summonMapFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonMapFunction, cancellationToken);
        }

        public Task<TransactionReceipt> SummonMapRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<SummonMapFunction>(null, cancellationToken);
        }

        public Task<string> SummonMileRequestAsync(SummonMileFunction summonMileFunction)
        {
             return ContractHandler.SendRequestAsync(summonMileFunction);
        }

        public Task<TransactionReceipt> SummonMileRequestAndWaitForReceiptAsync(SummonMileFunction summonMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonMileFunction, cancellationToken);
        }

        public Task<string> SummonMileRequestAsync(byte[] causedBy)
        {
            var summonMileFunction = new SummonMileFunction();
                summonMileFunction.CausedBy = causedBy;
            
             return ContractHandler.SendRequestAsync(summonMileFunction);
        }

        public Task<TransactionReceipt> SummonMileRequestAndWaitForReceiptAsync(byte[] causedBy, CancellationTokenSource cancellationToken = null)
        {
            var summonMileFunction = new SummonMileFunction();
                summonMileFunction.CausedBy = causedBy;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonMileFunction, cancellationToken);
        }

        public Task<string> TeleportRequestAsync(TeleportFunction teleportFunction)
        {
             return ContractHandler.SendRequestAsync(teleportFunction);
        }

        public Task<TransactionReceipt> TeleportRequestAndWaitForReceiptAsync(TeleportFunction teleportFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportFunction, cancellationToken);
        }

        public Task<string> TeleportRequestAsync(byte[] player, int x, int y)
        {
            var teleportFunction = new TeleportFunction();
                teleportFunction.Player = player;
                teleportFunction.X = x;
                teleportFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(teleportFunction);
        }

        public Task<TransactionReceipt> TeleportRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var teleportFunction = new TeleportFunction();
                teleportFunction.Player = player;
                teleportFunction.X = x;
                teleportFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportFunction, cancellationToken);
        }

        public Task<string> TeleportAdminRequestAsync(TeleportAdminFunction teleportAdminFunction)
        {
             return ContractHandler.SendRequestAsync(teleportAdminFunction);
        }

        public Task<TransactionReceipt> TeleportAdminRequestAndWaitForReceiptAsync(TeleportAdminFunction teleportAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportAdminFunction, cancellationToken);
        }

        public Task<string> TeleportAdminRequestAsync(int x, int y)
        {
            var teleportAdminFunction = new TeleportAdminFunction();
                teleportAdminFunction.X = x;
                teleportAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(teleportAdminFunction);
        }

        public Task<TransactionReceipt> TeleportAdminRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var teleportAdminFunction = new TeleportAdminFunction();
                teleportAdminFunction.X = x;
                teleportAdminFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportAdminFunction, cancellationToken);
        }

        public Task<string> TeleportScrollRequestAsync(TeleportScrollFunction teleportScrollFunction)
        {
             return ContractHandler.SendRequestAsync(teleportScrollFunction);
        }

        public Task<TransactionReceipt> TeleportScrollRequestAndWaitForReceiptAsync(TeleportScrollFunction teleportScrollFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportScrollFunction, cancellationToken);
        }

        public Task<string> TeleportScrollRequestAsync(byte[] player, int x, int y)
        {
            var teleportScrollFunction = new TeleportScrollFunction();
                teleportScrollFunction.Player = player;
                teleportScrollFunction.X = x;
                teleportScrollFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(teleportScrollFunction);
        }

        public Task<TransactionReceipt> TeleportScrollRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var teleportScrollFunction = new TeleportScrollFunction();
                teleportScrollFunction.Player = player;
                teleportScrollFunction.X = x;
                teleportScrollFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportScrollFunction, cancellationToken);
        }

        public Task<string> TickBehaviourRequestAsync(TickBehaviourFunction tickBehaviourFunction)
        {
             return ContractHandler.SendRequestAsync(tickBehaviourFunction);
        }

        public Task<TransactionReceipt> TickBehaviourRequestAndWaitForReceiptAsync(TickBehaviourFunction tickBehaviourFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickBehaviourFunction, cancellationToken);
        }

        public Task<string> TickBehaviourRequestAsync(byte[] causedBy, byte[] player, byte[] entity, PositionData playerPos, PositionData entityPos)
        {
            var tickBehaviourFunction = new TickBehaviourFunction();
                tickBehaviourFunction.CausedBy = causedBy;
                tickBehaviourFunction.Player = player;
                tickBehaviourFunction.Entity = entity;
                tickBehaviourFunction.PlayerPos = playerPos;
                tickBehaviourFunction.EntityPos = entityPos;
            
             return ContractHandler.SendRequestAsync(tickBehaviourFunction);
        }

        public Task<TransactionReceipt> TickBehaviourRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] player, byte[] entity, PositionData playerPos, PositionData entityPos, CancellationTokenSource cancellationToken = null)
        {
            var tickBehaviourFunction = new TickBehaviourFunction();
                tickBehaviourFunction.CausedBy = causedBy;
                tickBehaviourFunction.Player = player;
                tickBehaviourFunction.Entity = entity;
                tickBehaviourFunction.PlayerPos = playerPos;
                tickBehaviourFunction.EntityPos = entityPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickBehaviourFunction, cancellationToken);
        }

        public Task<string> TriggerEntitiesRequestAsync(TriggerEntitiesFunction triggerEntitiesFunction)
        {
             return ContractHandler.SendRequestAsync(triggerEntitiesFunction);
        }

        public Task<TransactionReceipt> TriggerEntitiesRequestAndWaitForReceiptAsync(TriggerEntitiesFunction triggerEntitiesFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(triggerEntitiesFunction, cancellationToken);
        }

        public Task<string> TriggerEntitiesRequestAsync(byte[] causedBy, byte[] player, PositionData pos)
        {
            var triggerEntitiesFunction = new TriggerEntitiesFunction();
                triggerEntitiesFunction.CausedBy = causedBy;
                triggerEntitiesFunction.Player = player;
                triggerEntitiesFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(triggerEntitiesFunction);
        }

        public Task<TransactionReceipt> TriggerEntitiesRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] player, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var triggerEntitiesFunction = new TriggerEntitiesFunction();
                triggerEntitiesFunction.CausedBy = causedBy;
                triggerEntitiesFunction.Player = player;
                triggerEntitiesFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(triggerEntitiesFunction, cancellationToken);
        }

        public Task<string> TriggerPuzzlesRequestAsync(TriggerPuzzlesFunction triggerPuzzlesFunction)
        {
             return ContractHandler.SendRequestAsync(triggerPuzzlesFunction);
        }

        public Task<TransactionReceipt> TriggerPuzzlesRequestAndWaitForReceiptAsync(TriggerPuzzlesFunction triggerPuzzlesFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(triggerPuzzlesFunction, cancellationToken);
        }

        public Task<string> TriggerPuzzlesRequestAsync(byte[] causedBy, byte[] entity, PositionData pos)
        {
            var triggerPuzzlesFunction = new TriggerPuzzlesFunction();
                triggerPuzzlesFunction.CausedBy = causedBy;
                triggerPuzzlesFunction.Entity = entity;
                triggerPuzzlesFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(triggerPuzzlesFunction);
        }

        public Task<TransactionReceipt> TriggerPuzzlesRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var triggerPuzzlesFunction = new TriggerPuzzlesFunction();
                triggerPuzzlesFunction.CausedBy = causedBy;
                triggerPuzzlesFunction.Entity = entity;
                triggerPuzzlesFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(triggerPuzzlesFunction, cancellationToken);
        }

        public Task<string> UpdateChunkRequestAsync(UpdateChunkFunction updateChunkFunction)
        {
             return ContractHandler.SendRequestAsync(updateChunkFunction);
        }

        public Task<string> UpdateChunkRequestAsync()
        {
             return ContractHandler.SendRequestAsync<UpdateChunkFunction>();
        }

        public Task<TransactionReceipt> UpdateChunkRequestAndWaitForReceiptAsync(UpdateChunkFunction updateChunkFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateChunkFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UpdateChunkRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<UpdateChunkFunction>(null, cancellationToken);
        }

        public Task<string> UpdateInFieldRequestAsync(UpdateInFieldFunction updateInFieldFunction)
        {
             return ContractHandler.SendRequestAsync(updateInFieldFunction);
        }

        public Task<TransactionReceipt> UpdateInFieldRequestAndWaitForReceiptAsync(UpdateInFieldFunction updateInFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateInFieldFunction, cancellationToken);
        }

        public Task<string> UpdateInFieldRequestAsync(byte[] table, List<byte[]> key, byte schemaIndex, BigInteger startByteIndex, byte[] dataToSet)
        {
            var updateInFieldFunction = new UpdateInFieldFunction();
                updateInFieldFunction.Table = table;
                updateInFieldFunction.Key = key;
                updateInFieldFunction.SchemaIndex = schemaIndex;
                updateInFieldFunction.StartByteIndex = startByteIndex;
                updateInFieldFunction.DataToSet = dataToSet;
            
             return ContractHandler.SendRequestAsync(updateInFieldFunction);
        }

        public Task<TransactionReceipt> UpdateInFieldRequestAndWaitForReceiptAsync(byte[] table, List<byte[]> key, byte schemaIndex, BigInteger startByteIndex, byte[] dataToSet, CancellationTokenSource cancellationToken = null)
        {
            var updateInFieldFunction = new UpdateInFieldFunction();
                updateInFieldFunction.Table = table;
                updateInFieldFunction.Key = key;
                updateInFieldFunction.SchemaIndex = schemaIndex;
                updateInFieldFunction.StartByteIndex = startByteIndex;
                updateInFieldFunction.DataToSet = dataToSet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateInFieldFunction, cancellationToken);
        }

        public Task<string> UpdateInFieldRequestAsync(UpdateInField1Function updateInField1Function)
        {
             return ContractHandler.SendRequestAsync(updateInField1Function);
        }

        public Task<TransactionReceipt> UpdateInFieldRequestAndWaitForReceiptAsync(UpdateInField1Function updateInField1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateInField1Function, cancellationToken);
        }

        public Task<string> UpdateInFieldRequestAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, BigInteger startByteIndex, byte[] dataToSet)
        {
            var updateInField1Function = new UpdateInField1Function();
                updateInField1Function.Namespace = @namespace;
                updateInField1Function.Name = name;
                updateInField1Function.Key = key;
                updateInField1Function.SchemaIndex = schemaIndex;
                updateInField1Function.StartByteIndex = startByteIndex;
                updateInField1Function.DataToSet = dataToSet;
            
             return ContractHandler.SendRequestAsync(updateInField1Function);
        }

        public Task<TransactionReceipt> UpdateInFieldRequestAndWaitForReceiptAsync(byte[] @namespace, byte[] name, List<byte[]> key, byte schemaIndex, BigInteger startByteIndex, byte[] dataToSet, CancellationTokenSource cancellationToken = null)
        {
            var updateInField1Function = new UpdateInField1Function();
                updateInField1Function.Namespace = @namespace;
                updateInField1Function.Name = name;
                updateInField1Function.Key = key;
                updateInField1Function.SchemaIndex = schemaIndex;
                updateInField1Function.StartByteIndex = startByteIndex;
                updateInField1Function.DataToSet = dataToSet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateInField1Function, cancellationToken);
        }

        public Task<string> WaterRequestAsync(WaterFunction waterFunction)
        {
             return ContractHandler.SendRequestAsync(waterFunction);
        }

        public Task<TransactionReceipt> WaterRequestAndWaitForReceiptAsync(WaterFunction waterFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(waterFunction, cancellationToken);
        }

        public Task<string> WaterRequestAsync(int x, int y)
        {
            var waterFunction = new WaterFunction();
                waterFunction.X = x;
                waterFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(waterFunction);
        }

        public Task<TransactionReceipt> WaterRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var waterFunction = new WaterFunction();
                waterFunction.X = x;
                waterFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(waterFunction, cancellationToken);
        }
    }
}

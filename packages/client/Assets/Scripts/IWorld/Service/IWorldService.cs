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

        public Task<string> AddGemXPRequestAsync(int amount)
        {
            var addGemXPFunction = new AddGemXPFunction();
                addGemXPFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(addGemXPFunction);
        }

        public Task<TransactionReceipt> AddGemXPRequestAndWaitForReceiptAsync(int amount, CancellationTokenSource cancellationToken = null)
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

        public Task<string> BatchCallRequestAsync(BatchCallFunction batchCallFunction)
        {
             return ContractHandler.SendRequestAsync(batchCallFunction);
        }

        public Task<TransactionReceipt> BatchCallRequestAndWaitForReceiptAsync(BatchCallFunction batchCallFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchCallFunction, cancellationToken);
        }

        public Task<string> BatchCallRequestAsync(List<SystemCallData> systemCalls)
        {
            var batchCallFunction = new BatchCallFunction();
                batchCallFunction.SystemCalls = systemCalls;
            
             return ContractHandler.SendRequestAsync(batchCallFunction);
        }

        public Task<TransactionReceipt> BatchCallRequestAndWaitForReceiptAsync(List<SystemCallData> systemCalls, CancellationTokenSource cancellationToken = null)
        {
            var batchCallFunction = new BatchCallFunction();
                batchCallFunction.SystemCalls = systemCalls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchCallFunction, cancellationToken);
        }

        public Task<string> BatchCallFromRequestAsync(BatchCallFromFunction batchCallFromFunction)
        {
             return ContractHandler.SendRequestAsync(batchCallFromFunction);
        }

        public Task<TransactionReceipt> BatchCallFromRequestAndWaitForReceiptAsync(BatchCallFromFunction batchCallFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchCallFromFunction, cancellationToken);
        }

        public Task<string> BatchCallFromRequestAsync(List<SystemCallFromData> systemCalls)
        {
            var batchCallFromFunction = new BatchCallFromFunction();
                batchCallFromFunction.SystemCalls = systemCalls;
            
             return ContractHandler.SendRequestAsync(batchCallFromFunction);
        }

        public Task<TransactionReceipt> BatchCallFromRequestAndWaitForReceiptAsync(List<SystemCallFromData> systemCalls, CancellationTokenSource cancellationToken = null)
        {
            var batchCallFromFunction = new BatchCallFromFunction();
                batchCallFromFunction.SystemCalls = systemCalls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchCallFromFunction, cancellationToken);
        }

        public Task<string> BuyRequestAsync(BuyFunction buyFunction)
        {
             return ContractHandler.SendRequestAsync(buyFunction);
        }

        public Task<TransactionReceipt> BuyRequestAndWaitForReceiptAsync(BuyFunction buyFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyFunction, cancellationToken);
        }

        public Task<string> BuyRequestAsync(uint id, byte payment)
        {
            var buyFunction = new BuyFunction();
                buyFunction.Id = id;
                buyFunction.Payment = payment;
            
             return ContractHandler.SendRequestAsync(buyFunction);
        }

        public Task<TransactionReceipt> BuyRequestAndWaitForReceiptAsync(uint id, byte payment, CancellationTokenSource cancellationToken = null)
        {
            var buyFunction = new BuyFunction();
                buyFunction.Id = id;
                buyFunction.Payment = payment;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyFunction, cancellationToken);
        }

        public Task<string> BuyCosmeticRequestAsync(BuyCosmeticFunction buyCosmeticFunction)
        {
             return ContractHandler.SendRequestAsync(buyCosmeticFunction);
        }

        public Task<TransactionReceipt> BuyCosmeticRequestAndWaitForReceiptAsync(BuyCosmeticFunction buyCosmeticFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCosmeticFunction, cancellationToken);
        }

        public Task<string> BuyCosmeticRequestAsync(byte[] player, byte cosmetic, BigInteger index)
        {
            var buyCosmeticFunction = new BuyCosmeticFunction();
                buyCosmeticFunction.Player = player;
                buyCosmeticFunction.Cosmetic = cosmetic;
                buyCosmeticFunction.Index = index;
            
             return ContractHandler.SendRequestAsync(buyCosmeticFunction);
        }

        public Task<TransactionReceipt> BuyCosmeticRequestAndWaitForReceiptAsync(byte[] player, byte cosmetic, BigInteger index, CancellationTokenSource cancellationToken = null)
        {
            var buyCosmeticFunction = new BuyCosmeticFunction();
                buyCosmeticFunction.Player = player;
                buyCosmeticFunction.Cosmetic = cosmetic;
                buyCosmeticFunction.Index = index;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCosmeticFunction, cancellationToken);
        }

        public Task<string> BuyItemRequestAsync(BuyItemFunction buyItemFunction)
        {
             return ContractHandler.SendRequestAsync(buyItemFunction);
        }

        public Task<TransactionReceipt> BuyItemRequestAndWaitForReceiptAsync(BuyItemFunction buyItemFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyItemFunction, cancellationToken);
        }

        public Task<string> BuyItemRequestAsync(byte[] sender, byte[] seller, uint id, byte payment)
        {
            var buyItemFunction = new BuyItemFunction();
                buyItemFunction.Sender = sender;
                buyItemFunction.Seller = seller;
                buyItemFunction.Id = id;
                buyItemFunction.Payment = payment;
            
             return ContractHandler.SendRequestAsync(buyItemFunction);
        }

        public Task<TransactionReceipt> BuyItemRequestAndWaitForReceiptAsync(byte[] sender, byte[] seller, uint id, byte payment, CancellationTokenSource cancellationToken = null)
        {
            var buyItemFunction = new BuyItemFunction();
                buyItemFunction.Sender = sender;
                buyItemFunction.Seller = seller;
                buyItemFunction.Id = id;
                buyItemFunction.Payment = payment;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyItemFunction, cancellationToken);
        }

        public Task<string> CallRequestAsync(CallFunction callFunction)
        {
             return ContractHandler.SendRequestAsync(callFunction);
        }

        public Task<TransactionReceipt> CallRequestAndWaitForReceiptAsync(CallFunction callFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFunction, cancellationToken);
        }

        public Task<string> CallRequestAsync(byte[] systemId, byte[] callData)
        {
            var callFunction = new CallFunction();
                callFunction.SystemId = systemId;
                callFunction.CallData = callData;
            
             return ContractHandler.SendRequestAsync(callFunction);
        }

        public Task<TransactionReceipt> CallRequestAndWaitForReceiptAsync(byte[] systemId, byte[] callData, CancellationTokenSource cancellationToken = null)
        {
            var callFunction = new CallFunction();
                callFunction.SystemId = systemId;
                callFunction.CallData = callData;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFunction, cancellationToken);
        }

        public Task<string> CallFlingRequestAsync(CallFlingFunction callFlingFunction)
        {
             return ContractHandler.SendRequestAsync(callFlingFunction);
        }

        public Task<TransactionReceipt> CallFlingRequestAndWaitForReceiptAsync(CallFlingFunction callFlingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFlingFunction, cancellationToken);
        }

        public Task<string> CallFlingRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var callFlingFunction = new CallFlingFunction();
                callFlingFunction.CausedBy = causedBy;
                callFlingFunction.Entity = entity;
                callFlingFunction.Target = target;
                callFlingFunction.EntityPos = entityPos;
                callFlingFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(callFlingFunction);
        }

        public Task<TransactionReceipt> CallFlingRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var callFlingFunction = new CallFlingFunction();
                callFlingFunction.CausedBy = causedBy;
                callFlingFunction.Entity = entity;
                callFlingFunction.Target = target;
                callFlingFunction.EntityPos = entityPos;
                callFlingFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFlingFunction, cancellationToken);
        }

        public Task<string> CallFromRequestAsync(CallFromFunction callFromFunction)
        {
             return ContractHandler.SendRequestAsync(callFromFunction);
        }

        public Task<TransactionReceipt> CallFromRequestAndWaitForReceiptAsync(CallFromFunction callFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFromFunction, cancellationToken);
        }

        public Task<string> CallFromRequestAsync(string delegator, byte[] systemId, byte[] callData)
        {
            var callFromFunction = new CallFromFunction();
                callFromFunction.Delegator = delegator;
                callFromFunction.SystemId = systemId;
                callFromFunction.CallData = callData;
            
             return ContractHandler.SendRequestAsync(callFromFunction);
        }

        public Task<TransactionReceipt> CallFromRequestAndWaitForReceiptAsync(string delegator, byte[] systemId, byte[] callData, CancellationTokenSource cancellationToken = null)
        {
            var callFromFunction = new CallFromFunction();
                callFromFunction.Delegator = delegator;
                callFromFunction.SystemId = systemId;
                callFromFunction.CallData = callData;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(callFromFunction, cancellationToken);
        }

        public Task<string> CanAggroEntityRequestAsync(CanAggroEntityFunction canAggroEntityFunction)
        {
             return ContractHandler.SendRequestAsync(canAggroEntityFunction);
        }

        public Task<TransactionReceipt> CanAggroEntityRequestAndWaitForReceiptAsync(CanAggroEntityFunction canAggroEntityFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canAggroEntityFunction, cancellationToken);
        }

        public Task<string> CanAggroEntityRequestAsync(byte[] attacker, byte[] target)
        {
            var canAggroEntityFunction = new CanAggroEntityFunction();
                canAggroEntityFunction.Attacker = attacker;
                canAggroEntityFunction.Target = target;
            
             return ContractHandler.SendRequestAsync(canAggroEntityFunction);
        }

        public Task<TransactionReceipt> CanAggroEntityRequestAndWaitForReceiptAsync(byte[] attacker, byte[] target, CancellationTokenSource cancellationToken = null)
        {
            var canAggroEntityFunction = new CanAggroEntityFunction();
                canAggroEntityFunction.Attacker = attacker;
                canAggroEntityFunction.Target = target;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canAggroEntityFunction, cancellationToken);
        }

        public Task<string> CanBuyRequestAsync(CanBuyFunction canBuyFunction)
        {
             return ContractHandler.SendRequestAsync(canBuyFunction);
        }

        public Task<TransactionReceipt> CanBuyRequestAndWaitForReceiptAsync(CanBuyFunction canBuyFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canBuyFunction, cancellationToken);
        }

        public Task<string> CanBuyRequestAsync(int price, int gems, int eth)
        {
            var canBuyFunction = new CanBuyFunction();
                canBuyFunction.Price = price;
                canBuyFunction.Gems = gems;
                canBuyFunction.Eth = eth;
            
             return ContractHandler.SendRequestAsync(canBuyFunction);
        }

        public Task<TransactionReceipt> CanBuyRequestAndWaitForReceiptAsync(int price, int gems, int eth, CancellationTokenSource cancellationToken = null)
        {
            var canBuyFunction = new CanBuyFunction();
                canBuyFunction.Price = price;
                canBuyFunction.Gems = gems;
                canBuyFunction.Eth = eth;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(canBuyFunction, cancellationToken);
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

        public Task<string> ContemplateMileRequestAsync(byte[] causedBy, int mileNumber)
        {
            var contemplateMileFunction = new ContemplateMileFunction();
                contemplateMileFunction.CausedBy = causedBy;
                contemplateMileFunction.MileNumber = mileNumber;
            
             return ContractHandler.SendRequestAsync(contemplateMileFunction);
        }

        public Task<TransactionReceipt> ContemplateMileRequestAndWaitForReceiptAsync(byte[] causedBy, int mileNumber, CancellationTokenSource cancellationToken = null)
        {
            var contemplateMileFunction = new ContemplateMileFunction();
                contemplateMileFunction.CausedBy = causedBy;
                contemplateMileFunction.MileNumber = mileNumber;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(contemplateMileFunction, cancellationToken);
        }

        public Task<string> CreateItemMappingRequestAsync(CreateItemMappingFunction createItemMappingFunction)
        {
             return ContractHandler.SendRequestAsync(createItemMappingFunction);
        }

        public Task<string> CreateItemMappingRequestAsync()
        {
             return ContractHandler.SendRequestAsync<CreateItemMappingFunction>();
        }

        public Task<TransactionReceipt> CreateItemMappingRequestAndWaitForReceiptAsync(CreateItemMappingFunction createItemMappingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createItemMappingFunction, cancellationToken);
        }

        public Task<TransactionReceipt> CreateItemMappingRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<CreateItemMappingFunction>(null, cancellationToken);
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

        public Task<string> CreateMiliariumRequestAsync(byte[] causedBy, int width, int up, int down)
        {
            var createMiliariumFunction = new CreateMiliariumFunction();
                createMiliariumFunction.CausedBy = causedBy;
                createMiliariumFunction.Width = width;
                createMiliariumFunction.Up = up;
                createMiliariumFunction.Down = down;
            
             return ContractHandler.SendRequestAsync(createMiliariumFunction);
        }

        public Task<TransactionReceipt> CreateMiliariumRequestAndWaitForReceiptAsync(byte[] causedBy, int width, int up, int down, CancellationTokenSource cancellationToken = null)
        {
            var createMiliariumFunction = new CreateMiliariumFunction();
                createMiliariumFunction.CausedBy = causedBy;
                createMiliariumFunction.Width = width;
                createMiliariumFunction.Up = up;
                createMiliariumFunction.Down = down;
            
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

        public Task<string> CreateStatuePuzzleRequestAsync(CreateStatuePuzzleFunction createStatuePuzzleFunction)
        {
             return ContractHandler.SendRequestAsync(createStatuePuzzleFunction);
        }

        public Task<TransactionReceipt> CreateStatuePuzzleRequestAndWaitForReceiptAsync(CreateStatuePuzzleFunction createStatuePuzzleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createStatuePuzzleFunction, cancellationToken);
        }

        public Task<string> CreateStatuePuzzleRequestAsync(byte[] causedBy, int width, int up, int down)
        {
            var createStatuePuzzleFunction = new CreateStatuePuzzleFunction();
                createStatuePuzzleFunction.CausedBy = causedBy;
                createStatuePuzzleFunction.Width = width;
                createStatuePuzzleFunction.Up = up;
                createStatuePuzzleFunction.Down = down;
            
             return ContractHandler.SendRequestAsync(createStatuePuzzleFunction);
        }

        public Task<TransactionReceipt> CreateStatuePuzzleRequestAndWaitForReceiptAsync(byte[] causedBy, int width, int up, int down, CancellationTokenSource cancellationToken = null)
        {
            var createStatuePuzzleFunction = new CreateStatuePuzzleFunction();
                createStatuePuzzleFunction.CausedBy = causedBy;
                createStatuePuzzleFunction.Width = width;
                createStatuePuzzleFunction.Up = up;
                createStatuePuzzleFunction.Down = down;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createStatuePuzzleFunction, cancellationToken);
        }

        public Task<string> CreateTickersRequestAsync(CreateTickersFunction createTickersFunction)
        {
             return ContractHandler.SendRequestAsync(createTickersFunction);
        }

        public Task<TransactionReceipt> CreateTickersRequestAndWaitForReceiptAsync(CreateTickersFunction createTickersFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTickersFunction, cancellationToken);
        }

        public Task<string> CreateTickersRequestAsync(byte[] causedBy, int width, int up, int down)
        {
            var createTickersFunction = new CreateTickersFunction();
                createTickersFunction.CausedBy = causedBy;
                createTickersFunction.Width = width;
                createTickersFunction.Up = up;
                createTickersFunction.Down = down;
            
             return ContractHandler.SendRequestAsync(createTickersFunction);
        }

        public Task<TransactionReceipt> CreateTickersRequestAndWaitForReceiptAsync(byte[] causedBy, int width, int up, int down, CancellationTokenSource cancellationToken = null)
        {
            var createTickersFunction = new CreateTickersFunction();
                createTickersFunction.CausedBy = causedBy;
                createTickersFunction.Width = width;
                createTickersFunction.Up = up;
                createTickersFunction.Down = down;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTickersFunction, cancellationToken);
        }

        public Task<string> CreateWorldRequestAsync(CreateWorldFunction createWorldFunction)
        {
             return ContractHandler.SendRequestAsync(createWorldFunction);
        }

        public Task<string> CreateWorldRequestAsync()
        {
             return ContractHandler.SendRequestAsync<CreateWorldFunction>();
        }

        public Task<TransactionReceipt> CreateWorldRequestAndWaitForReceiptAsync(CreateWorldFunction createWorldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createWorldFunction, cancellationToken);
        }

        public Task<TransactionReceipt> CreateWorldRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<CreateWorldFunction>(null, cancellationToken);
        }

        public Task<string> CreatorQueryAsync(CreatorFunction creatorFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CreatorFunction, string>(creatorFunction, blockParameter);
        }

        
        public Task<string> CreatorQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CreatorFunction, string>(null, blockParameter);
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

        public Task<string> DeleteRecordRequestAsync(DeleteRecordFunction deleteRecordFunction)
        {
             return ContractHandler.SendRequestAsync(deleteRecordFunction);
        }

        public Task<TransactionReceipt> DeleteRecordRequestAndWaitForReceiptAsync(DeleteRecordFunction deleteRecordFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteRecordFunction, cancellationToken);
        }

        public Task<string> DeleteRecordRequestAsync(byte[] tableId, List<byte[]> keyTuple)
        {
            var deleteRecordFunction = new DeleteRecordFunction();
                deleteRecordFunction.TableId = tableId;
                deleteRecordFunction.KeyTuple = keyTuple;
            
             return ContractHandler.SendRequestAsync(deleteRecordFunction);
        }

        public Task<TransactionReceipt> DeleteRecordRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, CancellationTokenSource cancellationToken = null)
        {
            var deleteRecordFunction = new DeleteRecordFunction();
                deleteRecordFunction.TableId = tableId;
                deleteRecordFunction.KeyTuple = keyTuple;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deleteRecordFunction, cancellationToken);
        }

        public Task<string> DestroyRequestAsync(DestroyFunction destroyFunction)
        {
             return ContractHandler.SendRequestAsync(destroyFunction);
        }

        public Task<TransactionReceipt> DestroyRequestAndWaitForReceiptAsync(DestroyFunction destroyFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(destroyFunction, cancellationToken);
        }

        public Task<string> DestroyRequestAsync(byte[] causedBy, byte[] target, byte[] attacker, PositionData pos)
        {
            var destroyFunction = new DestroyFunction();
                destroyFunction.CausedBy = causedBy;
                destroyFunction.Target = target;
                destroyFunction.Attacker = attacker;
                destroyFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(destroyFunction);
        }

        public Task<TransactionReceipt> DestroyRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] target, byte[] attacker, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var destroyFunction = new DestroyFunction();
                destroyFunction.CausedBy = causedBy;
                destroyFunction.Target = target;
                destroyFunction.Attacker = attacker;
                destroyFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(destroyFunction, cancellationToken);
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

        public Task<string> DoAggroRequestAsync(DoAggroFunction doAggroFunction)
        {
             return ContractHandler.SendRequestAsync(doAggroFunction);
        }

        public Task<TransactionReceipt> DoAggroRequestAndWaitForReceiptAsync(DoAggroFunction doAggroFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doAggroFunction, cancellationToken);
        }

        public Task<string> DoAggroRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var doAggroFunction = new DoAggroFunction();
                doAggroFunction.CausedBy = causedBy;
                doAggroFunction.Entity = entity;
                doAggroFunction.Target = target;
                doAggroFunction.EntityPos = entityPos;
                doAggroFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doAggroFunction);
        }

        public Task<TransactionReceipt> DoAggroRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doAggroFunction = new DoAggroFunction();
                doAggroFunction.CausedBy = causedBy;
                doAggroFunction.Entity = entity;
                doAggroFunction.Target = target;
                doAggroFunction.EntityPos = entityPos;
                doAggroFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doAggroFunction, cancellationToken);
        }

        public Task<string> DoArrowRequestAsync(DoArrowFunction doArrowFunction)
        {
             return ContractHandler.SendRequestAsync(doArrowFunction);
        }

        public Task<TransactionReceipt> DoArrowRequestAndWaitForReceiptAsync(DoArrowFunction doArrowFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doArrowFunction, cancellationToken);
        }

        public Task<string> DoArrowRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var doArrowFunction = new DoArrowFunction();
                doArrowFunction.CausedBy = causedBy;
                doArrowFunction.Entity = entity;
                doArrowFunction.Target = target;
                doArrowFunction.EntityPos = entityPos;
                doArrowFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doArrowFunction);
        }

        public Task<TransactionReceipt> DoArrowRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doArrowFunction = new DoArrowFunction();
                doArrowFunction.CausedBy = causedBy;
                doArrowFunction.Entity = entity;
                doArrowFunction.Target = target;
                doArrowFunction.EntityPos = entityPos;
                doArrowFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doArrowFunction, cancellationToken);
        }

        public Task<string> DoCurseRequestAsync(DoCurseFunction doCurseFunction)
        {
             return ContractHandler.SendRequestAsync(doCurseFunction);
        }

        public Task<TransactionReceipt> DoCurseRequestAndWaitForReceiptAsync(DoCurseFunction doCurseFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doCurseFunction, cancellationToken);
        }

        public Task<string> DoCurseRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var doCurseFunction = new DoCurseFunction();
                doCurseFunction.CausedBy = causedBy;
                doCurseFunction.Entity = entity;
                doCurseFunction.Target = target;
                doCurseFunction.EntityPos = entityPos;
                doCurseFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doCurseFunction);
        }

        public Task<TransactionReceipt> DoCurseRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doCurseFunction = new DoCurseFunction();
                doCurseFunction.CausedBy = causedBy;
                doCurseFunction.Entity = entity;
                doCurseFunction.Target = target;
                doCurseFunction.EntityPos = entityPos;
                doCurseFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doCurseFunction, cancellationToken);
        }

        public Task<string> DoFlingRequestAsync(DoFlingFunction doFlingFunction)
        {
             return ContractHandler.SendRequestAsync(doFlingFunction);
        }

        public Task<TransactionReceipt> DoFlingRequestAndWaitForReceiptAsync(DoFlingFunction doFlingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doFlingFunction, cancellationToken);
        }

        public Task<string> DoFlingRequestAsync(byte[] causedBy, byte[] target, PositionData startPos, PositionData endPos)
        {
            var doFlingFunction = new DoFlingFunction();
                doFlingFunction.CausedBy = causedBy;
                doFlingFunction.Target = target;
                doFlingFunction.StartPos = startPos;
                doFlingFunction.EndPos = endPos;
            
             return ContractHandler.SendRequestAsync(doFlingFunction);
        }

        public Task<TransactionReceipt> DoFlingRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] target, PositionData startPos, PositionData endPos, CancellationTokenSource cancellationToken = null)
        {
            var doFlingFunction = new DoFlingFunction();
                doFlingFunction.CausedBy = causedBy;
                doFlingFunction.Target = target;
                doFlingFunction.StartPos = startPos;
                doFlingFunction.EndPos = endPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doFlingFunction, cancellationToken);
        }

        public Task<string> DoSeekRequestAsync(DoSeekFunction doSeekFunction)
        {
             return ContractHandler.SendRequestAsync(doSeekFunction);
        }

        public Task<TransactionReceipt> DoSeekRequestAndWaitForReceiptAsync(DoSeekFunction doSeekFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doSeekFunction, cancellationToken);
        }

        public Task<string> DoSeekRequestAsync(byte[] causedBy, byte[] seek, byte[] target, PositionData seekerPos, PositionData targetPos)
        {
            var doSeekFunction = new DoSeekFunction();
                doSeekFunction.CausedBy = causedBy;
                doSeekFunction.Seek = seek;
                doSeekFunction.Target = target;
                doSeekFunction.SeekerPos = seekerPos;
                doSeekFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doSeekFunction);
        }

        public Task<TransactionReceipt> DoSeekRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] seek, byte[] target, PositionData seekerPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doSeekFunction = new DoSeekFunction();
                doSeekFunction.CausedBy = causedBy;
                doSeekFunction.Seek = seek;
                doSeekFunction.Target = target;
                doSeekFunction.SeekerPos = seekerPos;
                doSeekFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doSeekFunction, cancellationToken);
        }

        public Task<string> DoSwapRequestAsync(DoSwapFunction doSwapFunction)
        {
             return ContractHandler.SendRequestAsync(doSwapFunction);
        }

        public Task<TransactionReceipt> DoSwapRequestAndWaitForReceiptAsync(DoSwapFunction doSwapFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doSwapFunction, cancellationToken);
        }

        public Task<string> DoSwapRequestAsync(byte[] causedBy, byte[] entity, PositionData entityPos, PositionData targetPos)
        {
            var doSwapFunction = new DoSwapFunction();
                doSwapFunction.CausedBy = causedBy;
                doSwapFunction.Entity = entity;
                doSwapFunction.EntityPos = entityPos;
                doSwapFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doSwapFunction);
        }

        public Task<TransactionReceipt> DoSwapRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doSwapFunction = new DoSwapFunction();
                doSwapFunction.CausedBy = causedBy;
                doSwapFunction.Entity = entity;
                doSwapFunction.EntityPos = entityPos;
                doSwapFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doSwapFunction, cancellationToken);
        }

        public Task<string> DoThiefRequestAsync(DoThiefFunction doThiefFunction)
        {
             return ContractHandler.SendRequestAsync(doThiefFunction);
        }

        public Task<TransactionReceipt> DoThiefRequestAndWaitForReceiptAsync(DoThiefFunction doThiefFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doThiefFunction, cancellationToken);
        }

        public Task<string> DoThiefRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var doThiefFunction = new DoThiefFunction();
                doThiefFunction.CausedBy = causedBy;
                doThiefFunction.Entity = entity;
                doThiefFunction.Target = target;
                doThiefFunction.EntityPos = entityPos;
                doThiefFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doThiefFunction);
        }

        public Task<TransactionReceipt> DoThiefRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doThiefFunction = new DoThiefFunction();
                doThiefFunction.CausedBy = causedBy;
                doThiefFunction.Entity = entity;
                doThiefFunction.Target = target;
                doThiefFunction.EntityPos = entityPos;
                doThiefFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doThiefFunction, cancellationToken);
        }

        public Task<string> DoThrowerRequestAsync(DoThrowerFunction doThrowerFunction)
        {
             return ContractHandler.SendRequestAsync(doThrowerFunction);
        }

        public Task<TransactionReceipt> DoThrowerRequestAndWaitForReceiptAsync(DoThrowerFunction doThrowerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doThrowerFunction, cancellationToken);
        }

        public Task<string> DoThrowerRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var doThrowerFunction = new DoThrowerFunction();
                doThrowerFunction.CausedBy = causedBy;
                doThrowerFunction.Entity = entity;
                doThrowerFunction.Target = target;
                doThrowerFunction.EntityPos = entityPos;
                doThrowerFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(doThrowerFunction);
        }

        public Task<TransactionReceipt> DoThrowerRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var doThrowerFunction = new DoThrowerFunction();
                doThrowerFunction.CausedBy = causedBy;
                doThrowerFunction.Entity = entity;
                doThrowerFunction.Target = target;
                doThrowerFunction.EntityPos = entityPos;
                doThrowerFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doThrowerFunction, cancellationToken);
        }

        public Task<string> DoWanderRequestAsync(DoWanderFunction doWanderFunction)
        {
             return ContractHandler.SendRequestAsync(doWanderFunction);
        }

        public Task<TransactionReceipt> DoWanderRequestAndWaitForReceiptAsync(DoWanderFunction doWanderFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doWanderFunction, cancellationToken);
        }

        public Task<string> DoWanderRequestAsync(byte[] causedBy, byte[] entity, PositionData entityPos)
        {
            var doWanderFunction = new DoWanderFunction();
                doWanderFunction.CausedBy = causedBy;
                doWanderFunction.Entity = entity;
                doWanderFunction.EntityPos = entityPos;
            
             return ContractHandler.SendRequestAsync(doWanderFunction);
        }

        public Task<TransactionReceipt> DoWanderRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData entityPos, CancellationTokenSource cancellationToken = null)
        {
            var doWanderFunction = new DoWanderFunction();
                doWanderFunction.CausedBy = causedBy;
                doWanderFunction.Entity = entity;
                doWanderFunction.EntityPos = entityPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(doWanderFunction, cancellationToken);
        }

        public Task<string> DressupRequestAsync(DressupFunction dressupFunction)
        {
             return ContractHandler.SendRequestAsync(dressupFunction);
        }

        public Task<TransactionReceipt> DressupRequestAndWaitForReceiptAsync(DressupFunction dressupFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(dressupFunction, cancellationToken);
        }

        public Task<string> DressupRequestAsync(byte cosmetic, byte index)
        {
            var dressupFunction = new DressupFunction();
                dressupFunction.Cosmetic = cosmetic;
                dressupFunction.Index = index;
            
             return ContractHandler.SendRequestAsync(dressupFunction);
        }

        public Task<TransactionReceipt> DressupRequestAndWaitForReceiptAsync(byte cosmetic, byte index, CancellationTokenSource cancellationToken = null)
        {
            var dressupFunction = new DressupFunction();
                dressupFunction.Cosmetic = cosmetic;
                dressupFunction.Index = index;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(dressupFunction, cancellationToken);
        }

        public Task<string> FinishMileRequestAsync(FinishMileFunction finishMileFunction)
        {
             return ContractHandler.SendRequestAsync(finishMileFunction);
        }

        public Task<TransactionReceipt> FinishMileRequestAndWaitForReceiptAsync(FinishMileFunction finishMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finishMileFunction, cancellationToken);
        }

        public Task<string> FinishMileRequestAsync(byte[] causedBy, byte[] chunk, int currentMile, uint pieces)
        {
            var finishMileFunction = new FinishMileFunction();
                finishMileFunction.CausedBy = causedBy;
                finishMileFunction.Chunk = chunk;
                finishMileFunction.CurrentMile = currentMile;
                finishMileFunction.Pieces = pieces;
            
             return ContractHandler.SendRequestAsync(finishMileFunction);
        }

        public Task<TransactionReceipt> FinishMileRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] chunk, int currentMile, uint pieces, CancellationTokenSource cancellationToken = null)
        {
            var finishMileFunction = new FinishMileFunction();
                finishMileFunction.CausedBy = causedBy;
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

        public Task<byte[]> GetDynamicFieldQueryAsync(GetDynamicFieldFunction getDynamicFieldFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDynamicFieldFunction, byte[]>(getDynamicFieldFunction, blockParameter);
        }

        
        public Task<byte[]> GetDynamicFieldQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, BlockParameter blockParameter = null)
        {
            var getDynamicFieldFunction = new GetDynamicFieldFunction();
                getDynamicFieldFunction.TableId = tableId;
                getDynamicFieldFunction.KeyTuple = keyTuple;
                getDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
            
            return ContractHandler.QueryAsync<GetDynamicFieldFunction, byte[]>(getDynamicFieldFunction, blockParameter);
        }

        public Task<BigInteger> GetDynamicFieldLengthQueryAsync(GetDynamicFieldLengthFunction getDynamicFieldLengthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDynamicFieldLengthFunction, BigInteger>(getDynamicFieldLengthFunction, blockParameter);
        }

        
        public Task<BigInteger> GetDynamicFieldLengthQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, BlockParameter blockParameter = null)
        {
            var getDynamicFieldLengthFunction = new GetDynamicFieldLengthFunction();
                getDynamicFieldLengthFunction.TableId = tableId;
                getDynamicFieldLengthFunction.KeyTuple = keyTuple;
                getDynamicFieldLengthFunction.DynamicFieldIndex = dynamicFieldIndex;
            
            return ContractHandler.QueryAsync<GetDynamicFieldLengthFunction, BigInteger>(getDynamicFieldLengthFunction, blockParameter);
        }

        public Task<byte[]> GetDynamicFieldSliceQueryAsync(GetDynamicFieldSliceFunction getDynamicFieldSliceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDynamicFieldSliceFunction, byte[]>(getDynamicFieldSliceFunction, blockParameter);
        }

        
        public Task<byte[]> GetDynamicFieldSliceQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, BigInteger start, BigInteger end, BlockParameter blockParameter = null)
        {
            var getDynamicFieldSliceFunction = new GetDynamicFieldSliceFunction();
                getDynamicFieldSliceFunction.TableId = tableId;
                getDynamicFieldSliceFunction.KeyTuple = keyTuple;
                getDynamicFieldSliceFunction.DynamicFieldIndex = dynamicFieldIndex;
                getDynamicFieldSliceFunction.Start = start;
                getDynamicFieldSliceFunction.End = end;
            
            return ContractHandler.QueryAsync<GetDynamicFieldSliceFunction, byte[]>(getDynamicFieldSliceFunction, blockParameter);
        }

        public Task<byte[]> GetFieldQueryAsync(GetField1Function getField1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetField1Function, byte[]>(getField1Function, blockParameter);
        }

        
        public Task<byte[]> GetFieldQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] fieldLayout, BlockParameter blockParameter = null)
        {
            var getField1Function = new GetField1Function();
                getField1Function.TableId = tableId;
                getField1Function.KeyTuple = keyTuple;
                getField1Function.FieldIndex = fieldIndex;
                getField1Function.FieldLayout = fieldLayout;
            
            return ContractHandler.QueryAsync<GetField1Function, byte[]>(getField1Function, blockParameter);
        }

        public Task<byte[]> GetFieldQueryAsync(GetFieldFunction getFieldFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldFunction, byte[]>(getFieldFunction, blockParameter);
        }

        
        public Task<byte[]> GetFieldQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, BlockParameter blockParameter = null)
        {
            var getFieldFunction = new GetFieldFunction();
                getFieldFunction.TableId = tableId;
                getFieldFunction.KeyTuple = keyTuple;
                getFieldFunction.FieldIndex = fieldIndex;
            
            return ContractHandler.QueryAsync<GetFieldFunction, byte[]>(getFieldFunction, blockParameter);
        }

        public Task<byte[]> GetFieldLayoutQueryAsync(GetFieldLayoutFunction getFieldLayoutFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldLayoutFunction, byte[]>(getFieldLayoutFunction, blockParameter);
        }

        
        public Task<byte[]> GetFieldLayoutQueryAsync(byte[] tableId, BlockParameter blockParameter = null)
        {
            var getFieldLayoutFunction = new GetFieldLayoutFunction();
                getFieldLayoutFunction.TableId = tableId;
            
            return ContractHandler.QueryAsync<GetFieldLayoutFunction, byte[]>(getFieldLayoutFunction, blockParameter);
        }

        public Task<BigInteger> GetFieldLengthQueryAsync(GetFieldLength1Function getFieldLength1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldLength1Function, BigInteger>(getFieldLength1Function, blockParameter);
        }

        
        public Task<BigInteger> GetFieldLengthQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] fieldLayout, BlockParameter blockParameter = null)
        {
            var getFieldLength1Function = new GetFieldLength1Function();
                getFieldLength1Function.TableId = tableId;
                getFieldLength1Function.KeyTuple = keyTuple;
                getFieldLength1Function.FieldIndex = fieldIndex;
                getFieldLength1Function.FieldLayout = fieldLayout;
            
            return ContractHandler.QueryAsync<GetFieldLength1Function, BigInteger>(getFieldLength1Function, blockParameter);
        }

        public Task<BigInteger> GetFieldLengthQueryAsync(GetFieldLengthFunction getFieldLengthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFieldLengthFunction, BigInteger>(getFieldLengthFunction, blockParameter);
        }

        
        public Task<BigInteger> GetFieldLengthQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, BlockParameter blockParameter = null)
        {
            var getFieldLengthFunction = new GetFieldLengthFunction();
                getFieldLengthFunction.TableId = tableId;
                getFieldLengthFunction.KeyTuple = keyTuple;
                getFieldLengthFunction.FieldIndex = fieldIndex;
            
            return ContractHandler.QueryAsync<GetFieldLengthFunction, BigInteger>(getFieldLengthFunction, blockParameter);
        }

        public Task<byte[]> GetKeySchemaQueryAsync(GetKeySchemaFunction getKeySchemaFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeySchemaFunction, byte[]>(getKeySchemaFunction, blockParameter);
        }

        
        public Task<byte[]> GetKeySchemaQueryAsync(byte[] tableId, BlockParameter blockParameter = null)
        {
            var getKeySchemaFunction = new GetKeySchemaFunction();
                getKeySchemaFunction.TableId = tableId;
            
            return ContractHandler.QueryAsync<GetKeySchemaFunction, byte[]>(getKeySchemaFunction, blockParameter);
        }

        public Task<GetRecord1OutputDTO> GetRecordQueryAsync(GetRecord1Function getRecord1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetRecord1Function, GetRecord1OutputDTO>(getRecord1Function, blockParameter);
        }

        public Task<GetRecord1OutputDTO> GetRecordQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte[] fieldLayout, BlockParameter blockParameter = null)
        {
            var getRecord1Function = new GetRecord1Function();
                getRecord1Function.TableId = tableId;
                getRecord1Function.KeyTuple = keyTuple;
                getRecord1Function.FieldLayout = fieldLayout;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetRecord1Function, GetRecord1OutputDTO>(getRecord1Function, blockParameter);
        }

        public Task<GetRecordOutputDTO> GetRecordQueryAsync(GetRecordFunction getRecordFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetRecordFunction, GetRecordOutputDTO>(getRecordFunction, blockParameter);
        }

        public Task<GetRecordOutputDTO> GetRecordQueryAsync(byte[] tableId, List<byte[]> keyTuple, BlockParameter blockParameter = null)
        {
            var getRecordFunction = new GetRecordFunction();
                getRecordFunction.TableId = tableId;
                getRecordFunction.KeyTuple = keyTuple;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetRecordFunction, GetRecordOutputDTO>(getRecordFunction, blockParameter);
        }

        public Task<byte[]> GetStaticFieldQueryAsync(GetStaticFieldFunction getStaticFieldFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetStaticFieldFunction, byte[]>(getStaticFieldFunction, blockParameter);
        }

        
        public Task<byte[]> GetStaticFieldQueryAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] fieldLayout, BlockParameter blockParameter = null)
        {
            var getStaticFieldFunction = new GetStaticFieldFunction();
                getStaticFieldFunction.TableId = tableId;
                getStaticFieldFunction.KeyTuple = keyTuple;
                getStaticFieldFunction.FieldIndex = fieldIndex;
                getStaticFieldFunction.FieldLayout = fieldLayout;
            
            return ContractHandler.QueryAsync<GetStaticFieldFunction, byte[]>(getStaticFieldFunction, blockParameter);
        }

        public Task<byte[]> GetValueSchemaQueryAsync(GetValueSchemaFunction getValueSchemaFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetValueSchemaFunction, byte[]>(getValueSchemaFunction, blockParameter);
        }

        
        public Task<byte[]> GetValueSchemaQueryAsync(byte[] tableId, BlockParameter blockParameter = null)
        {
            var getValueSchemaFunction = new GetValueSchemaFunction();
                getValueSchemaFunction.TableId = tableId;
            
            return ContractHandler.QueryAsync<GetValueSchemaFunction, byte[]>(getValueSchemaFunction, blockParameter);
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

        public Task<string> GiveGemRequestAsync(byte[] player, int amount)
        {
            var giveGemFunction = new GiveGemFunction();
                giveGemFunction.Player = player;
                giveGemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(giveGemFunction);
        }

        public Task<TransactionReceipt> GiveGemRequestAndWaitForReceiptAsync(byte[] player, int amount, CancellationTokenSource cancellationToken = null)
        {
            var giveGemFunction = new GiveGemFunction();
                giveGemFunction.Player = player;
                giveGemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveGemFunction, cancellationToken);
        }

        public Task<string> GiveKillRewardRequestAsync(GiveKillRewardFunction giveKillRewardFunction)
        {
             return ContractHandler.SendRequestAsync(giveKillRewardFunction);
        }

        public Task<TransactionReceipt> GiveKillRewardRequestAndWaitForReceiptAsync(GiveKillRewardFunction giveKillRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveKillRewardFunction, cancellationToken);
        }

        public Task<string> GiveKillRewardRequestAsync(byte[] player)
        {
            var giveKillRewardFunction = new GiveKillRewardFunction();
                giveKillRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAsync(giveKillRewardFunction);
        }

        public Task<TransactionReceipt> GiveKillRewardRequestAndWaitForReceiptAsync(byte[] player, CancellationTokenSource cancellationToken = null)
        {
            var giveKillRewardFunction = new GiveKillRewardFunction();
                giveKillRewardFunction.Player = player;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveKillRewardFunction, cancellationToken);
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

        public Task<string> GiveRoadLotteryRequestAsync(GiveRoadLotteryFunction giveRoadLotteryFunction)
        {
             return ContractHandler.SendRequestAsync(giveRoadLotteryFunction);
        }

        public Task<TransactionReceipt> GiveRoadLotteryRequestAndWaitForReceiptAsync(GiveRoadLotteryFunction giveRoadLotteryFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadLotteryFunction, cancellationToken);
        }

        public Task<string> GiveRoadLotteryRequestAsync(byte[] road)
        {
            var giveRoadLotteryFunction = new GiveRoadLotteryFunction();
                giveRoadLotteryFunction.Road = road;
            
             return ContractHandler.SendRequestAsync(giveRoadLotteryFunction);
        }

        public Task<TransactionReceipt> GiveRoadLotteryRequestAndWaitForReceiptAsync(byte[] road, CancellationTokenSource cancellationToken = null)
        {
            var giveRoadLotteryFunction = new GiveRoadLotteryFunction();
                giveRoadLotteryFunction.Road = road;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveRoadLotteryFunction, cancellationToken);
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

        public Task<string> GrantAccessRequestAsync(byte[] resourceId, string grantee)
        {
            var grantAccessFunction = new GrantAccessFunction();
                grantAccessFunction.ResourceId = resourceId;
                grantAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAsync(grantAccessFunction);
        }

        public Task<TransactionReceipt> GrantAccessRequestAndWaitForReceiptAsync(byte[] resourceId, string grantee, CancellationTokenSource cancellationToken = null)
        {
            var grantAccessFunction = new GrantAccessFunction();
                grantAccessFunction.ResourceId = resourceId;
                grantAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(grantAccessFunction, cancellationToken);
        }

        public Task<string> HandleMoveTypeRequestAsync(HandleMoveTypeFunction handleMoveTypeFunction)
        {
             return ContractHandler.SendRequestAsync(handleMoveTypeFunction);
        }

        public Task<TransactionReceipt> HandleMoveTypeRequestAndWaitForReceiptAsync(HandleMoveTypeFunction handleMoveTypeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(handleMoveTypeFunction, cancellationToken);
        }

        public Task<string> HandleMoveTypeRequestAsync(byte[] causedBy, byte[] entity, PositionData to, byte[] atDest, byte moveTypeAtDest, byte actionType)
        {
            var handleMoveTypeFunction = new HandleMoveTypeFunction();
                handleMoveTypeFunction.CausedBy = causedBy;
                handleMoveTypeFunction.Entity = entity;
                handleMoveTypeFunction.To = to;
                handleMoveTypeFunction.AtDest = atDest;
                handleMoveTypeFunction.MoveTypeAtDest = moveTypeAtDest;
                handleMoveTypeFunction.ActionType = actionType;
            
             return ContractHandler.SendRequestAsync(handleMoveTypeFunction);
        }

        public Task<TransactionReceipt> HandleMoveTypeRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData to, byte[] atDest, byte moveTypeAtDest, byte actionType, CancellationTokenSource cancellationToken = null)
        {
            var handleMoveTypeFunction = new HandleMoveTypeFunction();
                handleMoveTypeFunction.CausedBy = causedBy;
                handleMoveTypeFunction.Entity = entity;
                handleMoveTypeFunction.To = to;
                handleMoveTypeFunction.AtDest = atDest;
                handleMoveTypeFunction.MoveTypeAtDest = moveTypeAtDest;
                handleMoveTypeFunction.ActionType = actionType;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(handleMoveTypeFunction, cancellationToken);
        }

        public Task<string> HelpSummonRequestAsync(HelpSummonFunction helpSummonFunction)
        {
             return ContractHandler.SendRequestAsync(helpSummonFunction);
        }

        public Task<string> HelpSummonRequestAsync()
        {
             return ContractHandler.SendRequestAsync<HelpSummonFunction>();
        }

        public Task<TransactionReceipt> HelpSummonRequestAndWaitForReceiptAsync(HelpSummonFunction helpSummonFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(helpSummonFunction, cancellationToken);
        }

        public Task<TransactionReceipt> HelpSummonRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<HelpSummonFunction>(null, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(InitializeFunction initializeFunction)
        {
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(InitializeFunction initializeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(string coreModule)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.CoreModule = coreModule;
            
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(string coreModule, CancellationTokenSource cancellationToken = null)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.CoreModule = coreModule;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
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

        public Task<string> MegaSummonRequestAsync(MegaSummonFunction megaSummonFunction)
        {
             return ContractHandler.SendRequestAsync(megaSummonFunction);
        }

        public Task<string> MegaSummonRequestAsync()
        {
             return ContractHandler.SendRequestAsync<MegaSummonFunction>();
        }

        public Task<TransactionReceipt> MegaSummonRequestAndWaitForReceiptAsync(MegaSummonFunction megaSummonFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(megaSummonFunction, cancellationToken);
        }

        public Task<TransactionReceipt> MegaSummonRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<MegaSummonFunction>(null, cancellationToken);
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

        public Task<string> MoveOrPushRequestAsync(MoveOrPushFunction moveOrPushFunction)
        {
             return ContractHandler.SendRequestAsync(moveOrPushFunction);
        }

        public Task<TransactionReceipt> MoveOrPushRequestAndWaitForReceiptAsync(MoveOrPushFunction moveOrPushFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveOrPushFunction, cancellationToken);
        }

        public Task<string> MoveOrPushRequestAsync(byte[] causedBy, byte[] player, PositionData startPos, PositionData vector, int distance)
        {
            var moveOrPushFunction = new MoveOrPushFunction();
                moveOrPushFunction.CausedBy = causedBy;
                moveOrPushFunction.Player = player;
                moveOrPushFunction.StartPos = startPos;
                moveOrPushFunction.Vector = vector;
                moveOrPushFunction.Distance = distance;
            
             return ContractHandler.SendRequestAsync(moveOrPushFunction);
        }

        public Task<TransactionReceipt> MoveOrPushRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] player, PositionData startPos, PositionData vector, int distance, CancellationTokenSource cancellationToken = null)
        {
            var moveOrPushFunction = new MoveOrPushFunction();
                moveOrPushFunction.CausedBy = causedBy;
                moveOrPushFunction.Player = player;
                moveOrPushFunction.StartPos = startPos;
                moveOrPushFunction.Vector = vector;
                moveOrPushFunction.Distance = distance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveOrPushFunction, cancellationToken);
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

        public Task<string> MoveSimpleDistanceRequestAsync(MoveSimpleDistanceFunction moveSimpleDistanceFunction)
        {
             return ContractHandler.SendRequestAsync(moveSimpleDistanceFunction);
        }

        public Task<TransactionReceipt> MoveSimpleDistanceRequestAndWaitForReceiptAsync(MoveSimpleDistanceFunction moveSimpleDistanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveSimpleDistanceFunction, cancellationToken);
        }

        public Task<string> MoveSimpleDistanceRequestAsync(byte[] player, int x, int y, int distance)
        {
            var moveSimpleDistanceFunction = new MoveSimpleDistanceFunction();
                moveSimpleDistanceFunction.Player = player;
                moveSimpleDistanceFunction.X = x;
                moveSimpleDistanceFunction.Y = y;
                moveSimpleDistanceFunction.Distance = distance;
            
             return ContractHandler.SendRequestAsync(moveSimpleDistanceFunction);
        }

        public Task<TransactionReceipt> MoveSimpleDistanceRequestAndWaitForReceiptAsync(byte[] player, int x, int y, int distance, CancellationTokenSource cancellationToken = null)
        {
            var moveSimpleDistanceFunction = new MoveSimpleDistanceFunction();
                moveSimpleDistanceFunction.Player = player;
                moveSimpleDistanceFunction.X = x;
                moveSimpleDistanceFunction.Y = y;
                moveSimpleDistanceFunction.Distance = distance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveSimpleDistanceFunction, cancellationToken);
        }

        public Task<string> MoveToRequestAsync(MoveToFunction moveToFunction)
        {
             return ContractHandler.SendRequestAsync(moveToFunction);
        }

        public Task<TransactionReceipt> MoveToRequestAndWaitForReceiptAsync(MoveToFunction moveToFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(moveToFunction, cancellationToken);
        }

        public Task<string> MoveToRequestAsync(byte[] causedBy, byte[] entity, PositionData from, PositionData to, List<byte[]> atDest, byte actionType)
        {
            var moveToFunction = new MoveToFunction();
                moveToFunction.CausedBy = causedBy;
                moveToFunction.Entity = entity;
                moveToFunction.From = from;
                moveToFunction.To = to;
                moveToFunction.AtDest = atDest;
                moveToFunction.ActionType = actionType;
            
             return ContractHandler.SendRequestAsync(moveToFunction);
        }

        public Task<TransactionReceipt> MoveToRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData from, PositionData to, List<byte[]> atDest, byte actionType, CancellationTokenSource cancellationToken = null)
        {
            var moveToFunction = new MoveToFunction();
                moveToFunction.CausedBy = causedBy;
                moveToFunction.Entity = entity;
                moveToFunction.From = from;
                moveToFunction.To = to;
                moveToFunction.AtDest = atDest;
                moveToFunction.ActionType = actionType;
            
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

        public Task<string> PocketRequestAsync(PocketFunction pocketFunction)
        {
             return ContractHandler.SendRequestAsync(pocketFunction);
        }

        public Task<TransactionReceipt> PocketRequestAndWaitForReceiptAsync(PocketFunction pocketFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pocketFunction, cancellationToken);
        }

        public Task<string> PocketRequestAsync(byte[] player, int x, int y)
        {
            var pocketFunction = new PocketFunction();
                pocketFunction.Player = player;
                pocketFunction.X = x;
                pocketFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(pocketFunction);
        }

        public Task<TransactionReceipt> PocketRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var pocketFunction = new PocketFunction();
                pocketFunction.Player = player;
                pocketFunction.X = x;
                pocketFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pocketFunction, cancellationToken);
        }

        public Task<string> PopFromDynamicFieldRequestAsync(PopFromDynamicFieldFunction popFromDynamicFieldFunction)
        {
             return ContractHandler.SendRequestAsync(popFromDynamicFieldFunction);
        }

        public Task<TransactionReceipt> PopFromDynamicFieldRequestAndWaitForReceiptAsync(PopFromDynamicFieldFunction popFromDynamicFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(popFromDynamicFieldFunction, cancellationToken);
        }

        public Task<string> PopFromDynamicFieldRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, BigInteger byteLengthToPop)
        {
            var popFromDynamicFieldFunction = new PopFromDynamicFieldFunction();
                popFromDynamicFieldFunction.TableId = tableId;
                popFromDynamicFieldFunction.KeyTuple = keyTuple;
                popFromDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
                popFromDynamicFieldFunction.ByteLengthToPop = byteLengthToPop;
            
             return ContractHandler.SendRequestAsync(popFromDynamicFieldFunction);
        }

        public Task<TransactionReceipt> PopFromDynamicFieldRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, BigInteger byteLengthToPop, CancellationTokenSource cancellationToken = null)
        {
            var popFromDynamicFieldFunction = new PopFromDynamicFieldFunction();
                popFromDynamicFieldFunction.TableId = tableId;
                popFromDynamicFieldFunction.KeyTuple = keyTuple;
                popFromDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
                popFromDynamicFieldFunction.ByteLengthToPop = byteLengthToPop;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(popFromDynamicFieldFunction, cancellationToken);
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

        public Task<string> PushToDynamicFieldRequestAsync(PushToDynamicFieldFunction pushToDynamicFieldFunction)
        {
             return ContractHandler.SendRequestAsync(pushToDynamicFieldFunction);
        }

        public Task<TransactionReceipt> PushToDynamicFieldRequestAndWaitForReceiptAsync(PushToDynamicFieldFunction pushToDynamicFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushToDynamicFieldFunction, cancellationToken);
        }

        public Task<string> PushToDynamicFieldRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, byte[] dataToPush)
        {
            var pushToDynamicFieldFunction = new PushToDynamicFieldFunction();
                pushToDynamicFieldFunction.TableId = tableId;
                pushToDynamicFieldFunction.KeyTuple = keyTuple;
                pushToDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
                pushToDynamicFieldFunction.DataToPush = dataToPush;
            
             return ContractHandler.SendRequestAsync(pushToDynamicFieldFunction);
        }

        public Task<TransactionReceipt> PushToDynamicFieldRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, byte[] dataToPush, CancellationTokenSource cancellationToken = null)
        {
            var pushToDynamicFieldFunction = new PushToDynamicFieldFunction();
                pushToDynamicFieldFunction.TableId = tableId;
                pushToDynamicFieldFunction.KeyTuple = keyTuple;
                pushToDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
                pushToDynamicFieldFunction.DataToPush = dataToPush;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pushToDynamicFieldFunction, cancellationToken);
        }

        public Task<string> RegisterDelegationRequestAsync(RegisterDelegationFunction registerDelegationFunction)
        {
             return ContractHandler.SendRequestAsync(registerDelegationFunction);
        }

        public Task<TransactionReceipt> RegisterDelegationRequestAndWaitForReceiptAsync(RegisterDelegationFunction registerDelegationFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerDelegationFunction, cancellationToken);
        }

        public Task<string> RegisterDelegationRequestAsync(string delegatee, byte[] delegationControlId, byte[] initCallData)
        {
            var registerDelegationFunction = new RegisterDelegationFunction();
                registerDelegationFunction.Delegatee = delegatee;
                registerDelegationFunction.DelegationControlId = delegationControlId;
                registerDelegationFunction.InitCallData = initCallData;
            
             return ContractHandler.SendRequestAsync(registerDelegationFunction);
        }

        public Task<TransactionReceipt> RegisterDelegationRequestAndWaitForReceiptAsync(string delegatee, byte[] delegationControlId, byte[] initCallData, CancellationTokenSource cancellationToken = null)
        {
            var registerDelegationFunction = new RegisterDelegationFunction();
                registerDelegationFunction.Delegatee = delegatee;
                registerDelegationFunction.DelegationControlId = delegationControlId;
                registerDelegationFunction.InitCallData = initCallData;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerDelegationFunction, cancellationToken);
        }

        public Task<string> RegisterFunctionSelectorRequestAsync(RegisterFunctionSelectorFunction registerFunctionSelectorFunction)
        {
             return ContractHandler.SendRequestAsync(registerFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterFunctionSelectorRequestAndWaitForReceiptAsync(RegisterFunctionSelectorFunction registerFunctionSelectorFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterFunctionSelectorRequestAsync(byte[] systemId, string systemFunctionSignature)
        {
            var registerFunctionSelectorFunction = new RegisterFunctionSelectorFunction();
                registerFunctionSelectorFunction.SystemId = systemId;
                registerFunctionSelectorFunction.SystemFunctionSignature = systemFunctionSignature;
            
             return ContractHandler.SendRequestAsync(registerFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterFunctionSelectorRequestAndWaitForReceiptAsync(byte[] systemId, string systemFunctionSignature, CancellationTokenSource cancellationToken = null)
        {
            var registerFunctionSelectorFunction = new RegisterFunctionSelectorFunction();
                registerFunctionSelectorFunction.SystemId = systemId;
                registerFunctionSelectorFunction.SystemFunctionSignature = systemFunctionSignature;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterNamespaceRequestAsync(RegisterNamespaceFunction registerNamespaceFunction)
        {
             return ContractHandler.SendRequestAsync(registerNamespaceFunction);
        }

        public Task<TransactionReceipt> RegisterNamespaceRequestAndWaitForReceiptAsync(RegisterNamespaceFunction registerNamespaceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerNamespaceFunction, cancellationToken);
        }

        public Task<string> RegisterNamespaceRequestAsync(byte[] namespaceId)
        {
            var registerNamespaceFunction = new RegisterNamespaceFunction();
                registerNamespaceFunction.NamespaceId = namespaceId;
            
             return ContractHandler.SendRequestAsync(registerNamespaceFunction);
        }

        public Task<TransactionReceipt> RegisterNamespaceRequestAndWaitForReceiptAsync(byte[] namespaceId, CancellationTokenSource cancellationToken = null)
        {
            var registerNamespaceFunction = new RegisterNamespaceFunction();
                registerNamespaceFunction.NamespaceId = namespaceId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerNamespaceFunction, cancellationToken);
        }

        public Task<string> RegisterNamespaceDelegationRequestAsync(RegisterNamespaceDelegationFunction registerNamespaceDelegationFunction)
        {
             return ContractHandler.SendRequestAsync(registerNamespaceDelegationFunction);
        }

        public Task<TransactionReceipt> RegisterNamespaceDelegationRequestAndWaitForReceiptAsync(RegisterNamespaceDelegationFunction registerNamespaceDelegationFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerNamespaceDelegationFunction, cancellationToken);
        }

        public Task<string> RegisterNamespaceDelegationRequestAsync(byte[] namespaceId, byte[] delegationControlId, byte[] initCallData)
        {
            var registerNamespaceDelegationFunction = new RegisterNamespaceDelegationFunction();
                registerNamespaceDelegationFunction.NamespaceId = namespaceId;
                registerNamespaceDelegationFunction.DelegationControlId = delegationControlId;
                registerNamespaceDelegationFunction.InitCallData = initCallData;
            
             return ContractHandler.SendRequestAsync(registerNamespaceDelegationFunction);
        }

        public Task<TransactionReceipt> RegisterNamespaceDelegationRequestAndWaitForReceiptAsync(byte[] namespaceId, byte[] delegationControlId, byte[] initCallData, CancellationTokenSource cancellationToken = null)
        {
            var registerNamespaceDelegationFunction = new RegisterNamespaceDelegationFunction();
                registerNamespaceDelegationFunction.NamespaceId = namespaceId;
                registerNamespaceDelegationFunction.DelegationControlId = delegationControlId;
                registerNamespaceDelegationFunction.InitCallData = initCallData;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerNamespaceDelegationFunction, cancellationToken);
        }

        public Task<string> RegisterRootFunctionSelectorRequestAsync(RegisterRootFunctionSelectorFunction registerRootFunctionSelectorFunction)
        {
             return ContractHandler.SendRequestAsync(registerRootFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterRootFunctionSelectorRequestAndWaitForReceiptAsync(RegisterRootFunctionSelectorFunction registerRootFunctionSelectorFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerRootFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterRootFunctionSelectorRequestAsync(byte[] systemId, string worldFunctionSignature, byte[] systemFunctionSelector)
        {
            var registerRootFunctionSelectorFunction = new RegisterRootFunctionSelectorFunction();
                registerRootFunctionSelectorFunction.SystemId = systemId;
                registerRootFunctionSelectorFunction.WorldFunctionSignature = worldFunctionSignature;
                registerRootFunctionSelectorFunction.SystemFunctionSelector = systemFunctionSelector;
            
             return ContractHandler.SendRequestAsync(registerRootFunctionSelectorFunction);
        }

        public Task<TransactionReceipt> RegisterRootFunctionSelectorRequestAndWaitForReceiptAsync(byte[] systemId, string worldFunctionSignature, byte[] systemFunctionSelector, CancellationTokenSource cancellationToken = null)
        {
            var registerRootFunctionSelectorFunction = new RegisterRootFunctionSelectorFunction();
                registerRootFunctionSelectorFunction.SystemId = systemId;
                registerRootFunctionSelectorFunction.WorldFunctionSignature = worldFunctionSignature;
                registerRootFunctionSelectorFunction.SystemFunctionSelector = systemFunctionSelector;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerRootFunctionSelectorFunction, cancellationToken);
        }

        public Task<string> RegisterStoreHookRequestAsync(RegisterStoreHookFunction registerStoreHookFunction)
        {
             return ContractHandler.SendRequestAsync(registerStoreHookFunction);
        }

        public Task<TransactionReceipt> RegisterStoreHookRequestAndWaitForReceiptAsync(RegisterStoreHookFunction registerStoreHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerStoreHookFunction, cancellationToken);
        }

        public Task<string> RegisterStoreHookRequestAsync(byte[] tableId, string hookAddress, byte enabledHooksBitmap)
        {
            var registerStoreHookFunction = new RegisterStoreHookFunction();
                registerStoreHookFunction.TableId = tableId;
                registerStoreHookFunction.HookAddress = hookAddress;
                registerStoreHookFunction.EnabledHooksBitmap = enabledHooksBitmap;
            
             return ContractHandler.SendRequestAsync(registerStoreHookFunction);
        }

        public Task<TransactionReceipt> RegisterStoreHookRequestAndWaitForReceiptAsync(byte[] tableId, string hookAddress, byte enabledHooksBitmap, CancellationTokenSource cancellationToken = null)
        {
            var registerStoreHookFunction = new RegisterStoreHookFunction();
                registerStoreHookFunction.TableId = tableId;
                registerStoreHookFunction.HookAddress = hookAddress;
                registerStoreHookFunction.EnabledHooksBitmap = enabledHooksBitmap;
            
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

        public Task<string> RegisterSystemRequestAsync(byte[] systemId, string system, bool publicAccess)
        {
            var registerSystemFunction = new RegisterSystemFunction();
                registerSystemFunction.SystemId = systemId;
                registerSystemFunction.System = system;
                registerSystemFunction.PublicAccess = publicAccess;
            
             return ContractHandler.SendRequestAsync(registerSystemFunction);
        }

        public Task<TransactionReceipt> RegisterSystemRequestAndWaitForReceiptAsync(byte[] systemId, string system, bool publicAccess, CancellationTokenSource cancellationToken = null)
        {
            var registerSystemFunction = new RegisterSystemFunction();
                registerSystemFunction.SystemId = systemId;
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

        public Task<string> RegisterSystemHookRequestAsync(byte[] systemId, string hookAddress, byte enabledHooksBitmap)
        {
            var registerSystemHookFunction = new RegisterSystemHookFunction();
                registerSystemHookFunction.SystemId = systemId;
                registerSystemHookFunction.HookAddress = hookAddress;
                registerSystemHookFunction.EnabledHooksBitmap = enabledHooksBitmap;
            
             return ContractHandler.SendRequestAsync(registerSystemHookFunction);
        }

        public Task<TransactionReceipt> RegisterSystemHookRequestAndWaitForReceiptAsync(byte[] systemId, string hookAddress, byte enabledHooksBitmap, CancellationTokenSource cancellationToken = null)
        {
            var registerSystemHookFunction = new RegisterSystemHookFunction();
                registerSystemHookFunction.SystemId = systemId;
                registerSystemHookFunction.HookAddress = hookAddress;
                registerSystemHookFunction.EnabledHooksBitmap = enabledHooksBitmap;
            
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

        public Task<string> RegisterTableRequestAsync(byte[] tableId, byte[] fieldLayout, byte[] keySchema, byte[] valueSchema, List<string> keyNames, List<string> fieldNames)
        {
            var registerTableFunction = new RegisterTableFunction();
                registerTableFunction.TableId = tableId;
                registerTableFunction.FieldLayout = fieldLayout;
                registerTableFunction.KeySchema = keySchema;
                registerTableFunction.ValueSchema = valueSchema;
                registerTableFunction.KeyNames = keyNames;
                registerTableFunction.FieldNames = fieldNames;
            
             return ContractHandler.SendRequestAsync(registerTableFunction);
        }

        public Task<TransactionReceipt> RegisterTableRequestAndWaitForReceiptAsync(byte[] tableId, byte[] fieldLayout, byte[] keySchema, byte[] valueSchema, List<string> keyNames, List<string> fieldNames, CancellationTokenSource cancellationToken = null)
        {
            var registerTableFunction = new RegisterTableFunction();
                registerTableFunction.TableId = tableId;
                registerTableFunction.FieldLayout = fieldLayout;
                registerTableFunction.KeySchema = keySchema;
                registerTableFunction.ValueSchema = valueSchema;
                registerTableFunction.KeyNames = keyNames;
                registerTableFunction.FieldNames = fieldNames;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerTableFunction, cancellationToken);
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

        public Task<string> RevokeAccessRequestAsync(byte[] resourceId, string grantee)
        {
            var revokeAccessFunction = new RevokeAccessFunction();
                revokeAccessFunction.ResourceId = resourceId;
                revokeAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAsync(revokeAccessFunction);
        }

        public Task<TransactionReceipt> RevokeAccessRequestAndWaitForReceiptAsync(byte[] resourceId, string grantee, CancellationTokenSource cancellationToken = null)
        {
            var revokeAccessFunction = new RevokeAccessFunction();
                revokeAccessFunction.ResourceId = resourceId;
                revokeAccessFunction.Grantee = grantee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(revokeAccessFunction, cancellationToken);
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

        public Task<string> SetCosmeticRequestAsync(SetCosmeticFunction setCosmeticFunction)
        {
             return ContractHandler.SendRequestAsync(setCosmeticFunction);
        }

        public Task<TransactionReceipt> SetCosmeticRequestAndWaitForReceiptAsync(SetCosmeticFunction setCosmeticFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCosmeticFunction, cancellationToken);
        }

        public Task<string> SetCosmeticRequestAsync(byte[] player, byte cosmetic, byte index)
        {
            var setCosmeticFunction = new SetCosmeticFunction();
                setCosmeticFunction.Player = player;
                setCosmeticFunction.Cosmetic = cosmetic;
                setCosmeticFunction.Index = index;
            
             return ContractHandler.SendRequestAsync(setCosmeticFunction);
        }

        public Task<TransactionReceipt> SetCosmeticRequestAndWaitForReceiptAsync(byte[] player, byte cosmetic, byte index, CancellationTokenSource cancellationToken = null)
        {
            var setCosmeticFunction = new SetCosmeticFunction();
                setCosmeticFunction.Player = player;
                setCosmeticFunction.Cosmetic = cosmetic;
                setCosmeticFunction.Index = index;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCosmeticFunction, cancellationToken);
        }

        public Task<string> SetDynamicFieldRequestAsync(SetDynamicFieldFunction setDynamicFieldFunction)
        {
             return ContractHandler.SendRequestAsync(setDynamicFieldFunction);
        }

        public Task<TransactionReceipt> SetDynamicFieldRequestAndWaitForReceiptAsync(SetDynamicFieldFunction setDynamicFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setDynamicFieldFunction, cancellationToken);
        }

        public Task<string> SetDynamicFieldRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, byte[] data)
        {
            var setDynamicFieldFunction = new SetDynamicFieldFunction();
                setDynamicFieldFunction.TableId = tableId;
                setDynamicFieldFunction.KeyTuple = keyTuple;
                setDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
                setDynamicFieldFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(setDynamicFieldFunction);
        }

        public Task<TransactionReceipt> SetDynamicFieldRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var setDynamicFieldFunction = new SetDynamicFieldFunction();
                setDynamicFieldFunction.TableId = tableId;
                setDynamicFieldFunction.KeyTuple = keyTuple;
                setDynamicFieldFunction.DynamicFieldIndex = dynamicFieldIndex;
                setDynamicFieldFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setDynamicFieldFunction, cancellationToken);
        }

        public Task<string> SetFieldRequestAsync(SetFieldFunction setFieldFunction)
        {
             return ContractHandler.SendRequestAsync(setFieldFunction);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(SetFieldFunction setFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFieldFunction, cancellationToken);
        }

        public Task<string> SetFieldRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] data)
        {
            var setFieldFunction = new SetFieldFunction();
                setFieldFunction.TableId = tableId;
                setFieldFunction.KeyTuple = keyTuple;
                setFieldFunction.FieldIndex = fieldIndex;
                setFieldFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(setFieldFunction);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var setFieldFunction = new SetFieldFunction();
                setFieldFunction.TableId = tableId;
                setFieldFunction.KeyTuple = keyTuple;
                setFieldFunction.FieldIndex = fieldIndex;
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

        public Task<string> SetFieldRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] data, byte[] fieldLayout)
        {
            var setField1Function = new SetField1Function();
                setField1Function.TableId = tableId;
                setField1Function.KeyTuple = keyTuple;
                setField1Function.FieldIndex = fieldIndex;
                setField1Function.Data = data;
                setField1Function.FieldLayout = fieldLayout;
            
             return ContractHandler.SendRequestAsync(setField1Function);
        }

        public Task<TransactionReceipt> SetFieldRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] data, byte[] fieldLayout, CancellationTokenSource cancellationToken = null)
        {
            var setField1Function = new SetField1Function();
                setField1Function.TableId = tableId;
                setField1Function.KeyTuple = keyTuple;
                setField1Function.FieldIndex = fieldIndex;
                setField1Function.Data = data;
                setField1Function.FieldLayout = fieldLayout;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setField1Function, cancellationToken);
        }

        public Task<string> SetPositionRequestAsync(SetPositionFunction setPositionFunction)
        {
             return ContractHandler.SendRequestAsync(setPositionFunction);
        }

        public Task<TransactionReceipt> SetPositionRequestAndWaitForReceiptAsync(SetPositionFunction setPositionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionFunction, cancellationToken);
        }

        public Task<string> SetPositionRequestAsync(byte[] causedBy, byte[] entity, PositionData pos, byte action)
        {
            var setPositionFunction = new SetPositionFunction();
                setPositionFunction.CausedBy = causedBy;
                setPositionFunction.Entity = entity;
                setPositionFunction.Pos = pos;
                setPositionFunction.Action = action;
            
             return ContractHandler.SendRequestAsync(setPositionFunction);
        }

        public Task<TransactionReceipt> SetPositionRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData pos, byte action, CancellationTokenSource cancellationToken = null)
        {
            var setPositionFunction = new SetPositionFunction();
                setPositionFunction.CausedBy = causedBy;
                setPositionFunction.Entity = entity;
                setPositionFunction.Pos = pos;
                setPositionFunction.Action = action;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPositionFunction, cancellationToken);
        }

        public Task<string> SetRecordRequestAsync(SetRecordFunction setRecordFunction)
        {
             return ContractHandler.SendRequestAsync(setRecordFunction);
        }

        public Task<TransactionReceipt> SetRecordRequestAndWaitForReceiptAsync(SetRecordFunction setRecordFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRecordFunction, cancellationToken);
        }

        public Task<string> SetRecordRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte[] staticData, byte[] encodedLengths, byte[] dynamicData)
        {
            var setRecordFunction = new SetRecordFunction();
                setRecordFunction.TableId = tableId;
                setRecordFunction.KeyTuple = keyTuple;
                setRecordFunction.StaticData = staticData;
                setRecordFunction.EncodedLengths = encodedLengths;
                setRecordFunction.DynamicData = dynamicData;
            
             return ContractHandler.SendRequestAsync(setRecordFunction);
        }

        public Task<TransactionReceipt> SetRecordRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte[] staticData, byte[] encodedLengths, byte[] dynamicData, CancellationTokenSource cancellationToken = null)
        {
            var setRecordFunction = new SetRecordFunction();
                setRecordFunction.TableId = tableId;
                setRecordFunction.KeyTuple = keyTuple;
                setRecordFunction.StaticData = staticData;
                setRecordFunction.EncodedLengths = encodedLengths;
                setRecordFunction.DynamicData = dynamicData;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRecordFunction, cancellationToken);
        }

        public Task<string> SetStaticFieldRequestAsync(SetStaticFieldFunction setStaticFieldFunction)
        {
             return ContractHandler.SendRequestAsync(setStaticFieldFunction);
        }

        public Task<TransactionReceipt> SetStaticFieldRequestAndWaitForReceiptAsync(SetStaticFieldFunction setStaticFieldFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setStaticFieldFunction, cancellationToken);
        }

        public Task<string> SetStaticFieldRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] data, byte[] fieldLayout)
        {
            var setStaticFieldFunction = new SetStaticFieldFunction();
                setStaticFieldFunction.TableId = tableId;
                setStaticFieldFunction.KeyTuple = keyTuple;
                setStaticFieldFunction.FieldIndex = fieldIndex;
                setStaticFieldFunction.Data = data;
                setStaticFieldFunction.FieldLayout = fieldLayout;
            
             return ContractHandler.SendRequestAsync(setStaticFieldFunction);
        }

        public Task<TransactionReceipt> SetStaticFieldRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte fieldIndex, byte[] data, byte[] fieldLayout, CancellationTokenSource cancellationToken = null)
        {
            var setStaticFieldFunction = new SetStaticFieldFunction();
                setStaticFieldFunction.TableId = tableId;
                setStaticFieldFunction.KeyTuple = keyTuple;
                setStaticFieldFunction.FieldIndex = fieldIndex;
                setStaticFieldFunction.Data = data;
                setStaticFieldFunction.FieldLayout = fieldLayout;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setStaticFieldFunction, cancellationToken);
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

        public Task<string> SoftWithdrawCoinsRequestAsync(SoftWithdrawCoinsFunction softWithdrawCoinsFunction)
        {
             return ContractHandler.SendRequestAsync(softWithdrawCoinsFunction);
        }

        public Task<TransactionReceipt> SoftWithdrawCoinsRequestAndWaitForReceiptAsync(SoftWithdrawCoinsFunction softWithdrawCoinsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(softWithdrawCoinsFunction, cancellationToken);
        }

        public Task<string> SoftWithdrawCoinsRequestAsync(byte[] player, int amount)
        {
            var softWithdrawCoinsFunction = new SoftWithdrawCoinsFunction();
                softWithdrawCoinsFunction.Player = player;
                softWithdrawCoinsFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(softWithdrawCoinsFunction);
        }

        public Task<TransactionReceipt> SoftWithdrawCoinsRequestAndWaitForReceiptAsync(byte[] player, int amount, CancellationTokenSource cancellationToken = null)
        {
            var softWithdrawCoinsFunction = new SoftWithdrawCoinsFunction();
                softWithdrawCoinsFunction.Player = player;
                softWithdrawCoinsFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(softWithdrawCoinsFunction, cancellationToken);
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

        public Task<string> SpawnEmptyRoadRequestAsync(SpawnEmptyRoadFunction spawnEmptyRoadFunction)
        {
             return ContractHandler.SendRequestAsync(spawnEmptyRoadFunction);
        }

        public Task<TransactionReceipt> SpawnEmptyRoadRequestAndWaitForReceiptAsync(SpawnEmptyRoadFunction spawnEmptyRoadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnEmptyRoadFunction, cancellationToken);
        }

        public Task<string> SpawnEmptyRoadRequestAsync(int x, int y)
        {
            var spawnEmptyRoadFunction = new SpawnEmptyRoadFunction();
                spawnEmptyRoadFunction.X = x;
                spawnEmptyRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnEmptyRoadFunction);
        }

        public Task<TransactionReceipt> SpawnEmptyRoadRequestAndWaitForReceiptAsync(int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnEmptyRoadFunction = new SpawnEmptyRoadFunction();
                spawnEmptyRoadFunction.X = x;
                spawnEmptyRoadFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnEmptyRoadFunction, cancellationToken);
        }

        public Task<string> SpawnFinishedRoadRequestAsync(SpawnFinishedRoadFunction spawnFinishedRoadFunction)
        {
             return ContractHandler.SendRequestAsync(spawnFinishedRoadFunction);
        }

        public Task<TransactionReceipt> SpawnFinishedRoadRequestAndWaitForReceiptAsync(SpawnFinishedRoadFunction spawnFinishedRoadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFinishedRoadFunction, cancellationToken);
        }

        public Task<string> SpawnFinishedRoadRequestAsync(byte[] causedBy, int x, int y, byte state)
        {
            var spawnFinishedRoadFunction = new SpawnFinishedRoadFunction();
                spawnFinishedRoadFunction.CausedBy = causedBy;
                spawnFinishedRoadFunction.X = x;
                spawnFinishedRoadFunction.Y = y;
                spawnFinishedRoadFunction.State = state;
            
             return ContractHandler.SendRequestAsync(spawnFinishedRoadFunction);
        }

        public Task<TransactionReceipt> SpawnFinishedRoadRequestAndWaitForReceiptAsync(byte[] causedBy, int x, int y, byte state, CancellationTokenSource cancellationToken = null)
        {
            var spawnFinishedRoadFunction = new SpawnFinishedRoadFunction();
                spawnFinishedRoadFunction.CausedBy = causedBy;
                spawnFinishedRoadFunction.X = x;
                spawnFinishedRoadFunction.Y = y;
                spawnFinishedRoadFunction.State = state;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFinishedRoadFunction, cancellationToken);
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

        public Task<string> SpawnFloraRequestAsync(byte[] player, byte[] entity, int x, int y, byte floraType)
        {
            var spawnFloraFunction = new SpawnFloraFunction();
                spawnFloraFunction.Player = player;
                spawnFloraFunction.Entity = entity;
                spawnFloraFunction.X = x;
                spawnFloraFunction.Y = y;
                spawnFloraFunction.FloraType = floraType;
            
             return ContractHandler.SendRequestAsync(spawnFloraFunction);
        }

        public Task<TransactionReceipt> SpawnFloraRequestAndWaitForReceiptAsync(byte[] player, byte[] entity, int x, int y, byte floraType, CancellationTokenSource cancellationToken = null)
        {
            var spawnFloraFunction = new SpawnFloraFunction();
                spawnFloraFunction.Player = player;
                spawnFloraFunction.Entity = entity;
                spawnFloraFunction.X = x;
                spawnFloraFunction.Y = y;
                spawnFloraFunction.FloraType = floraType;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFloraFunction, cancellationToken);
        }

        public Task<string> SpawnFloraRandomRequestAsync(SpawnFloraRandomFunction spawnFloraRandomFunction)
        {
             return ContractHandler.SendRequestAsync(spawnFloraRandomFunction);
        }

        public Task<TransactionReceipt> SpawnFloraRandomRequestAndWaitForReceiptAsync(SpawnFloraRandomFunction spawnFloraRandomFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFloraRandomFunction, cancellationToken);
        }

        public Task<string> SpawnFloraRandomRequestAsync(byte[] player, byte[] entity, int x, int y)
        {
            var spawnFloraRandomFunction = new SpawnFloraRandomFunction();
                spawnFloraRandomFunction.Player = player;
                spawnFloraRandomFunction.Entity = entity;
                spawnFloraRandomFunction.X = x;
                spawnFloraRandomFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(spawnFloraRandomFunction);
        }

        public Task<TransactionReceipt> SpawnFloraRandomRequestAndWaitForReceiptAsync(byte[] player, byte[] entity, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var spawnFloraRandomFunction = new SpawnFloraRandomFunction();
                spawnFloraRandomFunction.Player = player;
                spawnFloraRandomFunction.Entity = entity;
                spawnFloraRandomFunction.X = x;
                spawnFloraRandomFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnFloraRandomFunction, cancellationToken);
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

        public Task<string> SpawnRoadFromPushRequestAsync(SpawnRoadFromPushFunction spawnRoadFromPushFunction)
        {
             return ContractHandler.SendRequestAsync(spawnRoadFromPushFunction);
        }

        public Task<TransactionReceipt> SpawnRoadFromPushRequestAndWaitForReceiptAsync(SpawnRoadFromPushFunction spawnRoadFromPushFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnRoadFromPushFunction, cancellationToken);
        }

        public Task<string> SpawnRoadFromPushRequestAsync(byte[] causedBy, byte[] pushed, byte[] road, PositionData pos)
        {
            var spawnRoadFromPushFunction = new SpawnRoadFromPushFunction();
                spawnRoadFromPushFunction.CausedBy = causedBy;
                spawnRoadFromPushFunction.Pushed = pushed;
                spawnRoadFromPushFunction.Road = road;
                spawnRoadFromPushFunction.Pos = pos;
            
             return ContractHandler.SendRequestAsync(spawnRoadFromPushFunction);
        }

        public Task<TransactionReceipt> SpawnRoadFromPushRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] pushed, byte[] road, PositionData pos, CancellationTokenSource cancellationToken = null)
        {
            var spawnRoadFromPushFunction = new SpawnRoadFromPushFunction();
                spawnRoadFromPushFunction.CausedBy = causedBy;
                spawnRoadFromPushFunction.Pushed = pushed;
                spawnRoadFromPushFunction.Road = road;
                spawnRoadFromPushFunction.Pos = pos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spawnRoadFromPushFunction, cancellationToken);
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

        public Task<string> SpliceDynamicDataRequestAsync(SpliceDynamicDataFunction spliceDynamicDataFunction)
        {
             return ContractHandler.SendRequestAsync(spliceDynamicDataFunction);
        }

        public Task<TransactionReceipt> SpliceDynamicDataRequestAndWaitForReceiptAsync(SpliceDynamicDataFunction spliceDynamicDataFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spliceDynamicDataFunction, cancellationToken);
        }

        public Task<string> SpliceDynamicDataRequestAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, ulong startWithinField, ulong deleteCount, byte[] data)
        {
            var spliceDynamicDataFunction = new SpliceDynamicDataFunction();
                spliceDynamicDataFunction.TableId = tableId;
                spliceDynamicDataFunction.KeyTuple = keyTuple;
                spliceDynamicDataFunction.DynamicFieldIndex = dynamicFieldIndex;
                spliceDynamicDataFunction.StartWithinField = startWithinField;
                spliceDynamicDataFunction.DeleteCount = deleteCount;
                spliceDynamicDataFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(spliceDynamicDataFunction);
        }

        public Task<TransactionReceipt> SpliceDynamicDataRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, byte dynamicFieldIndex, ulong startWithinField, ulong deleteCount, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var spliceDynamicDataFunction = new SpliceDynamicDataFunction();
                spliceDynamicDataFunction.TableId = tableId;
                spliceDynamicDataFunction.KeyTuple = keyTuple;
                spliceDynamicDataFunction.DynamicFieldIndex = dynamicFieldIndex;
                spliceDynamicDataFunction.StartWithinField = startWithinField;
                spliceDynamicDataFunction.DeleteCount = deleteCount;
                spliceDynamicDataFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spliceDynamicDataFunction, cancellationToken);
        }

        public Task<string> SpliceStaticDataRequestAsync(SpliceStaticDataFunction spliceStaticDataFunction)
        {
             return ContractHandler.SendRequestAsync(spliceStaticDataFunction);
        }

        public Task<TransactionReceipt> SpliceStaticDataRequestAndWaitForReceiptAsync(SpliceStaticDataFunction spliceStaticDataFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spliceStaticDataFunction, cancellationToken);
        }

        public Task<string> SpliceStaticDataRequestAsync(byte[] tableId, List<byte[]> keyTuple, ulong start, byte[] data)
        {
            var spliceStaticDataFunction = new SpliceStaticDataFunction();
                spliceStaticDataFunction.TableId = tableId;
                spliceStaticDataFunction.KeyTuple = keyTuple;
                spliceStaticDataFunction.Start = start;
                spliceStaticDataFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(spliceStaticDataFunction);
        }

        public Task<TransactionReceipt> SpliceStaticDataRequestAndWaitForReceiptAsync(byte[] tableId, List<byte[]> keyTuple, ulong start, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var spliceStaticDataFunction = new SpliceStaticDataFunction();
                spliceStaticDataFunction.TableId = tableId;
                spliceStaticDataFunction.KeyTuple = keyTuple;
                spliceStaticDataFunction.Start = start;
                spliceStaticDataFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(spliceStaticDataFunction, cancellationToken);
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

        public Task<byte[]> StoreVersionQueryAsync(StoreVersionFunction storeVersionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StoreVersionFunction, byte[]>(storeVersionFunction, blockParameter);
        }

        
        public Task<byte[]> StoreVersionQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StoreVersionFunction, byte[]>(null, blockParameter);
        }

        public Task<string> SummonMileRequestAsync(SummonMileFunction summonMileFunction)
        {
             return ContractHandler.SendRequestAsync(summonMileFunction);
        }

        public Task<TransactionReceipt> SummonMileRequestAndWaitForReceiptAsync(SummonMileFunction summonMileFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonMileFunction, cancellationToken);
        }

        public Task<string> SummonMileRequestAsync(byte[] causedBy, bool summonAll)
        {
            var summonMileFunction = new SummonMileFunction();
                summonMileFunction.CausedBy = causedBy;
                summonMileFunction.SummonAll = summonAll;
            
             return ContractHandler.SendRequestAsync(summonMileFunction);
        }

        public Task<TransactionReceipt> SummonMileRequestAndWaitForReceiptAsync(byte[] causedBy, bool summonAll, CancellationTokenSource cancellationToken = null)
        {
            var summonMileFunction = new SummonMileFunction();
                summonMileFunction.CausedBy = causedBy;
                summonMileFunction.SummonAll = summonAll;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonMileFunction, cancellationToken);
        }

        public Task<string> SummonRowRequestAsync(SummonRowFunction summonRowFunction)
        {
             return ContractHandler.SendRequestAsync(summonRowFunction);
        }

        public Task<TransactionReceipt> SummonRowRequestAndWaitForReceiptAsync(SummonRowFunction summonRowFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonRowFunction, cancellationToken);
        }

        public Task<string> SummonRowRequestAsync(byte[] causedBy, int left, int right, BigInteger difficulty)
        {
            var summonRowFunction = new SummonRowFunction();
                summonRowFunction.CausedBy = causedBy;
                summonRowFunction.Left = left;
                summonRowFunction.Right = right;
                summonRowFunction.Difficulty = difficulty;
            
             return ContractHandler.SendRequestAsync(summonRowFunction);
        }

        public Task<TransactionReceipt> SummonRowRequestAndWaitForReceiptAsync(byte[] causedBy, int left, int right, BigInteger difficulty, CancellationTokenSource cancellationToken = null)
        {
            var summonRowFunction = new SummonRowFunction();
                summonRowFunction.CausedBy = causedBy;
                summonRowFunction.Left = left;
                summonRowFunction.Right = right;
                summonRowFunction.Difficulty = difficulty;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(summonRowFunction, cancellationToken);
        }

        public Task<string> SupRequestAsync(SupFunction supFunction)
        {
             return ContractHandler.SendRequestAsync(supFunction);
        }

        public Task<string> SupRequestAsync()
        {
             return ContractHandler.SendRequestAsync<SupFunction>();
        }

        public Task<TransactionReceipt> SupRequestAndWaitForReceiptAsync(SupFunction supFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(supFunction, cancellationToken);
        }

        public Task<TransactionReceipt> SupRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<SupFunction>(null, cancellationToken);
        }

        public Task<string> SwapScrollRequestAsync(SwapScrollFunction swapScrollFunction)
        {
             return ContractHandler.SendRequestAsync(swapScrollFunction);
        }

        public Task<TransactionReceipt> SwapScrollRequestAndWaitForReceiptAsync(SwapScrollFunction swapScrollFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(swapScrollFunction, cancellationToken);
        }

        public Task<string> SwapScrollRequestAsync(byte[] player, int x, int y)
        {
            var swapScrollFunction = new SwapScrollFunction();
                swapScrollFunction.Player = player;
                swapScrollFunction.X = x;
                swapScrollFunction.Y = y;
            
             return ContractHandler.SendRequestAsync(swapScrollFunction);
        }

        public Task<TransactionReceipt> SwapScrollRequestAndWaitForReceiptAsync(byte[] player, int x, int y, CancellationTokenSource cancellationToken = null)
        {
            var swapScrollFunction = new SwapScrollFunction();
                swapScrollFunction.Player = player;
                swapScrollFunction.X = x;
                swapScrollFunction.Y = y;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(swapScrollFunction, cancellationToken);
        }

        public Task<string> TeleportRequestAsync(TeleportFunction teleportFunction)
        {
             return ContractHandler.SendRequestAsync(teleportFunction);
        }

        public Task<TransactionReceipt> TeleportRequestAndWaitForReceiptAsync(TeleportFunction teleportFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(teleportFunction, cancellationToken);
        }

        public Task<string> TeleportRequestAsync(byte[] player, int x, int y, byte actionType)
        {
            var teleportFunction = new TeleportFunction();
                teleportFunction.Player = player;
                teleportFunction.X = x;
                teleportFunction.Y = y;
                teleportFunction.ActionType = actionType;
            
             return ContractHandler.SendRequestAsync(teleportFunction);
        }

        public Task<TransactionReceipt> TeleportRequestAndWaitForReceiptAsync(byte[] player, int x, int y, byte actionType, CancellationTokenSource cancellationToken = null)
        {
            var teleportFunction = new TeleportFunction();
                teleportFunction.Player = player;
                teleportFunction.X = x;
                teleportFunction.Y = y;
                teleportFunction.ActionType = actionType;
            
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

        public Task<string> TickActionRequestAsync(TickActionFunction tickActionFunction)
        {
             return ContractHandler.SendRequestAsync(tickActionFunction);
        }

        public Task<TransactionReceipt> TickActionRequestAndWaitForReceiptAsync(TickActionFunction tickActionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickActionFunction, cancellationToken);
        }

        public Task<string> TickActionRequestAsync(byte[] causedBy, byte[] entity, PositionData entityPos)
        {
            var tickActionFunction = new TickActionFunction();
                tickActionFunction.CausedBy = causedBy;
                tickActionFunction.Entity = entity;
                tickActionFunction.EntityPos = entityPos;
            
             return ContractHandler.SendRequestAsync(tickActionFunction);
        }

        public Task<TransactionReceipt> TickActionRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, PositionData entityPos, CancellationTokenSource cancellationToken = null)
        {
            var tickActionFunction = new TickActionFunction();
                tickActionFunction.CausedBy = causedBy;
                tickActionFunction.Entity = entity;
                tickActionFunction.EntityPos = entityPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickActionFunction, cancellationToken);
        }

        public Task<string> TickBehaviourRequestAsync(TickBehaviourFunction tickBehaviourFunction)
        {
             return ContractHandler.SendRequestAsync(tickBehaviourFunction);
        }

        public Task<TransactionReceipt> TickBehaviourRequestAndWaitForReceiptAsync(TickBehaviourFunction tickBehaviourFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickBehaviourFunction, cancellationToken);
        }

        public Task<string> TickBehaviourRequestAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos)
        {
            var tickBehaviourFunction = new TickBehaviourFunction();
                tickBehaviourFunction.CausedBy = causedBy;
                tickBehaviourFunction.Entity = entity;
                tickBehaviourFunction.Target = target;
                tickBehaviourFunction.EntityPos = entityPos;
                tickBehaviourFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAsync(tickBehaviourFunction);
        }

        public Task<TransactionReceipt> TickBehaviourRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, byte[] target, PositionData entityPos, PositionData targetPos, CancellationTokenSource cancellationToken = null)
        {
            var tickBehaviourFunction = new TickBehaviourFunction();
                tickBehaviourFunction.CausedBy = causedBy;
                tickBehaviourFunction.Entity = entity;
                tickBehaviourFunction.Target = target;
                tickBehaviourFunction.EntityPos = entityPos;
                tickBehaviourFunction.TargetPos = targetPos;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickBehaviourFunction, cancellationToken);
        }

        public Task<string> TickEntityRequestAsync(TickEntityFunction tickEntityFunction)
        {
             return ContractHandler.SendRequestAsync(tickEntityFunction);
        }

        public Task<TransactionReceipt> TickEntityRequestAndWaitForReceiptAsync(TickEntityFunction tickEntityFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickEntityFunction, cancellationToken);
        }

        public Task<string> TickEntityRequestAsync(byte[] causedBy, byte[] entity)
        {
            var tickEntityFunction = new TickEntityFunction();
                tickEntityFunction.CausedBy = causedBy;
                tickEntityFunction.Entity = entity;
            
             return ContractHandler.SendRequestAsync(tickEntityFunction);
        }

        public Task<TransactionReceipt> TickEntityRequestAndWaitForReceiptAsync(byte[] causedBy, byte[] entity, CancellationTokenSource cancellationToken = null)
        {
            var tickEntityFunction = new TickEntityFunction();
                tickEntityFunction.CausedBy = causedBy;
                tickEntityFunction.Entity = entity;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tickEntityFunction, cancellationToken);
        }

        public Task<string> TransferBalanceToAddressRequestAsync(TransferBalanceToAddressFunction transferBalanceToAddressFunction)
        {
             return ContractHandler.SendRequestAsync(transferBalanceToAddressFunction);
        }

        public Task<TransactionReceipt> TransferBalanceToAddressRequestAndWaitForReceiptAsync(TransferBalanceToAddressFunction transferBalanceToAddressFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferBalanceToAddressFunction, cancellationToken);
        }

        public Task<string> TransferBalanceToAddressRequestAsync(byte[] fromNamespaceId, string toAddress, BigInteger amount)
        {
            var transferBalanceToAddressFunction = new TransferBalanceToAddressFunction();
                transferBalanceToAddressFunction.FromNamespaceId = fromNamespaceId;
                transferBalanceToAddressFunction.ToAddress = toAddress;
                transferBalanceToAddressFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferBalanceToAddressFunction);
        }

        public Task<TransactionReceipt> TransferBalanceToAddressRequestAndWaitForReceiptAsync(byte[] fromNamespaceId, string toAddress, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferBalanceToAddressFunction = new TransferBalanceToAddressFunction();
                transferBalanceToAddressFunction.FromNamespaceId = fromNamespaceId;
                transferBalanceToAddressFunction.ToAddress = toAddress;
                transferBalanceToAddressFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferBalanceToAddressFunction, cancellationToken);
        }

        public Task<string> TransferBalanceToNamespaceRequestAsync(TransferBalanceToNamespaceFunction transferBalanceToNamespaceFunction)
        {
             return ContractHandler.SendRequestAsync(transferBalanceToNamespaceFunction);
        }

        public Task<TransactionReceipt> TransferBalanceToNamespaceRequestAndWaitForReceiptAsync(TransferBalanceToNamespaceFunction transferBalanceToNamespaceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferBalanceToNamespaceFunction, cancellationToken);
        }

        public Task<string> TransferBalanceToNamespaceRequestAsync(byte[] fromNamespaceId, byte[] toNamespaceId, BigInteger amount)
        {
            var transferBalanceToNamespaceFunction = new TransferBalanceToNamespaceFunction();
                transferBalanceToNamespaceFunction.FromNamespaceId = fromNamespaceId;
                transferBalanceToNamespaceFunction.ToNamespaceId = toNamespaceId;
                transferBalanceToNamespaceFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferBalanceToNamespaceFunction);
        }

        public Task<TransactionReceipt> TransferBalanceToNamespaceRequestAndWaitForReceiptAsync(byte[] fromNamespaceId, byte[] toNamespaceId, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferBalanceToNamespaceFunction = new TransferBalanceToNamespaceFunction();
                transferBalanceToNamespaceFunction.FromNamespaceId = fromNamespaceId;
                transferBalanceToNamespaceFunction.ToNamespaceId = toNamespaceId;
                transferBalanceToNamespaceFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferBalanceToNamespaceFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(byte[] namespaceId, string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NamespaceId = namespaceId;
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(byte[] namespaceId, string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NamespaceId = namespaceId;
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
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

        public Task<string> TriggerTicksRequestAsync(TriggerTicksFunction triggerTicksFunction)
        {
             return ContractHandler.SendRequestAsync(triggerTicksFunction);
        }

        public Task<TransactionReceipt> TriggerTicksRequestAndWaitForReceiptAsync(TriggerTicksFunction triggerTicksFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(triggerTicksFunction, cancellationToken);
        }

        public Task<string> TriggerTicksRequestAsync(byte[] causedby)
        {
            var triggerTicksFunction = new TriggerTicksFunction();
                triggerTicksFunction.Causedby = causedby;
            
             return ContractHandler.SendRequestAsync(triggerTicksFunction);
        }

        public Task<TransactionReceipt> TriggerTicksRequestAndWaitForReceiptAsync(byte[] causedby, CancellationTokenSource cancellationToken = null)
        {
            var triggerTicksFunction = new TriggerTicksFunction();
                triggerTicksFunction.Causedby = causedby;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(triggerTicksFunction, cancellationToken);
        }

        public Task<string> UnregisterStoreHookRequestAsync(UnregisterStoreHookFunction unregisterStoreHookFunction)
        {
             return ContractHandler.SendRequestAsync(unregisterStoreHookFunction);
        }

        public Task<TransactionReceipt> UnregisterStoreHookRequestAndWaitForReceiptAsync(UnregisterStoreHookFunction unregisterStoreHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unregisterStoreHookFunction, cancellationToken);
        }

        public Task<string> UnregisterStoreHookRequestAsync(byte[] tableId, string hookAddress)
        {
            var unregisterStoreHookFunction = new UnregisterStoreHookFunction();
                unregisterStoreHookFunction.TableId = tableId;
                unregisterStoreHookFunction.HookAddress = hookAddress;
            
             return ContractHandler.SendRequestAsync(unregisterStoreHookFunction);
        }

        public Task<TransactionReceipt> UnregisterStoreHookRequestAndWaitForReceiptAsync(byte[] tableId, string hookAddress, CancellationTokenSource cancellationToken = null)
        {
            var unregisterStoreHookFunction = new UnregisterStoreHookFunction();
                unregisterStoreHookFunction.TableId = tableId;
                unregisterStoreHookFunction.HookAddress = hookAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unregisterStoreHookFunction, cancellationToken);
        }

        public Task<string> UnregisterSystemHookRequestAsync(UnregisterSystemHookFunction unregisterSystemHookFunction)
        {
             return ContractHandler.SendRequestAsync(unregisterSystemHookFunction);
        }

        public Task<TransactionReceipt> UnregisterSystemHookRequestAndWaitForReceiptAsync(UnregisterSystemHookFunction unregisterSystemHookFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unregisterSystemHookFunction, cancellationToken);
        }

        public Task<string> UnregisterSystemHookRequestAsync(byte[] systemId, string hookAddress)
        {
            var unregisterSystemHookFunction = new UnregisterSystemHookFunction();
                unregisterSystemHookFunction.SystemId = systemId;
                unregisterSystemHookFunction.HookAddress = hookAddress;
            
             return ContractHandler.SendRequestAsync(unregisterSystemHookFunction);
        }

        public Task<TransactionReceipt> UnregisterSystemHookRequestAndWaitForReceiptAsync(byte[] systemId, string hookAddress, CancellationTokenSource cancellationToken = null)
        {
            var unregisterSystemHookFunction = new UnregisterSystemHookFunction();
                unregisterSystemHookFunction.SystemId = systemId;
                unregisterSystemHookFunction.HookAddress = hookAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unregisterSystemHookFunction, cancellationToken);
        }

        public Task<string> UpdateChunkRequestAsync(UpdateChunkFunction updateChunkFunction)
        {
             return ContractHandler.SendRequestAsync(updateChunkFunction);
        }

        public Task<TransactionReceipt> UpdateChunkRequestAndWaitForReceiptAsync(UpdateChunkFunction updateChunkFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateChunkFunction, cancellationToken);
        }

        public Task<string> UpdateChunkRequestAsync(byte[] causedBy)
        {
            var updateChunkFunction = new UpdateChunkFunction();
                updateChunkFunction.CausedBy = causedBy;
            
             return ContractHandler.SendRequestAsync(updateChunkFunction);
        }

        public Task<TransactionReceipt> UpdateChunkRequestAndWaitForReceiptAsync(byte[] causedBy, CancellationTokenSource cancellationToken = null)
        {
            var updateChunkFunction = new UpdateChunkFunction();
                updateChunkFunction.CausedBy = causedBy;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateChunkFunction, cancellationToken);
        }

        public Task<string> WalkRequestAsync(WalkFunction walkFunction)
        {
             return ContractHandler.SendRequestAsync(walkFunction);
        }

        public Task<TransactionReceipt> WalkRequestAndWaitForReceiptAsync(WalkFunction walkFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(walkFunction, cancellationToken);
        }

        public Task<string> WalkRequestAsync(int x, int y, int distance)
        {
            var walkFunction = new WalkFunction();
                walkFunction.X = x;
                walkFunction.Y = y;
                walkFunction.Distance = distance;
            
             return ContractHandler.SendRequestAsync(walkFunction);
        }

        public Task<TransactionReceipt> WalkRequestAndWaitForReceiptAsync(int x, int y, int distance, CancellationTokenSource cancellationToken = null)
        {
            var walkFunction = new WalkFunction();
                walkFunction.X = x;
                walkFunction.Y = y;
                walkFunction.Distance = distance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(walkFunction, cancellationToken);
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

        public Task<byte[]> WorldVersionQueryAsync(WorldVersionFunction worldVersionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WorldVersionFunction, byte[]>(worldVersionFunction, blockParameter);
        }

        
        public Task<byte[]> WorldVersionQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WorldVersionFunction, byte[]>(null, blockParameter);
        }
    }
}

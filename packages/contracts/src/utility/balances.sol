pragma solidity ^0.8.0;

contract WithdrawalContract {
    mapping(address => uint) public balances;

    function deposit() public payable {
        balances[msg.sender] += msg.value;
    }

    function withdraw() public {
        uint amount = balances[msg.sender];
        require(amount > 0, "No funds to withdraw");

        // Following the Checks-Effects-Interactions Pattern
        // Check
        require(address(this).balance >= amount, "Insufficient contract balance");

        // Effects
        balances[msg.sender] = 0;

        // Interactions
        (bool success, ) = msg.sender.call{value: amount}("");
        require(success, "Transfer failed.");
    }
}
pragma solidity >=0.8.21;

library GaulItems {
    
    struct GaulItem {
        uint cost; 
        uint gem; 
        uint eth; 
        uint level; 
    }
}

contract ItemContract {
    // uint[4] public numbers = [
    //     0,
    //     0,
    //     0,
    //     0
    // ];

    // Items.GaulItem item = Items.GaulItem(0,0,0,0);

    //apparently we cant do this
    // Items.GaulItem[] itinerary = [
    //     Items.GaulItem(0,0,0,0),
    //     Items.GaulItem(0,0,0,0),
    //     Items.GaulItem(0,0,0,0),
    //     Items.GaulItem(0,0,0,0)
    // ];
}

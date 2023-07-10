
# Swap and NFT backend API

Functions:
homeControlle with the path: ""

Examle:
https://qweq.ir/health => Returns {status:"success"}

Endpoitn:

### // Fee => Applied fee  
"/fee" => Returns {
    "status": "success",
    "data": {
        "isFeeTaken": false,
        "feeRecipientAddress": false,
        "feePercentage": "0.01"
    }
}



### // Get current market price for a pair
"/pairPrice/Chain/payToke}/receiveToken/value/slippage" => Returns {
    "status": "success",
    "data": {
        "chainId": 56,
        "price": "0.99739153609438425",
        "guaranteedPrice": "0.947521959289665",
        "estimatedPriceImpact": "0.3357",
        "to": "0xdef1c0ded9bec7f1a1670819833240f027b25eff",
        "data": "0xc43c9ef6000000000000........."
    }
    .......
}


### // record info
[HttpPost] => object data needed to be sent
"/record/title/cat" => { status = "success", id = newId } or { status = "error", message = ex.Message }


### // get all records
"/allrecord" => Returns { status = "success", data = records } or { status = "error", message = ex.Message }

### // delete a record
"delRecord/id" => Returns { status = "success", data = records } or { status = "error", message = ex.Message } or { status = "error", message="Record not found." }


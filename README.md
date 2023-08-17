# swapSwrver - Swap and NFT Backend API

Welcome to the swapSwrver repository! This backend API serves as the foundation for Swap and NFT-related operations, catering to a seamless and efficient user experience. The API provides endpoints to facilitate various functionalities, including fee retrieval, pair price calculation, record management, and more.

  
## Table of Contents

- [Introduction](#introduction)
- [Endpoints](#endpoints)
- [User Interface](#user-interface)
- [License](#license)
- [Contact](#contact)

 
## Introduction

swapSwrver is a backend API developed to support cryptocurrency swapping and NFT-related tasks. It offers a range of endpoints that allow users to retrieve fees, calculate pair prices, manage records, and more. This API is designed to be utilized by various projects within the blockchain ecosystem.

 
## Endpoints

The API provides the following endpoints for different functionalities:

- **GET /fee**: Retrieve the applied fee information.
- **GET /pairPrice/{chain}/{payToken}/{receiveToken}/{value}/{slippage}**: Calculate the current market price for a specified pair.
- **POST /record/{title}/{cat}**: Record information and get a new ID.
- **GET /allrecord**: Get all records.
- **DELETE /delRecord/{id}**: Delete a specific record.

Please refer to the [API documentation](/api-documentation.md) for detailed information about each endpoint, including input parameters, expected responses, and usage examples.

 
## User Interface

To utilize the Swap user interface, follow these steps:

Access the official Swap website: https://matthewshelby.github.io/swap.
Connect your Ethereum-compatible wallet to the platform.
Select the paying and receiving currencies, set the desired amount, and follow the on-screen instructions.

## License

This project is not licensed and all its contents are under copyright. All rights are reserved.

 
## Contact

For technical queries: [matthewShelB@gmail.com](mailto:matthewShelB@gmail.com)


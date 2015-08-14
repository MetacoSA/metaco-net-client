# Metaco API client for .NET [![Build Status](https://travis-ci.org/MetacoSA/metaco-net-client.svg?branch=master)](https://travis-ci.org/MetacoSA/metaco-net-client)

[Metaco](https://metaco.com) REST API provides a set of services to integrate Metaco into third-party applications. It offers trading and payment facilities as well as wallet management features.

Our .NET Client implements every single functionality of the API.
You can find a detailed documentation here : [API Documentation](http://docs.metaco.apiary.io/).

Installation
----------------------------------------------

### Using NuGet

With nuget :
> **Install-Package MetacoClient** 

Go on the [nuget website](https://www.nuget.org/packages/MetacoClient/) for more information.


Usage
----------------------------------------------

You can use our [Unit tests](https://github.com/MetacoSA/metaco-net-client/tree/master/MetacoClient.Tests) to learn the basics or the links in the summary of this document.

Testing
----------------------------------------------
The tests requires a testnet environnement to work.

Define the following appSetings in the MetacoClient.Tests' app.config file:

* METACO_ENV_API_ID : Your testnet API ID
* METACO_ENV_API_KEY : Your testnet API Key
* METACO_ENV_API_URL : http://api.testnet.metaco.com/v1/ (Or the endpoint you want to run your tests with)
* METACO_ENV_WALLET_PRIVATE_KEY_HEX : The private key of your testnet wallet (hex-encoded)

And run the tests

Known Issues / Gotcha
----------------------------------------------
* The api client is still unstable.

License
----------------------------------------------
MIT (See LICENSE).


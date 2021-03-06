# Deribit.S4KTNET

Deribit Websocket library for .NET.

## Functionality
* Supports Api V2.0.0
* Supports WebSocket Api
* Does not support rest or FIX api

## Links
* https://docs.deribit.com/v2/#deribit-api-v2-0-0
* https://test.deribit.com/ 

# Installation

#### Nuget packages:

* https://www.nuget.org/packages/Deribit.S4KTNET.Core/

# Usage
* Namespaces
```C#
using Deribit.S4KTNET.Core;
using Deribit.S4KTNET.Core.Authentication;
using Deribit.S4KTNET.Core.SubscriptionManagement;
using Deribit.S4KTNET.Core.SessionManagement;
using Deribit.S4KTNET.Core.Trading;
```
* Configure
```C#
var deribitconfig = new DeribitConfig
{
    Environment = DeribitEnvironment.Live,
    // ConnectOnConstruction // automatically connect
    // EnableJsonRpcTracing // troubleshooting jsonrpc
    // NoRefreshAuthToken // do not automatically refresh authentication token
    // NoRespondHeartbeats // do not automatically response to hearbeats,
};
```
* Construct client object
```C#
var deribit = new DeribitService(deribitconfig);
```
* Connect
```C#
await deribit.Connect();
```
* Test connection
```C#
await deribit.Supporting.Test(new TestRequest());
```
* Enable heartbeats
```C#
deribit.SessionManagement.SetHeartbeat(new SetHeartbeatRequest()
{
    interval = 10, // seconds
});
```
* Subscribe to data streams
```C#
// subscribe to BTC perp quotes
await deribit.SubscriptionManagement.SubscribePublic(new SubscribeRequest()
{
    channels = new string[]
    {
        DeribitSubscriptions.quote(DeribitInstruments.Perpetual.BTCPERPETUAL),
    },
});
// listen to stream
deribit.SubscriptionManagement.QuoteStream.Subscribe(quoteObserver);
```
* Authenticate
```C#
deribit.Authentication.Auth(new AuthRequest()
{
    grant_type = GrantType.client_credentials,
    client_id = "<yourapikey>",
    client_secret = "<yourapisecret>",
});
```
* Submit orders
```C#
await deribit.Trading.buy(new BuySellRequest()
{
    instrument_name = DeribitInstruments.Perpetual.BTCPERPETUAL,
    type = OrderType.limit,
    amount = 10,
    price = 1234,
});
```
* Disconnect
```C#
deribit.Dispose();
```

## Sample Code

See the sample client `Deribit.S4KTNET.Sample`.

# Coverage

## Rest Api

| Api | Method | Covered | Notes |
| --- | ---- | ------- | ----- |
| Authentication | *   | ✗ | unsupported |
| SessionManagement | *   | ✗ | unsupported |
| Supporting | *   | ✗ | unsupported |
| Subscriptions | *   | ✗ | unsupported |
| AccountManagement | *   | ✗ | unsupported |
| Trading | *   | ✗ | unsupported |
| MarketData | *   | ✗ | unsupported |
| Wallet | *   | ✗ | unsupported |
| Notifications | *   | ✗ | unsupported |

## WebSocket Api

| Api | Method | Covered | Notes |
| --- | ------ | ------- | ----- |
| Authentication | [public/auth](https://docs.deribit.com/v2/#public-auth) | ○ | signed auth not working |
| Authentication | [private/logout](https://docs.deribit.com/v2/#private-logout) | ✔ | |
| SessionManagement | [public/set_heartbeat](https://docs.deribit.com/v2/#session-management) | ✔ | |
| SessionManagement | [public/disable_heartbeat](https://docs.deribit.com/v2/#public-disable_heartbeat) | ✔ | |
| SessionManagement | [private/enable_cancel_on_disconnect](https://docs.deribit.com/v2/#private-enable_cancel_on_disconnect) | ✔ | |
| SessionManagement | [private/disable_cancel_on_disconnect](https://docs.deribit.com/v2/#private-disable_cancel_on_disconnect) | ✔ | |
| Supporting | [public/hello](https://docs.deribit.com/v2/#public-hello) | ✔ | |
| Supporting | [public/get_time](https://docs.deribit.com/v2/#public-get_time) | ✔ | |
| Supporting | [public/test](https://docs.deribit.com/v2/#public-test) | ✔ | |
| Subscriptions | [public/subscribe](https://docs.deribit.com/v2/#public-subscribe) | ✔ | |
| Subscriptions | [public/unsubscribe](https://docs.deribit.com/v2/#public-unsubscribe) | ✔ | |
| Subscriptions | [private/subscribe](https://docs.deribit.com/v2/#private-subscribe) | ✔ | |
| Subscriptions | [private/unsubscribe](https://docs.deribit.com/v2/#private-unsubscribe) | ✔ | |
| AccountManagement | [public/get_announcements](https://docs.deribit.com/v2/#public-get_announcements) | ✗ | |
| AccountManagement | private/change_subaccount_name | ✗ | |
| AccountManagement | private/create_subaccount | ✗ | |
| AccountManagement | private/diable_tfa_for_subaccount | ✗ | |
| AccountManagement | private/get_account_summary | ✔ | |
| AccountManagement | private/get_email_language | ✗ | |
| AccountManagement | private/get_new_announcements | ✗ | |
| AccountManagement | [private/get_position](https://docs.deribit.com/v2/#private-get_position) | ✔ | |
| AccountManagement | [private/get_positions](https://docs.deribit.com/v2/#private-get_positions) | ✔ | |
| AccountManagement | [private/get_subaccounts](https://docs.deribit.com/v2/#private-get_subaccounts) | ✗ | todo |
| AccountManagement | private/set_announcement_as_read | ✗ | |
| AccountManagement | private/set_email_for_subaccount | ✗ | |
| AccountManagement | private/set_email_language | ✗ | |
| AccountManagement | private/set_password_for_subaccount | ✗ | |
| AccountManagement | private/toggle_notifications_from_subaccount | ✗ | |
| AccountManagement | private/toggle_subaccount_login | ✗ | |
| Trading | [private/buy](https://docs.deribit.com/v2/#private-buy) | ✔ | |
| Trading | [private/sell](https://docs.deribit.com/v2/#private-sell) | ✔ | |
| Trading | [private/edit](https://docs.deribit.com/v2/#private-edit) | ✔ | |
| Trading | [private/cancel](https://docs.deribit.com/v2/#private-cancel) | ✔ | |
| Trading | [private/cancel_all](https://docs.deribit.com/v2/#private-cancel_all) | ✔ | |
| Trading | [private/cancel_all_by_currency](https://docs.deribit.com/v2/#private-cancel_all_by_currency) | ✔ | |
| Trading | [private/cancel_all_by_instrument](https://docs.deribit.com/v2/#private-cancel_all_by_instrument) | ✔ | |
| Trading | [private/close_position](https://docs.deribit.com/v2/#private-close_position) | ✔ | |
| Trading | [private/get_margins](https://docs.deribit.com/v2/#private-get_margins) | ✔ | |
| Trading | [private/get_open_orders_by_currency](https://docs.deribit.com/v2/#private-get_open_orders_by_currency) | ✔ | |
| Trading | [private/get_open_orders_by_instrument](https://docs.deribit.com/v2/#private-get_open_orders_by_instrument) | ✔ | |
| Trading | [private/get_order_history_by_currency](https://docs.deribit.com/v2/#private-get_order_history_by_currency) | ✔ | |
| Trading | [private/get_order_history_by_instrument](https://docs.deribit.com/v2/#private-get_order_history_by_instrument) | ✔ | |
| Trading | [private/get_order_margin_by_ids](https://docs.deribit.com/v2/#private-get_order_margin_by_ids) | ✗ | todo |
| Trading | [private/get_order_state](https://docs.deribit.com/v2/#private-get_order_state) | ✔ | |
| Trading | [private/get_stop_order_history](https://docs.deribit.com/v2/#private-get_stop_order_history) | ✗ | todo |
| Trading | [private/get_user_trades_by_currency](https://docs.deribit.com/v2/#private-get_user_trades_by_currency) | ✗ | todo |
| Trading | [private/get_user_trades_by_currency_and_time](https://docs.deribit.com/v2/#private-get_user_trades_by_currency_and_time) | ✗ | todo |
| Trading | [private/get_user_trades_by_instrument](https://docs.deribit.com/v2/#private-get_user_trades_by_instrument) | ✔ | |
| Trading | [private/get_user_trades_by_instrument_and_time](https://docs.deribit.com/v2/#private-get_user_trades_by_instrument_and_time) | ✗ | todo |
| Trading | [private/get_user_trades_by_order](https://docs.deribit.com/v2/#private-get_user_trades_by_order) | ✔ | |
| Trading | [private/get_settlement_history_by_instrument](https://docs.deribit.com/v2/#private-get_settlement_history_by_instrument) | ✗ | |
| Trading | [private/get_settlement_history_by_currency](https://docs.deribit.com/v2/#private-get_settlement_history_by_currency) | ✗ | |
| MarketData | [public/get_book_summary_by_currency](https://docs.deribit.com/v2/#public-get_book_summary_by_currency) | ✗ | todo |
| MarketData | [public/get_book_summary_by_instrument](https://docs.deribit.com/v2/#public-get_book_summary_by_instrument) | ✔ | |
| MarketData | [public/get_contract_size](https://docs.deribit.com/v2/#public-get_contract_size) | ✔ | |
| MarketData | [public/get_currencies](https://docs.deribit.com/v2/#public-get_currencies) | ✔ | |
| MarketData | [public/get_funding_chart_data](https://docs.deribit.com/v2/#public-get_funding_chart_data) | ✗ | todo |
| MarketData | [public/get_historical_volatility](https://docs.deribit.com/v2/#public-get_historical_volatility) | ✗ | todo |
| MarketData | [public/get_index](https://docs.deribit.com/v2/#public-get_index) | ✔ | |
| MarketData | [public/get_instruments](https://docs.deribit.com/v2/#public-get_instruments) | ✔ | |
| MarketData | [public/get_last_settlements_by_currency](https://docs.deribit.com/v2/#public-get_last_settlements_by_currency) | ✗ | todo |
| MarketData | [public/get_last_settlements_by_instrument](https://docs.deribit.com/v2/#public-get_last_settlements_by_instrument) | ✗ | todo |
| MarketData | [public/get_last_trades_by_currency](https://docs.deribit.com/v2/#public-get_last_trades_by_currency) | ✗ | todo |
| MarketData | [public/get_last_trades_by_currency_and_time](https://docs.deribit.com/v2/#public-get_last_trades_by_currency_and_time) | ✗ | todo |
| MarketData | [public/get_last_trades_by_instrument](https://docs.deribit.com/v2/#public-get_last_trades_by_instrument) | ✔ | |
| MarketData | [public/get_last_trades_by_instrument_and_time](https://docs.deribit.com/v2/#public-get_last_trades_by_instrument_and_time) | ✗ | todo |
| MarketData | [public/get_order_book](https://docs.deribit.com/v2/#public-get_order_book) | ✔ | |
| MarketData | [public/get_trade_volumes](https://docs.deribit.com/v2/#public-get_trade_volumes) | ✗ | todo |
| MarketData | [public/get_tradingview_chart_data](https://docs.deribit.com/v2/#public-get_tradingview_chart_data) | ✗ | |
| MarketData | [public/ticker](https://docs.deribit.com/v2/#public-ticker) | ✔ | |
| Wallet | [private/cancel_transfer_by_id](https://docs.deribit.com/v2/#private-cancel_transfer_by_id) | ✗ | |
| Wallet | [private/cancel_withdrawal](https://docs.deribit.com/v2/#private-cancel_withdrawal) | ✗ | |
| Wallet | [private/create_deposit_address](https://docs.deribit.com/v2/#private-create_deposit_address) | ✗ | |
| Wallet | [private/get_current_deposit_address](https://docs.deribit.com/v2/#private-get_current_deposit_address) | ✗ | |
| Wallet | [private/get_deposits](https://docs.deribit.com/v2/#private-get_deposits) | ✗ | |
| Wallet | [private/get_transfers](https://docs.deribit.com/v2/#private-get_transfers) | ✗ | |
| Wallet | [private/get_withdrawals](https://docs.deribit.com/v2/#private-get_withdrawals) | ✗ | |
| Wallet | [private/withdraw](https://docs.deribit.com/v2/#private-withdraw) | ✗ | |
| Notifications | [announcements](https://docs.deribit.com/v2/#announcements) | ○ | partially tested |
| Notifications | [book.{instrument_name}.{group}.{depth}.{interval}](https://docs.deribit.com/v2/#book-instrument_name-group-depth-interval) | ✔ | |
| Notifications | [book.{instrument_name}.{interval}](https://docs.deribit.com/v2/#book-instrument_name-interval) | ✔ |  |
| Notifications | [deribit_price_index.{index_name}](https://docs.deribit.com/v2/#deribit_price_index-index_name) | ✔ |  |
| Notifications | [deribit_price_ranking.{index_name}](https://docs.deribit.com/v2/#deribit_price_ranking-index_name) | ✗ | todo |
| Notifications | [estimated_expiration_price.{index_name}](https://docs.deribit.com/v2/#estimated_expiration_price-index_name) | ✗ | todo |
| Notifications | [markprice.options.{index_name}](https://docs.deribit.com/v2/#markprice-options-index_name) | ✗ | todo |
| Notifications | [perpetual.{instrument_name}.{interval}](https://docs.deribit.com/v2/#perpetual-instrument_name-interval) | ✗ | todo |
| Notifications | [quote.{instrument_name}](https://docs.deribit.com/v2/#quote-instrument_name) | ✔ |  |
| Notifications | [ticker.{instrument_name}.{interval}](https://docs.deribit.com/v2/#ticker-instrument_name-interval) | ✔ |  |
| Notifications | [trades.{instrument_name}.{interval}](https://docs.deribit.com/v2/#trades-instrument_name-interval) | ✔ | |
| Notifications | [user.orders.{instrument_name}.{interval}](https://docs.deribit.com/v2/#user-orders-instrument_name-interval) | ✔ | |
| Notifications | [user.orders.{kind}.{currency}.{interval}](https://docs.deribit.com/v2/#user-orders-kind-currency-interval) | ✔ | |
| Notifications | [user.portfolio.{currency}](https://docs.deribit.com/v2/#user-portfolio-currency) | ✔ | |
| Notifications | [user.trades.{instrument_name}.{interval}](https://docs.deribit.com/v2/#user-trades-instrument_name-interval) | ✔ | |
| Notifications | [user.trades.{kind}.{currency}.{interval}](https://docs.deribit.com/v2/#user-trades-kind-currency-interval) | ✔ | |



# Authentication

Private methods require authentication. The library attempts to support all authentication flows available.

`SecureString` is not supported, as the credentials are exposed in memory through the websocket libraries anyway.

## Authentication - via username/password

This flow is not recommended.

```C#
// form request
var request = new AuthRequest()
{
    grant_type = GrantType.password,
    username = "<yourusername>",
    password = "<yourpassword>",
}
// execute request
deribit.Authentication.Auth(request);
```
## Authentication - via Api Key

This is the recommended method.
```C#
// form request
var request = new AuthRequest()
{
    grant_type = GrantType.client_credentials,
    client_id = "<yourapikey>",
    client_secret = "<yourapisecret>",
}
// execute request
deribit.Authentication.Auth(request);
```
## Authentication - via signed client credentials

**This flow is not computing the correct signature, for reasons unknown. PR fix welcome.**

This is the most secure option available.
Client signatures are computed as per [documentation](https://docs.deribit.com/v2/#authentication).

```C#
// form request
var request = new AuthRequest()
{
    grant_type = GrantType.client_credentials,
    client_id = "<yourapikey>",
    client_secret = "yourapisecret>",
};
// sign the request
request = request.Sign();
// execute request
deribit.Authentication.Auth(request);

```


## Authentication - via refresh token

Token refresh is handled by the library.
If a valid refresh token is available, the library will periodically refresh the auth token every 15m.
Note that Deribit seems to grant auth tokens with extended lifetimes (several months) which I do not understand; this functionality may not be required.

If required, you can refresh manually:
```C#
var request = new AuthRequest()
{
    grant_type = GrantType.refresh_token,
    refresh_token = "<refresh_token>",
}
```

You can disable auto refresh through the config object
```C#
new DeribitConfig()
{
    NoRefreshAuthToken = true,
}
```

# Heartbeats

The client automatically responds to heartbeat requests.
But you must enable heartbeats yourself:
```C#
deribit.SessionManagement.SetHeartbeat(new SetHeartbeatRequest()
{
    interval = 10, // 10 seconds
})
```

# Stream Multi-Threading

Rx observables are synchronous by default. That means your observer code is called on the same thread the message is received on the websocket feed.
Be sure to unblock the thread as soon as possible.

Rx supports customizable stream synchronization:
* http://introtorx.com/Content/v1.0.10621.0/15_SchedulingAndThreading.html

# Performance

This library was written with extensive defensive programming through validation checks, at the expense of cpu/memory performance. If you need the extra performance you can remove this validation. I can help with that.

# Security

Take appropriate security measures to protect yourself from malicious code:
* Do not trust the binaries. Compile from source.
* Do not store plaintext api secrets in source control.
﻿using Autofac;
using AutoMapper;
using Deribit.S4KTNET.Core.JsonRpc;
using FluentValidation;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;
using System.Linq;

namespace Deribit.S4KTNET.Core.SubscriptionManagement
{
    public interface IDeribitSubscriptionManagementService
    {
        //------------------------------------------------------------------------------------------------
        // subscription streams
        //------------------------------------------------------------------------------------------------
        IObservable<AnnouncementsNotification> AnnouncementsStream { get; }
        IObservable<BookDepthLimitedNotification> BookDepthLimitedStream { get; }
        IObservable<BookFullNotification> BookFullStream { get; }
        IObservable<DeribitPriceIndexNotification> DeribitPriceIndexStream { get; }
        IObservable<QuoteNotification> QuoteStream { get; }
        IObservable<TickerNotification> TickerStream { get; }
        IObservable<TradeNotification> TradeStream { get; }
        IObservable<UserOrdersNotification> UserOrdersStream { get; }
        IObservable<UserPortfolioNotification> UserPortfolioStream { get; }
        IObservable<UserTradesNotification> UserTradesStream { get; }
        //------------------------------------------------------------------------------------------------
        // subscribe / unsubscribe
        //------------------------------------------------------------------------------------------------
        Task<SubscribeResponse> SubscribePublic(SubscribeRequest request, CancellationToken ct = default);
        Task<UnsubscribeResponse> UnsubscribePublic(UnsubscribeRequest request, CancellationToken ct = default);
        Task<SubscribeResponse> SubscribePrivate(SubscribeRequest request, CancellationToken ct = default);
        Task<UnsubscribeResponse> UnsubscribePrivate(UnsubscribeRequest request, CancellationToken ct = default);
        //------------------------------------------------------------------------------------------------
    }

    internal class DeribitSubscriptionManagementService : IDeribitSubscriptionManagementService, IDisposable
    {
        //------------------------------------------------------------------------------------------------
        // streams
        //------------------------------------------------------------------------------------------------

        public IObservable<AnnouncementsNotification> AnnouncementsStream => AnnouncementsSubject;
        private readonly ISubject<AnnouncementsNotification> AnnouncementsSubject = new Subject<AnnouncementsNotification>();

        public IObservable<BookDepthLimitedNotification> BookDepthLimitedStream => BookDepthLimitedSubject;
        private readonly ISubject<BookDepthLimitedNotification> BookDepthLimitedSubject = new Subject<BookDepthLimitedNotification>();

        public IObservable<BookFullNotification> BookFullStream => BookFullSubject;
        private readonly ISubject<BookFullNotification> BookFullSubject = new Subject<BookFullNotification>();

        public IObservable<DeribitPriceIndexNotification> DeribitPriceIndexStream => DeribitPriceIndexSubject;
        private readonly ISubject<DeribitPriceIndexNotification> DeribitPriceIndexSubject = new Subject<DeribitPriceIndexNotification>();

        public IObservable<QuoteNotification> QuoteStream => QuoteSubject;
        private readonly ISubject<QuoteNotification> QuoteSubject = new Subject<QuoteNotification>();

        public IObservable<TickerNotification> TickerStream => TickerSubject;
        private readonly ISubject<TickerNotification> TickerSubject = new Subject<TickerNotification>();

        public IObservable<TradeNotification> TradeStream => TradeSubject;
        private readonly ISubject<TradeNotification> TradeSubject = new Subject<TradeNotification>();

        public IObservable<UserOrdersNotification> UserOrdersStream => UserOrdersSubject;
        private readonly ISubject<UserOrdersNotification> UserOrdersSubject = new Subject<UserOrdersNotification>();

        public IObservable<UserPortfolioNotification> UserPortfolioStream => UserPortfolioSubject;
        private readonly ISubject<UserPortfolioNotification> UserPortfolioSubject = new Subject<UserPortfolioNotification>();

        public IObservable<UserTradesNotification> UserTradesStream => UserTradesSubject;
        private readonly ISubject<UserTradesNotification> UserTradesSubject = new Subject<UserTradesNotification>();

        //------------------------------------------------------------------------------------------------
        // dependencies
        //------------------------------------------------------------------------------------------------

        private readonly DeribitService deribit;
        private readonly IMapper mapper;
        private readonly IDeribitJsonRpcService jsonrpc;

        //------------------------------------------------------------------------------------------------
        // fields
        //------------------------------------------------------------------------------------------------

        private readonly JsonSerializer jsonser = new JsonSerializer();

        //------------------------------------------------------------------------------------------------
        // locks
        //------------------------------------------------------------------------------------------------

        private readonly object notificationhandlesynclock = new object();

        //------------------------------------------------------------------------------------------------
        // construction
        //------------------------------------------------------------------------------------------------

        public DeribitSubscriptionManagementService(DeribitService deribit, IMapper mapper,
            IDeribitJsonRpcService jsonrpc)
        {
            this.deribit = deribit;
            this.mapper = mapper;
            this.jsonrpc = jsonrpc;
            this.jsonrpc.ReconnectionHappened += this.Jsonrpc_ReconnectionHappened;
        }

        //------------------------------------------------------------------------------------------------
        // module
        //------------------------------------------------------------------------------------------------

        internal class Module : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<DeribitSubscriptionManagementService>()
                    .AsSelf()
                    .As<IDeribitSubscriptionManagementService>()
                    .SingleInstance();
            }
        }

        //------------------------------------------------------------------------------------------------
        // disposal
        //------------------------------------------------------------------------------------------------

        public void Dispose()
        {
            this.jsonrpc.RpcProxy.subscription -= this.handle_notification;
        }

        //------------------------------------------------------------------------------------------------
        // reconnection
        //------------------------------------------------------------------------------------------------

        private void Jsonrpc_ReconnectionHappened(ReconnectionType obj)
        {
            this.jsonrpc.RpcProxy.subscription += this.handle_notification;
        }

        //------------------------------------------------------------------------------------------------
        // subscribe api
        //------------------------------------------------------------------------------------------------

        public async Task<SubscribeResponse> SubscribePublic(SubscribeRequest request, CancellationToken ct)
        {
            // validate
            new SubscribeRequest.Validator().ValidateAndThrow(request);
            // map request
            SubscribeRequestDto requestdto = mapper.Map<SubscribeRequestDto>(request);
            // execute request
            var responsedto = await this.jsonrpc.RpcProxy.subscribe_public
            (
                channels: requestdto.channels,
                ct
            );
            // map response
            SubscribeResponse response = mapper.Map<SubscribeResponse>(responsedto);
            // return
            return response;
        }

        public async Task<UnsubscribeResponse> UnsubscribePublic(UnsubscribeRequest request, CancellationToken ct)
        {
            // validate
            new UnsubscribeRequest.Validator().ValidateAndThrow(request);
            // map request
            UnsubscribeRequestDto requestdto = mapper.Map<UnsubscribeRequestDto>(request);
            // execute request
            var responsedto = await this.jsonrpc.RpcProxy.unsubscribe_public
            (
                channels: requestdto.channels,
                ct
            );
            // map response
            UnsubscribeResponse response = mapper.Map<UnsubscribeResponse>(responsedto);
            // return
            return response;
        }

        public async Task<SubscribeResponse> SubscribePrivate(SubscribeRequest request, CancellationToken ct)
        {
            // validate
            new SubscribeRequest.Validator().ValidateAndThrow(request);
            // map request
            SubscribeRequestDto requestdto = mapper.Map<SubscribeRequestDto>(request);
            // execute request
            var responsedto = await this.jsonrpc.RpcProxy.subscribe_private
            (
                channels: requestdto.channels,
                ct
            );
            // map response
            SubscribeResponse response = mapper.Map<SubscribeResponse>(responsedto);
            // return
            return response;
        }

        public async Task<UnsubscribeResponse> UnsubscribePrivate(UnsubscribeRequest request, CancellationToken ct)
        {
            // validate
            new UnsubscribeRequest.Validator().ValidateAndThrow(request);
            // map request
            UnsubscribeRequestDto requestdto = mapper.Map<UnsubscribeRequestDto>(request);
            // execute request
            var responsedto = await this.jsonrpc.RpcProxy.unsubscribe_private
            (
                channels: requestdto.channels,
                ct
            );
            // map response
            UnsubscribeResponse response = mapper.Map<UnsubscribeResponse>(responsedto);
            // return
            return response;
        }

        //------------------------------------------------------------------------------------------------
        // notification handling
        //------------------------------------------------------------------------------------------------

        private void handle_notification(object sender, JToken e)
        {
            lock (this.notificationhandlesynclock) // REMOVE THIS!
            {
                // get channel name
                string channel = (string) e["channel"];
                // validate
                if (string.IsNullOrEmpty(channel))
                    throw new Exception();
                // switch channel
                if (channel.StartsWith(DeribitChannelPrefix.announcements))
                {
                    // deserialize
                    var dto = e.ToObject<AnnouncementsNotificationDto>(jsonser);
                    // map
                    var noti = this.mapper.Map<AnnouncementsNotification>(dto);
                    // validate
                    new AnnouncementsNotification.Validator().ValidateAndThrow(noti);
                    // raise
                    this.AnnouncementsSubject.OnNext(noti);
                }
                else if (channel.StartsWith(DeribitChannelPrefix.book))
                {
                    // depth limited orderbook
                    if (channel.Count(c => c == '.') == 4)
                    {
                        // deserialize
                        var dto = e.ToObject<BookDepthLimitedNotificationDto>(jsonser);
                        // map
                        var noti = this.mapper.Map<BookDepthLimitedNotification>(dto);
                        // validate
                        new BookDepthLimitedNotification.Validator().ValidateAndThrow(noti);
                        // raise
                        this.BookDepthLimitedSubject.OnNext(noti);
                    }
                    // full orderbook
                    else if (channel.Count(c => c == '.') == 2)
                    {
                        // deserialize
                        var dto = e.ToObject<BookFullNotificationDto>(jsonser);
                        // map
                        var noti = this.mapper.Map<BookFullNotification>(dto);
                        // validate
                        new BookFullNotification.Validator().ValidateAndThrow(noti);
                        // raise
                        this.BookFullSubject.OnNext(noti);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (channel.StartsWith(DeribitChannelPrefix.deribit_price_index))
                {
                    // deserialize
                    var dto = e.ToObject<DeribitPriceIndexNotificationDto>(jsonser);
                    // map
                    var noti = this.mapper.Map<DeribitPriceIndexNotification>(dto);
                    // validate
                    new DeribitPriceIndexNotification.Validator().ValidateAndThrow(noti);
                    // raise
                    this.DeribitPriceIndexSubject.OnNext(noti);
                }
                else if (channel.StartsWith(DeribitChannelPrefix.quote))
                {
                    // deserialize
                    var dto = e.ToObject<QuoteNotificationDto>(jsonser);
                    // map
                    var noti = this.mapper.Map<QuoteNotification>(dto);
                    // validate
                    new QuoteNotification.Validator().ValidateAndThrow(noti);
                    // raise
                    this.QuoteSubject.OnNext(noti);
                }
                else if (channel.StartsWith(DeribitChannelPrefix.ticker))
                {
                    // deserialize
                    var dto = e.ToObject<TickerNotificationDto>(jsonser);
                    // map
                    var noti = this.mapper.Map<TickerNotification>(dto);
                    // validate
                    new TickerNotification.Validator().ValidateAndThrow(noti);
                    // raise
                    this.TickerSubject.OnNext(noti);
                }
                else if (channel.StartsWith(DeribitChannelPrefix.trades))
                {
                    // deserialize
                    var dto = e.ToObject<TradeNotificationDto>(jsonser);
                    // map
                    var noti = this.mapper.Map<TradeNotification>(dto);
                    // validate
                    new TradeNotification.Validator().ValidateAndThrow(noti);
                    // raise
                    this.TradeSubject.OnNext(noti);
                }
                else if (channel.StartsWith(DeribitChannelPrefix.userorders))
                {
                    try
                    {
                        // deserialize
                        var dto = e.ToObject<UserOrdersNotificationDto>(jsonser);
                        // map
                        var noti = this.mapper.Map<UserOrdersNotification>(dto);
                        // validate
                        new UserOrdersNotification.Validator().ValidateAndThrow(noti);
                        // raise
                        this.UserOrdersSubject.OnNext(noti);
                    }
                    catch (JsonSerializationException)
                    {
                        // deserialize
                        var dto = e.ToObject<UserOrderNotificationDto>(jsonser);
                        // map
                        var noti = this.mapper.Map<UserOrdersNotification>(dto);
                        // validate
                        new UserOrdersNotification.Validator().ValidateAndThrow(noti);
                        // raise
                        this.UserOrdersSubject.OnNext(noti);
                    }
                }
                else if (channel.StartsWith(DeribitChannelPrefix.userportfolio))
                {
                    // deserialize
                    var dto = e.ToObject<UserPortfolioNotificationDto>(jsonser);
                    // map
                    var noti = this.mapper.Map<UserPortfolioNotification>(dto);
                    // validate
                    new UserPortfolioNotification.Validator().ValidateAndThrow(noti);
                    // raise
                    this.UserPortfolioSubject.OnNext(noti);
                }
                else if (channel.StartsWith(DeribitChannelPrefix.usertrades))
                {
                    try
                    {
                        // deserialize
                        var dto = e.ToObject<UserTradesNotificationDto>(jsonser);
                        // map
                        var noti = this.mapper.Map<UserTradesNotification>(dto);
                        // validate
                        new UserTradesNotification.Validator().ValidateAndThrow(noti);
                        // raise
                        this.UserTradesSubject.OnNext(noti);
                    }
                    catch (JsonSerializationException)
                    {
                        // deserialize
                        var dto = e.ToObject<UserTradeNotificationDto>(jsonser);
                        // map
                        var noti = this.mapper.Map<UserTradesNotification>(dto);
                        // validate
                        new UserTradesNotification.Validator().ValidateAndThrow(noti);
                        // raise
                        this.UserTradesSubject.OnNext(noti);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        //------------------------------------------------------------------------------------------------

    }
}

﻿using FluentValidation;
using System;

namespace Deribit.S4KTNET.Core
{
    public class Ticker
    {
        public decimal? ask_iv { get; set; }

        public decimal best_ask_amount { get; set; }

        public decimal best_ask_price { get; set; }

        public decimal best_bid_amount { get; set; }

        public decimal best_bid_price { get; set; }

        public decimal? bid_iv { get; set; }

        public decimal? current_funding { get; set; }

        public decimal? delivery_price { get; set; }

        public decimal? funding_8h { get; set; }

        public object greeks { get; set; }

        public decimal index_price { get; set; }

        public string instrument_name { get; set; }

        public decimal? interest_rate { get; set; }

        public decimal last_price { get; set; }

        public decimal? mark_iv { get; set; }

        public decimal mark_price { get; set; }

        public decimal max_price { get; set; }

        public decimal min_price { get; set; }

        public decimal open_interest { get; set; }

        public decimal settlement_price { get; set; }

        public string state { get; set; }

        public TickerStats stats { get; set; }

        public DateTime timestamp { get; set; }

        public decimal underlying_index { get; set; }

        public decimal underlying_price { get; set; }


        public class Validator : AbstractValidator<Ticker>
        {
            public Validator()
            {
                this.RuleFor(x => x.instrument_name).NotEmpty();
                this.RuleFor(x => x.best_ask_amount).GreaterThan(0);
                this.RuleFor(x => x.best_bid_amount).GreaterThan(0);
                this.RuleFor(x => x.best_ask_price).GreaterThan(x => x.best_bid_price);
                this.RuleFor(x => x.best_bid_price).GreaterThan(0);
                this.RuleFor(x => x.best_ask_price).GreaterThan(0);
                this.RuleFor(x => x.stats).SetValidator(new TickerStats.Validator());
            }
        }
    }

    public class TickerStats
    {
        public decimal high { get; set; }

        public decimal low { get; set; }

        public decimal volume { get; set; }

        public class Validator : AbstractValidator<TickerStats>
        {
            public Validator()
            {
                this.RuleFor(x => x.high).GreaterThan(0);
                this.RuleFor(x => x.low).GreaterThan(0);
                this.RuleFor(x => x.volume).GreaterThan(0);
            }
        }
    }
}

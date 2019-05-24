﻿using Deribit.S4KTNET.Core;
using Deribit.S4KTNET.Core.MarketData;
using FluentValidation;
using NUnit.Framework;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace Deribit.S4KTNET.Test.Integration
{
    [TestFixture]
    [Category(TestCategories.integration)]
    class MarketDataTestFixture : DeribitIntegrationTestFixtureBase
    {

        //----------------------------------------------------------------------------
        // lifecycle
        //----------------------------------------------------------------------------

        [SetUp]
        public new async Task SetUp()
        {
            // dont spam
            await Task.Delay(1000);
        }

        //----------------------------------------------------------------------------
        // public/get_book_summary_by_instrument
        //----------------------------------------------------------------------------

        [Test]
        [Description("public/get_book_summary_by_instrument")]
        public async Task Test_getbooksummarybyinstrument_success()
        {
            // execute
            var books = await deribit.MarketData.GetBookSummaryByInstrument
                (new GetBookSummaryByInstrumentRequest()
            {
                instrument_name = DeribitInstruments.Perpetual.BTCPERPETUAL,
            });
            // assert
            Assert.That(books.Count, Is.GreaterThan(0));
            foreach (var book in books)
            {
                new BookSummary.Validator().ValidateAndThrow(book);
            }
        }

        //----------------------------------------------------------------------------
        // public/get_contract_size
        //----------------------------------------------------------------------------

        [Test]
        [Description("public/get_contract_size")]
        public async Task Test_getcontractsize_success()
        {
            // btc perp
            {
                // execute
                var btcperpsize = await deribit.MarketData.GetContractSize
                    (new GetContractSizeRequest()
                    {
                        instrument_name = DeribitInstruments.Perpetual.BTCPERPETUAL,
                    });
                // assert
                new GetContractSizeResponse.Validator().ValidateAndThrow(btcperpsize);
                var expectedsize = DeribitContracts.ByInstrumentName[DeribitInstruments.Perpetual.BTCPERPETUAL].ContractSize;
                Assert.That(btcperpsize.contract_size, Is.EqualTo(expectedsize));
            }
            // wait
            await Task.Delay(1 << 9);
            // eth perp
            {
                // execute
                var ethperpsize = await deribit.MarketData.GetContractSize
                    (new GetContractSizeRequest()
                    {
                        instrument_name = DeribitInstruments.Perpetual.ETHPERPETUAL,
                    });
                // assert
                new GetContractSizeResponse.Validator().ValidateAndThrow(ethperpsize);
                var expectedsize = DeribitContracts.ByInstrumentName[DeribitInstruments.Perpetual.ETHPERPETUAL].ContractSize;
                Assert.That(ethperpsize.contract_size, Is.EqualTo(expectedsize));
            }
        }

        //----------------------------------------------------------------------------
    }
}
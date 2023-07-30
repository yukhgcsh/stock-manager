using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Utils;

namespace StockManager.Core.Services
{
    public class InvestmentTrustService
    {
        private readonly IInvestmentTrustHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public InvestmentTrustService(IInvestmentTrustHistoryRepository investmentTrustHistoryRepository, IMapper mapper)
        {
            this._historyRepository = investmentTrustHistoryRepository;
            this._mapper = mapper;
        }

        public async ValueTask<IList<InvestmentTrustInfo>> GetInvestmentTrustAsync()
        {
            var transactions = await this._historyRepository.FetchAsync();

            var trustDictionary = new Dictionary<int, InvestmentTrustInfo>();
            foreach (var transaction in transactions)
            {
                if (!trustDictionary.TryGetValue(transaction.Code, out var trustInfo))
                {
                    trustInfo = new InvestmentTrustInfo
                    {
                        Code = transaction.Code,
                        Name = transaction.Name,
                    };
                    trustDictionary[transaction.Code] = trustInfo;
                }

                trustInfo.Histories.Add(this._mapper.Map<InvestmentTrustHistoryEntity, InvestmentTrustInfo.History>(transaction));
            }

            foreach (var trust in trustDictionary.Values)
            {
                var trustProfit = 0d;
                var dividendProfit = 0d;
                var restTrust = new List<BuyInfo>();
                foreach (var history in trust.Histories.OrderBy(x => x.Date))
                {
                    if (history.Type == TransactionType.Buy)
                    {
                        restTrust.Add(
                            new BuyInfo
                            {
                                Quantity = history.Quantity,
                                Amount = history.Amount
                            }
                       );
                    }
                    else if (history.Type == TransactionType.Sell)
                    {
                        for (var i = 0; i < restTrust.Count; i++)
                        {
                            if (restTrust[i].Quantity == 0)
                            {
                                continue;
                            }

                            if (restTrust[i].Quantity >= history.Quantity)
                            {
                                restTrust[i].Quantity -= history.Quantity;
                                trustProfit += (history.Amount - restTrust[i].Amount) * history.Quantity;
                                break;
                            }
                            else
                            {
                                trustProfit += (history.Amount - restTrust[i].Amount) * restTrust[i].Quantity;
                                history.Quantity -= restTrust[i].Quantity;
                            }
                        }
                    }
                    else
                    {
                        dividendProfit += history.Amount;
                    }
                }
                trust.InvestmentTrustProfit = (int)trustProfit;
                trust.DividendProfit = (int)dividendProfit;
            }
            return trustDictionary.Values.OrderBy(x =>
                {
                    var currentQuantity = x.Histories.Sum(y =>
                    {
                        if (y.Type == TransactionType.Buy)
                        {
                            return y.Quantity;
                        }
                        else if (y.Type == TransactionType.Sell)
                        {
                            return -1 * y.Quantity;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    if (currentQuantity == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                })
                .ThenByDescending(x => x.Histories.Max(y => y.Date))
                .ToList();
        }

        public async ValueTask RegisterDividendAsync(InvestmentTrustTransaction transaction)
        {
            var entity = this._mapper.Map<InvestmentTrustTransaction, InvestmentTrustHistoryEntity>(transaction);
            await this._historyRepository.RegisterAsync(entity);
        }

        public async ValueTask RegisterInvestmentTrustAsync(InvestmentTrustTransaction transaction)
        {
            var entity = this._mapper.Map<InvestmentTrustTransaction, InvestmentTrustHistoryEntity>(transaction);
            await this._historyRepository.RegisterAsync(entity);
        }

        private class BuyInfo
        {
            public int Quantity { get; set; }

            public double Amount { get; set; }
        }
    }
}

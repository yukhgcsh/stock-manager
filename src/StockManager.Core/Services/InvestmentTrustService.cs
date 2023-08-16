using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Transactions;
using StockManager.Core.Utils;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     投資信託に関する操作を提供します。
    /// </summary>
    public class InvestmentTrustService
    {
        private readonly IInvestmentTrustHistoryRepository _historyRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="investmentTrustHistoryRepository"><see cref="IInvestmentTrustHistoryRepository"/> 。</param>
        /// <param name="mapper"><see cref="IMapper"/> 。</param>
        public InvestmentTrustService(IInvestmentTrustHistoryRepository investmentTrustHistoryRepository, ITransactionManager transactionManager, IMapper mapper)
        {
            this._historyRepository = investmentTrustHistoryRepository;
            this._transactionManager = transactionManager;
            this._mapper = mapper;
        }

        public async ValueTask<IList<InvestmentTrustTransactionHistory>> FetchInvestmentTrustAsync()
        {
            await this._transactionManager.OpenAsync();
            var histories = await this._historyRepository.FetchAsync();
            return histories.Select(x => this._mapper.Map<InvestmentTrustHistoryEntity, InvestmentTrustTransactionHistory>(x)).OrderByDescending(x => x.Date).ToList();
        }

        /// <summary>
        ///     投資信託の一覧を取得します。
        /// </summary>
        /// <returns></returns>
        public async ValueTask<IList<InvestmentTrustInfo>> GetInvestmentTrustAsync()
        {
            await this._transactionManager.OpenAsync();
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
                        dividendProfit += history.Amount * history.Quantity;
                    }
                }
                trust.InvestmentTrustProfit = (int)trustProfit;
                trust.DividendProfit = (int)dividendProfit;
            }
            return trustDictionary.Values.OrderBy(x =>
                {
                    int currentQuantity = x.Histories.Sum(y =>
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

        /// <summary>
        ///     投資信託の取引履歴を登録します。
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async ValueTask RegisterDividendAsync(InvestmentTrustTransaction transaction)
        {
            await using var manager = await this._transactionManager.BeginTransactionAsync();
            var entity = this._mapper.Map<InvestmentTrustTransaction, InvestmentTrustHistoryEntity>(transaction);
            await this._historyRepository.RegisterAsync(entity);
            await manager.CommitAsync();
        }

        /// <summary>
        ///     投資信託の分配金を登録します。
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async ValueTask RegisterInvestmentTrustAsync(InvestmentTrustTransaction transaction)
        {
            await using var manager = await this._transactionManager.BeginTransactionAsync();
            var entity = this._mapper.Map<InvestmentTrustTransaction, InvestmentTrustHistoryEntity>(transaction);
            await this._historyRepository.RegisterAsync(entity);
            await manager.CommitAsync();
        }

        private class BuyInfo
        {
            public int Quantity { get; set; }

            public double Amount { get; set; }
        }
    }
}

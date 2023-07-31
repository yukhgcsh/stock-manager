using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;
using StockManager.Core.Utils;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     投資信託に関する操作を提供します。
    /// </summary>
    public class InvestmentTrustService
    {
        private readonly IInvestmentTrustHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="investmentTrustHistoryRepository"><see cref="IInvestmentTrustHistoryRepository"/> 。</param>
        /// <param name="mapper"><see cref="IMapper"/> 。</param>
        public InvestmentTrustService(IInvestmentTrustHistoryRepository investmentTrustHistoryRepository, IMapper mapper)
        {
            this._historyRepository = investmentTrustHistoryRepository;
            this._mapper = mapper;
        }

        /// <summary>
        ///     投資信託の一覧を取得します。
        /// </summary>
        /// <returns></returns>
        public async ValueTask<IList<InvestmentTrustInfo>> GetInvestmentTrustAsync()
        {
            IEnumerable<InvestmentTrustHistoryEntity> transactions = await this._historyRepository.FetchAsync();

            Dictionary<int, InvestmentTrustInfo> trustDictionary = new Dictionary<int, InvestmentTrustInfo>();
            foreach (InvestmentTrustHistoryEntity transaction in transactions)
            {
                if (!trustDictionary.TryGetValue(transaction.Code, out InvestmentTrustInfo? trustInfo))
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

            foreach (InvestmentTrustInfo trust in trustDictionary.Values)
            {
                double trustProfit = 0d;
                double dividendProfit = 0d;
                List<BuyInfo> restTrust = new List<BuyInfo>();
                foreach (InvestmentTrustInfo.History? history in trust.Histories.OrderBy(x => x.Date))
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
                        for (int i = 0; i < restTrust.Count; i++)
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
            InvestmentTrustHistoryEntity entity = this._mapper.Map<InvestmentTrustTransaction, InvestmentTrustHistoryEntity>(transaction);
            await this._historyRepository.RegisterAsync(entity);
        }

        /// <summary>
        ///     投資信託の分配金を登録します。
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async ValueTask RegisterInvestmentTrustAsync(InvestmentTrustTransaction transaction)
        {
            InvestmentTrustHistoryEntity entity = this._mapper.Map<InvestmentTrustTransaction, InvestmentTrustHistoryEntity>(transaction);
            await this._historyRepository.RegisterAsync(entity);
        }

        private class BuyInfo
        {
            public int Quantity { get; set; }

            public double Amount { get; set; }
        }
    }
}

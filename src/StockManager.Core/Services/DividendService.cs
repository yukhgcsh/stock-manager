using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.Repositories;

namespace StockManager.Core.Services
{
    /// <summary>
    ///     配当に関する操作を提供します。
    /// </summary>
    public class DividendService
    {
        private IStockRepository _stockRepository;
        private IStockHistoryRepository _stockHistoryRepository;
        private IMapper _mapper;

        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        /// <param name="stockRepository">株式に関するリポジトリ。</param>
        /// <param name="stockHistoryRepository">株式の取引履歴に関するリポジトリ。</param>
        /// <param name="mapper"></param>
        public DividendService(IStockRepository stockRepository, IStockHistoryRepository stockHistoryRepository, IMapper mapper)
        {
            this._stockRepository = stockRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._mapper = mapper;
        }

        /// <summary>
        ///     配当金の支給情報を登録します。
        /// </summary>
        /// <param name="dividend">配当情報。</param>
        /// <returns>非同期処理の状態。</returns>
        public async ValueTask RegisterDividendAsync(DividendHistory dividend)
        {
            var entity = this._mapper.Map<DividendHistory, StockDividendEntity>(dividend);
            var stockCode = new StockCodeEntity
            {
                Code = entity.Code,
                Name = dividend.Name
            };

            await Task.WhenAll(
                this._stockRepository.UpsertStockCodeAsync(stockCode).AsTask(),
                this._stockHistoryRepository.RegisterDividendAsync(entity).AsTask()
            );
        }
    }
}

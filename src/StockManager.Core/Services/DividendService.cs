using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.Repositories;

namespace StockManager.Core.Services
{
    public class DividendService
    {
        private IStockRepository _stockRepository;
        private IStockHistoryRepository _stockHistoryRepository;
        private IMapper _mapper;

        public DividendService(IStockRepository stockRepository, IStockHistoryRepository stockHistoryRepository, IMapper mapper)
        {
            this._stockRepository = stockRepository;
            this._stockHistoryRepository = stockHistoryRepository;
            this._mapper = mapper;
        }

        public async ValueTask RegisterDividendAsync(DividendHistory dividend)
        {
            var entity = this._mapper.Map<DividendHistory, DividendEntity>(dividend);
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

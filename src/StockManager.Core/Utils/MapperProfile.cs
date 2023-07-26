using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;

namespace StockManager.Core.Utils
{
    /// <summary>
    ///     <see cref="IMapper"/> 用のプロファイルを構築します。
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        ///     新しいインスタンスを作成します。
        /// </summary>
        public MapperProfile()
        {
            CreateMap<HoldingStockEntity, HoldingStock>();
            CreateMap<SoldStockEntity, SoldStock>();
            CreateMap<StockTransaction, StockTransactionHistoryEntity>();
            CreateMap<StockTransactionHistoryEntity, StockTransactionHistory>();
            CreateMap<StockDividendHistory, StockDividendEntity>();
            CreateMap<StockTransaction, HoldingStockEntity>();
            CreateMap<StockTransactionHistoryEntity, StockInfo.History>();
        }
    }
}

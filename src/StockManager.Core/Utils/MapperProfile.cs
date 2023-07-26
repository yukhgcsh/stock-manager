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
            CreateMap<StockTransaction, TransactionHistoryEntity>();
            CreateMap<DividendHistory, StockDividendEntity>();
            CreateMap<StockTransaction, HoldingStockEntity>();
        }
    }
}

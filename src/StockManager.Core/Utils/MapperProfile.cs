using AutoMapper;
using StockManager.Core.Entities;
using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;

namespace StockManager.Core.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<HoldingStockEntity, HoldingStock>();
            CreateMap<SoldStockEntity, SoldStock>();
            CreateMap<StockTransaction, TransactionHistoryEntity>();
            CreateMap<DividendHistory, DividendEntity>();
            CreateMap<StockTransaction, HoldingStockEntity>();
        }
    }
}

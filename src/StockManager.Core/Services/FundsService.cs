using StockManager.Core.InputModels;
using StockManager.Core.OutputModels;
using StockManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Core.Services
{
    public class FundsService
    {
        private readonly IFundsRepository _fundsRepository;

        public FundsService(IFundsRepository fundsRepository)
        {
            this._fundsRepository = fundsRepository;
        }

        public async ValueTask<int> GetCapitalAsync()
        {
            return await this._fundsRepository.GetCapitalAsync();
        }

        public async ValueTask<IList<FundsTransition>> FetchFundsTransitionsAsync()
        {
            var histories = await this._fundsRepository.FetchFundsHistoryAsync();
            var capital = 0;
            return histories.Select(x =>
            {
                if (x.Type == Utils.FundsHistoryType.Deposit)
                {
                    capital += x.Amount;
                }
                else
                {
                    capital -= x.Amount;
                }
                return new FundsTransition
                {
                    Date = x.Date,
                    Capital = capital,
                };
            }
            ).ToList();
        }
    }
}

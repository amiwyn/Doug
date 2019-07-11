using System;
using Doug.Items;
using Doug.Repositories;

namespace Doug.Services
{
    public interface IGovernmentService
    {
        void CollectSalesTaxes(Item item);
        int GetPriceWithTaxes(Item item);
    }

    public class GovernmentService : IGovernmentService
    {
        private readonly IGovernmentRepository _governmentRepository;

        public GovernmentService(IGovernmentRepository governmentRepository)
        {
            _governmentRepository = governmentRepository;
        }

        public void CollectSalesTaxes(Item item)
        {
            var amount = CalculateTaxAmount(item.Price);
            _governmentRepository.AddTaxesToRuler(amount);
        }

        public int GetPriceWithTaxes(Item item)
        {
            var taxes = CalculateTaxAmount(item.Price);
            return item.Price + taxes;
        }

        private int CalculateTaxAmount(int amount)
        {
            var rate = _governmentRepository.GetTaxRate();
            return (int)Math.Ceiling(amount * rate);
        }
    }
}

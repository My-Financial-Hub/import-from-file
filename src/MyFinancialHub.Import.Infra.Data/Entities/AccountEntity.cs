﻿namespace MyFinancialHub.Import.Infra.Data.Entities
{
    public class AccountEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<BalanceEntity> Balances { get; set; }
    }
}

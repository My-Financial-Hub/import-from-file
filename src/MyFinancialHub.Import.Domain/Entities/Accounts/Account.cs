namespace MyFinancialHub.Import.Domain.Entities.Accounts
{
    public class Account(string name, decimal amount)
    {
        public string Name { get; private set; } = name;
        public decimal Amount { get; private set; } = amount;
        public List<Balance> Balances { get; private set; } = new List<Balance>();

        public void AddBalances(IEnumerable<Balance> balances)
        {
            if (balances?.Any() ?? true)
                throw new ArgumentNullException(nameof(balances), "Balances cannot be null.");

            foreach (var balance in balances)
            {
                AddBalance(balance);
            }
        }

        public void AddBalance(Balance balance)
        {
            ArgumentNullException.ThrowIfNull(balance);
            Balances.Add(balance);
            Amount += balance.Amount;
        }
    }
}

using System.Text.RegularExpressions;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf
{
    internal class PdfBalanceMapper
    {
        private const string BALANCE_NAME = "Contas: ";
        private readonly Regex BalanceRegex = new(pattern: @"Contas:\s?.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public IEnumerable<PdfBalance> Map(IEnumerable<DocumentParagraph> paragraphs)
        {
            var paragraph = paragraphs.FirstOrDefault(p => BalanceRegex.IsMatch(p.Content));
            if (paragraph is null)
                return Enumerable.Empty<PdfBalance>();

            var balances = paragraph.Content
                .Split(BALANCE_NAME)
                .Skip(1)
                .First()
                .Split("Grupos:");

            return balances[0].Split(',')
                .Select(name => name.Trim())
                .Distinct()
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => new PdfBalance(name, 0));
        }
    }
}

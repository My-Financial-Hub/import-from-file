namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf
{
    internal class PdfDataMapper(
        PdfCategoryMapper categoryMapper,
        PdfBalanceMapper balanceMapper,
        PdfTransactionMapper transactionMapper
    )
    {
        private readonly PdfCategoryMapper categoryMapper = categoryMapper;
        private readonly PdfBalanceMapper balanceMapper = balanceMapper;
        private readonly PdfTransactionMapper transactionMapper = transactionMapper;

        public PdfDataAggregate Map(AnalyzeResult analyzeResult)
        {
            if (analyzeResult == null)
                throw new ArgumentNullException(nameof(analyzeResult), "AnalyzeResult cannot be null.");

            var categories = categoryMapper.Map(analyzeResult.Paragraphs).ToList();
            var balances = balanceMapper.Map(analyzeResult.Paragraphs).ToList();
            var transactions = transactionMapper.Map(analyzeResult.Tables).ToList();

            return new PdfDataAggregate(categories, balances, transactions);
        }
    }
}

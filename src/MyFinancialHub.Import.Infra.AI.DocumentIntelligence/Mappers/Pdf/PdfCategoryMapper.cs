using System.Text.RegularExpressions;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf
{
    internal class PdfCategoryMapper
    {
        private const string CATEGORY_NAME = "Grupos: ";
        private readonly Regex Category = new(pattern: @"Grupos:\s?.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public IEnumerable<PdfCategory> Map(IEnumerable<DocumentParagraph> paragraphs)
        {
            var paragraph = paragraphs.FirstOrDefault(p => Category.IsMatch(p.Content));
            if(paragraph is null)
                return Enumerable.Empty<PdfCategory>();

            var categories = paragraph.Content
                .Split(CATEGORY_NAME)
                .Skip(1)
                .First();

            return categories.Split(',')
                .Select(name => name.Trim())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => new PdfCategory(name));
        }
    }
}

using System.Text;

namespace MyFinancialHub.Import.Application.Extensions
{
    internal static class StreamExtensions
    {
        /// <summary>
        /// Validates if a file is a PDF by checking its header.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>True if the file is likely a PDF, false otherwise.</returns>
        public static bool IsPdf(this Stream stream)
        {
            var pdfHeader = Encoding.ASCII.GetBytes("%PDF-");

            using var memoryStream = new MemoryStream();

            stream.CopyToAsync(memoryStream);
            byte[] buffer = new byte[pdfHeader.Length];
            int bytesRead = memoryStream.Read(buffer, 0, buffer.Length);

            if (bytesRead < pdfHeader.Length)
                return false;

            for (int i = 0; i < pdfHeader.Length; i++)
            {
                if (buffer[i] != pdfHeader[i])
                    return false;
            }

            return true;
        }
    }
}

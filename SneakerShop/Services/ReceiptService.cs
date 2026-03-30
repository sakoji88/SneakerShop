using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using SneakerShop.Model;

namespace SneakerShop.Services
{
    /// <summary>
    /// Сервис формирования PDF-чека с QR-кодом.
    /// </summary>
    public static class ReceiptService
    {
        public static string GenerateReceipt(Order order, Clone clone)
        {
            // Чеки сохраняются в папку Documents\\CloneShopReceipts.
            var receiptDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CloneShopReceipts");
            Directory.CreateDirectory(receiptDirectory);

            var filePath = Path.Combine(receiptDirectory, $"receipt_order_{order.Id}.pdf");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var document = new Document(PageSize.A4, 36, 36, 36, 36);
                PdfWriter.GetInstance(document, stream);
                document.Open();

                var title = new Paragraph("CloneShop - Чек покупки", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18));
                document.Add(title);
                document.Add(new Paragraph($"ID заказа: {order.Id}"));
                document.Add(new Paragraph($"Дата: {order.Date:dd.MM.yyyy HH:mm}"));
                document.Add(new Paragraph($"Название клона: {clone.Name}"));
                document.Add(new Paragraph($"Цена: {clone.Price:N2} ₽"));
                document.Add(new Paragraph(" "));

                var qrBytes = BuildQrCode($"OrderId={order.Id};UserId={order.UserId}");
                var qrImage = Image.GetInstance(qrBytes);
                qrImage.ScaleToFit(140f, 140f);

                document.Add(new Paragraph("QR (OrderId/UserId):"));
                document.Add(qrImage);

                document.Close();
            }

            return filePath;
        }

        private static byte[] BuildQrCode(string payload)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q))
            {
                var qrCode = new PngByteQRCode(qrData);
                return qrCode.GetGraphic(20);
            }
        }
    }
}

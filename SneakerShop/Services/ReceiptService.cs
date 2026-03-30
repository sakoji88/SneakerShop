using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
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
            // Чеки сохраняются в папку Documents\CloneShopReceipts.
            var receiptDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CloneShopReceipts");
            Directory.CreateDirectory(receiptDirectory);

            var filePath = Path.Combine(receiptDirectory, $"receipt_order_{order.Id}.pdf");

            using (var writer = new PdfWriter(filePath))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("CloneShop - Чек покупки")
                    .SetBold()
                    .SetFontSize(18));

                document.Add(new Paragraph($"ID заказа: {order.Id}"));
                document.Add(new Paragraph($"Дата: {order.Date:dd.MM.yyyy HH:mm}"));
                document.Add(new Paragraph($"Название клона: {clone.Name}"));
                document.Add(new Paragraph($"Цена: {clone.Price:N2} ₽"));

                var qrBytes = BuildQrCode($"OrderId={order.Id};UserId={order.UserId}");
                var qrImageData = ImageDataFactory.Create(qrBytes);
                var qrImage = new Image(qrImageData).ScaleToFit(140, 140);

                document.Add(new Paragraph("QR (OrderId/UserId):"));
                document.Add(qrImage);
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

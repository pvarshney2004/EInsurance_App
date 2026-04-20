using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace EInsurance_App.Services.Pdf
{
    public class InvoiceDocument : IDocument
    {
        private readonly string _customerName;
        private readonly string _schemeName;
        private readonly int _policyId;
        private readonly decimal _amount;
        private readonly DateTime _date;

        public InvoiceDocument(string customerName, string schemeName,
            int policyId, decimal amount, DateTime date)
        {
            _customerName = customerName;
            _schemeName = schemeName;
            _policyId = policyId;
            _amount = amount;
            _date = date;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text("EInsurance Invoice")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter();

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text($"Customer: {_customerName}");
                    col.Item().Text($"Policy ID: {_policyId}");
                    col.Item().Text($"Scheme: {_schemeName}");
                    col.Item().Text($"Payment Date: {_date:dd MMM yyyy}");

                    col.Item().LineHorizontal(1);

                    col.Item().Text($"Amount Paid: ₹{_amount}")
                        .FontSize(16)
                        .Bold();
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for your payment!")
                    .FontSize(10);
            });
        }
    }
}

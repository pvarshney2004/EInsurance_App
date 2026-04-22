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
        private readonly DateTime _paymentDate;

        public InvoiceDocument(string customerName, string schemeName, int policyId, decimal amount, DateTime paymentDate)
        {
            _customerName = customerName;
            _schemeName = schemeName;
            _policyId = policyId;
            _amount = amount;
            _paymentDate = paymentDate;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(25);
                page.Size(PageSizes.A4);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        //  HEADER
        void ComposeHeader(IContainer container)
        {
            container.Background("#0f1f3d").Padding(15).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("EInsurance")
                        .FontSize(20)
                        .Bold()
                        .FontColor(Colors.White);

                    col.Item().Text("Secure Insurance Platform")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Lighten2);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().Text("INVOICE")
                        .Bold().FontSize(16)
                        .FontColor(Colors.White);

                    col.Item().Text($"Date: {_paymentDate:dd MMM yyyy}")
                        .FontColor(Colors.White);

                    col.Item().Text($"Invoice ID: INV-{_policyId}-{_paymentDate.Ticks.ToString().Substring(10)}")
                        .FontColor(Colors.White);
                });
            });
        }

        // CONTENT
        void ComposeContent(IContainer container)
        {
            container.Padding(20).Column(col =>
            {
                // CUSTOMER
                col.Item().Element(Card).Column(c =>
                {
                    c.Item().Text("Billed To").Bold().FontSize(12);
                    c.Item().Text(_customerName).FontSize(11);
                });

                col.Item().PaddingVertical(10);

                // POLICY TABLE
                col.Item().Element(Card).Column(c =>
                {
                    c.Item().PaddingBottom(8).Text("Policy Details")
                        .Bold().FontSize(12);

                    c.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Element(TableHeader).Text("Field").SemiBold();
                            header.Cell().Element(TableHeader).Text("Details").SemiBold();
                        });

                        // Rows
                        table.Cell().Element(TableCell).Text("Policy ID");
                        table.Cell().Element(TableCell).Text(_policyId.ToString());

                        table.Cell().Element(TableCell).Text("Scheme");
                        table.Cell().Element(TableCell).Text(_schemeName);

                        table.Cell().Element(TableCell).Text("Payment Date");
                        table.Cell().Element(TableCell).Text(_paymentDate.ToString("dd MMM yyyy"));
                    });
                });

                col.Item().PaddingVertical(10);

                // PAYMENT SUMMARY
                col.Item().AlignRight().Element(Card).Width(220).Column(c =>
                {
                    c.Item().Text("Payment Summary").Bold().FontSize(12);

                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Amount Paid");
                        r.ConstantItem(80).AlignRight().Text($"₹ {_amount}");
                    });

                    c.Item().PaddingTop(5).LineHorizontal(1);

                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Status").Bold();
                        r.ConstantItem(80).AlignRight().Text("PAID")
                            .FontColor(Colors.Green.Medium)
                            .Bold();
                    });

                    c.Item().PaddingTop(8);

                    c.Item().AlignCenter().Text($"₹ {_amount}")
                        .FontSize(20)
                        .Bold()
                        .FontColor(Colors.Green.Darken2);
                });
            });
        }

        // FOOTER
        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Column(col =>
            {
                col.Item().LineHorizontal(1);

                col.Item().PaddingTop(5).Text("This is a system-generated invoice.")
                    .FontSize(9)
                    .FontColor(Colors.Grey.Medium);

                col.Item().Text("Thank you for choosing EInsurance")
                    .FontSize(10)
                    .Bold();
            });
        }

        // STYLES

        IContainer Card(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(12)
                .Background(Colors.Grey.Lighten5);
        }

        IContainer TableHeader(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .Padding(6);
        }

        IContainer TableCell(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(6);
        }
    }
}
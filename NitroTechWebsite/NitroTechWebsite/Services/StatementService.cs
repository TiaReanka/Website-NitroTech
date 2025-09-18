using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.IO;

namespace NitroTechWebsite.Services
{
    public class StatementService
    {
        public PdfDocument GetStatement(string clientName, string clientAddress, string clientEmail, string clientPhone, string statementNumber, string[,] transactions, string initialBalance)
        {
            // Create a new PDF document
            GlobalFontSettings.FontResolver = new CourierFontResolver();
            var document = new Document();

            BuildDocument(document, clientName, clientAddress, clientEmail, clientPhone, statementNumber, transactions, initialBalance);
            // Create the renderer for the MigraDoc document
            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;


            // Layout and render the document to PDF
            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, string clientName, string clientAddress, string clientEmail, string clientPhone, string statementNumber, string[,] transactions, string initialBalance)
        {

            double currentBalance = double.TryParse(initialBalance, out double parsedInitialBalance)
            ? parsedInitialBalance
            : 0.0;
            // Define the document's sections and content
            Section section = document.AddSection();

            //Header section
            Table headerTable = section.AddTable();
            headerTable.Borders.Visible = false;
            headerTable.AddColumn("10cm");
            headerTable.AddColumn("6cm");
            Row headerRow = headerTable.AddRow();

            //Business Details
            var leftCell = headerRow.Cells[0];
            Paragraph infoPara = leftCell.AddParagraph();
            infoPara = section.AddParagraph("JAE Automotive Engineering");
            infoPara.Format.Font.Size = 18;
            infoPara = section.AddParagraph();
            infoPara.AddFormattedText("Address: ", TextFormat.Bold);
            infoPara.AddText("No.6 Ashfield Avenue, Unit 1. Springfield Business Park, 4034");
            infoPara.AddLineBreak();
            infoPara.AddFormattedText("Email: ", TextFormat.Bold);
            infoPara.AddText("desen@jaeauto.co.za");
            infoPara.AddLineBreak();
            infoPara.AddFormattedText("Phone: ", TextFormat.Bold);
            infoPara.AddText("+27 31 577 9590");
            infoPara.Format.SpaceAfter = 20;

            //Logo of Business
            var rightCell = headerRow.Cells[1];
            Paragraph imagePara = rightCell.AddParagraph();
            imagePara.Format.Alignment = ParagraphAlignment.Right;
            string logoPath = Path.Combine(Application.StartupPath, "Resources", "Logo.jpeg");
            MigraDoc.DocumentObjectModel.Shapes.Image image = imagePara.AddImage(logoPath);
            image.Height = "3cm";
            image.LockAspectRatio = true;

            //Document Type
            var paragraph = section.AddParagraph();
            paragraph.AddFormattedText("STATEMENT", TextFormat.Bold);
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //Client's Details
            paragraph = section.AddParagraph();
            paragraph.AddFormattedText("STATEMENT FOR ACCOUNT HOLDER:\n", TextFormat.Bold);
            paragraph.AddText(clientName);
            paragraph.AddLineBreak();
            paragraph.AddText(clientAddress);
            paragraph.AddLineBreak();
            paragraph.AddText(clientEmail);
            paragraph.AddLineBreak();
            paragraph.AddText(clientPhone);
            paragraph.Format.SpaceAfter = 20;

            //Invoice Details
            paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddFormattedText("Statement no: ", TextFormat.Bold);
            paragraph.AddText(statementNumber); // Custom invoice number
            paragraph.AddLineBreak();
            paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.SpaceAfter = 20;

            Table transactionTable = section.AddTable();
            transactionTable.Borders.Width = 0.75;
            transactionTable.AddColumn("1.5cm");  // #
            transactionTable.AddColumn("4.5cm");  // Reference
            transactionTable.AddColumn("3.0cm");  // Date
            transactionTable.AddColumn("3.0cm");  // Amount
            transactionTable.AddColumn("3.0cm");  // Balance

            Row header = transactionTable.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Cells[0].AddParagraph("#").Format.Font.Bold = true;
            header.Cells[1].AddParagraph("Reference").Format.Font.Bold = true;
            header.Cells[2].AddParagraph("Date").Format.Font.Bold = true;
            header.Cells[3].AddParagraph("Amount").Format.Font.Bold = true;
            header.Cells[4].AddParagraph("Balance").Format.Font.Bold = true;

            // Initial Balance Row
            DateTime now = DateTime.Now;
            DateTime oneMonthAgo = now.AddMonths(-1);
            Row initRow = transactionTable.AddRow();
            initRow.Cells[0].AddParagraph("0");
            initRow.Cells[1].AddParagraph("Initial Balance");
            initRow.Cells[2].AddParagraph($"{oneMonthAgo:yyyy/MM/dd}");
            initRow.Cells[3].AddParagraph("-");
            initRow.Cells[4].AddParagraph("R" + currentBalance.ToString("F2"));

            // Process each transaction
            for (int i = 0; i < transactions.GetLength(0); i++)
            {
                string refNum = transactions[i, 0];       // e.g. "I001" or "P045"
                string amountStr = transactions[i, 1];    // e.g. "2500" (just number string)
                string dateStr = transactions[i, 2];      // e.g. "2025/06/01"

                // Parse amount as double directly (no currency symbol expected)
                double parsedAmount;

                try
                {
                    parsedAmount = Convert.ToDouble(amountStr);
                }
                catch
                {
                    parsedAmount = 0.0;
                }

                // Determine if transaction is an Invoice (positive amount) or Payment (negative amount)
                bool isInvoice = refNum.StartsWith("I", StringComparison.OrdinalIgnoreCase);
                double delta = isInvoice ? parsedAmount : -parsedAmount;

                currentBalance += delta;

                Row row = transactionTable.AddRow();
                row.Cells[0].AddParagraph((i + 1).ToString());
                row.Cells[1].AddParagraph(refNum);
                row.Cells[2].AddParagraph(dateStr);
                row.Cells[3].AddParagraph((delta < 0 ? "-R" : "R") + Math.Abs(delta).ToString("F2"));
                row.Cells[4].AddParagraph("R" + currentBalance.ToString("F2"));
            }
            // Create a table
            var table = document.LastSection.AddTable();
            table.Borders.Width = 0.75;


            // Definition for Columns

            table.AddColumn("1.5cm"); // No
            table.AddColumn("7.5cm"); // Invoice Number
            table.AddColumn("5cm"); // Invoice Date
            table.AddColumn("2.5cm"); // Total

            //Totals
            Table totalsTable = section.AddTable();
            totalsTable.Format.Alignment = ParagraphAlignment.Right;
            totalsTable.Borders.Width = 0.75;

            totalsTable.AddColumn("3.5cm"); // Labels
            totalsTable.AddColumn("2.5cm"); // Values

            // Row - Total
            Row totalRow = totalsTable.AddRow();
            totalRow.Shading.Color = Colors.Gray; // Background color for final total
            totalRow.Cells[0].AddParagraph("TOTAL OWED: ").Format.Font.Bold = true;
            totalRow.Cells[1].AddParagraph("R" + currentBalance.ToString("F2")).Format.Font.Bold = true;


            // Spacer before the box
            section.AddParagraph().AddLineBreak();

            // Create a table to act as the box
            Table bankingTable = section.AddTable();
            bankingTable.Borders.Width = 0.75;
            bankingTable.Borders.Color = Colors.Black;
            bankingTable.Shading.Color = Colors.WhiteSmoke;

            // One full-width column
            bankingTable.AddColumn("13cm");

            // Add a single row
            Row bankingRow = bankingTable.AddRow();
            Cell cellb = bankingRow.Cells[0];
            cellb.Format.SpaceBefore = "0.1cm";
            cellb.Format.SpaceAfter = "0.5cm";
            cellb.VerticalAlignment = VerticalAlignment.Top;

            // Add contents to the cellb
            Paragraph parab = cellb.AddParagraph();
            parab.Format.Font.Size = 9;
            FormattedText titleText = parab.AddFormattedText("Banking Details:\n");
            titleText.Font.Bold = true;
            titleText.Font.Underline = Underline.Single;

            parab.Format.SpaceAfter = 5;

            parab.Format.Font.Bold = false;

            parab.Format.Font.Bold = false;
            parab.AddFormattedText("Bank: ", TextFormat.Bold);
            parab.AddText("First National Bank");
            parab.AddLineBreak();

            parab.AddFormattedText("Account Number: ", TextFormat.Bold);
            parab.AddText("62198096127");
            parab.AddLineBreak();

            parab.AddFormattedText("Branch: ", TextFormat.Bold);
            parab.AddText("Bank City");
            parab.AddLineBreak();

            parab.AddFormattedText("Branch Code: ", TextFormat.Bold);
            parab.AddText("250805");

            //Footer
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("JAE Automotive Engineering - No.6 Ashfield Avenue, Unit 1. Springfield Business Park, 4034");
            paragraph.Format.Alignment = ParagraphAlignment.Center;


            // Statement Period Notice     
            Paragraph notice = section.AddParagraph();
            notice.Format.SpaceBefore = "1cm";
            notice.Format.Font.Size = 9;
            notice.Format.Font.Italic = true;
            notice.AddText($"Note: This statement covers transactions from {oneMonthAgo:yyyy/MM/dd} to {now:yyyy/MM/dd}.");
        }

    }
}

using MigraDoc.Rendering;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Fonts;
using System.Windows.Forms;
using System.IO;

namespace NitroTech.Services
{
    public class InvoiceService
    {
        public PdfDocument GetInvoice(string[,] products, string clientName, string clientAddress, string clientEmail, string clientPhone, string vehicleVIN, string vehicleName, string invoiceNumber, string labourFee, string productTotal)
        {
            // Create a new PDF document
            GlobalFontSettings.FontResolver = new CourierFontResolver();
            var document = new Document();

            string addedTotal = (double.Parse(productTotal) + double.Parse(labourFee)).ToString();

            BuildDocument(document, products, clientName, clientAddress, clientEmail, clientPhone, vehicleVIN, vehicleName, invoiceNumber, labourFee, addedTotal);
            // Create the renderer for the MigraDoc document
            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;


            // Layout and render the document to PDF
            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, string[,] products, string clientName, string clientAddress, string clientEmail, string clientPhone, string vehicleVIN, string vehicleName, string invoiceNumber, string labourFee, string addedTotal)
        {
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
            paragraph.AddFormattedText("INVOICE", TextFormat.Bold);
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //Client's Details
            paragraph = section.AddParagraph();
            paragraph.AddFormattedText("BILLED TO\n", TextFormat.Bold);
            paragraph.AddText(clientName);
            paragraph.AddLineBreak();
            paragraph.AddText(clientAddress);
            paragraph.AddLineBreak();
            paragraph.AddText(clientEmail);
            paragraph.AddLineBreak();
            paragraph.AddText(clientPhone);
            paragraph.AddLineBreak();
            paragraph.AddText(vehicleName + " - " + vehicleVIN);
            paragraph.Format.SpaceAfter = 20;

            //Invoice Details
            paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddFormattedText("Invoice no: ", TextFormat.Bold);
            paragraph.AddText(invoiceNumber); // Custom invoice number
            paragraph.AddLineBreak();
            paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.SpaceAfter = 20;


            // Create a table
            var table = document.LastSection.AddTable();
            table.Borders.Width = 0.75;


            // Definition for Columns

            table.AddColumn("0.8cm"); // No
            table.AddColumn("4.5cm"); // Fault
            table.AddColumn("4cm"); // Items
            table.AddColumn("2.5cm"); // Quantity
            table.AddColumn("2.5cm"); // Unit Price
            table.AddColumn("2.5cm");   // Total


            //Header for the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Shading.Color = Colors.LightGray;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("No.");
            row.Cells[1].AddParagraph("Fault");
            row.Cells[2].AddParagraph("Items Required");
            row.Cells[3].AddParagraph("Quantity");
            row.Cells[4].AddParagraph("Unit Price");
            row.Cells[5].AddParagraph("Total Price");

            //Populating table
            for (int i = 0; i < products.GetLength(0); i++)
            {
                row = table.AddRow();
                for (int j = 0; j < products.GetLength(1); j++)
                    row.Cells[j].AddParagraph(products[i, j]);
            }
            row = table.AddRow();
            row.Shading.Color = Colors.LightGray;
            row.Format.Font.Bold = true;
            row.Cells[1].MergeRight = 3; // Merge middle 4 columns
            row.Cells[1].AddParagraph("Labour Fee");
            row.Cells[5].AddParagraph("R" + labourFee); // Set your labour amount here



            //TotalsTable
            Table totalsTable = section.AddTable();
            totalsTable.Format.Alignment = ParagraphAlignment.Right;
            totalsTable.Borders.Width = 0.75;

            totalsTable.AddColumn("3.5cm"); // Labels
            totalsTable.AddColumn("2.5cm"); // Values

            // Row 1 - Subtotal
            Row totalRow = totalsTable.AddRow();
            totalRow.Shading.Color = Colors.LightGray; // Background color for Subtotal
            totalRow.Cells[0].AddParagraph("Subtotal: ");
            totalRow.Cells[1].AddParagraph("R" + double.Parse(addedTotal).ToString("F2"));

            // Row 2 - Shipping Fee
            totalRow = totalsTable.AddRow();
            totalRow.Shading.Color = Colors.LightGray; // Background color for Tax
            totalRow.Cells[0].AddParagraph("Value Added Tax (VAT): ");
            totalRow.Cells[0].Format.Font.Bold = true; // Make it bold
            totalRow.Cells[1].AddParagraph("R" + (double.Parse(addedTotal) * 0.15).ToString("F2"));

            // Row 3 - Total
            totalRow = totalsTable.AddRow();
            totalRow.Shading.Color = Colors.Gray; // Background color for final total
            totalRow.Cells[0].AddParagraph("TOTAL: ").Format.Font.Bold = true;
            totalRow.Cells[1].AddParagraph("R" + (double.Parse(addedTotal) + (double.Parse(addedTotal) * 0.15)).ToString("F2")).Format.Font.Bold = true;

            //Formatting
            foreach (Row rowt in totalsTable.Rows)
            {
                rowt.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                rowt.Cells[1].Format.Alignment = ParagraphAlignment.Right;

                rowt.Height = "0.7cm";
                rowt.HeightRule = RowHeightRule.AtLeast;

                foreach (Cell cell in rowt.Cells)
                {
                    cell.VerticalAlignment = VerticalAlignment.Center;
                    var para = cell.Elements[0] as Paragraph;
                    if (para != null)
                    {
                        para.Format.SpaceBefore = 3;
                        para.Format.SpaceAfter = 3;
                    }
                }
            }

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
        }
    }
}

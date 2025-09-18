using MigraDoc.Rendering;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Tables;
using System.IO;
using System.Windows.Forms;
using PdfSharp.Fonts;
using MigraDoc.DocumentObjectModel.Shapes;

namespace NitroTech.Services
{
    public class JobCardService
    {
        public PdfDocument GetJobCard(string[,] products, string clientName, string clientAddress, string clientEmail, string clientPhone, string jobCardNumber)
        {
            // Create a new PDF document
            GlobalFontSettings.FontResolver = new CourierFontResolver();
            var document = new Document();

            BuildDocument(document, products, clientName, clientAddress, clientEmail, clientPhone, jobCardNumber);
            // Create the renderer for the MigraDoc document
            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;


            // Layout and render the document to PDF
            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, string[,] products, string clientName, string clientAddress, string clientEmail, string clientPhone, string jobCardNumber)
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
            paragraph.AddFormattedText("JOB CARD", TextFormat.Bold);
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //Client's Details
            paragraph = section.AddParagraph();
            paragraph.AddFormattedText("JOB FOR:\n", TextFormat.Bold);
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
            paragraph.AddFormattedText("Job Card no: ", TextFormat.Bold);
            paragraph.AddText(jobCardNumber); // Custom invoice number
            paragraph.AddLineBreak();
            paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.SpaceAfter = 20;


            // Create a table
            var table = document.LastSection.AddTable();
            table.Borders.Width = 0.75;


            // Definition for Columns

            table.AddColumn("1.5cm"); // No
            table.AddColumn("7.5cm"); // Fault
            table.AddColumn("5.5cm"); // Items
            table.AddColumn("2cm"); // Quantity Required



            //Header for the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Shading.Color = Colors.LightGray;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("No.");
            row.Cells[1].AddParagraph("Fault");
            row.Cells[2].AddParagraph("Items Required");
            row.Cells[3].AddParagraph("Quantity");

            //Populating table
            for (int i = 0; i < products.GetLength(0); i++)
            {
                row = table.AddRow();
                for (int j = 0; j < products.GetLength(1); j++)
                    row.Cells[j].AddParagraph(products[i, j]);
            }

            //Footer
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("JAE Automotive Engineering - No.6 Ashfield Avenue, Unit 1. Springfield Business Park, 4034");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }
    }
}

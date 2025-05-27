using CarRental.Domain.RentalModule;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.Shared
{
    public class PdfConverter
    {
        private string fontFamily;
        private double textFontSize;
        private double titleFontSize;

        public PdfConverter(double textFontSize, double titleFontSize)
        {
            fontFamily = "Verdana";
            this.textFontSize = textFontSize;   //default: 10
            this.titleFontSize = titleFontSize;     //default: 18
        }

        public double TextFontSize { get => textFontSize; set => textFontSize = value; }
        public string FontFamily { get => fontFamily; set => fontFamily = value; }
        public double TitleFontSize { get => titleFontSize; set => titleFontSize = value; }

        public void ConvertRentalToPdf(Rental rental)
        {
            string filePath = $@"..\..\..\Receipts\receipt{rental.Id}.pdf";
            string title = $"Vehicle Rental Receipt - Rental {rental.Id}";

            List<string> lines = new List<string>
            {
                $"• Rental Details:",
                $"      - Selected insurance: {rental.InsuranceType}",
                $"      - Selected plan: {rental.PlanType}",
                $"      - Rental date: {rental.DepartureDate.Date.ToString("dd/MM/yyyy")}",
                $"      - Expected return date: {rental.ExpectedReturnDate.Date.ToString("dd/MM/yyyy")}",
                $"      - Initial rental price: R${rental.RentalPrice}",
                $"• Contracting Customer:",
                $"      - Name: {rental.ContractingCustomer.Name}",
                $"      - Unique ID: {rental.ContractingCustomer.UniqueId}",
                $"      - Email: {rental.ContractingCustomer.Email}",
                $"• Driver Customer:",
                $"      - Name: {rental.DriverCustomer.Name}",
                $"      - Unique ID: {rental.DriverCustomer.UniqueId}",
                $"      - Email: {rental.DriverCustomer.Email}",
                $"      - Driver's License: {rental.DriverCustomer.DriverLicense}",
                $"• Rented Vehicle:",
                $"      - Model: {rental.Vehicle.model}",
                $"      - Brand: {rental.Vehicle.brand}",
                $"      - License Plate: {rental.Vehicle.licensePlate}",
                $"      - Year: {rental.Vehicle.year}",
                $"      - Color: {rental.Vehicle.color}",
                $"      - Number of doors: {rental.Vehicle.numberOfDoors}",
                $"      - Current mileage: {rental.Vehicle.mileage} km"
            };

            GeneratePdf(filePath, title, lines);
        }

        private void GeneratePdf(string fileName, string titleText, List<string> textLines) 
        {
            double lineXPosition = 70;
            double lineYPosition = 15;
            XFont titleFont = new XFont(fontFamily, titleFontSize, XFontStyle.Bold);
            XFont textFont = new XFont(fontFamily, textFontSize, XFontStyle.Regular);

            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = titleText;
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Title and header
            graph.DrawString(titleText, titleFont, XBrushes.Black, new XRect(lineXPosition * 0.60, lineYPosition, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
            lineYPosition += titleFontSize * 0.5;
            graph.DrawString("___________________________________________________________", titleFont, XBrushes.Black, new XRect(0, lineYPosition, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
            lineYPosition += titleFontSize * 2;

            // Lines
            foreach (string line in textLines)
            {
                graph.DrawString(line, textFont, XBrushes.Black, new XRect(lineXPosition, lineYPosition, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                lineYPosition += textFontSize * 2;
            }

            pdf.Save(fileName);
            
            // Use platform-specific approach to open the file
            OpenFile(fileName);
        }
        
        private void OpenFile(string filePath)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", filePath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", filePath);
                }
                else
                {
                    // Fallback or throw an exception for unsupported OS
                    throw new PlatformNotSupportedException("This platform is not supported for opening PDF files.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                Console.WriteLine($"Failed to open PDF file: {ex.Message}");
            }
        }
    }
}

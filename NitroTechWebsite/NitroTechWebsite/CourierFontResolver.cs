using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Fonts;
using System.IO;

namespace NitroTechWebsite
{
    public class CourierFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            string fontFile;

            if (faceName == "CourierNew#R")
                fontFile = "Resources/Font/cour.ttf";
            else if (faceName == "CourierNew#B")
                fontFile = "Resources/Font/courbd.ttf";
            else if (faceName == "CourierNew#I")
                fontFile = "Resources/Font/couri.ttf";
            else if (faceName == "CourierNew#BI")
                fontFile = "Resources/Font/courbi.ttf";
            else
                throw new InvalidOperationException("Font not supported: " + faceName);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fontFile);
            return File.ReadAllBytes(path);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Courier New", StringComparison.OrdinalIgnoreCase))
    {
                if (isBold && isItalic)
                {
                    return new FontResolverInfo("CourierNew#BI");
                }
                if (isBold)
                {
                    return new FontResolverInfo("CourierNew#B");
                }
                if (isItalic)
                {
                    return new FontResolverInfo("CourierNew#I");
                }
                return new FontResolverInfo("CourierNew#R");
            }

            // Fallback for unsupported fonts like Arial, Times New Roman, etc.
            // This avoids crashing and uses Courier New instead
            Console.WriteLine($"Warning: Unsupported font '{familyName}', falling back to Courier New.");
            if (isBold && isItalic)
            {
                return new FontResolverInfo("CourierNew#BI");
            }
            if (isBold)
            {
                return new FontResolverInfo("CourierNew#B");
            }
            if (isItalic)
            {
                return new FontResolverInfo("CourierNew#I");
            }
            return new FontResolverInfo("CourierNew#R");
        }

        public string DefaultFontName => "Courier New";
    }
}

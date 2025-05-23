﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace System.Drawing.Tests;

public class FontTests
{
    public static IEnumerable<object[]> Ctor_Family_Size_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1 };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue };
    }

    public static IEnumerable<object[]> FontFamily_Equals_SameFamily_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1 };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue };
        yield return new object[] { FontFamily.GenericSansSerif, 10 };
    }

    public static IEnumerable<object[]> FontFamily_Equals_DifferentFamily_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, FontFamily.GenericSerif };
        yield return new object[] { FontFamily.GenericSansSerif, FontFamily.GenericSerif };
        yield return new object[] { FontFamily.GenericSansSerif, FontFamily.GenericMonospace };
    }

    [Theory]
    [MemberData(nameof(FontFamily_Equals_SameFamily_TestData))]
    public void Font_Equals_SameFontFamily(FontFamily fontFamily, float size)
    {
        using Font font1 = new(fontFamily, size);
        using Font font2 = new(fontFamily, size);
        Assert.True(font1.Equals(font2));
    }

    [Theory]
    [MemberData(nameof(FontFamily_Equals_DifferentFamily_TestData))]
    public void Font_Equals_DifferentFontFamily(FontFamily fontFamily1, FontFamily fontFamily2)
    {
        using Font font1 = new(fontFamily1, 9);
        using Font font2 = new(fontFamily2, 9);
        // In some Linux distros all the default fonts live under the same family (Fedora, Centos, Redhat)
        if (font1.FontFamily.GetHashCode() != font2.FontFamily.GetHashCode())
            Assert.False(font1.Equals(font2));
    }

    [Fact]
    public void FontFamily_Equals_NullObject()
    {
        FontFamily nullFamily = null;
        Assert.False(FontFamily.GenericMonospace.Equals(nullFamily));
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_TestData))]
    public void Ctor_Family_Size(FontFamily fontFamily, float emSize)
    {
        try
        {
            using Font font = new(fontFamily, emSize);
            VerifyFont(font, fontFamily.Name, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_TestData))]
    public void Ctor_FamilyName_Size(FontFamily fontFamily, float emSize)
    {
        try
        {
            using Font font = new(fontFamily.Name, emSize);
            VerifyFont(font, fontFamily.Name, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    public static IEnumerable<object[]> Ctor_Family_Size_Style_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1, FontStyle.Bold };
        yield return new object[] { FontFamily.GenericSerif, 2, FontStyle.Italic };
        yield return new object[] { FontFamily.GenericSansSerif, 3, FontStyle.Regular };
        yield return new object[] { FontFamily.GenericSerif, 4, FontStyle.Strikeout };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue, FontStyle.Underline };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)(-1) };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MinValue };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MaxValue };
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_TestData))]
    public void Ctor_Family_Size_Style(FontFamily fontFamily, float emSize, FontStyle style)
    {
        try
        {
            using Font font = new(fontFamily, emSize, style);
            VerifyFont(font, fontFamily.Name, emSize, style, GraphicsUnit.Point, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_TestData))]
    public void Ctor_FamilyName_Size_Style(FontFamily fontFamily, float emSize, FontStyle style)
    {
        try
        {
            using Font font = new(fontFamily.Name, emSize, style);
            VerifyFont(font, fontFamily.Name, emSize, style, GraphicsUnit.Point, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    public static IEnumerable<object[]> Ctor_Family_Size_Unit_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1, GraphicsUnit.Document };
        yield return new object[] { FontFamily.GenericSerif, 2, GraphicsUnit.Inch };
        yield return new object[] { FontFamily.GenericSansSerif, 3, GraphicsUnit.Millimeter };
        yield return new object[] { FontFamily.GenericSerif, 4, GraphicsUnit.Point };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue, GraphicsUnit.Pixel };
        yield return new object[] { FontFamily.GenericSerif, 16, GraphicsUnit.World };
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Unit_TestData))]
    public void Ctor_Family_Size_Unit(FontFamily fontFamily, float emSize, GraphicsUnit unit)
    {
        try
        {
            using Font font = new(fontFamily, emSize, unit);
            VerifyFont(font, fontFamily.Name, emSize, FontStyle.Regular, unit, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Unit_TestData))]
    public void Ctor_FamilyName_Size_Unit(FontFamily fontFamily, float emSize, GraphicsUnit unit)
    {
        try
        {
            using Font font = new(fontFamily.Name, emSize, unit);
            VerifyFont(font, fontFamily.Name, emSize, FontStyle.Regular, unit, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    public static IEnumerable<object[]> Ctor_Family_Size_Style_Unit_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1, FontStyle.Bold, GraphicsUnit.Document };
        yield return new object[] { FontFamily.GenericSerif, 2, FontStyle.Italic, GraphicsUnit.Inch };
        yield return new object[] { FontFamily.GenericSansSerif, 3, FontStyle.Regular, GraphicsUnit.Millimeter };
        yield return new object[] { FontFamily.GenericSerif, 4, FontStyle.Strikeout, GraphicsUnit.Point };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue, FontStyle.Underline, GraphicsUnit.Pixel };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)(-1), GraphicsUnit.World };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MinValue, GraphicsUnit.Millimeter };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MaxValue, GraphicsUnit.Millimeter };
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_Unit_TestData))]
    public void Ctor_Family_Size_Style_Unit(FontFamily fontFamily, float emSize, FontStyle style, GraphicsUnit unit)
    {
        try
        {
            using Font font = new(fontFamily, emSize, style, unit);
            VerifyFont(font, fontFamily.Name, emSize, style, unit, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_Unit_TestData))]
    public void Ctor_FamilyName_Size_Style_Unit(FontFamily fontFamily, float emSize, FontStyle style, GraphicsUnit unit)
    {
        try
        {
            using Font font = new(fontFamily.Name, emSize, style, unit);
            VerifyFont(font, fontFamily.Name, emSize, style, unit, 1, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    public static IEnumerable<object[]> Ctor_Family_Size_Style_Unit_GdiCharSet_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1, FontStyle.Bold, GraphicsUnit.Document, 0 };
        yield return new object[] { FontFamily.GenericSerif, 2, FontStyle.Italic, GraphicsUnit.Inch, 1 };
        yield return new object[] { FontFamily.GenericSansSerif, 3, FontStyle.Regular, GraphicsUnit.Millimeter, 255 };
        yield return new object[] { FontFamily.GenericSerif, 4, FontStyle.Strikeout, GraphicsUnit.Point, 10 };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue, FontStyle.Underline, GraphicsUnit.Pixel, 10 };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)(-1), GraphicsUnit.World, 8 };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MinValue, GraphicsUnit.Millimeter, 127 };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MaxValue, GraphicsUnit.Millimeter, 200 };
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_Unit_GdiCharSet_TestData))]
    public void Ctor_Family_Size_Style_Unit_GdiCharSet(FontFamily fontFamily, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
    {
        try
        {
            using Font font = new(fontFamily, emSize, style, unit, gdiCharSet);
            VerifyFont(font, fontFamily.Name, emSize, style, unit, gdiCharSet, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_Unit_GdiCharSet_TestData))]
    public void Ctor_FamilyName_Size_Style_Unit_GdiCharSet(FontFamily fontFamily, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
    {
        try
        {
            using Font font = new(fontFamily.Name, emSize, style, unit, gdiCharSet);
            VerifyFont(font, fontFamily.Name, emSize, style, unit, gdiCharSet, false);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    public static IEnumerable<object[]> Ctor_Family_Size_Style_Unit_GdiCharSet_GdiVerticalFont_TestData()
    {
        yield return new object[] { FontFamily.GenericMonospace, 1, FontStyle.Bold, GraphicsUnit.Document, 0, true };
        yield return new object[] { FontFamily.GenericSerif, 2, FontStyle.Italic, GraphicsUnit.Inch, 1, false };
        yield return new object[] { FontFamily.GenericSansSerif, 3, FontStyle.Regular, GraphicsUnit.Millimeter, 255, true };
        yield return new object[] { FontFamily.GenericSerif, 4, FontStyle.Strikeout, GraphicsUnit.Point, 10, false };
        yield return new object[] { FontFamily.GenericSerif, float.MaxValue, FontStyle.Underline, GraphicsUnit.Pixel, 10, true };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)(-1), GraphicsUnit.World, 8, false };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MinValue, GraphicsUnit.Millimeter, 127, true };
        yield return new object[] { FontFamily.GenericSerif, 16, (FontStyle)int.MaxValue, GraphicsUnit.Millimeter, 200, false };
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_Unit_GdiCharSet_GdiVerticalFont_TestData))]
    public void Ctor_Family_Size_Style_Unit_GdiCharSet_GdiVerticalFont(FontFamily fontFamily, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
    {
        try
        {
            using Font font = new(fontFamily, emSize, style, unit, gdiCharSet, gdiVerticalFont);
            VerifyFont(font, fontFamily.Name, emSize, style, unit, gdiCharSet, gdiVerticalFont);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Ctor_Family_Size_Style_Unit_GdiCharSet_GdiVerticalFont_TestData))]
    public void Ctor_FamilyName_Size_Style_Unit_GdiCharSet_GdiVerticalFont(FontFamily fontFamily, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
    {
        try
        {
            using Font font = new(fontFamily.Name, emSize, style, unit, gdiCharSet, gdiVerticalFont);
            VerifyFont(font, fontFamily.Name, emSize, style, unit, gdiCharSet, gdiVerticalFont);
        }
        finally
        {
            fontFamily.Dispose();
        }
    }

    [Fact]
    public void Ctor_FamilyNamePrefixedWithAtSign_StripsSign()
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new($"@{family.Name}", 10);
        Assert.Equal(family.Name, font.Name);
        Assert.Equal($"@{family.Name}", font.OriginalFontName);
    }

    [Fact]
    public void Ctor_NullFont_ThrowsNullReferenceException()
    {
        Assert.Throws<NullReferenceException>(() => new Font(null, FontStyle.Regular));
    }

    [Fact]
    public void Ctor_DisposedFont_Success()
    {
        using FontFamily family = FontFamily.GenericSerif;
        Font font = new(family, 10);
        font.Dispose();

        using Font copy = new(font, FontStyle.Italic);
        Assert.Equal(FontStyle.Italic, copy.Style);
        Assert.Equal(family.Name, copy.Name);
    }

    [Fact]
    public void Ctor_NullFamily_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException>("family", () => new Font((FontFamily)null, 10));
        AssertExtensions.Throws<ArgumentNullException>("family", () => new Font((FontFamily)null, 10, FontStyle.Italic));
        AssertExtensions.Throws<ArgumentNullException>("family", () => new Font((FontFamily)null, 10, GraphicsUnit.Display));
        AssertExtensions.Throws<ArgumentNullException>("family", () => new Font((FontFamily)null, 10, FontStyle.Italic, GraphicsUnit.Display));
        AssertExtensions.Throws<ArgumentNullException>("family", () => new Font((FontFamily)null, 10, FontStyle.Italic, GraphicsUnit.Display, 10));
        AssertExtensions.Throws<ArgumentNullException>("family", () => new Font((FontFamily)null, 10, FontStyle.Italic, GraphicsUnit.Display, 10, gdiVerticalFont: true));
    }

    [Fact]
    public void Ctor_DisposedFamily_ThrowsArgumentException()
    {
        using PrivateFontCollection collection = new();
        collection.AddFontFile(Helpers.GetTestFontPath("CodeNewRoman.otf"));

        FontFamily family = new("Code New Roman", collection);
        family.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, GraphicsUnit.Display));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic, GraphicsUnit.Display));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic, GraphicsUnit.Display, 10));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic, GraphicsUnit.Display, 10, gdiVerticalFont: true));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(float.NaN)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.PositiveInfinity)]
    public void Ctor_InvalidEmSize_ThrowsArgumentException(float emSize)
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family, emSize));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family.Name, emSize));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family, emSize, FontStyle.Italic));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family.Name, emSize, FontStyle.Italic));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family, emSize, GraphicsUnit.Document));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family.Name, emSize, GraphicsUnit.Document));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family, emSize, FontStyle.Italic, GraphicsUnit.Document));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family.Name, emSize, FontStyle.Italic, GraphicsUnit.Document));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family, emSize, FontStyle.Italic, GraphicsUnit.Document, 10));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family.Name, emSize, FontStyle.Italic, GraphicsUnit.Document, 10));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family, emSize, FontStyle.Italic, GraphicsUnit.Document, 10, gdiVerticalFont: true));
        AssertExtensions.Throws<ArgumentException>("emSize", () => new Font(family.Name, emSize, FontStyle.Italic, GraphicsUnit.Document, 10, gdiVerticalFont: true));
    }

    [Theory]
    [InlineData(GraphicsUnit.Display)]
    [InlineData(GraphicsUnit.World - 1)]
    [InlineData(GraphicsUnit.Millimeter + 1)]
    public void Ctor_InvalidUnit_ThrowsArgumentException(GraphicsUnit unit)
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, unit));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family.Name, 10, unit));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic, unit));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family.Name, 10, FontStyle.Italic, unit));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic, unit, 10));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family.Name, 10, FontStyle.Italic, unit, 10));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family, 10, FontStyle.Italic, unit, 10, gdiVerticalFont: true));
        AssertExtensions.Throws<ArgumentException>(null, () => new Font(family.Name, 10, FontStyle.Italic, unit, 10, gdiVerticalFont: true));
    }

    [Fact]
    public void Clone_Invoke_ReturnsExpected()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: true);
        Font clone = Assert.IsType<Font>(font.Clone());
        Assert.NotSame(font, clone);

        Assert.Equal(font.Name, clone.FontFamily.Name);
        Assert.Equal(font.Size, clone.Size);
        Assert.Equal(font.Style, clone.Style);
        Assert.Equal(font.Unit, clone.Unit);
        Assert.Equal(font.GdiCharSet, clone.GdiCharSet);
        Assert.Equal(font.GdiVerticalFont, clone.GdiVerticalFont);
    }

    [Fact]
    public void Clone_DisposedFont_ThrowsArgumentException()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        Font font = new(family, 10);
        font.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, font.Clone);
    }

    public static IEnumerable<object[]> Equals_TestData()
    {
        FontFamily family = FontFamily.GenericSansSerif;
        Font font = new(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: true);

        yield return new object[] { font, font, true };
        yield return new object[] { font.Clone(), new Font(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: true), false };
        yield return new object[] { font.Clone(), new Font(family, 9, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: true), false };
        yield return new object[] { font.Clone(), new Font(family, 10, FontStyle.Italic, GraphicsUnit.Millimeter, 10, gdiVerticalFont: true), false };
        yield return new object[] { font.Clone(), new Font(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 9, gdiVerticalFont: true), false };
        yield return new object[] { font.Clone(), new Font(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: false), false };

        yield return new object[] { new Font(family, 10), new(), false };
        yield return new object[] { new Font(family, 10), null, false };
    }

    [Theory]
    [MemberData(nameof(Equals_TestData))]
    public void Equals_Other_ReturnsExpected(Font font, object other, bool expected)
    {
        // Windows7 GDI+ returns different results than later versions of Windows.
        if (PlatformDetection.IsWindows7)
        {
            return;
        }

        try
        {
            Assert.Equal(expected, font.Equals(other));
            Assert.Equal(font.GetHashCode(), font.GetHashCode());
        }
        finally
        {
            font.Dispose();
            if (other is Font otherFont && !ReferenceEquals(font, otherFont))
            {
                otherFont.Dispose();
            }
        }
    }

    [Fact]
    public void FromHdc_ZeroHdc_ThrowsArgumentException()
    {
        AssertExtensions.Throws<ArgumentException>(null, () => Font.FromHdc(IntPtr.Zero));
    }

    [Fact]
    public void FromHdc_GraphicsHdc_ThrowsArgumentException()
    {
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        IntPtr hdc = graphics.GetHdc();
        try
        {
            AssertExtensions.Throws<ArgumentException>(null, () => Font.FromHdc(hdc));
        }
        finally
        {
            graphics.ReleaseHdc();
        }
    }

    [Fact]
    public void FromHfont_Zero_ThrowsArgumentException()
    {
        AssertExtensions.Throws<ArgumentException>(null, () => Font.FromHfont(IntPtr.Zero));
    }

    [Fact]
    public void GetHeight_Parameterless_ReturnsExpected()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10);
        float height = font.GetHeight();
        AssertExtensions.GreaterThan(height, 0);

        Assert.Equal((int)Math.Ceiling(height), font.Height);
    }

    [Fact]
    public void GetHeight_Graphics_ReturnsExpected()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10);
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        Assert.Equal((double)font.GetHeight(graphics.DpiY), font.GetHeight(graphics), 5);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, -0.1571995)]
    [InlineData(1, 0.1571995)]
    [InlineData(float.NaN, float.NaN)]
    [InlineData(float.PositiveInfinity, float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity, float.NegativeInfinity)]
    public void GetHeight_Dpi_ReturnsExpected(float dpi, float expected)
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10);
        Assert.Equal((double)expected, font.GetHeight(dpi), 5);
    }

    [Fact]
    public void GetHeight_NullGraphics_ThrowsArgumentNullException()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10);
        AssertExtensions.Throws<ArgumentNullException>("graphics", () => font.GetHeight(null));
    }

    [Fact]
    public void GetHeight_DisposedGraphics_ThrowsArgumentException()
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10);
        using Bitmap image = new(10, 10);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Dispose();

        AssertExtensions.Throws<ArgumentException>("graphics", () => font.GetHeight(graphics));
    }

    [Fact]
    public void GetHeight_Disposed_ThrowsArgumentException()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        Font font = new(family, 10);
        font.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => font.GetHeight());
        AssertExtensions.Throws<ArgumentException>(null, () => font.GetHeight(10));
        AssertExtensions.Throws<ArgumentException>(null, () => font.GetHeight(graphics));
    }

    [Theory]
    [InlineData(FontStyle.Bold, int.MinValue, 0)]
    [InlineData(FontStyle.Bold, -2147483099, 0)]
    [InlineData(FontStyle.Regular, -2147483098, 0)]
    [InlineData(FontStyle.Regular, -1, 0)]
    [InlineData(FontStyle.Regular, 0, 0)]
    [InlineData(FontStyle.Regular, 300, 0)]
    [InlineData(FontStyle.Regular, 400, 0)]
    [InlineData(FontStyle.Strikeout | FontStyle.Underline | FontStyle.Italic, 549, 1)]
    [InlineData(FontStyle.Strikeout | FontStyle.Underline | FontStyle.Italic | FontStyle.Bold, 550, 1)]
    [InlineData(FontStyle.Strikeout | FontStyle.Underline | FontStyle.Bold | FontStyle.Italic, int.MaxValue, 1)]
    public void FromLogFont_ValidLogFont_ReturnsExpected(FontStyle fontStyle, int weight, byte charSet)
    {
        // The boundary values of the weight that is considered Bold are different between Windows 7 and Windows 8.
        if (PlatformDetection.IsWindows7 || PlatformDetection.IsWindows8x)
        {
            return;
        }

        using FontFamily family = FontFamily.GenericMonospace;
        LOGFONT logFont = new()
        {
            lfFaceName = family.Name,
            lfWeight = weight,
            lfItalic = (fontStyle & FontStyle.Italic) != 0 ? (byte)1 : (byte)0,
            lfStrikeOut = (fontStyle & FontStyle.Strikeout) != 0 ? (byte)1 : (byte)0,
            lfUnderline = (fontStyle & FontStyle.Underline) != 0 ? (byte)1 : (byte)0,
            lfCharSet = charSet
        };
        using Font font = Font.FromLogFont(logFont);
        VerifyFont(font, family.Name, font.Size, fontStyle, GraphicsUnit.World, charSet, expectedGdiVerticalFont: false);
    }

    [Fact]
    public void FromLogFont_NullLogFont_ThrowsArgumentNullException()
    {
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        IntPtr hdc = graphics.GetHdc();
        try
        {
            if (PlatformDetection.IsNetFramework)
            {
                AssertExtensions.Throws<ArgumentException>(null, () => Font.FromLogFont(null));
                AssertExtensions.Throws<ArgumentException>(null, () => Font.FromLogFont(null, hdc));
            }
            else
            {
                AssertExtensions.Throws<ArgumentNullException>("lf", () => Font.FromLogFont(null));
                AssertExtensions.Throws<ArgumentNullException>("lf", () => Font.FromLogFont(null, hdc));
            }
        }
        finally
        {
            graphics.ReleaseHdc();
        }
    }

    [Fact]
    public void FromLogFont_InvalidLogFont_ThrowsArgumentException()
    {
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        IntPtr hdc = graphics.GetHdc();
        try
        {
            LOGFONT logFont = new();
            AssertExtensions.Throws<ArgumentException>(null, () => Font.FromLogFont(logFont));
            AssertExtensions.Throws<ArgumentException>(null, () => Font.FromLogFont(logFont, hdc));
        }
        finally
        {
            graphics.ReleaseHdc();
        }
    }

    [Fact]
    public void FromLogFont_UnblittableStruct()
    {
        const byte OUT_TT_ONLY_PRECIS = 7;
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        graphics.GetHdc();
        try
        {
            UnblittableLOGFONT logFont = new()
            {
                lfOutPrecision = OUT_TT_ONLY_PRECIS
            };

            using Font font = Font.FromLogFont(logFont);
            Assert.NotNull(font);
            Assert.NotEmpty(font.Name);
        }
        finally
        {
            graphics.ReleaseHdc();
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct UnblittableLOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }

    [Theory]
    [InlineData(GraphicsUnit.Document)]
    [InlineData(GraphicsUnit.Inch)]
    [InlineData(GraphicsUnit.Millimeter)]
    [InlineData(GraphicsUnit.Pixel)]
    [InlineData(GraphicsUnit.Point)]
    [InlineData(GraphicsUnit.World)]
    public void SizeInPoints_Get_ReturnsExpected(GraphicsUnit unit)
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10, unit);
        float sizeInPoints = font.SizeInPoints;
        if (unit == GraphicsUnit.Point)
        {
            Assert.Equal(font.Size, sizeInPoints);
        }
        else
        {
            Assert.True(sizeInPoints > 0);
        }
    }

    [Theory]
    [InlineData(FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic, 255, true, "@", 700)]
    [InlineData(FontStyle.Regular, 0, false, "", 400)]
    [InlineData(FontStyle.Regular, 10, false, "", 400)]
    public void ToLogFont_Invoke_ReturnsExpected(FontStyle fontStyle, byte gdiCharSet, bool gdiVerticalFont, string expectedNamePrefix, int expectedWeight)
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10, fontStyle, GraphicsUnit.Point, gdiCharSet, gdiVerticalFont);
        LOGFONT logFont = new();
        font.ToLogFont(logFont);

        Assert.Equal(-13, logFont.lfHeight);
        Assert.Equal(0, logFont.lfWidth);
        Assert.Equal(0, logFont.lfEscapement);
        Assert.Equal(0, logFont.lfOrientation);
        Assert.Equal(expectedWeight, logFont.lfWeight);
        Assert.Equal(font.Italic ? 1 : 0, logFont.lfItalic);
        Assert.Equal(font.Underline ? 1 : 0, logFont.lfUnderline);
        Assert.Equal(font.Strikeout ? 1 : 0, logFont.lfStrikeOut);
        Assert.Equal(SystemFonts.DefaultFont.GdiCharSet <= 2 ? font.GdiCharSet : SystemFonts.DefaultFont.GdiCharSet, logFont.lfCharSet);
        Assert.Equal(0, logFont.lfOutPrecision);
        Assert.Equal(0, logFont.lfClipPrecision);
        Assert.Equal(0, logFont.lfQuality);
        Assert.Equal(0, logFont.lfPitchAndFamily);
        Assert.Equal($"{expectedNamePrefix}{family.Name}", logFont.lfFaceName);
    }

    [Theory]
    [InlineData(TextRenderingHint.SystemDefault)]
    [InlineData(TextRenderingHint.AntiAlias)]
    [InlineData(TextRenderingHint.AntiAliasGridFit)]
    [InlineData(TextRenderingHint.SingleBitPerPixel)]
    [InlineData(TextRenderingHint.SingleBitPerPixelGridFit)]
    [InlineData(TextRenderingHint.ClearTypeGridFit)]
    public void ToLogFont_InvokeGraphics_ReturnsExpected(TextRenderingHint textRenderingHint)
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10);
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        graphics.TextRenderingHint = textRenderingHint;

        LOGFONT logFont = new();
        font.ToLogFont(logFont, graphics);

        Assert.Equal(-13, logFont.lfHeight);
        Assert.Equal(0, logFont.lfWidth);
        Assert.Equal(0, logFont.lfEscapement);
        Assert.Equal(0, logFont.lfOrientation);
        Assert.Equal(400, logFont.lfWeight);
        Assert.Equal(0, logFont.lfItalic);
        Assert.Equal(0, logFont.lfUnderline);
        Assert.Equal(0, logFont.lfStrikeOut);
        Assert.Equal(SystemFonts.DefaultFont.GdiCharSet <= 2 ? font.GdiCharSet : SystemFonts.DefaultFont.GdiCharSet, logFont.lfCharSet);
        Assert.Equal(0, logFont.lfOutPrecision);
        Assert.Equal(0, logFont.lfClipPrecision);
        Assert.Equal(0, logFont.lfQuality);
        Assert.Equal(0, logFont.lfPitchAndFamily);
        Assert.Equal(family.Name, logFont.lfFaceName);
    }

    [Fact]
    public void ToLogFont_NullLogFont_ThrowsArgumentNullException()
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10);
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        Assert.Throws<ArgumentNullException>(() => font.ToLogFont(null));
        Assert.Throws<ArgumentNullException>(() => font.ToLogFont(null, graphics));
    }

    [Fact]
    public void ToLogFont_NullGraphics_ThrowsArgumentNullException()
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10);
        AssertExtensions.Throws<ArgumentNullException>("graphics", () => font.ToLogFont(new LOGFONT(), null));
    }

    [Fact]
    public void ToLogFont_DisposedGraphics_ThrowsArgumentException()
    {
        using FontFamily family = FontFamily.GenericMonospace;
        using Font font = new(family, 10);
        using Bitmap image = new(10, 10);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Dispose();

        Assert.Throws<ArgumentException>(() => font.ToLogFont(new LOGFONT(), graphics));
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }

    [Fact]
    public void ToHfont_SimpleFont_Roundtrips()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10);
        IntPtr hfont = font.ToHfont();
        Assert.NotEqual(IntPtr.Zero, hfont);
        Assert.NotEqual(hfont, font.ToHfont());

        Font newFont = Font.FromHfont(hfont);
        Assert.Equal(font.Name, newFont.Name);
    }

    [Fact]
    public void ToHfont_ComplicatedFont_DoesNotRoundtrip()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: true);
        IntPtr hfont = font.ToHfont();
        Assert.NotEqual(IntPtr.Zero, hfont);
        Assert.NotEqual(hfont, font.ToHfont());

        Font newFont = Font.FromHfont(hfont);
        Assert.NotEqual(font.Name, newFont.Name);
    }

    [Fact]
    public void ToHfont_Disposed_ThrowsArgumentException()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Bitmap image = new(10, 10);
        using Graphics graphics = Graphics.FromImage(image);
        Font font = new(family, 10);
        font.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => font.ToHfont());
    }

    [Fact]
    public void ToString_Invoke_ReturnsExpected()
    {
        using FontFamily family = FontFamily.GenericSansSerif;
        using Font font = new(family, 10, FontStyle.Bold, GraphicsUnit.Inch, 10, gdiVerticalFont: true);
        Assert.Equal($"[Font: Name={family.Name}, Size=10, Units=4, GdiCharSet=10, GdiVerticalFont=True]", font.ToString());
    }

    private static void VerifyFont(Font font, string expectedName, float expectedEmSize, FontStyle expectedStyle, GraphicsUnit expectedUnit, byte expectedGdiCharset, bool expectedGdiVerticalFont)
    {
        Assert.Equal(expectedName, font.Name);
        Assert.Equal(expectedEmSize, font.Size);

        Assert.Equal(expectedStyle, font.Style);
        Assert.Equal((expectedStyle & FontStyle.Bold) != 0, font.Bold);
        Assert.Equal((expectedStyle & FontStyle.Italic) != 0, font.Italic);
        Assert.Equal((expectedStyle & FontStyle.Strikeout) != 0, font.Strikeout);
        Assert.Equal((expectedStyle & FontStyle.Underline) != 0, font.Underline);

        Assert.Equal(expectedUnit, font.Unit);
        Assert.Equal(expectedGdiCharset, font.GdiCharSet);
        Assert.Equal(expectedGdiVerticalFont, font.GdiVerticalFont);

        Assert.False(font.IsSystemFont);
        Assert.Empty(font.SystemFontName);
    }

    [Fact]
    public void GetHashCode_DifferentNameSameSizeStyleUnit_HashCodeIsNotSame()
    {
        using FontFamily family1 = FontFamily.GenericSansSerif;
        using Font font1 = new(family1, 1, FontStyle.Bold, GraphicsUnit.Point);

        using FontFamily family2 = FontFamily.GenericMonospace;
        using Font font2 = new(family2, 1, FontStyle.Bold, GraphicsUnit.Point);
        // This test depends on machine setup and whether the fonts we use are installed or not.
        // If not installed we could get the same font for the two Font families we are testing for.
        if (font1.Name.Equals(font2.Name, StringComparison.OrdinalIgnoreCase))
            return;

        Assert.NotEqual(font1.GetHashCode(), font2.GetHashCode());
    }
}

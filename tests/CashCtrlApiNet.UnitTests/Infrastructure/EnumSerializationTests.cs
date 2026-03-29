/*
MIT License

Copyright (c) 2022 Philip Nf <philip.naef@amanda-technology.ch>
Copyright (c) 2022 Manuel Gysin <manuel.gysin@amanda-technology.ch>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using CashCtrlApiNet.Abstractions.Enums.Account;
using CashCtrlApiNet.Abstractions.Enums.Api;
using CashCtrlApiNet.Abstractions.Enums.Common;
using CashCtrlApiNet.Abstractions.Enums.Salary;
using CashCtrlApiNet.Abstractions.Helpers;
using Shouldly;

namespace CashCtrlApiNet.UnitTests.Infrastructure;

/// <summary>
/// Tests that all enum types serialize and deserialize correctly, preserving the API wire format
/// after renaming members to PascalCase with <c>[JsonStringEnumMemberName]</c> attributes.
/// </summary>
[TestFixture]
public class EnumSerializationTests
{
    // --- BankAccountType ---

    [TestCase(BankAccountType.Default, "DEFAULT")]
    [TestCase(BankAccountType.Order, "ORDER")]
    [TestCase(BankAccountType.Salary, "SALARY")]
    [TestCase(BankAccountType.Historical, "HISTORICAL")]
    [TestCase(BankAccountType.Other, "OTHER")]
    public void BankAccountType_SerializesToWireFormat(BankAccountType value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("DEFAULT", BankAccountType.Default)]
    [TestCase("ORDER", BankAccountType.Order)]
    [TestCase("SALARY", BankAccountType.Salary)]
    [TestCase("HISTORICAL", BankAccountType.Historical)]
    [TestCase("OTHER", BankAccountType.Other)]
    public void BankAccountType_DeserializesFromWireFormat(string wireFormat, BankAccountType expected)
    {
        CashCtrlSerialization.Deserialize<BankAccountType>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- Language ---

    [TestCase(Language.De, "de")]
    [TestCase(Language.Fr, "fr")]
    [TestCase(Language.It, "it")]
    [TestCase(Language.En, "en")]
    public void Language_SerializesToWireFormat(Language value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("de", Language.De)]
    [TestCase("fr", Language.Fr)]
    [TestCase("it", Language.It)]
    [TestCase("en", Language.En)]
    public void Language_DeserializesFromWireFormat(string wireFormat, Language expected)
    {
        CashCtrlSerialization.Deserialize<Language>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- CustomFieldDataType ---

    [TestCase(CustomFieldDataType.Text, "TEXT")]
    [TestCase(CustomFieldDataType.Textarea, "TEXTAREA")]
    [TestCase(CustomFieldDataType.Checkbox, "CHECKBOX")]
    [TestCase(CustomFieldDataType.Date, "DATE")]
    [TestCase(CustomFieldDataType.Combobox, "COMBOBOX")]
    [TestCase(CustomFieldDataType.Number, "NUMBER")]
    [TestCase(CustomFieldDataType.Account, "ACCOUNT")]
    [TestCase(CustomFieldDataType.Person, "PERSON")]
    public void CustomFieldDataType_SerializesToWireFormat(CustomFieldDataType value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("TEXT", CustomFieldDataType.Text)]
    [TestCase("TEXTAREA", CustomFieldDataType.Textarea)]
    [TestCase("CHECKBOX", CustomFieldDataType.Checkbox)]
    [TestCase("DATE", CustomFieldDataType.Date)]
    [TestCase("COMBOBOX", CustomFieldDataType.Combobox)]
    [TestCase("NUMBER", CustomFieldDataType.Number)]
    [TestCase("ACCOUNT", CustomFieldDataType.Account)]
    [TestCase("PERSON", CustomFieldDataType.Person)]
    public void CustomFieldDataType_DeserializesFromWireFormat(string wireFormat, CustomFieldDataType expected)
    {
        CashCtrlSerialization.Deserialize<CustomFieldDataType>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- CustomFieldType ---

    [TestCase(CustomFieldType.Journal, "JOURNAL")]
    [TestCase(CustomFieldType.Account, "ACCOUNT")]
    [TestCase(CustomFieldType.InventoryArticle, "INVENTORY_ARTICLE")]
    [TestCase(CustomFieldType.InventoryAsset, "INVENTORY_ASSET")]
    [TestCase(CustomFieldType.Order, "ORDER")]
    [TestCase(CustomFieldType.Person, "PERSON")]
    [TestCase(CustomFieldType.File, "FILE")]
    [TestCase(CustomFieldType.SalaryStatement, "SALARY_STATEMENT")]
    public void CustomFieldType_SerializesToWireFormat(CustomFieldType value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("JOURNAL", CustomFieldType.Journal)]
    [TestCase("ACCOUNT", CustomFieldType.Account)]
    [TestCase("INVENTORY_ARTICLE", CustomFieldType.InventoryArticle)]
    [TestCase("INVENTORY_ASSET", CustomFieldType.InventoryAsset)]
    [TestCase("ORDER", CustomFieldType.Order)]
    [TestCase("PERSON", CustomFieldType.Person)]
    [TestCase("FILE", CustomFieldType.File)]
    [TestCase("SALARY_STATEMENT", CustomFieldType.SalaryStatement)]
    public void CustomFieldType_DeserializesFromWireFormat(string wireFormat, CustomFieldType expected)
    {
        CashCtrlSerialization.Deserialize<CustomFieldType>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- RoundingMode ---

    [TestCase(RoundingMode.Up, "UP")]
    [TestCase(RoundingMode.Down, "DOWN")]
    [TestCase(RoundingMode.HalfUp, "HALF_UP")]
    public void RoundingMode_SerializesToWireFormat(RoundingMode value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("UP", RoundingMode.Up)]
    [TestCase("DOWN", RoundingMode.Down)]
    [TestCase("HALF_UP", RoundingMode.HalfUp)]
    public void RoundingMode_DeserializesFromWireFormat(string wireFormat, RoundingMode expected)
    {
        CashCtrlSerialization.Deserialize<RoundingMode>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- TaxCalcType ---

    [TestCase(TaxCalcType.Net, "NET")]
    [TestCase(TaxCalcType.Gross, "GROSS")]
    public void TaxCalcType_SerializesToWireFormat(TaxCalcType value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("NET", TaxCalcType.Net)]
    [TestCase("GROSS", TaxCalcType.Gross)]
    public void TaxCalcType_DeserializesFromWireFormat(string wireFormat, TaxCalcType expected)
    {
        CashCtrlSerialization.Deserialize<TaxCalcType>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- TextTemplateType ---

    [TestCase(TextTemplateType.OrderHeader, "ORDER_HEADER")]
    [TestCase(TextTemplateType.OrderFooter, "ORDER_FOOTER")]
    [TestCase(TextTemplateType.OrderMail, "ORDER_MAIL")]
    [TestCase(TextTemplateType.Person, "PERSON")]
    [TestCase(TextTemplateType.SalaryMail, "SALARY_MAIL")]
    public void TextTemplateType_SerializesToWireFormat(TextTemplateType value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("ORDER_HEADER", TextTemplateType.OrderHeader)]
    [TestCase("ORDER_FOOTER", TextTemplateType.OrderFooter)]
    [TestCase("ORDER_MAIL", TextTemplateType.OrderMail)]
    [TestCase("PERSON", TextTemplateType.Person)]
    [TestCase("SALARY_MAIL", TextTemplateType.SalaryMail)]
    public void TextTemplateType_DeserializesFromWireFormat(string wireFormat, TextTemplateType expected)
    {
        CashCtrlSerialization.Deserialize<TextTemplateType>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- SalarySettingType ---

    [TestCase(SalarySettingType.Text, "TEXT")]
    [TestCase(SalarySettingType.Boolean, "BOOLEAN")]
    [TestCase(SalarySettingType.Decimal, "DECIMAL")]
    public void SalarySettingType_SerializesToWireFormat(SalarySettingType value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("TEXT", SalarySettingType.Text)]
    [TestCase("BOOLEAN", SalarySettingType.Boolean)]
    [TestCase("DECIMAL", SalarySettingType.Decimal)]
    public void SalarySettingType_DeserializesFromWireFormat(string wireFormat, SalarySettingType expected)
    {
        CashCtrlSerialization.Deserialize<SalarySettingType>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- SalaryStatusIcon ---

    [TestCase(SalaryStatusIcon.Blue, "BLUE")]
    [TestCase(SalaryStatusIcon.Green, "GREEN")]
    [TestCase(SalaryStatusIcon.Red, "RED")]
    [TestCase(SalaryStatusIcon.Yellow, "YELLOW")]
    [TestCase(SalaryStatusIcon.Orange, "ORANGE")]
    [TestCase(SalaryStatusIcon.Black, "BLACK")]
    [TestCase(SalaryStatusIcon.Grey, "GREY")]
    public void SalaryStatusIcon_SerializesToWireFormat(SalaryStatusIcon value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("BLUE", SalaryStatusIcon.Blue)]
    [TestCase("GREEN", SalaryStatusIcon.Green)]
    [TestCase("RED", SalaryStatusIcon.Red)]
    [TestCase("YELLOW", SalaryStatusIcon.Yellow)]
    [TestCase("ORANGE", SalaryStatusIcon.Orange)]
    [TestCase("BLACK", SalaryStatusIcon.Black)]
    [TestCase("GREY", SalaryStatusIcon.Grey)]
    public void SalaryStatusIcon_DeserializesFromWireFormat(string wireFormat, SalaryStatusIcon expected)
    {
        CashCtrlSerialization.Deserialize<SalaryStatusIcon>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- SalaryTypeKind ---

    [TestCase(SalaryTypeKind.Addition, "ADDITION")]
    [TestCase(SalaryTypeKind.Subtraction, "SUBTRACTION")]
    [TestCase(SalaryTypeKind.Information, "INFORMATION")]
    public void SalaryTypeKind_SerializesToWireFormat(SalaryTypeKind value, string expectedWireFormat)
    {
        CashCtrlSerialization.Serialize(value).ShouldBe($"\"{expectedWireFormat}\"");
    }

    [TestCase("ADDITION", SalaryTypeKind.Addition)]
    [TestCase("SUBTRACTION", SalaryTypeKind.Subtraction)]
    [TestCase("INFORMATION", SalaryTypeKind.Information)]
    public void SalaryTypeKind_DeserializesFromWireFormat(string wireFormat, SalaryTypeKind expected)
    {
        CashCtrlSerialization.Deserialize<SalaryTypeKind>($"\"{wireFormat}\"").ShouldBe(expected);
    }

    // --- SerializeEnumValue / TryDeserializeEnum helpers ---

    [Test]
    public void SerializeEnumValue_Language_ReturnsWireFormat()
    {
        CashCtrlSerialization.SerializeEnumValue(Language.De).ShouldBe("de");
        CashCtrlSerialization.SerializeEnumValue(Language.Fr).ShouldBe("fr");
        CashCtrlSerialization.SerializeEnumValue(Language.It).ShouldBe("it");
        CashCtrlSerialization.SerializeEnumValue(Language.En).ShouldBe("en");
    }

    [Test]
    public void SerializeEnumValue_BankAccountType_ReturnsWireFormat()
    {
        CashCtrlSerialization.SerializeEnumValue(BankAccountType.Default).ShouldBe("DEFAULT");
        CashCtrlSerialization.SerializeEnumValue(BankAccountType.Order).ShouldBe("ORDER");
    }

    [Test]
    public void TryDeserializeEnum_ValidLanguageString_ReturnsTrueAndCorrectValue()
    {
        CashCtrlSerialization.TryDeserializeEnum<Language>("de", out var result).ShouldBeTrue();
        result.ShouldBe(Language.De);
    }

    [Test]
    public void TryDeserializeEnum_InvalidString_ReturnsFalse()
    {
        CashCtrlSerialization.TryDeserializeEnum<Language>("invalid", out _).ShouldBeFalse();
    }

    [Test]
    public void TryDeserializeEnum_NullString_ReturnsFalse()
    {
        CashCtrlSerialization.TryDeserializeEnum<Language>(null, out _).ShouldBeFalse();
    }

    [Test]
    public void TryDeserializeEnum_EmptyString_ReturnsFalse()
    {
        CashCtrlSerialization.TryDeserializeEnum<Language>("", out _).ShouldBeFalse();
    }
}

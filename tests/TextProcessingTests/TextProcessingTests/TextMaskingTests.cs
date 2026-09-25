using System;
using Xunit;
using Elevator.Logic;

namespace Elevator.Tests
{
    public class TextMaskingTests
    {
        private readonly TextMasking _masking;

        public TextMaskingTests()
        {
            _masking = new TextMasking();
        }


        [Theory]
        [InlineData("89991234567", "+7 (999) 123-45-67")]
        [InlineData("79991234567", "+7 (999) 123-45-67")]
        [InlineData("+7 (999) 123-45-67", "+7 (999) 123-45-67")]
        [InlineData("9991234567", "+7 (999) 123-45-67")]
        public void RuNumberMask_ValidInputs_FormatsCorrectly(string input, string expected)
        {
            ProcessResult result = _masking.RuNumberMask(input);

            Assert.True(result.isSuccess);
            Assert.Equal(expected, result.OutputText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void RuNumberMask_EmptyInput_FailsPreCondition(string input)
        {
            ProcessResult result = _masking.RuNumberMask(input);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Строка содержит данные" && !c.IsMet);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("123456789012")]
        public void RuNumberMask_InvalidDigitCount_FailsPreCondition(string input)
        {
            ProcessResult result = _masking.RuNumberMask(input);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Корректное количество цифр" && !c.IsMet);
        }


        [Theory]
        [InlineData("12345678901", "123-456-789 01")]
        [InlineData("123-456-789 01", "123-456-789 01")]
        [InlineData(" 123 456 789 01 ", "123-456-789 01")]
        public void SnilsMask_ValidInputs_FormatsCorrectly(string input, string expected)
        {
            ProcessResult result = _masking.SnilsMask(input);

            Assert.True(result.isSuccess);
            Assert.Equal(expected, result.OutputText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SnilsMask_EmptyInput_FailsPreCondition(string input)
        {
            ProcessResult result = _masking.SnilsMask(input);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Строка содержит данные" && !c.IsMet);
        }

        [Theory]
        [InlineData("1234567890")]
        [InlineData("123456789012")]
        public void SnilsMask_InvalidDigitCount_FailsPreCondition(string input)
        {
            ProcessResult result = _masking.SnilsMask(input);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Корректное количество цифр" && !c.IsMet);
        }


        [Theory]
        [InlineData("1234567890123456", "1234 5678 9012 3456")]
        [InlineData("1234 5678 9012 3456", "1234 5678 9012 3456")]
        [InlineData("1234-5678-9012-3456", "1234 5678 9012 3456")]
        public void BankCardMask_ValidInputs_FormatsCorrectly(string input, string expected)
        {
            ProcessResult result = _masking.BankCardMask(input);

            Assert.True(result.isSuccess);
            Assert.Equal(expected, result.OutputText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void BankCardMask_EmptyInput_FailsPreCondition(string input)
        {
            ProcessResult result = _masking.BankCardMask(input);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Строка содержит данные" && !c.IsMet);
        }

        [Theory]
        [InlineData("123456789012345")]
        [InlineData("12345678901234567")]
        public void BankCardMask_InvalidDigitCount_FailsPreCondition(string input)
        {
            ProcessResult result = _masking.BankCardMask(input);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Корректное количество цифр" && !c.IsMet);
        }
    }
}
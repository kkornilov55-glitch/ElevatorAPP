using System;
using Xunit;
using TextProcessor.Logic;

namespace TextProcessingTests
{
    public class TextFilteringTests
    {
        private readonly TextFiltering _filter;

        public TextFilteringTests()
        {
            _filter = new TextFiltering();
        }

        [Fact]
        public void FilterLinesByKeyword_ValidInput_FiltersCorrectly()
        {
            // Arrange
            string input = "Яблоко\nГруша\nяблочный сок\nАпельсин";
            string keyword = "яблок";

            // Act
            ProcessResult result = _filter.FilterLinesByKeyword(input, keyword);

            // Assert
            Assert.True(result.isSuccess);
            Assert.False(string.IsNullOrWhiteSpace(result.OutputText));

            Assert.Contains("Яблоко", result.OutputText);

            Assert.DoesNotContain("Груша", result.OutputText);
            Assert.DoesNotContain("Апельсин", result.OutputText);
        }

        [Fact]
        public void FilterLinesByKeyword_CaseInsensitive_MatchesCorrectly()
        {
            string input = "Привет\nПРИВЕТ мир\nпока";
            string keyword = "привет";

            ProcessResult result = _filter.FilterLinesByKeyword(input, keyword);

            Assert.True(result.isSuccess);
            Assert.Equal(2, result.OutputText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [Theory]
        [InlineData(null, "слово")]
        [InlineData("", "слово")]
        [InlineData("   ", "слово")]
        public void FilterLinesByKeyword_EmptyOrWhitespaceInput_FailsPreCondition(string input, string keyword)
        {
            ProcessResult result = _filter.FilterLinesByKeyword(input, keyword);

            Assert.False(result.isSuccess);
            Assert.Empty(result.OutputText);
            Assert.Contains(result.PreConditions, c => c.Name == "Исходный текст не пуст" && !c.IsMet);
        }

        [Theory]
        [InlineData("Текст", null)]
        [InlineData("Текст", "")]
        [InlineData("Текст", "   ")]
        public void FilterLinesByKeyword_EmptyOrWhitespaceKeyword_FailsPreCondition(string input, string keyword)
        {
            ProcessResult result = _filter.FilterLinesByKeyword(input, keyword);

            Assert.False(result.isSuccess);
            Assert.Contains(result.PreConditions, c => c.Name == "Ключевое слово задано" && !c.IsMet);
        }

        [Fact]
        public void FilterLinesByKeyword_NoMatchesFound_FailsPostCondition()
        {
            string input = "Раз\nДва\nТри";
            string keyword = "Четыре";

            ProcessResult result = _filter.FilterLinesByKeyword(input, keyword);

            Assert.False(result.isSuccess);
            Assert.Empty(result.OutputText);
            Assert.Contains(result.PostConditions, c => c.Name == "Найдены совпадения в тексте" && !c.IsMet);
        }
    }
}
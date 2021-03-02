using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class OTDBHelper
    {
        public static ICollection<(string id, string name)> GetDifficultyLevels() => new[]
        {
            ("easy", "Easy"),
            ("medium", "Medium"),
            ("hard", "Hard")
        };

        public static ICollection<(string id, string name)> GetQuestionTypes() => new[]
{
            ("boolean", "Yes/No"),
            ("multiple", "Multiple choices")
        };

        public static string ParseQuantity(int? quantity)
        {
            if (!quantity.HasValue)
                return "10"; // default

            if (quantity < 1)
                return "1"; // at least 1 question

            if (quantity > 50)
                return "50"; // at most 50 questions

            return quantity.Value.ToString();
        }

        public static string ParseCategoryId(int? categoryId) => categoryId.HasValue
            ? categoryId.Value.ToString()
            : null;

        public static string ParseDifficulty(string difficulty) =>
            string.IsNullOrWhiteSpace(difficulty)
                ? null
                : GetDifficultyLevels()
                    .Select(x => x.id)
                    .FirstOrDefault(id => string.Equals(
                        id,
                        difficulty.Trim(),
                        StringComparison.OrdinalIgnoreCase));

        public static string ParseQuestionType(string questionType) =>
            string.IsNullOrWhiteSpace(questionType)
                ? null
                : GetQuestionTypes()
                    .Select(x => x.id)
                    .FirstOrDefault(id => string.Equals(
                        id,
                        questionType.Trim(),
                        StringComparison.OrdinalIgnoreCase));
    }
}

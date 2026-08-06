using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Validation.Contracts.Models;

namespace TrajectoryAdvAIcer.Validation.Contracts.Interfaces;

/// <summary>
/// Сервис валидации данных перед анализом
/// </summary>
public interface IDataValidator
{
    /// <summary>
    /// Валидировать результаты анектирования
    /// </summary>
    ValidationResult Validate(LearningHistory results);
}

using Sparq.Shared.Models.QuestionDto;
using Sparq.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.SnapshotDto
{
    public class SnapshotCreateRequestDto
    {
        [Required(ErrorMessage = "QuizId is required.")]
        public string QuizId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(510, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 510 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Time limit is required.")]
        [Range(10, 7200, ErrorMessage = "Time limit must be between 10 seconds and 7200 seconds (2 hours).")]
        public int TimeLimit { get; set; }
        public string PinCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one question is required.")]
        [MinLength(1, ErrorMessage = "At least one question must be provided.")]
        public List<QuestionCreateRequestDto> Questions { get; set; } = new();
    }
}

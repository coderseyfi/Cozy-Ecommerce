using Cozy.Domain.AppCode.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cozy.Domain.Models.Entites
{
    public class ContactPost : BaseEntity
    {
        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} cannot be left empty")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} cannot be left empty")]

        public string Subject { get; set; }
        [Required(ErrorMessage = "{0} cannot be left empty")]

        public string Content { get; set; }
        public string Answer { get; set; }
        public string EmailSubject { get; set; }
        public DateTime? AnswerDate { get; set; }
    }
}

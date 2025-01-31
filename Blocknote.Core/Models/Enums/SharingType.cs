using System.ComponentModel.DataAnnotations;

namespace Blocknote.Core.Models.Enums;
public enum SharingType
{
    [Display(Name = "Все")]
    All,

    [Display(Name = "Только зарегистрированные")]
    Registered
}
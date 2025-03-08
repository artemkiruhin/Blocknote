using System.ComponentModel.DataAnnotations;

namespace Blocknote.Core.Models.Enums;
public enum SharingType
{
    [Display(Name = "Все")]
    Public,

    [Display(Name = "Только зарегистрированные")]
    Registered
}
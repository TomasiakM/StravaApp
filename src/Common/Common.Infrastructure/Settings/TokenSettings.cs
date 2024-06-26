﻿using System.ComponentModel.DataAnnotations;

namespace Common.Infrastructure.Settings;
public sealed class TokenSettings
{
    [Required]
    [MinLength(20)]
    public string Key { get; set; } = default!;
    [Required]
    public string Issuer { get; set; } = default!;
    [Required]
    public int ExpiresInMinutes { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Common.MessageBroker.Settings;
public class MessageBrokerSettings
{
    [Required]
    public string Host { get; set; } = default!;
    [Required]
    public string Username { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}

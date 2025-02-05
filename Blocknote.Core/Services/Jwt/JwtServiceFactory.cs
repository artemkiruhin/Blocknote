using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blocknote.Core.Services.Jwt
{
    public class JwtServiceFactory
    {
        public static JwtService Create ()
        {
            try
            {
                string filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Файл конфигурации appsettings.json не найден.");
                }

                var text = File.ReadAllText(filePath);
                var jsonData = JsonSerializer.Deserialize<JsonElement>(text);

                var jwtSection = jsonData.GetProperty("jwt");

                var audience = jwtSection.GetProperty("audience").GetString() ?? throw new InvalidOperationException("Параметр audience не указан в конфигурации.");
                var issuer = jwtSection.GetProperty("issuer").GetString() ?? throw new InvalidOperationException("Параметр issuer не указан в конфигурации.");
                var key =  Encoding.UTF8.GetBytes(jwtSection.GetProperty("key").GetString() ?? throw new InvalidOperationException("Параметр key не указан в конфигурации."));
                var result = jwtSection.GetProperty("expires").TryGetInt32(out int expires);
                if (!result) throw new InvalidOperationException("Не удалось прочитать expires в конфигурации.");

                return new JwtService(key, issuer, audience, expires);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при загрузке конфигурации: " + ex.Message, ex);
            }
        }
    }
}

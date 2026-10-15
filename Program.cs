// Configura o Host, Injeção de Dependência, Logging e dispara a simulação com múltiplos jogadores simultâneos

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameInventoryDemo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameInventoryDemo;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Configurando o Host padrão do .NET (Boas práticas corporativas)
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Registrando o serviço como Singleton para compartilhar o mesmo estado de estoque na memória
                services.AddSingleton<GameStoreService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        var storeService = host.Services.GetRequiredService<GameStoreService>();

        logger.LogInformation("=== INICIANDO SIMULAÇÃO DE ALTA CONCORRÊNCIA NA LOJA ===");

        // Simulando 20 jogadores tentando comprar o ÚNICO item disponível EXATAMENTE ao mesmo tempo
        var tasks = new List<Task>();
        int totalPlayers = 20;

        for (int i = 1; i <= totalPlayers; i++)
        {
            string playerId = $"Player_{i:D2}";

            // Task.Run dispara threads paralelas simulando requisições HTTP simultâneas
            tasks.Add(Task.Run(async () =>
            {
                await storeService.BuyItemAsync(playerId);
            }));
        }

        // Aguarda todas as 20 requisições concorrentes finalizarem
        await Task.WhenAll(tasks);

        logger.LogInformation("=== SIMULAÇÃO CONCLUÍDA COM SUCESSO. Verifique se o estoque final é 0 e apenas 1 comprou. ===");
    }
}
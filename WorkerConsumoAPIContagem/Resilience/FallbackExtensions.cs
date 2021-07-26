using System;
using System.Threading.Tasks;
using Polly;
using Polly.Fallback;
using WorkerConsumoAPIContagem.Models;

namespace WorkerConsumoAPIContagem.Resilience
{
    public static class FallbackExtensions
    {
        public static AsyncFallbackPolicy<ResultadoContador> CreateFallbackPolicy()
        {
            return Policy<ResultadoContador>
                .Handle<Exception>()
                .FallbackAsync<ResultadoContador>(
                    fallbackAction: (_, _) =>
                    {                        
                        Console.Out.WriteLineAsync();
                        PrintStatusFallback(
                            "Gerando o valor alternativo via Policy de Fallback (fallbackAction)...");
                        Console.Out.WriteLineAsync();

                        return Task.FromResult(new ResultadoContador()
                        {
                            ValorAtual = -1,
                            MensagemVariavel = "Valor gerado na Policy de Fallback"
                        });
                    },
                    onFallbackAsync: (responseToFailedRequest, _) =>
                    {
                        Console.Out.WriteLineAsync();
                        PrintStatusFallback(
                            "Iniciando a execução da Policy de Fallback (onFallbackAsync) | " +
                           $"Descrição da falha: {responseToFailedRequest.Exception.Message}");
                        return Task.CompletedTask;
                    });
        }

        private static void PrintStatusFallback(string status)
        {
            var previousForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Out.WriteLineAsync($" ***** {status} **** ");            
            Console.ForegroundColor = previousForegroundColor;
        }
    }
}
Este projeto simula uma loja de itens de um jogo online onde múltiplos jogadores tentam comprar um item raro com estoque limitado de forma totalmente concorrente. O código resolve o clássico problema de Race Condition (condição de corrida) e evita duplicações de itens (dupes) usando SemaphoreSlim e boas práticas de injeção de dependência e logging estruturado.

# Decisões Técnicas

- Por que usar SemaphoreSlim em vez de lock tradicional?

Em C#, o comando lock tradicional bloqueia a thread sincronamente. Como estamos em um ambiente moderno assíncrono (async/await), bloquear threads degrada a performance do servidor (Thread Pool Starvation). O SemaphoreSlim com WaitAsync() permite que a thread seja liberada para outras tarefas enquanto aguarda a liberação da seção crítica.

----------------------
- Uso de Injeção de Dependência (Microsoft.Extensions.Hosting) e Logging Estruturado:

Instrumentação e telemetria para debugar produção com facilidade. O uso de logs estruturados com chaves ({PlayerId}, {Item}) facilita a indexação futura em ferramentas de observabilidade.

----------------------
- Bloco try-finally obrigatório:

O _semaphore.Release() deve estar sempre no finally. Se ocorrer um erro ou exceção dentro do método de compra e o semáforo não for liberado, ocorre um Deadlock permanente, travando o serviço inteiro da loja.

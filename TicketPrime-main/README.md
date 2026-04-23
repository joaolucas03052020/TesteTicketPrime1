# TicketPrime - Motor de Ingressos

Sistema de bilheteria desenvolvido com C# Minimal API, Blazor WebAssembly, Dapper e SQL Server.

## Pre-requisitos

- SDK do .NET 10
- SQL Server (Express ou superior) instalado localmente
- SQL Server Management Studio (SSMS), Azure Data Studio ou `sqlcmd` (opcional)

## 0. Restaurar Dependencias

O repositorio inclui um `NuGet.Config` local na raiz do projeto para facilitar o restore em ambientes como VS Code, sandbox e CI.

Execute os restores abaixo antes de compilar ou rodar:

```powershell
dotnet restore src\TicketPrimeApi\TicketPrimeApi.csproj --configfile NuGet.Config
dotnet restore src\TicketPrimeFront\TicketPrimeFront.csproj --configfile NuGet.Config
dotnet restore tests\TicketPrimeTests.csproj --configfile NuGet.Config
```

## 1. Configurar o Banco de Dados

A API usa, por padrao, a conexao abaixo:

```csharp
Server=localhost\SQLEXPRESS;Database=TicketPrime;Trusted_Connection=True;TrustServerCertificate=True;
```

Se a sua instancia do SQL Server for diferente de `localhost\SQLEXPRESS`, ajuste a constante `connStr` em `src/TicketPrimeApi/Program.cs`.

Depois, execute o script do banco:

```powershell
# Na raiz do projeto
sqlcmd -S localhost\SQLEXPRESS -i db\script.sql
```

O script cria o banco `TicketPrime` com as tabelas:

- `Usuarios (Cpf, Nome, Email)`
- `Eventos (Id, Nome, CapacidadeTotal, DataEvento, PrecoPadrao)`
- `Cupons (Codigo, PorcentagemDesconto, ValorMinimoRegra)`
- `Reservas (Id, UsuarioCpf, EventoId, CupomUtilizado, ValorFinalPago, DataReserva)`

## 2. Executar a API (Backend)

```powershell
# Terminal 1
cd src\TicketPrimeApi
dotnet run --launch-profile http --no-restore
```

A API ficara disponivel em `http://localhost:5246`.

### Endpoints disponiveis

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/usuarios` | Cadastrar usuario com validacao de CPF e e-mail |
| POST | `/api/eventos` | Cadastrar evento |
| GET | `/api/eventos` | Listar todos os eventos |
| GET | `/api/eventos/{id}` | Buscar evento por ID |
| POST | `/api/cupons` | Cadastrar cupom |
| GET | `/api/cupons/{codigo}` | Consultar cupom por codigo |
| POST | `/api/reservas` | Realizar reserva com cupom opcional |

## 3. Executar o Front-end (Blazor WebAssembly)

```powershell
# Terminal 2
cd src\TicketPrimeFront
dotnet run --no-restore
```

O front-end usa um servidor de desenvolvimento do Blazor e a URL local sera exibida no terminal quando a aplicacao iniciar.

Importante:

- A API deve estar rodando em `http://localhost:5246`.
- O front esta configurado para consumir essa URL em `src/TicketPrimeFront/Program.cs`.
- Se voce alterar a porta da API, atualize tambem o `BaseAddress` do `HttpClient` no front-end.

## 4. Testes

A pasta `tests/` contem um projeto xUnit com testes automatizados.

Para executar:

```powershell
dotnet test tests\TicketPrimeTests.csproj --no-restore
```

## 5. Estrutura do Projeto

```text
TicketPrime-main/
|-- db/
|   `-- script.sql               # Script SQL com CREATE DATABASE e CREATE TABLE
|-- docs/
|   `-- requisitos.md            # Resumo de historias de usuario e criterios BDD
|-- src/
|   |-- TicketPrimeApi/          # Minimal API em C# com Dapper e SQL Server
|   `-- TicketPrimeFront/        # Front-end em Blazor WebAssembly
|-- tests/
|   |-- TicketPrimeTests.csproj  # Projeto xUnit
|   `-- UnitTest1.cs             # Teste unitario do calculo de desconto
|-- requisitos.md                # Documento principal de requisitos
|-- teste.http                   # Requisicoes HTTP de exemplo para a API
|-- NuGet.Config                 # Configuracao local de restore do NuGet
|-- Directory.Build.props        # Cache/restauracao local de pacotes
`-- README.md
```

## 6. Regras de Negocio Implementadas

| Regra | Descricao |
| --- | --- |
| R1 - Cadastro de usuario | CPF deve ter exatamente 11 caracteres |
| R2 - Unicidade de cadastro | CPF e e-mail nao podem ser duplicados |
| R3 - Validade de evento | Nao e permitido cadastrar evento com data passada |
| R4 - Cadastro de cupom | Codigo obrigatorio, desconto entre 1 e 100 e valor minimo nao negativo |
| R5 - Estoque | Bloqueia reservas quando o total de ingressos atinge a capacidade do evento |
| R6 - Limite por CPF | Permite apenas 1 reserva por CPF no mesmo evento |
| R7 - Motor de cupons | O desconto so e aplicado se o preco do evento atender ao valor minimo do cupom |
| R8 - Valor final | O valor final pago nunca pode ficar negativo |
| R9 - Reserva sem cupom | Quando nenhum cupom e informado, a reserva usa o preco padrao do evento |

## 7. Seguranca e Observacoes Tecnicas

- Todas as consultas SQL usam parametros do Dapper, reduzindo risco de SQL Injection.
- A conexao com o SQL Server usa `Trusted_Connection=True`, sem senha em texto plano no codigo.
- O projeto nao utiliza Entity Framework; o acesso ao banco e feito com Dapper e SQL manual.
- O CORS esta liberado para permitir a comunicacao do front-end Blazor com a API.
- O endpoint `GET /api/reservas/{cpf}` aparece nos requisitos, mas nao esta implementado na API atual.

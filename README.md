# EncurtadorDeUrl

Aplicação ASP.NET Core (.NET 6) para encurtar URLs, com interface web simples e persistência local em arquivo usando LiteDB.

## Visão geral

O projeto recebe uma URL válida, gera um identificador curto (9 caracteres) com Nanoid e salva o mapeamento em banco local (`short.db`).  
Quando uma rota `/{chunck}` é acessada, a aplicação consulta o banco e redireciona para a URL original.

## Tecnologias

- .NET 6 (`Microsoft.NET.Sdk.Web`)
- [Carter](https://github.com/CarterCommunity/Carter) para organização de rotas
- [LiteDB](https://www.litedb.org/) para persistência local
- [Nanoid](https://github.com/codeyu/nanoid-net) para geração do código curto
- Frontend estático em `wwwroot` (HTML + JS)

## Estrutura do projeto

- `Program.cs`: configuração da aplicação, DI e mapeamento dos módulos Carter
- `CarterModules/UrlsModule.cs`: endpoint de criação da URL curta
- `CarterModules/PagesModule.cs`: página inicial e redirecionamento por código curto
- `Models/ShortUrl.cs`: modelo persistido no LiteDB
- `wwwroot/index.html`: interface do usuário
- `wwwroot/js/index.js`: chamada `fetch` para `/urls`
- `appsettings.json`: configuração geral (logs, hosts)
- `short.db`: arquivo de banco local LiteDB (criado/atualizado em runtime)

## Como executar

### Pré-requisitos

- SDK do .NET 6+ instalado

### Passos

1. Restaurar dependências:
   ```bash
   dotnet restore
   ```

2. Executar a aplicação:
   ```bash
   dotnet run
   ```

3. Acessar no navegador:
   - `http://localhost:5238` (perfil `http`, conforme `launchSettings.json`)
   - ou URL exibida no terminal ao subir a aplicação

## Endpoints

### `GET /`

Retorna a página web (`wwwroot/index.html`) com formulário para encurtar URLs.

### `POST /urls`

Cria um novo encurtamento.

**Body JSON:**
```json
{
  "url": "https://exemplo.com"
}
```

**Resposta de sucesso (200):**
```json
{
  "shortUrl": "http://localhost:5238/abc123xyz"
}
```

**Resposta de erro (400):**
```json
{
  "error": "Url inválida"
}
```

### `GET /{chunck}`

Procura o código curto no banco:
- se encontrar, redireciona para a URL original;
- se não encontrar, redireciona para `/`.

## Configuração opcional de URL base

No `UrlsModule`, a URL curta é montada a partir de:

1. `BaseUrl` em configuração (`IConfiguration`), se existir;
2. fallback para `"{req.Scheme}://{req.Host}"`.

Para fixar domínio/base (ex.: produção), adicione no `appsettings.json`:

```json
{
  "BaseUrl": "https://seu-dominio.com"
}
```

## Observações

- Este projeto foi inspirado no repositório original: [ricardodemauro/YT-UrlShortner](https://github.com/ricardodemauro/YT-UrlShortner).
- Mudanças realizadas em relação ao original:
  - validação mais robusta da URL no `POST /urls` (campo obrigatório e aceitando apenas `http/https`);
  - retorno HTTP `201 Created`, com header `Location` e payload mais estruturado de resposta/erro;
  - suporte a `BaseUrl` por configuração (`appsettings.json`) para montagem da URL encurtada;
  - melhorias na interface web (mensagens de status, estado de carregamento e botão de copiar link);
  - adição e configuração de Swagger no ambiente de desenvolvimento;
  - atualização de dependências e ajustes de organização/documentação do projeto.
- O campo e rota usam o nome `Chunck` (com "n"), mantendo o padrão atual do código.
- O banco LiteDB é local e baseado em arquivo (`short.db`), ideal para desenvolvimento e projetos simples.


## Autor
Desenvolvido por [David Tavares](https://github.com/davidtav) - [LinkedIn](https://www.linkedin.com/in/david-mclaurel/)

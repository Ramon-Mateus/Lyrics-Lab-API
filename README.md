# Instalar
- .Net 8+: [Aqui](https://dotnet.microsoft.com/pt-br/download)
- Execute esse comando para baixar a ferramenta do Entity Framework para gerenciar migrations e updates no banco:
```shell
dotnet tool install --global dotnet-ef
```

_Após instalar os itens listados acima, vamos baixar a imagem docker do SQL Server e subir o container com a imagem baixada._

## Docker
- Baixar a imagem do MSSQL:
```shell
docker pull mcr.microsoft.com/mssql/server
```

- Subir o container do MSSQL:
```shell
docker run --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SenhaForte123#" -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

- Rodar o container:
```shell
docker start sqlserver
```

- Para verificar se o container subiu e rodou corretamente execute o comando abaixo no terminal e veja se o status está UP:
```shell
docker ps
```

_Adiante, para rodar o projeto basta estar na raiz e rodar os comandos abaixo em sequência. Eles vão, respectivamente, criar a migration e atualizar o banco e ,por fim, rodar o projeto._

## .Net

Para os comandos abaixo é necessário estar dentro da pasta do projeto

- Criar a migration
```shell
dotnet ef migrations add CreateTables
```

- Atualizar o banco com as migrations criadas
```shell
dotnet ef database update
```

- Rodar o projeto
```shell
dotnet run
```
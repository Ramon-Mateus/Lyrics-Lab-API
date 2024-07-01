# Lyrics Lab
API de um sistema para compositores amadores

# Comandos importantes
Listado abaixo alguns comandos importantes durante o processo de desenvolvimento e teste.

## Docker
- Baixar imagem do MSSQL:
```shell
docker pull mcr.microsoft.com/mssql/server
```

- Subir o container do MSSQL:
```shell
docker run --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SenhaForte123#" -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

## .Net

- String de conexao com o banco usando Docker(MSSQL)
```javascript
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=Fina;User ID=sa;Password=SenhaForte123#;Trusted_Connection=False;TrustServerCertificate=True;"
  }
```

- buildar a solução ou projeto.
```shell
dotnet build
```

- Rodar o projeto (Precisa estar dentro da pasta do projeto)
```shell
dotnet run
```

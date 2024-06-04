## Entity Framework

### Bibliotecas necessárias


`dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0-preview.3.24172.4`

`dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0-preview.3.24172.4`

Você precisará instalar as bibliotecas de acordo
com o banco de dados que deseja utilizar.

#### Sql Server

`dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0-preview.3.24172.4`
    

#### Postgres

`dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.2`
    
#### MySQL

`dotnet add package MySql.EntityFrameworkCore --version 8.0.2`

### Migrations

#### Instalar o Entity Framework
`dotnet tool install --global dotnet-ef`

#### Criar uma migração
`dotnet ef migrations add "initial migration"`

#### Remover última migração
`dotnet ef migrations remove`

#### Gerar script SQL da última migração
`dotnet ef migrations script -o script.sql`

### Database Update
`dotnet ef database update`

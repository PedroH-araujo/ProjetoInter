## Entity Framework

### Bibliotecas necessárias


`dotnet add package Microsoft.EntityFrameworkCore --version 8.0.4`

`dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.4`

`dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.4`

Você precisará instalar as bibliotecas de acordo
com o banco de dados que deseja utilizar.

#### Sql Server

`dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.4`

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

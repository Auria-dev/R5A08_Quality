Install `dotnet-ef` using the Following command:

```bash
dotnet tool install --global dotnet-ef --version 8.\*
```



```bash

dotnet-ef migrations add MigrationName

```



```bash

dotnet-ef database update

```







undo db update:

```bash

dotnet-ef database update <specific/previous migration name>

```



followed by



```bash

dotnet-ef migrations remove

```





Also other thing:
```bash



dotnet ef dbcontext scaffold "Server=localhost,5432;Database=DATABASE\_NAME;UserId=postgres;Password=postgres;TrustServerCertificate=True" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Entities



```


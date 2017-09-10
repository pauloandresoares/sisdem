#Aplicação utilizada para estudar o .NET core

1 - dotnet new mvc <name> // Se não definir o nome do projeto ele irá pegar o diretório atual
2 - Instalar o code generator tools:
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Tools
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

//Caso no arquivo do projeto .csproj não estiver a linha:
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
  </ItemGroup>
//Adiciona-la
  
  
3 - dotnet restore //para instalar todas as dependencias
4 - dotnet build // Comando usado para copilar

5 - Gerar uma controller:

dotnet aspnet-codegenerator controller  -name <Nome da Controller. Ex.: OrderController> --relativeFolderPath Controllers

//é importante informar a opção "--relativeFolderPath Controllers" pq caso contrario ele irá gera na raiz do projeto

5 - Gerar uma view

dotnet aspnet-codegenerator view <Nome> <tipo: Empty|Create|Edit|Delete|Details|List> -m <Model> --relativeFolderPath Views\Order  --useDefaultLayout

//o -m é opcional caso vc use algum padrão que já tenha modelo


6 - Instalar o Entity Framework Core:

//Instalar o Entity Framework
dotnet add package Microsoft.EntityFrameworkCore

//Instalar o ferramental do Entity Framework
dotnet add package Microsoft.EntityFrameworkCore.Tools

//Instalar o provider o Postgres SQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

//Pacote para facilitar a migração utilizando o provider do postgres
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.Design

7 - Adicionar o Entity Framework para ser utilizado por linha de comando

dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNe

//Abra o arquivo <Nome do Projeto>.csproj e adicione a seguinte linha

    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
// Para ficar dessa forma

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.All" Version="2.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.0.0" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="2.0.0" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="2.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL.Design" Version="1.1.1" />
  </ItemGroup>
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
  
	// >>>>> AQUI O EXEMPLO <<<<<<<<
	<DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
    
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="2.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
  </ItemGroup>	
  
8 -  Caso não exista a pasta Models na raiz do projeto criar e criar a classe:

// Models/Person.cs
namespace Sisdem.Models
{
    public class Person
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public int Age { get; set;}
        public string Description { get; set;}
    }
}




9 - Criar o diretorio Context e modificar o <Nome do Projeto>.csproj:
// adicionando uma nova tag ItemGroup:

  <!-- Checar a real necessidade da linha abaixo: -->
   <ItemGroup>
      <folder Include="/Models" />
      <folder Include="/Context" />
   </ItemGroup>


10 - Criar o aqruivo DBContext :

//SisdemDBContext:

using Microsoft.EntityFrameworkCore;
using Sisdem.Models;

namespace Sisdem.Context
{
    public class SisdemDBContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

        public SisdemDBContext(DbContextOptions<SisdemDBContext> opts)  
            :  base (opts)
        {

        }
    }
}  

11 - Modificar o arquivo appsettings.json :

{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  },
  
  "ConnectionStrings": {
    "SisdemDBContext":  "User ID=postgres; Password=abcd1234; Host=localhost; Port=5432; Database=Sisdem"
  }
}

12 - Modificar a classe: Startup.cs e o metodo: ConfigureServices para adicionar o novo contexto



		//adicionar estes namespace:
		
		using Sisdem.Context;
		using Microsoft.EntityFrameworkCore;



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
			
			// --> Inicio
            string connectionString = Configuration.GetConnectionString("SisdemDBContext");
            services.AddDbContext<SisdemDBContext>((optBuilder) => 
            {
                optBuilder.UseNpgsql(connectionString);
            });
			// --> Fim
        }
  
13 - Fazer uma migração	

dotnet ef migrations add create_person

14 - Executar uma migração

dotnet ef database update


15 - Gerar uma controller com uma model
dotnet aspnet-codegenerator controller  -name PersonController --model Sisdem.Models.Person --dataContext Sisdem.Context.SisdemDBContext --relativeFolderPath Controllers --useDefaultLayout

16 - Modificar o arquivo _Layout.cshtml

//e adicionar o link                     
<li><a asp-area="" asp-controller="Person" asp-action="Index">Person</a></li>



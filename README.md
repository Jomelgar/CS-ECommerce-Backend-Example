
# Backend de Ejemplo de E-Commerce (C#)

## Descripción
Este proyecto es un **backend de ejemplo para un sistema de e-commerce** desarrollado en **C# con .NET 9**. La API permite manejar productos, inventarios y órdenes, incluyendo operaciones CRUD completas. Está diseñado siguiendo buenas prácticas, con soporte para **Entity Framework Core**, **PostgreSQL**, **Polly** para resiliencia (Circuit Breaker y Bulkhead), y documentación automática mediante **Swagger / OpenAPI**.

- Autor: Johnny Melgar  
- Contacto: [johnny@unitec.edu](mailto:johnny@unitec.edu)  
- OpenAPI v3: compatible con Swagger UI

---

## Tecnologías y Paquetes

- **.NET 9 Web API**
- **Entity Framework Core 9.0** → ORM para PostgreSQL
- **Npgsql.EntityFrameworkCore.PostgreSQL** → Proveedor de PostgreSQL
- **Polly** y **Polly.Extensions.Http** → Estrategias de resiliencia (Retry, Circuit Breaker, Bulkhead)
- **Swashbuckle.AspNetCore** → Documentación Swagger/OpenAPI
- **Microsoft.AspNetCore.OpenApi** → Integración OpenAPI
- **Microsoft.Extensions.Http.Polly** → Integración de Polly con HttpClient

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.10" />
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.10" />
  <PackageReference Include="Microsoft.Extensions.Http.Polly" Version="9.0.10" />
  <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
  <PackageReference Include="Polly" Version="8.6.4" />
  <PackageReference Include="Polly.Extensions.Http" Version="3.0.0" />
  <PackageReference Include="Swashbuckle.AspNetCore" Version="9.0.6" />
</ItemGroup>
````

---

## Endpoints Disponibles

### Health

* `GET /Health` → Verifica el estado de la API.

### Product

* `GET /Product` → Obtiene todos los productos.
* `GET /Product/{id}` → Obtiene un producto por id.
* `POST /Product` → Crea un nuevo producto.
* `PUT /Product/{id}` → Actualiza un producto existente.
* `DELETE /Product/{id}` → Elimina un producto por id.

### Inventory

* `GET /Inventory` → Obtiene todos los inventarios.
* `POST /Inventory` → Crea un inventario nuevo.

### Order

* `GET /Order` → Obtiene todas las órdenes.
* `GET /Order/{id}` → Obtiene una orden por id.
* `POST /Order` → Crea una nueva orden.
* `PUT /Order/{id}` → Actualiza una orden existente.
* `DELETE /Order/{id}` → Elimina una orden por id.

### Test (Polly)

* `GET /Test/CircuitBreaker` → Prueba de Circuit Breaker.
* `GET /Test/Bulkhead` → Prueba de Bulkhead.

---

## Modelos

```csharp
public class Product {
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
}

public class Inventory {
    public int Id { get; set; }
    public int Id_Product { get; set; }
    public int Total_Amount { get; set; }
}

public class Order {
    public int Id { get; set; }
    public int Amount { get; set; }
    public int Id_Inventory { get; set; }
}
```

---

## Configuración y Ejecución

1. Clonar el repositorio:

```bash
git clone <tu-repo-url>
cd EC_CS
```

2. Configurar la conexión a PostgreSQL en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ECommerceDB;Username=postgres;Password=123456"
  }
}
```

3. Aplicar migraciones y crear la base de datos:

```bash
dotnet ef database update
```

4. Ejecutar la API:

```bash
dotnet run
```

5. Acceder a la documentación Swagger:

```
http://localhost:5132/swagger
```

---

## Resiliencia

Se utiliza **Polly** para manejar políticas de resiliencia:

* Retry exponencial para HttpClient
* Circuit Breaker
* Bulkhead para limitar concurrencia

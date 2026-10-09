# R5.A.08 Quality

An ASP.NET Core REST API and Blazor WebAssembly application built according to modern 2-tier architectural principles and software quality guidelines (R5.A.08).

## Architectural Overview

The ASP.NET Core REST API is located inside the `R5A08_API` project.
The Blazor client following an MVVM pattern is located inside the `R5A08_Client` project.
This solution also contains unit and integration tests inside the `R5A08_APITests` project.

## Divergence from the TDs

While preserving the core requirements of the original course exercises, this project introduced several key architectural refinements. In the reference materials, repository methods returned `ActionResult<T>` types. Because `ActionResult` belongs strictly to `Microsoft.AspNetCore.Mvc`, HTTP status handling was removed from the data repository and moved into the controller layer. Repositories now return domain entities directly (`Task<IEnumerable<T>>`, `Task<T?>`).

Domain-specific string search methods were removed from the base `IRepository<T>` interface, as not all database entities have textual name properties. These methods were relocated into specialized interfaces like `IProductRepository.SearchByNameAsync`.

Controllers no longer expose raw database entities directly. I/O payloads instead use dedicated DTOs such as `ProductCreateDto` or `ProductDetailDto` who handle mapping transparently via AutoMapper.

## Service Layer

Having a dedicated service layer (such as `ProductService` for example) is a standard abstraction in larger enterprise systems with complex business logic where the data needs to be manipulated before it is served. This project focuses on straightforward CRUD operations and entity management, so omitting the intermediate service layer avoids redundant pass-through code without sacrificing clarity or maintainability. This therefore adheres to the 2-tier architecture demonstrated in the course material. Also this means it aligns with the YAGNI principle (*"You Ain't Gonna Need It"*) which is nice.
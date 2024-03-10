# Carepatron-Test-Full
## Architectural
### Objective
The objective of this section is to approximate the architecture in the production environment. This will include what support services the system may need, their implementation priority, development direction & engineering roadmap, and the codebase & structure. 
### Implementation
The following is the archtecture approximation:
![architecture](arch.svg)
From the above diagram, we're adding some services for the production best practice:
| Functionality<br>name | Serves<br>multiple<br>services | Purpose |
|---|---|---|
|EmailSender|YES|To send email, given recipient email addresses and content| 
|EmailVerifier|Possibly|To verify an email address|
|DocSynchronizer|No|To synchronize documents|
|RateLimiter|YES|To limit execution of commands in services|
|Log|YES|To collect logs from each nodes using agents|
|MetricCollector|YES|To collect metrics from services/nodes|
|TraceCollector|YES|To collect traces from services|

#### Modularity analysis
- Any functionality that serves multiple services shall be implemented as a standalone service.
- EmailVerifier functionality will be implemented as a standalone service. This is because this functionality is generic and does not depend on the model.
- DocSyncronizer functionality will be implemented as a standalone service. This is because this functionality is tightly depend on document provider service specification. The code of this functionality might change if the doc provider changes its specs or we switch to other doc provider, thus we need to isolate this from our api app.

## Code Structure
### Objective
The objective of this phase is to restructure the code, both physically (source code files/dirs) and logically to reduce developer confusions and/or misconceptions about the real world problem being modeled by the code.
### Implementation
From the existing code, we already have the 
- DocumentRepository
- EmailRepository

I'm a little bit _name sensitive_ so I think I'll change the name:
`DocumentRepository` to `DocSyncClient`, and 
`EmailRepository` to `EmailSenderClient`. Please refer to next section for details.

### Directory structure
The following is the proposed directory structure:
```
── api
    ├── Application
    │   ├── Mappers
    │   ├── Services
    │   ├── Usecases
    │   └── Validators
    ├── Domain
    │   ├── Entities
    │   └── Interfaces
    └── Infrastructure
        ├── Config
        ├── Data
        │   └── Repositories
        └── Services
            ├── DocSynchronizer
            ├── EmailSender
            ├── EmailVerifier
            ├── LogCollector
            ├── MetricsCollector
            ├── RateLimiter
            └── TraceCollector
```
|Directory|Purpose|
|---|---|
|**Application**|
|Mappers||
|`Services`||
|`Usecases`||
|`Validators`||
|**Domain**|
|`Entities`||
|`Interfaces`||
|**Infrastructure**|
|`Config`|To store configuration source code files|
|`Data`|To store data (in-rest) related source code files|
|`Data/Repositories`|To store the source code files for bridging in-rest and in-app representation of the models/entities|
|`Services`|To store the interfaces for utilizing external services|
|`Services/DocSynchronizer`|To store the implementation class of the IDocSyncronizer client interface|
|`Services/EmailSender`|To store the implementation class of the IEmailSender client interface|
|`Services/EmailVerifier`|To store the implementation class of the IEmailVerifier client interface|
|`Services/LogCollector`|To store the implementation class of the ILogCollector client interface|
|`Services/MetricsCollector`|To store the implementation class of the IMetricsCollector client interface|
|`Services/RateLimiter`|To store the implementation class of the IRateLimiter client interface|
|`Services/TraceCollector`|To store the implementation class of the ITraceCollector client interface|

### Functional Structure
The code will be devided into three main parts:
1. The model/entity, the real world entity being modeled in the app
2. The controller, the logic of the app that handles interaction with main stakeholder (users)
3. Repositories, the application part that handles repository functionalities (CRUD) related to the models.
4. Services, the application part that handles the logics to interact with external services/systems.

## Tests
### Objective
The objective of this phase is to 
- Unit tests & its coverage
- Integration tests
- End-to-end tests
### Implementation

## Best Practice Checklists
### Objective
The objective of this phase is to as much align the code with the well-known & battle-tested best practice in the software development industry while keeping the efforts under control. In this project we are going to check the implementation against the following best practices:
- Clean Architecture
- SOLID Principles
- 12 Factors App
### Implementation
#### Clean Architecture Checklist
|Check items|Planned|Actual|
|---|:---:|:---:|
|**Separation of Concerns**|
|Isolate business logic from external dependencies (frameworks, databases, UI)|YES|YES
|Follow the clear boundaries of the Domain, Application, and Infrastructure layers.|YES|YES
|**Dependency Inversion Principle (DIP)**|
|Depend on abstractions (interfaces) rather than concrete implementations.|YES|YES
|Use dependency injection to wire components together.|YES|YES
|**Entities and Value Objects**|
|Define domain models (entities) and value objects.|YES|YES
|Keep domain logic within entities.|YES|YES
|**Use Cases (Application Services)**|
|Implement use cases (application services) that orchestrate domain logic.|YES|YES
|Keep them free from infrastructure details.|YES|YES
|**Repositories**|
|Define repository interfaces in the Domain layer.|YES|YES
|Implement them in the Infrastructure layer (e.g., using Entity Framework).|YES|YES
|**Testability**|
|Write unit tests for domain models, use cases, and repositories.|YES|NO
|Use mock repositories for testing.|YES|NO

#### SOLID Principles Checklist
|Check items|Planned|Actual|
|---|:---:|:---:|
|Single Responsibility Principle (SRP)|YES|YES
|Open/Closed Principle (OCP)|YES|YES
|Liskov Substitution Principle (LSP)|YES|YES
|Interface Segregation Principle (ISP)|YES|YES
|Dependency Inversion Principle (DIP)|YES|YES

#### 12 Factors Checklist
|Check items|Planned|Actual|Remarks|
|---|:---:|:---:|:---:|
|**Codebase**|
|Track code in version control (e.g., Git)|YES|YES||
|Use a single source of truth for builds and deployments|YES|NO|n/a|
|**Dependencies**|
|Explicitly declare and isolate dependencies (e.g., package managers)|YES|YES||
|**Config**|
|Store configuration in environment variables|YES|NO||
|Avoid hardcoding configuration values|YES|NO||
|**Backing Services**|
|Treat databases, caches, and other services as attached resources|YES|YES||
|**Build, Release, Run**|
|Separate build, release, and run stages|YES|NO|n/a|
|Use immutable releases|YES|NO|n/a|
|**Processes**|
|Execute the app as stateless processes|YES|NO||
|Scale horizontally|YES|NO|n/a|
|**Port Binding**|
|Export services via port binding|YES|YES||
|Use environment variables for port configuration|YES|NO||
|**Concurrency**|
|Scale out by adding more processes|YES|NO|n/a|
|Avoid relying on shared memory|YES|YES||
|**Disposability**|
|Maximize robustness with fast startup and graceful shutdown|YES|YES||
|**Dev/Prod Parity**|
|Keep development, staging, and production environments as similar as possible|YES|NO|n/a|
|**Logs**|
|Treat logs as event streams|YES|NO||
|Aggregate logs centrally|YES|NO||
|**Admin Processes**|
|Run admin/management tasks as one-off processes|YES|NO|n/a|

## Further Enhancements
> If time wasn't a constraint what else would you have done?
- Separate document synchronization and sending email as individual usecase for 
    - testability
    - eliminate status ambiguity
    - easier rate limiting mechanism
    - enable retry mechanism
- Create unit tests (planned to use xunit & moq)
- Define other production supporting services (logging, rate-limiter etc.)

> How was this test overall? I.e too hard, too easy, how long it took, etc
- For Senior Engineer, IMHO:
    - The difficulty level can still be raised even more
    - Needs to cover even wider area (architecture, patterns, best-practices, tech culture etc.)
    - Specific skill inspection needs more specific instruction and more focus contexts
- Chronologically:
    - +/- 1 hour analyze the code
    - +/- 8 hours deciding refactoring strategy, and wrote this doc according to the strategy
    - +/- 6 hours implementation/coding
    - +/- 0.5 hours post-implementation update to this document (can't update them all)

## Extras
> Quality and best practices
- please refer to `Best Practice Checklists`
> Can this submission's code architecture easily scale to a codebase with 20 developers?
- Yes, it's physically and contextually sharded.
> How can you ensure data integrity in case of failures?
- All data write should use transaction (ACID) 
> How can you ensure the API behaves as you intend it to?
- Unit test for each levels: repositories, usecases and handlers. If possible also implement integration test with all possible flaky responses resulted from uncontrollable subsystem (for example: external service, etc.)
> How can you improve the performance of this?
- Data storage context: use index
- Data access context: use cache. If data is big, consider using multilevel caching.
- System load context: make sure users only uses just enough resources they need (rate limiter).
- System performance context: need to analyze first using metrics & trace collector to see which area needs improvements
- Object sze context: If entity objects are big, consider using Dto.
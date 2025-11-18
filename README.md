# IMPLEMENTAÇÃO DE INFRAESTRUTURA DE MICROSERVIÇOS COM DOCKER

## Trabalho Acadêmico - Infraestrutura e Serviços Essenciais

---

**Instituição:** [Nome da Instituição]  
**Curso:** [Nome do Curso]  
**Disciplina:** Infraestrutura e Serviços Essenciais  
**Aluno(a):** Sarah Ribeiro  
**Professor(a):** [Nome do Professor]  
**Data:** Novembro de 2025

---

## SUMÁRIO

1. [Introdução](#1-introdução)
2. [Objetivos](#2-objetivos)
3. [Fundamentação Teórica](#3-fundamentação-teórica)
4. [Metodologia](#4-metodologia)
5. [Implementação](#5-implementação)
6. [Testes e Resultados](#6-testes-e-resultados)
7. [Conclusão](#7-conclusão)
8. [Referências](#8-referências)
9. [Apêndices](#9-apêndices)

---

## 1. INTRODUÇÃO

### 1.1 Contexto

A arquitetura de microserviços tem se tornado cada vez mais relevante no desenvolvimento de sistemas modernos, permitindo escalabilidade, manutenibilidade e independência no desenvolvimento de componentes de software. Este trabalho implementa uma infraestrutura completa de microserviços utilizando containers Docker, demonstrando conceitos fundamentais de redes, DNS e comunicação entre serviços.

### 1.2 Justificativa

A containerização com Docker tornou-se um padrão da indústria para implantação de aplicações, oferecendo:
- **Isolamento**: Cada serviço executa em seu próprio ambiente
- **Portabilidade**: "Build once, run anywhere"
- **Escalabilidade**: Fácil replicação de containers
- **Consistência**: Mesmo ambiente em dev, test e produção

### 1.3 Escopo

Este projeto implementa dois microserviços independentes:
- **AuthService**: Gerenciamento de autenticação e autorização
- **ProductService**: Gerenciamento de produtos

Ambos comunicam-se através de uma rede Docker privada, utilizando DNS interno para service discovery e JWT (JSON Web Tokens) para autenticação segura.

---

## 2. OBJETIVOS

### 2.1 Objetivo Geral

Implementar uma infraestrutura de microserviços utilizando containers Docker, demonstrando conceitos de redes, DHCP, DNS e comunicação inter-serviços.

### 2.2 Objetivos Específicos

1. **Criar dois microserviços independentes** utilizando .NET Core 9.0
2. **Configurar uma rede Docker isolada** com subnet definida
3. **Implementar Service Discovery** através de DNS interno do Docker
4. **Simular atribuição dinâmica de IPs** (DHCP) via Docker Engine
5. **Estabelecer comunicação segura** entre serviços usando JWT
6. **Validar a infraestrutura** através de testes automatizados
7. **Documentar todo o processo** de forma clara e reproduzível

---

## 3. FUNDAMENTAÇÃO TEÓRICA

### 3.1 Arquitetura de Microserviços

#### 3.1.1 Definição

Microserviços são uma abordagem arquitetural onde uma aplicação é construída como um conjunto de pequenos serviços independentes, cada um executando em seu próprio processo e comunicando-se através de mecanismos leves, geralmente HTTP/REST APIs.

#### 3.1.2 Características Principais

- **Componentização via Serviços**: Cada serviço é uma unidade independente
- **Organização em torno de Capacidades de Negócio**: Serviços refletem domínios
- **Descentralização**: Cada serviço pode usar sua própria tecnologia e banco de dados
- **Automação de Infraestrutura**: Deploy e gestão automatizados

#### 3.1.3 Vantagens

- Escalabilidade independente por serviço
- Tecnologias heterogêneas
- Maior resiliência (falha isolada)
- Facilita desenvolvimento paralelo por equipes

#### 3.1.4 Desafios

- Complexidade de gerenciamento
- Necessidade de service discovery
- Comunicação entre serviços
- Consistência de dados distribuídos

### 3.2 Containerização com Docker

#### 3.2.1 Conceito

Docker é uma plataforma de containerização que empacota aplicações e suas dependências em containers isolados, garantindo que funcionem de forma consistente em qualquer ambiente.

#### 3.2.2 Componentes Principais

- **Docker Engine**: Runtime que executa containers
- **Docker Image**: Template imutável para criar containers
- **Docker Container**: Instância em execução de uma imagem
- **Docker Compose**: Ferramenta para definir aplicações multi-container
- **Docker Network**: Subsistema de rede para comunicação entre containers

#### 3.2.3 Diferença: Container vs Virtual Machine

| Aspecto | Container | Virtual Machine |
|---------|-----------|-----------------|
| Inicialização | Segundos | Minutos |
| Tamanho | Megabytes | Gigabytes |
| Isolamento | Processos | Hardware virtualizado |
| Overhead | Mínimo | Significativo |
| Portabilidade | Alta | Média |

### 3.3 Redes Docker

#### 3.3.1 Tipos de Redes

- **Bridge**: Rede privada interna (usado neste projeto)
- **Host**: Usa a rede do host diretamente
- **Overlay**: Para comunicação entre múltiplos Docker hosts
- **Macvlan**: Atribui endereço MAC aos containers

#### 3.3.2 Bridge Network

A rede bridge é o tipo padrão e mais comum:
- Cria uma rede privada interna no host
- Containers conectados podem se comunicar
- Oferece isolamento de outros containers
- Suporta resolução DNS automática

### 3.4 Service Discovery e DNS

#### 3.4.1 Conceito de Service Discovery

Service Discovery é o processo de detectar automaticamente serviços e suas localizações em uma rede, eliminando a necessidade de configuração manual de IPs.

#### 3.4.2 DNS no Docker

O Docker fornece um servidor DNS embutido que:
- Resolve nomes de containers para IPs automaticamente
- Atualiza registros quando containers são criados/destruídos
- Permite comunicação usando nomes legíveis
- Facilita mudanças de topologia sem reconfiguração

#### 3.4.3 Funcionamento

```
Container A → "http://containerB:8080"
             ↓
        Docker DNS Server
             ↓
        Resolve "containerB" → 172.20.0.3
             ↓
        Conecta em 172.20.0.3:8080
```

### 3.5 DHCP (Dynamic Host Configuration Protocol)

#### 3.5.1 Conceito

DHCP é um protocolo de rede que atribui automaticamente endereços IP e outros parâmetros de configuração a dispositivos em uma rede.

#### 3.5.2 Componentes

- **DHCP Server**: Gerencia pool de IPs e atribui aos clientes
- **DHCP Client**: Solicita configuração de rede
- **IP Pool**: Faixa de endereços disponíveis
- **Lease Time**: Tempo de validade da atribuição

#### 3.5.3 Simulação no Docker

O Docker Engine atua como servidor DHCP:
- Define subnet na criação da rede
- Atribui IPs sequencialmente aos containers
- Mantém registro de IPs atribuídos
- Libera IPs quando containers são removidos

### 3.6 Autenticação JWT

#### 3.6.1 Conceito

JSON Web Token (JWT) é um padrão aberto (RFC 7519) para transmitir informações de forma segura entre partes como um objeto JSON.

#### 3.6.2 Estrutura

Um JWT consiste de três partes separadas por pontos:

```
Header.Payload.Signature
```

- **Header**: Tipo de token e algoritmo de assinatura
- **Payload**: Claims (dados) do usuário
- **Signature**: Assinatura criptográfica para validação

#### 3.6.3 Fluxo de Autenticação

```
1. Cliente envia credenciais → AuthService
2. AuthService valida → gera JWT
3. Cliente recebe JWT
4. Cliente envia JWT → ProductService
5. ProductService valida assinatura do JWT
6. Se válido, processa requisição
```

### 3.7 ASP.NET Core Identity

Sistema completo de gerenciamento de identidade que fornece:
- Armazenamento de usuários e senhas (hashed)
- Autenticação de usuários
- Gerenciamento de roles e claims
- Validação de senhas
- Lockout de conta
- Autenticação de dois fatores

### 3.8 Entity Framework Core

ORM (Object-Relational Mapper) para .NET que:
- Mapeia objetos C# para tabelas de banco de dados
- Fornece migrations para versionamento de schema
- Suporta LINQ para queries
- Abstrai diferenças entre SGBDs
- Gerencia conexões e transações

---

## 4. METODOLOGIA

### 4.1 Abordagem de Desenvolvimento

Este projeto foi desenvolvido seguindo uma metodologia ágil iterativa:

1. **Planejamento**: Definição da arquitetura e tecnologias
2. **Implementação Incremental**: Desenvolvimento serviço por serviço
3. **Testes Contínuos**: Validação após cada implementação
4. **Documentação**: Registro simultâneo do processo

### 4.2 Ferramentas Utilizadas

#### 4.2.1 Desenvolvimento

- **IDE**: Visual Studio Code / Visual Studio 2025
- **SDK**: .NET 9.0
- **Linguagem**: C# 12.0
- **Versionamento**: Git

#### 4.2.2 Infraestrutura

- **Containerização**: Docker Desktop 4.35
- **Orquestração**: Docker Compose v2
- **Banco de Dados**: PostgreSQL 16
- **Sistema Operacional**: macOS / Windows / Linux

#### 4.2.3 Bibliotecas e Frameworks

**AuthService**:
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.0
- Microsoft.AspNetCore.Authentication.JwtBearer 9.0.0
- Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4
- System.IdentityModel.Tokens.Jwt 8.2.1

**ProductService**:
- Microsoft.EntityFrameworkCore.Design 9.0.0
- Microsoft.AspNetCore.Authentication.JwtBearer 9.0.0
- Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4

### 4.3 Arquitetura do Sistema

#### 4.3.1 Visão Geral

```
┌─────────────────────────────────────────────────────────┐
│         Docker Network: microservices_network           │
│              Subnet: 172.20.0.0/16                      │
│              Gateway: 172.20.0.1                        │
│                                                         │
│  ┌──────────────────┐  ┌──────────────────┐           │
│  │   AuthService    │  │  ProductService  │           │
│  │   172.20.0.3     │  │   172.20.0.4     │           │
│  │   Porta: 5001    │  │   Porta: 5002    │           │
│  │                  │  │                  │           │
│  │ • Register User  │  │ • CRUD Products  │           │
│  │ • Login/JWT      │  │ • Validate JWT   │           │
│  │ • ASP.NET Ident. │  │ • EF Core        │           │
│  └────────┬─────────┘  └────────┬─────────┘           │
│           │                     │                      │
│           │   Service Discovery │                      │
│           │   (Docker DNS)      │                      │
│           └──────────┬──────────┘                      │
│                      │                                 │
│           ┌──────────▼──────────┐                      │
│           │    PostgreSQL 16    │                      │
│           │    172.20.0.2       │                      │
│           │    Porta: 5432      │                      │
│           │                     │                      │
│           │  • authservicedb    │                      │
│           │    - AspNetUsers    │                      │
│           │    - AspNetRoles    │                      │
│           │                     │                      │
│           │  • productservicedb │                      │
│           │    - Products       │                      │
│           └─────────────────────┘                      │
└─────────────────────────────────────────────────────────┘
```

#### 4.3.2 Fluxo de Dados

**Cenário: Criação de Produto**

1. **Cliente → AuthService** (Login)
   - Método: POST /api/auth/login
   - Entrada: Email e senha
   - Saída: Token JWT

2. **AuthService → PostgreSQL** (Validação)
   - DNS: postgres → 172.20.0.2
   - Query: SELECT FROM AspNetUsers
   - Validação de senha hashed

3. **Cliente → ProductService** (Criar Produto)
   - Método: POST /api/products
   - Header: Authorization: Bearer {JWT}
   - Entrada: Dados do produto

4. **ProductService** (Validação JWT)
   - Verifica assinatura do token
   - Extrai claims do usuário
   - Valida expiração

5. **ProductService → PostgreSQL** (Persistência)
   - DNS: postgres → 172.20.0.2
   - Query: INSERT INTO Products
   - Registra criador (from JWT)

6. **ProductService → Cliente** (Resposta)
   - Status: 201 Created
   - Body: Produto criado com ID

### 4.4 Decisões de Design

#### 4.4.1 Separação de Bancos de Dados

Optou-se por bancos separados por serviço para:
- **Autonomia**: Cada serviço controla seu próprio schema
- **Escalabilidade**: Bancos podem escalar independentemente
- **Isolamento**: Falha em um banco não afeta outros
- **Evolução**: Schemas evoluem independentemente

#### 4.4.2 Autenticação JWT

JWT foi escolhido por:
- **Stateless**: Não requer sessão no servidor
- **Distribuído**: Funciona bem em ambientes de microserviços
- **Performance**: Validação local sem consulta a banco
- **Padrão**: Amplamente suportado e documentado

#### 4.4.3 Docker Compose

Docker Compose foi escolhido para:
- **Simplicidade**: Define toda infraestrutura em um arquivo
- **Reprodutibilidade**: Mesmo ambiente em qualquer máquina
- **Versionamento**: YAML pode ser versionado no Git
- **Desenvolvimento**: Fácil iniciar/parar todos serviços

---

## 5. IMPLEMENTAÇÃO

### 5.1 Estrutura de Diretórios

```
vine-connect-platform/
├── AuthService/
│   ├── Controllers/
│   │   └── AuthController.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Models/
│   │   └── Auth/
│   │       ├── LoginModel.cs
│   │       ├── RegisterModel.cs
│   │       └── AuthResponse.cs
│   ├── Migrations/
│   ├── Dockerfile
│   ├── Program.cs
│   └── AuthService.csproj
│
├── ProductService/
│   ├── Controllers/
│   │   └── ProductsController.cs
│   ├── Data/
│   │   └── ProductDbContext.cs
│   ├── Models/
│   │   └── Product.cs
│   ├── Services/
│   │   └── AuthServiceClient.cs
│   ├── Migrations/
│   ├── Dockerfile
│   ├── Program.cs
│   └── ProductService.csproj
│
├── docker-compose.yml
├── init-db.sql
├── test-infrastructure.sh
└── README.md
```

### 5.2 Configuração de Rede Docker

**Arquivo: docker-compose.yml**

```yaml
networks:
  microservices_network:
    driver: bridge
    ipam:
      config:
        - subnet: 172.20.0.0/16
```

**Características**:
- **Driver Bridge**: Rede privada interna
- **IPAM**: IP Address Management configurado
- **Subnet /16**: Permite até 65.536 endereços IP
- **Gateway**: 172.20.0.1 (automático)

### 5.3 AuthService

#### 5.3.1 Responsabilidades

- Registro de novos usuários
- Autenticação de credenciais
- Geração de tokens JWT
- Gerenciamento de identidade (ASP.NET Identity)

#### 5.3.2 Endpoints

**POST /api/auth/register**
```json
Request:
{
  "email": "user@example.com",
  "password": "Password@123",
  "username": "user"
}

Response: 200 OK
{
  "message": "User created successfully!"
}
```

**POST /api/auth/login**
```json
Request:
{
  "email": "user@example.com",
  "password": "Password@123"
}

Response: 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiration": "2025-11-18T20:00:00Z",
  "username": "user"
}
```

#### 5.3.3 Configuração JWT

```csharp
// appsettings.json
"Jwt": {
  "Key": "chave_secreta_de_pelo_menos_32_caracteres",
  "Issuer": "AuthService",
  "Audience": "AuthServiceUsers",
  "ExpiryInHours": 24
}
```

#### 5.3.4 Geração de Token

```csharp
private JwtSecurityToken GenerateToken(List<Claim> authClaims)
{
    var authSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
    );
    
    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        expires: DateTime.Now.AddHours(24),
        claims: authClaims,
        signingCredentials: new SigningCredentials(
            authSigningKey, 
            SecurityAlgorithms.HmacSha256
        )
    );

    return token;
}
```

### 5.4 ProductService

#### 5.4.1 Responsabilidades

- CRUD de produtos
- Validação de tokens JWT
- Registro de auditoria (createdBy)
- Autorização de operações

#### 5.4.2 Endpoints

**GET /api/products**
```json
Response: 200 OK
[
  {
    "id": 1,
    "name": "Notebook Dell",
    "price": 3500.00,
    "description": "Notebook premium",
    "createdAt": "2025-11-17T20:00:00Z",
    "createdBy": "admin"
  }
]
```

**POST /api/products**
```json
Request:
Headers: Authorization: Bearer {token}
{
  "name": "Mouse Logitech",
  "price": 150.00,
  "description": "Mouse sem fio"
}

Response: 201 Created
{
  "id": 2,
  "name": "Mouse Logitech",
  "price": 150.00,
  "description": "Mouse sem fio",
  "createdAt": "2025-11-17T20:30:00Z",
  "createdBy": "admin"
}
```

#### 5.4.3 Validação JWT

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey!)
        )
    };
});
```

#### 5.4.4 Extração de Usuário

```csharp
[HttpPost]
public async Task<ActionResult<Product>> PostProduct(Product product)
{
    // Extrai username do JWT
    var username = User.FindFirst(ClaimTypes.Name)?.Value;
    product.CreatedBy = username;
    
    _context.Products.Add(product);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetProduct), 
        new { id = product.Id }, product);
}
```

### 5.5 PostgreSQL

#### 5.5.1 Configuração

```yaml
postgres:
  image: postgres:16
  environment:
    POSTGRES_USER: postgres
    POSTGRES_PASSWORD: postgres123
    POSTGRES_DB: postgres
  ports:
    - "5432:5432"
  volumes:
    - postgres_data:/var/lib/postgresql/data
    - ./init-db.sql:/docker-entrypoint-initdb.d/init-db.sql:ro
```

#### 5.5.2 Script de Inicialização

```sql
-- init-db.sql
CREATE DATABASE authservicedb;
CREATE DATABASE productservicedb;
```

#### 5.5.3 Schema AuthService

Tabelas criadas pelo ASP.NET Identity:
- **AspNetUsers**: Dados dos usuários
- **AspNetRoles**: Roles do sistema
- **AspNetUserRoles**: Relacionamento usuário-role
- **AspNetUserClaims**: Claims customizadas
- **AspNetUserLogins**: Logins externos
- **AspNetUserTokens**: Tokens de autenticação

#### 5.5.4 Schema ProductService

```sql
CREATE TABLE "Products" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Price" DECIMAL(18,2) NOT NULL,
    "Description" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256)
);
```

### 5.6 Docker Configuration

#### 5.6.1 Dockerfile AuthService

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AuthService.csproj", "./"]
RUN dotnet restore "AuthService.csproj"
COPY . .
RUN dotnet build "AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
```

#### 5.6.2 Docker Compose Completo

```yaml
services:
  postgres:
    image: postgres:16
    container_name: microservices_postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
      POSTGRES_DB: postgres
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./init-db.sql:/docker-entrypoint-initdb.d/init-db.sql:ro
    networks:
      - microservices_network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5

  authservice:
    build:
      context: ./AuthService
      dockerfile: Dockerfile
    container_name: authservice
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=authservicedb;Username=postgres;Password=postgres123
      - Jwt__Key=XkT9mP2vL4wR7nQ8hS6jK3dF5gH1aZ0yB9xC4tN7uM2pE8qW5oI6rV3sA1bG4cJ7
      - Jwt__Issuer=AuthService
      - Jwt__Audience=AuthServiceUsers
      - Jwt__ExpiryInHours=24
    ports:
      - "5001:8080"
    depends_on:
      postgres:
        condition: service_healthy
    networks:
      - microservices_network
    restart: on-failure

  productservice:
    build:
      context: ./ProductService
      dockerfile: Dockerfile
    container_name: productservice
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=productservicedb;Username=postgres;Password=postgres123
      - Jwt__Key=XkT9mP2vL4wR7nQ8hS6jK3dF5gH1aZ0yB9xC4tN7uM2pE8qW5oI6rV3sA1bG4cJ7
      - Jwt__Issuer=AuthService
      - Jwt__Audience=AuthServiceUsers
      - Services__AuthService__Url=http://authservice:8080
    ports:
      - "5002:8080"
    depends_on:
      postgres:
        condition: service_healthy
      authservice:
        condition: service_started
    networks:
      - microservices_network
    restart: on-failure

networks:
  microservices_network:
    driver: bridge
    ipam:
      config:
        - subnet: 172.20.0.0/16

volumes:
  postgres_data:
```

### 5.7 Migrations

#### 5.7.1 AuthService Migrations

```bash
cd AuthService
dotnet ef migrations add InitialIdentitySetup
dotnet ef database update
```

**Resultado**: Cria todas as tabelas do ASP.NET Identity

#### 5.7.2 ProductService Migrations

```bash
cd ProductService
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Resultado**: Cria tabela Products com schema definido

---

## 6. TESTES E RESULTADOS

### 6.1 Estratégia de Testes

Os testes foram divididos em três categorias:
1. **Testes de Infraestrutura**: Rede, DNS, conectividade
2. **Testes Funcionais**: Endpoints e lógica de negócio
3. **Testes de Integração**: Comunicação entre serviços

### 6.2 Script de Testes Automatizados

Foi desenvolvido um script Bash (`test-infrastructure.sh`) que executa:
- 8 categorias de testes
- Validação de rede e DNS
- Testes de endpoints
- Verificação de banco de dados
- Geração de relatório

### 6.3 Resultados dos Testes

#### 6.3.1 Testes de Infraestrutura

| # | Teste | Status | Detalhes |
|---|-------|--------|----------|
| 1 | Rede Docker | ✅ PASSOU | Subnet 172.20.0.0/16 configurada |
| 2 | DNS - AuthService | ✅ PASSOU | Resolvido para 172.20.0.3 |
| 3 | DNS - PostgreSQL | ✅ PASSOU | Resolvido para 172.20.0.2 |
| 4 | Conectividade HTTP | ✅ PASSOU | ProductService → AuthService OK |

**Evidência**:
```bash
📡 Verificando rede Docker...
✓ Rede 'microservices_network' existe
  Subnet: 172.20.0.0/16

🔍 Testando resolução DNS de 'authservice'...
✓ ProductService consegue resolver 'authservice'
  IP resolvido: 172.20.0.3

🔍 Testando resolução DNS de 'postgres'...
✓ ProductService consegue resolver 'postgres'
  IP resolvido: 172.20.0.2
```

#### 6.3.2 Testes Funcionais

| # | Teste | Status | Detalhes |
|---|-------|--------|----------|
| 5 | Registro de Usuário | ✅ PASSOU | Usuário criado em authservicedb |
| 6 | Login | ✅ PASSOU | Token JWT gerado com sucesso |
| 7 | Criar Produto | ✅ PASSOU | Produto ID 3 criado |
| 8 | Listar Produtos | ✅ PASSOU | 3 produtos retornados |

**Evidência**:
```bash
[4/8] Testando AuthService - Registro de Usuário
✓ Usuário registrado (ou já existia)
  Resposta: {"message":"Usuário criado com sucesso!"}

[5/8] Testando AuthService - Login
✓ Login realizado com sucesso
  Token (primeiros 50 chars): eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

[6/8] Testando ProductService - Criar Produto
✓ Produto criado com sucesso
  ID do produto: 3
  Resposta: {"id":3,"name":"Produto de Teste","price":1000.00,...}
```

#### 6.3.3 Testes de Banco de Dados

| Banco | Status | Registros |
|-------|--------|-----------|
| authservicedb | ✅ ACESSÍVEL | 2 usuários |
| productservicedb | ✅ ACESSÍVEL | 3 produtos |

**Evidência**:
```bash
[8/8] Verificando Bancos de Dados
💾 Verificando banco 'authservicedb'...
✓ Banco 'authservicedb' acessível
  Total de usuários: 2

💾 Verificando banco 'productservicedb'...
✓ Banco 'productservicedb' acessível
  Total de produtos: 3
```

### 6.4 Demonstração de Service Discovery

**Teste**: Container ProductService resolve nome do AuthService

```bash
$ docker exec productservice sh -c "getent hosts authservice"
172.20.0.3      authservice
```

**Análise**: O DNS interno do Docker resolveu automaticamente o nome `authservice` para o IP `172.20.0.3`, demonstrando o funcionamento do Service Discovery.

### 6.5 Demonstração de DHCP Simulado

**Verificação de IPs atribuídos**:

```bash
$ docker network inspect vine-connect-platform_microservices_network

"Containers": {
    "authservice": {
        "IPv4Address": "172.20.0.3/16"
    },
    "productservice": {
        "IPv4Address": "172.20.0.4/16"
    },
    "postgres": {
        "IPv4Address": "172.20.0.2/16"
    }
}
```

**Análise**: O Docker Engine atribuiu IPs sequencialmente dentro da subnet configurada (172.20.0.0/16), simulando o comportamento de um servidor DHCP.

### 6.6 Teste de Comunicação Completa

**Cenário**: Usuário se autentica e cria um produto

```bash
# 1. Registrar usuário
curl -X POST http://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"teste@infra.com","password":"Teste@123","username":"teste"}'

# Resposta: {"message":"Usuário criado com sucesso!"}

# 2. Login
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"teste@infra.com","password":"Teste@123"}'

# Resposta: {"token":"eyJhbGc...","expiration":"2025-11-18...","username":"teste"}

# 3. Criar produto (usando token)
curl -X POST http://localhost:5002/api/products \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "Content-Type: application/json" \
  -d '{"name":"Notebook","price":3500,"description":"Dell XPS"}'

# Resposta: {"id":4,"name":"Notebook","price":3500,"createdBy":"teste",...}
```

**Fluxo de Dados**:
1. ✅ Cliente → AuthService (porta 5001)
2. ✅ AuthService → PostgreSQL via DNS (postgres:5432)
3. ✅ AuthService retorna JWT
4. ✅ Cliente → ProductService (porta 5002) com JWT
5. ✅ ProductService valida JWT (chave compartilhada)
6. ✅ ProductService → PostgreSQL via DNS (postgres:5432)
7. ✅ Produto criado com campo `createdBy` preenchido

### 6.7 Análise de Performance

**Tempo de resposta médio**:
- Registro: ~150ms
- Login: ~120ms
- Criar produto: ~80ms
- Listar produtos: ~50ms

**Uso de recursos** (docker stats):
```
CONTAINER         CPU %    MEM USAGE / LIMIT
authservice       0.50%    95MB / 4GB
productservice    0.45%    92MB / 4GB
postgres          1.20%    45MB / 4GB
```

### 6.8 Relatório Final de Testes

```
================================================
  ✓ TESTES CONCLUÍDOS COM SUCESSO
================================================

📋 Resumo da Infraestrutura:
  • Rede Docker: microservices_network (172.20.0.0/16)
  • DNS Interno: Service Discovery ativo
  • AuthService: Operacional (porta 5001)
  • ProductService: Operacional (porta 5002)
  • PostgreSQL: Operacional (porta 5432)
  • Usuários cadastrados: 2
  • Produtos cadastrados: 3

🎯 Requisitos do Exercício:
  ✓ 2 microsserviços implementados
  ✓ Estrutura de rede em containers Docker
  ✓ Simulação de DHCP (IPs dinâmicos)
  ✓ Simulação de DNS (Service Discovery)
  ✓ Comunicação entre serviços funcionando
```

---

## 7. CONCLUSÃO

### 7.1 Objetivos Alcançados

Este trabalho alcançou todos os objetivos propostos:

1. ✅ **Dois microserviços implementados**: AuthService e ProductService foram desenvolvidos e estão funcionais
2. ✅ **Rede Docker configurada**: Subnet 172.20.0.0/16 com isolamento adequado
3. ✅ **Service Discovery**: DNS interno resolve nomes de serviços automaticamente
4. ✅ **DHCP simulado**: Docker Engine atribui IPs dinamicamente
5. ✅ **Comunicação segura**: JWT garante autenticação entre serviços
6. ✅ **Testes validados**: 100% dos testes automatizados passaram
7. ✅ **Documentação completa**: Todo o processo foi documentado

### 7.2 Aprendizados Principais

#### 7.2.1 Técnicos

- **Containerização**: Compreensão profunda de como Docker funciona
- **Redes**: Entendimento de subnets, DNS e comunicação entre containers
- **Microserviços**: Experiência prática com arquitetura distribuída
- **Segurança**: Implementação de autenticação JWT
- **DevOps**: Uso de Docker Compose para orquestração

#### 7.2.2 Conceituais

- **Service Discovery**: Importância da descoberta automática de serviços
- **Isolamento**: Benefícios de isolar serviços e bancos de dados
- **Escalabilidade**: Como a arquitetura facilita crescimento
- **Resiliência**: Como containers podem ser reiniciados automaticamente

### 7.3 Desafios Enfrentados

#### 7.3.1 Configuração de Portas

**Problema**: Containers escutavam na porta 8080 interna, mas docker-compose mapeava porta 80.

**Solução**: Atualizar mapeamento de portas para `5001:8080` e `5002:8080`.

**Aprendizado**: Entender diferença entre portas internas (container) e externas (host).

#### 7.3.2 Certificados SSL

**Problema**: ProductService tentava usar HTTPS sem certificados.

**Solução**: Remover configuração de HTTPS, usar apenas HTTP na rede interna.

**Aprendizado**: Ambiente de desenvolvimento pode usar HTTP em rede interna.

#### 7.3.3 Models em Português vs Inglês

**Problema**: API esperava `Password` mas código usava `Senha`.

**Solução**: Padronizar todos os models para inglês.

**Aprendizado**: Importância de convenções de nomenclatura consistentes.

### 7.4 Trabalhos Futuros

Melhorias que podem ser implementadas:

1. **API Gateway**: Centralizar acesso aos microserviços
2. **Load Balancer**: Distribuir carga entre múltiplas instâncias
3. **Circuit Breaker**: Implementar resiliência com Polly
4. **Monitoring**: Adicionar Prometheus + Grafana
5. **Logging Centralizado**: ELK Stack ou Seq
6. **CI/CD**: Pipeline automatizado com GitHub Actions
7. **Kubernetes**: Migrar de Docker Compose para K8s
8. **HTTPS**: Implementar TLS com Let's Encrypt
9. **Cache Distribuído**: Redis para performance
10. **Message Broker**: RabbitMQ para comunicação assíncrona

### 7.5 Contribuições

Este projeto demonstra na prática conceitos fundamentais de:
- Arquitetura de microserviços moderna
- Infraestrutura como código
- DevOps e containerização
- Segurança em APIs
- Testes automatizados

### 7.6 Considerações Finais

A implementação deste projeto proporcionou uma visão completa sobre como construir, testar e documentar uma infraestrutura de microserviços. Os conceitos aprendidos são diretamente aplicáveis em ambientes de produção reais.

A arquitetura implementada é escalável, manutenível e segue boas práticas da indústria. O uso de Docker e Docker Compose facilita a replicação do ambiente em qualquer máquina, garantindo consistência entre desenvolvimento e produção.

Os testes automatizados garantem que a infraestrutura funciona conforme esperado, e a documentação detalhada permite que outros desenvolvedores compreendam e reproduzam o projeto.

**Este trabalho demonstra que os requisitos do exercício de "Infraestrutura e Serviços Essenciais" foram plenamente atendidos**, com implementação prática de:
- ✅ 2 microsserviços escolhidos e implementados
- ✅ Estrutura básica de rede em containers Docker
- ✅ Simulação de ambiente com DHCP e DNS
- ✅ Serviços registrados e comunicando-se entre si

---

## 8. REFERÊNCIAS

### 8.1 Documentação Oficial

1. **Docker Documentation**. Docker Inc. Disponível em: https://docs.docker.com/. Acesso em: nov. 2025.

2. **ASP.NET Core Documentation**. Microsoft. Disponível em: https://docs.microsoft.com/aspnet/core. Acesso em: nov. 2025.

3. **Entity Framework Core Documentation**. Microsoft. Disponível em: https://docs.microsoft.com/ef/core. Acesso em: nov. 2025.

4. **PostgreSQL Documentation**. PostgreSQL Global Development Group. Disponível em: https://www.postgresql.org/docs/. Acesso em: nov. 2025.

### 8.2 Livros e Artigos

5. NEWMAN, Sam. **Building Microservices: Designing Fine-Grained Systems**. 2nd ed. O'Reilly Media, 2021.

6. RICHARDS, Mark; FORD, Neal. **Fundamentals of Software Architecture**. O'Reilly Media, 2020.

7. KANE, Sean P.; MATTHIAS, Karl. **Docker: Up & Running**. 3rd ed. O'Reilly Media, 2023.

### 8.3 Especificações e RFCs

8. **RFC 7519 - JSON Web Token (JWT)**. IETF, 2015. Disponível em: https://tools.ietf.org/html/rfc7519.

9. **RFC 2131 - Dynamic Host Configuration Protocol**. IETF, 1997. Disponível em: https://tools.ietf.org/html/rfc2131.

10. **RFC 1035 - Domain Names - Implementation and Specification**. IETF, 1987. Disponível em: https://tools.ietf.org/html/rfc1035.

### 8.4 Tutoriais e Guias

11. **Microsoft Learn - ASP.NET Core tutorials**. Microsoft. Disponível em: https://learn.microsoft.com/aspnet/core/tutorials.

12. **Docker Compose Documentation**. Docker Inc. Disponível em: https://docs.docker.com/compose/.

13. **JWT.io - Introduction to JSON Web Tokens**. Auth0. Disponível em: https://jwt.io/introduction.

### 8.5 Ferramentas e Tecnologias

14. **.NET 9.0**. Microsoft, 2024. Disponível em: https://dotnet.microsoft.com/download/dotnet/9.0.

15. **Docker Desktop**. Docker Inc., 2024. Disponível em: https://www.docker.com/products/docker-desktop.

16. **Visual Studio Code**. Microsoft, 2024. Disponível em: https://code.visualstudio.com/.

---

## 9. APÊNDICES

### APÊNDICE A - Códigos Fonte Principais

#### A.1 AuthController.cs (Completo)

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuthService.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<IdentityUser> userManager, 
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return BadRequest(new { message = "Usuário já existe!" });

        var user = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return BadRequest(new { 
                message = "Erro ao criar usuário", 
                errors = result.Errors 
            });

        return Ok(new { message = "Usuário criado com sucesso!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized(new { message = "Email ou senha inválidos" });

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = GenerateToken(authClaims);

        return Ok(new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            Username = user.UserName!
        });
    }

    private JwtSecurityToken GenerateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(
                Convert.ToDouble(_configuration["Jwt:ExpiryInHours"])
            ),
            claims: authClaims,
            signingCredentials: new SigningCredentials(
                authSigningKey, 
                SecurityAlgorithms.HmacSha256
            )
        );

        return token;
    }
}
```

#### A.2 ProductsController.cs (Completo)

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using System.Security.Claims;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ProductDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        ProductDbContext context, 
        ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        _logger.LogInformation($"User {username} listing products");
        
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            _logger.LogWarning($"Product {id} not found");
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        product.CreatedBy = username;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Product {product.Id} created by {username}");
        
        return CreatedAtAction(
            nameof(GetProduct), 
            new { id = product.Id }, 
            product
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Product {id} updated");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Product {id} deleted");

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }

    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult Health()
    {
        return Ok(new { 
            service = "ProductService", 
            status = "healthy", 
            timestamp = DateTime.UtcNow 
        });
    }
}
```

### APÊNDICE B - Comandos Docker Úteis

```bash
# Construir e iniciar todos os serviços
docker-compose up --build

# Iniciar em background
docker-compose up -d

# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f authservice

# Parar todos os serviços
docker-compose down

# Parar e remover volumes (limpar banco de dados)
docker-compose down -v

# Reconstruir apenas um serviço
docker-compose build authservice
docker-compose up authservice

# Ver containers rodando
docker ps

# Entrar em um container
docker exec -it authservice sh

# Ver uso de recursos
docker stats

# Inspecionar rede
docker network inspect vine-connect-platform_microservices_network

# Conectar ao PostgreSQL
docker exec -it microservices_postgres psql -U postgres -d authservicedb
```

### APÊNDICE C - Comandos de Banco de Dados

```sql
-- Conectar ao banco
\c authservicedb

-- Listar tabelas
\dt

-- Ver estrutura de uma tabela
\d "AspNetUsers"

-- Ver usuários cadastrados
SELECT "Id", "UserName", "Email", "EmailConfirmed" 
FROM "AspNetUsers";

-- Conectar ao banco de produtos
\c productservicedb

-- Ver produtos
SELECT * FROM "Products";

-- Ver produtos com filtro
SELECT "Name", "Price", "CreatedBy" 
FROM "Products" 
WHERE "Price" > 1000;

-- Contar registros
SELECT COUNT(*) FROM "Products";

-- Sair
\q
```

### APÊNDICE D - Troubleshooting

#### Problema: Container não inicia

```bash
# Ver logs detalhados
docker-compose logs authservice

# Ver últimas 50 linhas
docker-compose logs --tail=50 authservice

# Reconstruir imagem
docker-compose build --no-cache authservice
docker-compose up authservice
```

#### Problema: Porta já em uso

```bash
# Ver o que está usando a porta
lsof -i :5001
lsof -i :5432

# Matar processo
kill -9 <PID>

# Ou mudar porta no docker-compose.yml
ports:
  - "5003:8080"  # Usa porta 5003 ao invés de 5001
```

#### Problema: Banco de dados não conecta

```bash
# Verificar se PostgreSQL está rodando
docker ps | grep postgres

# Testar conexão
docker exec -it microservices_postgres psql -U postgres

# Ver logs do PostgreSQL
docker-compose logs postgres

# Recriar banco
docker-compose down -v
docker-compose up postgres
```

---

**FIM DO DOCUMENTO**

---

*Este documento foi gerado como parte do trabalho acadêmico da disciplina de Infraestrutura e Serviços Essenciais.*

*Última atualização: Novembro de 2025*

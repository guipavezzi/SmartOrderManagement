# Tempo 86 - Smart Order Management (Back-end & API)

O **Tempo 86** é um sistema moderno de gerenciamento de pedidos e tempos de preparo (KDS - Kitchen Display System).

🔗 **Repositório do Front-end (Interface & Tauri):** [smart_order_management_web](https://github.com/guipavezzi/smart_order_management_web)

## 🏗️ Sobre este Repositório (Back-end)

Este repositório contém toda a **Lógica de Negócios**, **API REST** e **Gerenciamento de Banco de Dados** do sistema. 

A arquitetura foi projetada para ser leve, autossuficiente e rápida, sendo executada como um serviço (Sidecar) invisível para o usuário final através da integração com o Tauri.

### Stack Tecnológica
* **Linguagem:** C# / .NET 8
* **Arquitetura:** Clean Architecture (Domain, Application, Infrastructure, API)
* **Banco de Dados:** SQLite (com Entity Framework Core)
* **Padrões:** Repository Pattern, Injeção de Dependência, DTOs

### Gerenciamento de Dados Seguros
Diferente das APIs web tradicionais, este sistema foi projetado para rodar nativamente no Desktop do usuário.
Por isso, o banco de dados SQLite (`smartordermanagement.db`) não fica salvo na pasta do projeto. Ele é **gerado automaticamente** na pasta de dados do próprio sistema operacional:
`%LocalAppData%\SmartOrderManagement\`
Isso previne erros de permissão do Windows (UAC) e evita a perda do banco de dados quando o aplicativo do cliente for atualizado.

## 🚀 Como Desenvolver Localmente

Se você for dar manutenção na API, siga os passos abaixo:

### Pré-requisitos
* .NET 8 SDK instalado.

### Rodando a API
Navegue até a pasta principal (`SmartOrderManagement.API`) e rode:
```bash
dotnet run
```
A API iniciará localmente (geralmente na porta `5000`).
*(Lembre-se de rodar o Front-end Angular em paralelo para testar o sistema completo)*.

## 📦 Como gerar o Executável para o Tauri (Sidecar)

Para que o Front-end (Tauri) consiga abrir esta API magicamente no computador do cliente, precisamos compilar este projeto em um **Único Arquivo Executável** (Single-file Executable) e mandar para a pasta do Tauri.

Siga os passos:

1. Abra o terminal na pasta raiz `SmartOrderManagement.API`.
2. Rode o seguinte comando para compilar e enviar o arquivo diretamente para a pasta do Tauri:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ../smart_order_management_web/src-tauri/binaries/
   ```
3. O comando acima vai gerar um arquivo `.exe` (ex: `SmartOrderManagement.API.exe`).
4. Vá até a pasta `smart_order_management_web/src-tauri/binaries/` e **renomeie esse executável** exatamente para:
   `api-x86_64-pc-windows-msvc.exe`

Pronto! Agora é só seguir os passos lá no README do Front-end para rodar o `npm run tauri build` e gerar o instalador oficial do cliente.

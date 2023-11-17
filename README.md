<h1>Dominando Injeção de Dependência</h1>

Ref.: Balta.io

> O conteúdo também foi organizado nos **commits**

<!--#region Sumário -->

<h2>Sumário</h2>

<!--#region Fundamentos -->

<details><summary>Fundamentos</summary>

<ul>
    <li><a href="#fund-apresentacao">Apresentação</a></li>
    <li><a href="#fund-oquee">O que é injeção de dependência</a></li>
</ul>

</details>

<!--#endregion -->

<!--#endregion -->

<!--#region Fundamentoss -->

<h2 id="fund">Fundamentos</h2>

<!--#region Apresentação  -->

<details id="fund-apresentacao"><summary>Apresentacao</summary>

<br/>

Agenda:

- O que é DI (injeção de dependência)?
- O que é IoC (inversão de controle)?
- O que é DIP (princípio da inversão da dependência)?
- Como os itens acima se relacionam
- DI no ASP.NET

Sobre este curso:

- Devs ASP.NET/ .NET
- Buscam aprimorar a teoria
- Querem conhecer mais DI

```c#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MagGet("/", () => "Dependency Injection!");

app.Run();
```

</details>

<!--#endregion -->

<!--#region O que é injeção de dependência  -->

<details id="fund-oquee"><summary>O que é injeção de dependência</summary>

<br/>

> Um termo bem confuso

Dependency Injection

- **Não é um padrão** (Design Pattern)
- Técnica que **implementa o IoC**
  - **IoC - Inversion of Control** (Inversão de Controle)
  - **DIP - Dependency Inversion Principle** (Princípio da Inversão de Dependência)
- Ajuda no baixo acoplamento
- Provê uma melhor divisão de responsabilidades
- O que eu preciso para trabalhar?
  - Quem vai me prover? Não importa.

</details>

<!--#endregion -->

<!--#endregion -->

<!--#endregion -->

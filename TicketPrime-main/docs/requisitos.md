# 📋 Requisitos do Sistema - TicketPrime

## 📖 Histórias de Usuário (Cap. 6)

1. **Cadastro de Evento**: Como Organizador de Eventos, Quero cadastrar um novo evento com nome, capacidade e preço, Para que o sistema possa controlar a venda de ingressos corretamente.
2. **Cadastro de Usuário**: Como Comprador, Quero me cadastrar informando meu CPF, nome e e-mail, Para que eu possa realizar reservas de ingressos no sistema.
3. **Gestão de Cupons**: Como Administrador, Quero cadastrar cupons de desconto com valor mínimo de regra, Para incentivar vendas de tickets de maior valor.

## ✅ Critérios de Aceitação (BDD - Cap. 6)

**Cenário: Cadastro de Usuário com CPF Duplicado**
* **Dado que** o sistema já possui um usuário cadastrado com o CPF "12345678901"
* **Quando** um novo cadastro é tentado com esse mesmo CPF
* **Então** o sistema deve retornar um erro 400 (Bad Request) informando que o CPF já existe.
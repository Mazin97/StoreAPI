# Desafio Back-end

Esse repositório guarda a implementação de um desafio de back-end que fiz por hobby.

### Requisitos

A seguir estão algumas regras de negócio que são importantes para o funcionamento do PicPay Simplificado:

- Para ambos tipos de usuário, precisamos do `Nome Completo`, `CPF`, `e-mail` e `Senha`. CPF/CNPJ e e-mails devem ser
  únicos no sistema. Sendo assim, seu sistema deve permitir apenas um cadastro com o mesmo CPF ou endereço de e-mail;

- Usuários podem enviar dinheiro (efetuar transferência) para lojistas e entre usuários;

- Lojistas **só recebem** transferências, não enviam dinheiro para ninguém;

- Validar se o usuário tem saldo antes da transferência;

- Antes de finalizar a transferência, deve-se consultar um serviço autorizador externo, use este mock
  [https://util.devi.tools/api/v2/authorize](https://util.devi.tools/api/v2/authorize) para simular o serviço
  utilizando o verbo `GET`;

- A operação de transferência deve ser uma transação (ou seja, revertida em qualquer caso de inconsistência) e o
  dinheiro deve voltar para a carteira do usuário que envia;

- No recebimento de pagamento, o usuário ou lojista precisa receber notificação (envio de email, sms) enviada por um
  serviço de terceiro e eventualmente este serviço pode estar indisponível/instável. Use este mock
  [https://util.devi.tools/api/v1/notify)](https://util.devi.tools/api/v1/notify)) para simular o envio da notificação
  utilizando o verbo `POST`;

- Este serviço deve ser RESTFul.

### Endpoints

Baseado no desafio, criei 3 endpoints:

1. create-user:

```
curl --request POST \
  --url https://localhost:44340/store/create-user \
  --header 'Content-Type: application/json' \
  --header 'User-Agent: insomnia/2023.5.8' \
  --data '{
	"name": "John Doerer",
	"document": "28720165030",
	"email": "john.doerer@test.com",
	"password": "MyP@ssw0rD",
	"type": 2
}'
```

2. transfer:

```
curl --request POST \
  --url https://localhost:44340/store/transfer \
  --header 'Content-Type: application/json' \
  --header 'User-Agent: insomnia/2023.5.8' \
  --data '{
	"value": 50.0,
	"payer": 2,
	"payee": 1
}'
```

3. deposit:

```
curl --request POST \
  --url https://localhost:44340/store/deposit \
  --header 'Content-Type: application/json' \
  --header 'User-Agent: insomnia/2023.5.8' \
  --data '{
	"value": 50.0,
	"email": "john.doerer@test.com",
	"document": "28720165030",
	"password": "MyP@ssw0rD"
}'
```

### Padrões aderidos:

- Para a modelagem da solução, procurei criar uma arquitetura limpa e desacoplada, segundo o padrão Clean Architecture.

- Para simplificar a implementação preferi unir coisas relacionadas á usuários e transações num conceito que chamei de "Store".

- No banco de dados preferi usar EF Core por facilidade, porém acredito que pro escopo do desafio poderia ter usado Dapper.

### Melhorias que poderiam ser implementadas:

- Uma tabela para guardar o histórico de transferências.
- Aumentar a cobertura dos testes unitários para abranger todo o escopo do projeto.
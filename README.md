# 📚 Bão de Prova API

API RESTful desenvolvida em ASP.NET Core para gerenciar questões de estudo, usuários e acompanhar o desempenho através de estatísticas e ranking.

## 🎯 Sobre o Projeto

O **Bão de Prova** é uma API simples e eficiente que serve como backend para um site de estudos para o ENEM. O objetivo é permitir que usuários pratiquem com questões de múltipla escolha, acompanhem seu progresso e comparem seu desempenho através de um sistema de ranking.

### ✨ Características

- ✅ Gerenciamento de questões (com categorias e anos)
- ✅ Cadastro e gerenciamento de usuários
- ✅ Registro de respostas dos usuários
- ✅ Sistema de pontuação (+10 por acerto, -2 por erro)
- ✅ Estatísticas individuais (acertos, erros, acurácia)
- ✅ Ranking geral entre usuários
- ✅ Armazenamento em JSON (sem banco de dados)

## 🛠️ Tecnologias

- .NET 9.0
- ASP.NET Core Web API
- C#
- JSON para persistência de dados

## 📁 Estrutura do Projeto

```
BaoProvaAPI/
├── Controllers/          # Endpoints da API
│   ├── QuestionsController.cs
│   ├── UsersController.cs
│   └── UserDataController.cs
├── Services/            # Lógica de negócio
│   ├── Interfaces/
│   │   ├── IQuestionService.cs
│   │   ├── IUserService.cs
│   │   └── IUserDataService.cs
│   └── Implementations/
│       ├── QuestionService.cs
│       ├── UserService.cs
│       └── UserDataService.cs
├── Models/              # Entidades
│   ├── Question.cs
│   ├── User.cs
│   └── UserData.cs
└── Data/                # Armazenamento JSON
    ├── questions.json
    ├── users.json
    └── userdata.json
```

## 🚀 Como Executar

### Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)

### Instalação

1. Clone o repositório:
```bash
git clone https://github.com/Extensao-IFTM/bao-de-prova-api.git
cd bao-de-prova-api
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Execute o projeto:
```bash
dotnet run --project BaoProvaAPI
```

4. A API estará disponível em:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

## 📖 Documentação da API

### Base URL
```
http://localhost:5000/api
```

---

## 🔷 Questions (Questões)

### 1. Verificar Status da API
```http
GET /api/questions/api-life
```

**Resposta de Sucesso (200):**
```json
{
  "status": "API is running",
  "timestamp": "2025-10-08T20:15:30"
}
```

---

### 2. Listar Todas as Questões
```http
GET /api/questions
```

**Parâmetros de Query (Opcionais):**
- `category` (string): Filtrar por categoria (ex: "Matemática")

**Exemplo:**
```http
GET /api/questions?category=Matemática
```

**Resposta de Sucesso (200):**
```json
[
  {
    "id": 1,
    "statement": "O contribuinte que vende mais de R$ 20 mil...",
    "alternatives": [
      "R$ 900,00.",
      "R$ 1 200,00.",
      "R$ 5 100,00.",
      "R$ 6 000,00.",
      "R$ 7 200,00."
    ],
    "correctAlternative": 1,
    "explanation": "Como o imposto deverá ser pago sobre o lucro...",
    "category": "Matemática",
    "year": 2013
  }
]
```

---

### 3. Buscar Questão por ID
```http
GET /api/questions/{id}
```

**Exemplo:**
```http
GET /api/questions/1
```

**Resposta de Sucesso (200):**
```json
{
  "id": 1,
  "statement": "O contribuinte que vende mais de R$ 20 mil...",
  "alternatives": ["R$ 900,00.", "R$ 1 200,00.", "..."],
  "correctAlternative": 1,
  "explanation": "Como o imposto deverá ser pago sobre o lucro...",
  "category": "Matemática",
  "year": 2013
}
```

**Resposta de Erro (404):**
```json
"Questão com o id 999 não foi encontrada"
```

---

### 4. Buscar Questão Aleatória
```http
GET /api/questions/random
```

**Parâmetros de Query (Opcionais):**
- `category` (string): Filtrar por categoria

**Exemplo:**
```http
GET /api/questions/random?category=Matemática
```

**Resposta de Sucesso (200):**
```json
{
  "id": 2,
  "statement": "...",
  "alternatives": ["...", "..."],
  "correctAlternative": 0,
  "explanation": "...",
  "category": "Matemática",
  "year": 2014
}
```

---

## 👤 Users (Usuários)

### 5. Criar Usuário
```http
POST /api/users
Content-Type: application/json
```

**Body:**
```json
{
  "name": "João Silva",
  "email": "joao.silva@email.com"
}
```

**Resposta de Sucesso (201):**
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "createdAt": "2025-10-08T20:30:00"
}
```

**Resposta de Erro (400):**
```json
{
  "message": "Já existe um usuário com este email."
}
```

---

### 6. Buscar Usuário por ID
```http
GET /api/users/{id}
```

**Exemplo:**
```http
GET /api/users/1
```

**Resposta de Sucesso (200):**
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "createdAt": "2025-10-08T20:30:00"
}
```

**Resposta de Erro (404):**
```json
{
  "message": "Usuário com ID 999 não encontrado"
}
```

---

### 7. Buscar Usuário por Email
```http
GET /api/users/email/{email}
```

**Exemplo:**
```http
GET /api/users/email/joao.silva@email.com
```

**Resposta de Sucesso (200):**
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "createdAt": "2025-10-08T20:30:00"
}
```

---

### 8. Listar Todos os Usuários
```http
GET /api/users
```

**Resposta de Sucesso (200):**
```json
[
  {
    "id": 1,
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "createdAt": "2025-10-08T20:30:00"
  },
  {
    "id": 2,
    "name": "Maria Santos",
    "email": "maria@email.com",
    "createdAt": "2025-10-08T21:00:00"
  }
]
```

---

## 📊 UserData (Respostas e Estatísticas)

### 9. Salvar Resposta do Usuário
```http
POST /api/userdata
Content-Type: application/json
```

**Body:**
```json
{
  "userId": 1,
  "questionId": 1,
  "selectedAlternative": 1,
  "isCorrect": true
}
```

**Resposta de Sucesso (200):**
```json
{
  "message": "Resposta salva com sucesso."
}
```

---

### 10. Obter Estatísticas do Usuário
```http
GET /api/userdata/user/{userId}/stats
```

**Exemplo:**
```http
GET /api/userdata/user/1/stats
```

**Resposta de Sucesso (200):**
```json
{
  "userId": 1,
  "totalQuestions": 10,
  "correctAnswers": 7,
  "wrongAnswers": 3,
  "score": 64,
  "accuracy": 70.0
}
```

**Sistema de Pontuação:**
- ✅ Acerto: **+10 pontos**
- ❌ Erro: **-2 pontos**

---

### 11. Obter Histórico de Respostas
```http
GET /api/userdata/user/{userId}/history
```

**Exemplo:**
```http
GET /api/userdata/user/1/history
```

**Resposta de Sucesso (200):**
```json
[
  {
    "userId": 1,
    "questionId": 5,
    "selectedAlternative": 2,
    "isCorrect": false,
    "timestamp": "2025-10-08T21:30:00"
  },
  {
    "userId": 1,
    "questionId": 1,
    "selectedAlternative": 1,
    "isCorrect": true,
    "timestamp": "2025-10-08T21:25:00"
  }
]
```
*Ordenado do mais recente para o mais antigo*

---

### 12. Obter Ranking Geral
```http
GET /api/userdata/ranking
```

**Resposta de Sucesso (200):**
```json
[
  {
    "userId": 1,
    "userName": "João Silva",
    "score": 98,
    "totalQuestions": 10,
    "correctAnswers": 10
  },
  {
    "userId": 2,
    "userName": "Maria Santos",
    "score": 64,
    "totalQuestions": 10,
    "correctAnswers": 7
  }
]
```
*Ordenado por score (maior para menor)*

---

## 🎮 Exemplo de Fluxo Completo

### 1. Criar um usuário
```javascript
const response = await fetch('http://localhost:5000/api/users', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    name: 'Lucas Emmanuel',
    email: 'lucas@email.com'
  })
});
const user = await response.json();
const userId = user.id; // Guarde este ID!
```

### 2. Buscar uma questão aleatória
```javascript
const response = await fetch('http://localhost:5000/api/questions/random');
const question = await response.json();
```

### 3. Salvar a resposta do usuário
```javascript
const selectedAlternative = 1; // Alternativa escolhida pelo usuário
const isCorrect = selectedAlternative === question.correctAlternative;

await fetch('http://localhost:5000/api/userdata', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    userId: userId,
    questionId: question.id,
    selectedAlternative: selectedAlternative,
    isCorrect: isCorrect
  })
});
```

### 4. Ver estatísticas do usuário
```javascript
const response = await fetch(`http://localhost:5000/api/userdata/user/${userId}/stats`);
const stats = await response.json();
console.log(`Acurácia: ${stats.accuracy}%`);
console.log(`Pontuação: ${stats.score} pontos`);
```

### 5. Ver ranking
```javascript
const response = await fetch('http://localhost:5000/api/userdata/ranking');
const ranking = await response.json();
ranking.forEach((entry, index) => {
  console.log(`${index + 1}º - ${entry.userName}: ${entry.score} pontos`);
});
```

---

## 📝 Modelos de Dados

### Question
```json
{
  "id": 1,
  "statement": "Enunciado da questão",
  "alternatives": ["Alternativa A", "Alternativa B", "..."],
  "correctAlternative": 0,
  "explanation": "Explicação da resposta correta",
  "category": "Matemática",
  "year": 2024
}
```

### User
```json
{
  "id": 1,
  "name": "Nome do Usuário",
  "email": "email@exemplo.com",
  "createdAt": "2025-10-08T20:00:00"
}
```

### UserData
```json
{
  "userId": 1,
  "questionId": 1,
  "selectedAlternative": 0,
  "isCorrect": true,
  "timestamp": "2025-10-08T20:00:00"
}
```

---

<!-- ## 🔒 CORS

A API possui CORS habilitado para todas as origens em modo de desenvolvimento. Para produção, configure as origens permitidas no arquivo `Program.cs`.

--- -->

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou enviar pull requests.

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👥 Autor

Desenvolvido por **Lucas Emmanuel** - [Extensao-IFTM](https://github.com/Extensao-IFTM)

---

## 📞 Contato

Se tiver dúvidas ou sugestões, sinta-se à vontade para entrar em contato!

---

⭐ Se este projeto foi útil para você, considere dar uma estrela no repositório!
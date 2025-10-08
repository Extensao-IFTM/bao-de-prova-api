# 🚀 Guia de Integração - Bão de Prova API

Este documento contém todas as informações necessárias para integrar a **Bão de Prova API** em seu site HTML/CSS/JavaScript.

---

## 📋 Índice

1. [Configuração Inicial](#-configuração-inicial)
2. [Autenticação](#-autenticação)
3. [Endpoints Disponíveis](#-endpoints-disponíveis)
4. [Exemplos Práticos](#-exemplos-práticos)
5. [Tratamento de Erros](#-tratamento-de-erros)
6. [Boas Práticas](#-boas-práticas)
7. [Exemplos Completos](#-exemplos-completos)

---

## 🔧 Configuração Inicial

### Base URL

```javascript
const API_BASE_URL = 'http://localhost:7093/api';
// OU em produção:
// const API_BASE_URL = 'https://sua-api.com/api';
```

### Headers Padrão

```javascript
const headers = {
  'Content-Type': 'application/json',
  'Accept': 'application/json'
};
```

---

## 🔐 Autenticação

**⚠️ Importante:** Esta API não requer autenticação no momento. Para adicionar segurança em produção, considere implementar JWT ou API Keys.

---

## 📚 Endpoints Disponíveis

### 🔷 Questions (Questões)

#### 1. Verificar Status da API
```javascript
GET /api/questions/api-life
```

#### 2. Listar Todas as Questões
```javascript
GET /api/questions
GET /api/questions?category=Matemática
```

#### 3. Buscar Questão por ID
```javascript
GET /api/questions/{id}
```

#### 4. Buscar Questão Aleatória
```javascript
GET /api/questions/random
GET /api/questions/random?category=Matemática
```

---

### 👤 Users (Usuários)

#### 5. Criar Usuário
```javascript
POST /api/users
Body: { "name": "string", "email": "string" }
```

#### 6. Buscar Usuário por ID
```javascript
GET /api/users/{id}
```

#### 7. Buscar Usuário por Email
```javascript
GET /api/users/email/{email}
```

#### 8. Listar Todos os Usuários
```javascript
GET /api/users
```

---

### 📊 UserData (Respostas e Estatísticas)

#### 9. Salvar Resposta
```javascript
POST /api/userdata
Body: {
  "userId": number,
  "questionId": number,
  "selectedAlternative": number,
  "isCorrect": boolean
}
```

#### 10. Estatísticas do Usuário
```javascript
GET /api/userdata/user/{userId}/stats
```

#### 11. Histórico de Respostas
```javascript
GET /api/userdata/user/{userId}/history
```

#### 12. Ranking Geral
```javascript
GET /api/userdata/ranking
```

---

## 💡 Exemplos Práticos

### 1️⃣ Criar Usuário

```javascript
async function createUser(name, email) {
  try {
    const response = await fetch(`${API_BASE_URL}/users`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        name: name,
        email: email
      })
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Erro ao criar usuário');
    }

    const user = await response.json();
    console.log('Usuário criado:', user);
    
    // Salvar ID do usuário no localStorage
    localStorage.setItem('userId', user.id);
    
    return user;
  } catch (error) {
    console.error('Erro:', error.message);
    alert(error.message);
  }
}

// Uso
createUser('João Silva', 'joao@email.com');
```

---

### 2️⃣ Buscar Questão Aleatória

```javascript
async function getRandomQuestion(category = null) {
  try {
    let url = `${API_BASE_URL}/questions/random`;
    
    if (category) {
      url += `?category=${encodeURIComponent(category)}`;
    }

    const response = await fetch(url);

    if (!response.ok) {
      throw new Error('Erro ao buscar questão');
    }

    const question = await response.json();
    return question;
  } catch (error) {
    console.error('Erro:', error.message);
    alert('Não foi possível carregar a questão');
  }
}

// Uso
const question = await getRandomQuestion();
console.log(question);

// Com categoria
const mathQuestion = await getRandomQuestion('Matemática');
```

---

### 3️⃣ Exibir Questão na Tela

```javascript
function displayQuestion(question) {
  const questionContainer = document.getElementById('question-container');
  
  const html = `
    <div class="question">
      <h2>Questão ${question.id}</h2>
      <p class="statement">${question.statement}</p>
      
      <div class="alternatives">
        ${question.alternatives.map((alt, index) => `
          <div class="alternative">
            <input type="radio" 
                   id="alt${index}" 
                   name="alternative" 
                   value="${index}"
                   data-question-id="${question.id}">
            <label for="alt${index}">${alt}</label>
          </div>
        `).join('')}
      </div>
      
      <button onclick="submitAnswer(${question.id}, ${question.correctAlternative})">
        Confirmar Resposta
      </button>
      
      <div class="metadata">
        <span class="category">${question.category || 'Geral'}</span>
        <span class="year">${question.year}</span>
      </div>
    </div>
  `;
  
  questionContainer.innerHTML = html;
}
```

---

### 4️⃣ Salvar Resposta do Usuário

```javascript
async function submitAnswer(questionId, correctAlternative) {
  const userId = localStorage.getItem('userId');
  
  if (!userId) {
    alert('Você precisa estar logado para responder questões!');
    return;
  }

  // Pegar alternativa selecionada
  const selected = document.querySelector('input[name="alternative"]:checked');
  
  if (!selected) {
    alert('Selecione uma alternativa!');
    return;
  }

  const selectedAlternative = parseInt(selected.value);
  const isCorrect = selectedAlternative === correctAlternative;

  try {
    const response = await fetch(`${API_BASE_URL}/userdata`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        userId: parseInt(userId),
        questionId: questionId,
        selectedAlternative: selectedAlternative,
        isCorrect: isCorrect
      })
    });

    if (!response.ok) {
      throw new Error('Erro ao salvar resposta');
    }

    // Mostrar feedback
    showFeedback(isCorrect, correctAlternative);
    
    // Atualizar estatísticas
    updateUserStats();
    
  } catch (error) {
    console.error('Erro:', error.message);
    alert('Não foi possível salvar sua resposta');
  }
}

function showFeedback(isCorrect, correctAlternative) {
  const feedback = document.getElementById('feedback');
  
  if (isCorrect) {
    feedback.innerHTML = `
      <div class="feedback success">
        <h3>✅ Parabéns! Você acertou!</h3>
        <p>+10 pontos</p>
      </div>
    `;
  } else {
    feedback.innerHTML = `
      <div class="feedback error">
        <h3>❌ Resposta incorreta</h3>
        <p>A alternativa correta era: ${correctAlternative}</p>
        <p>-2 pontos</p>
      </div>
    `;
  }
  
  feedback.style.display = 'block';
}
```

---

### 5️⃣ Exibir Estatísticas do Usuário

```javascript
async function getUserStats(userId) {
  try {
    const response = await fetch(`${API_BASE_URL}/userdata/user/${userId}/stats`);

    if (!response.ok) {
      throw new Error('Erro ao buscar estatísticas');
    }

    const stats = await response.json();
    return stats;
  } catch (error) {
    console.error('Erro:', error.message);
    return null;
  }
}

async function displayUserStats() {
  const userId = localStorage.getItem('userId');
  
  if (!userId) return;

  const stats = await getUserStats(userId);
  
  if (!stats) return;

  const statsContainer = document.getElementById('user-stats');
  
  statsContainer.innerHTML = `
    <div class="stats-card">
      <h3>Suas Estatísticas</h3>
      <div class="stat-item">
        <span class="label">Total de Questões:</span>
        <span class="value">${stats.totalQuestions}</span>
      </div>
      <div class="stat-item">
        <span class="label">Acertos:</span>
        <span class="value success">${stats.correctAnswers}</span>
      </div>
      <div class="stat-item">
        <span class="label">Erros:</span>
        <span class="value error">${stats.wrongAnswers}</span>
      </div>
      <div class="stat-item">
        <span class="label">Acurácia:</span>
        <span class="value">${stats.accuracy.toFixed(1)}%</span>
      </div>
      <div class="stat-item total">
        <span class="label">Pontuação:</span>
        <span class="value">${stats.score} pts</span>
      </div>
    </div>
  `;
}
```

---

### 6️⃣ Exibir Ranking

```javascript
async function getRanking() {
  try {
    const response = await fetch(`${API_BASE_URL}/userdata/ranking`);

    if (!response.ok) {
      throw new Error('Erro ao buscar ranking');
    }

    const ranking = await response.json();
    return ranking;
  } catch (error) {
    console.error('Erro:', error.message);
    return [];
  }
}

async function displayRanking() {
  const ranking = await getRanking();
  const rankingContainer = document.getElementById('ranking');
  
  if (ranking.length === 0) {
    rankingContainer.innerHTML = '<p>Nenhum dado disponível ainda.</p>';
    return;
  }

  const html = `
    <div class="ranking-container">
      <h2>🏆 Ranking Geral</h2>
      <table class="ranking-table">
        <thead>
          <tr>
            <th>Posição</th>
            <th>Nome</th>
            <th>Questões</th>
            <th>Acertos</th>
            <th>Pontuação</th>
          </tr>
        </thead>
        <tbody>
          ${ranking.map((user, index) => `
            <tr class="${index < 3 ? 'top-' + (index + 1) : ''}">
              <td class="position">
                ${index === 0 ? '🥇' : index === 1 ? '🥈' : index === 2 ? '🥉' : (index + 1)}
              </td>
              <td class="name">${user.userName}</td>
              <td>${user.totalQuestions}</td>
              <td>${user.correctAnswers}</td>
              <td class="score"><strong>${user.score}</strong> pts</td>
            </tr>
          `).join('')}
        </tbody>
      </table>
    </div>
  `;
  
  rankingContainer.innerHTML = html;
}
```

---

### 7️⃣ Filtrar Questões por Categoria

```javascript
async function getQuestionsByCategory(category) {
  try {
    const response = await fetch(
      `${API_BASE_URL}/questions?category=${encodeURIComponent(category)}`
    );

    if (!response.ok) {
      throw new Error('Erro ao buscar questões');
    }

    const questions = await response.json();
    return questions;
  } catch (error) {
    console.error('Erro:', error.message);
    return [];
  }
}

// Uso
const mathQuestions = await getQuestionsByCategory('Matemática');
console.log(`Encontradas ${mathQuestions.length} questões de Matemática`);
```

---

## ⚠️ Tratamento de Erros

### Verificar Status HTTP

```javascript
async function fetchWithErrorHandling(url, options = {}) {
  try {
    const response = await fetch(url, options);

    // Sucesso
    if (response.ok) {
      return await response.json();
    }

    // Erro do cliente (4xx)
    if (response.status >= 400 && response.status < 500) {
      const error = await response.json();
      throw new Error(error.message || 'Erro na requisição');
    }

    // Erro do servidor (5xx)
    if (response.status >= 500) {
      throw new Error('Erro no servidor. Tente novamente mais tarde.');
    }

  } catch (error) {
    if (error instanceof TypeError) {
      throw new Error('Erro de conexão. Verifique sua internet.');
    }
    throw error;
  }
}
```

### Tratamento de Erros Comum

```javascript
function handleError(error) {
  console.error('Erro:', error);
  
  const errorMessages = {
    'Failed to fetch': 'Erro de conexão. Verifique sua internet.',
    'NetworkError': 'Erro de rede. Tente novamente.',
    'Já existe um usuário': 'Este email já está cadastrado.',
  };

  const message = errorMessages[error.message] || error.message || 'Erro desconhecido';
  
  // Exibir mensagem para o usuário
  showNotification(message, 'error');
}

function showNotification(message, type = 'info') {
  const notification = document.createElement('div');
  notification.className = `notification ${type}`;
  notification.textContent = message;
  
  document.body.appendChild(notification);
  
  setTimeout(() => {
    notification.remove();
  }, 5000);
}
```

---

## ✅ Boas Práticas

### 1. Armazenar ID do Usuário

```javascript
// Salvar ao criar/login
localStorage.setItem('userId', user.id);

// Recuperar
const userId = localStorage.getItem('userId');

// Verificar se está logado
function isLoggedIn() {
  return localStorage.getItem('userId') !== null;
}
```

### 2. Cache de Questões

```javascript
const questionsCache = new Map();

async function getQuestionWithCache(id) {
  // Verificar cache
  if (questionsCache.has(id)) {
    return questionsCache.get(id);
  }

  // Buscar da API
  const question = await fetch(`${API_BASE_URL}/questions/${id}`).then(r => r.json());
  
  // Salvar no cache
  questionsCache.set(id, question);
  
  return question;
}
```

### 3. Loading States

```javascript
function showLoading(show = true) {
  const loader = document.getElementById('loader');
  loader.style.display = show ? 'block' : 'none';
}

async function loadQuestionWithLoading() {
  showLoading(true);
  
  try {
    const question = await getRandomQuestion();
    displayQuestion(question);
  } catch (error) {
    handleError(error);
  } finally {
    showLoading(false);
  }
}
```

### 4. Debounce para Pesquisa

```javascript
function debounce(func, wait) {
  let timeout;
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
}

// Uso em campo de busca
const searchInput = document.getElementById('search');
searchInput.addEventListener('input', debounce(async (e) => {
  const results = await searchQuestions(e.target.value);
  displayResults(results);
}, 300));
```

---

## 📦 Exemplos Completos

### Exemplo 1: Sistema de Login/Cadastro

```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Login - Bão de Prova</title>
  <style>
    .container {
      max-width: 400px;
      margin: 50px auto;
      padding: 20px;
      border: 1px solid #ddd;
      border-radius: 8px;
    }
    .form-group {
      margin-bottom: 15px;
    }
    label {
      display: block;
      margin-bottom: 5px;
      font-weight: bold;
    }
    input {
      width: 100%;
      padding: 10px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }
    button {
      width: 100%;
      padding: 12px;
      background-color: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 16px;
    }
    button:hover {
      background-color: #0056b3;
    }
    .error {
      color: red;
      margin-top: 10px;
    }
    .success {
      color: green;
      margin-top: 10px;
    }
  </style>
</head>
<body>
  <div class="container">
    <h2>Cadastro</h2>
    <form id="signup-form">
      <div class="form-group">
        <label for="name">Nome:</label>
        <input type="text" id="name" required>
      </div>
      <div class="form-group">
        <label for="email">Email:</label>
        <input type="email" id="email" required>
      </div>
      <button type="submit">Cadastrar</button>
      <div id="message"></div>
    </form>
  </div>

  <script>
    const API_BASE_URL = 'http://localhost:5000/api';

    document.getElementById('signup-form').addEventListener('submit', async (e) => {
      e.preventDefault();
      
      const name = document.getElementById('name').value;
      const email = document.getElementById('email').value;
      const messageDiv = document.getElementById('message');

      try {
        const response = await fetch(`${API_BASE_URL}/users`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ name, email })
        });

        if (!response.ok) {
          const error = await response.json();
          throw new Error(error.message);
        }

        const user = await response.json();
        localStorage.setItem('userId', user.id);
        localStorage.setItem('userName', user.name);

        messageDiv.className = 'success';
        messageDiv.textContent = 'Cadastro realizado com sucesso!';

        setTimeout(() => {
          window.location.href = 'questions.html';
        }, 1500);

      } catch (error) {
        messageDiv.className = 'error';
        messageDiv.textContent = error.message;
      }
    });
  </script>
</body>
</html>
```

---

### Exemplo 2: Página de Questões

```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Questões - Bão de Prova</title>
  <style>
    * {
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }
    body {
      font-family: Arial, sans-serif;
      padding: 20px;
      background-color: #f5f5f5;
    }
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
      padding: 20px;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    .user-info {
      font-weight: bold;
    }
    .question-container {
      max-width: 800px;
      margin: 0 auto;
      padding: 30px;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    .statement {
      font-size: 18px;
      line-height: 1.6;
      margin-bottom: 30px;
    }
    .alternative {
      padding: 15px;
      margin-bottom: 10px;
      border: 2px solid #ddd;
      border-radius: 8px;
      cursor: pointer;
      transition: all 0.3s;
    }
    .alternative:hover {
      background-color: #f0f0f0;
    }
    .alternative input[type="radio"] {
      margin-right: 10px;
    }
    .submit-btn {
      width: 100%;
      padding: 15px;
      margin-top: 20px;
      background-color: #28a745;
      color: white;
      border: none;
      border-radius: 8px;
      font-size: 16px;
      cursor: pointer;
    }
    .submit-btn:hover {
      background-color: #218838;
    }
    .feedback {
      margin-top: 20px;
      padding: 20px;
      border-radius: 8px;
      display: none;
    }
    .feedback.success {
      background-color: #d4edda;
      color: #155724;
      border: 1px solid #c3e6cb;
    }
    .feedback.error {
      background-color: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }
    .next-btn {
      margin-top: 15px;
      padding: 10px 20px;
      background-color: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    .loader {
      text-align: center;
      padding: 50px;
      display: none;
    }
    .metadata {
      margin-top: 20px;
      display: flex;
      gap: 20px;
      color: #666;
    }
  </style>
</head>
<body>
  <div class="header">
    <h1>📚 Bão de Prova</h1>
    <div class="user-info">
      Olá, <span id="user-name"></span>!
    </div>
  </div>

  <div class="loader" id="loader">
    <p>Carregando questão...</p>
  </div>

  <div class="question-container" id="question-container">
    <!-- Questão será inserida aqui -->
  </div>

  <div class="feedback" id="feedback">
    <!-- Feedback será inserido aqui -->
  </div>

  <script>
    const API_BASE_URL = 'http://localhost:5000/api';
    let currentQuestion = null;

    // Verificar se está logado
    const userId = localStorage.getItem('userId');
    const userName = localStorage.getItem('userName');

    if (!userId) {
      window.location.href = 'login.html';
    }

    document.getElementById('user-name').textContent = userName;

    // Carregar questão ao iniciar
    loadQuestion();

    async function loadQuestion() {
      const loader = document.getElementById('loader');
      const container = document.getElementById('question-container');
      
      loader.style.display = 'block';
      container.style.display = 'none';
      document.getElementById('feedback').style.display = 'none';

      try {
        const response = await fetch(`${API_BASE_URL}/questions/random`);
        
        if (!response.ok) {
          throw new Error('Erro ao carregar questão');
        }

        currentQuestion = await response.json();
        displayQuestion(currentQuestion);

      } catch (error) {
        alert('Erro ao carregar questão: ' + error.message);
      } finally {
        loader.style.display = 'none';
        container.style.display = 'block';
      }
    }

    function displayQuestion(question) {
      const container = document.getElementById('question-container');
      
      const html = `
        <h2>Questão ${question.id}</h2>
        <p class="statement">${question.statement}</p>
        
        <div class="alternatives">
          ${question.alternatives.map((alt, index) => `
            <div class="alternative">
              <label>
                <input type="radio" 
                       name="alternative" 
                       value="${index}">
                ${alt}
              </label>
            </div>
          `).join('')}
        </div>
        
        <button class="submit-btn" onclick="submitAnswer()">
          Confirmar Resposta
        </button>
        
        <div class="metadata">
          <span>📁 Categoria: ${question.category || 'Geral'}</span>
          <span>📅 Ano: ${question.year}</span>
        </div>
      `;
      
      container.innerHTML = html;
    }

    async function submitAnswer() {
      const selected = document.querySelector('input[name="alternative"]:checked');
      
      if (!selected) {
        alert('Por favor, selecione uma alternativa!');
        return;
      }

      const selectedAlternative = parseInt(selected.value);
      const isCorrect = selectedAlternative === currentQuestion.correctAlternative;

      try {
        const response = await fetch(`${API_BASE_URL}/userdata`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            userId: parseInt(userId),
            questionId: currentQuestion.id,
            selectedAlternative: selectedAlternative,
            isCorrect: isCorrect
          })
        });

        if (!response.ok) {
          throw new Error('Erro ao salvar resposta');
        }

        showFeedback(isCorrect);

      } catch (error) {
        alert('Erro ao salvar resposta: ' + error.message);
      }
    }

    function showFeedback(isCorrect) {
      const feedback = document.getElementById('feedback');
      
      if (isCorrect) {
        feedback.className = 'feedback success';
        feedback.innerHTML = `
          <h3>✅ Parabéns! Você acertou!</h3>
          <p><strong>+10 pontos</strong></p>
          <p>${currentQuestion.explanation}</p>
          <button class="next-btn" onclick="loadQuestion()">Próxima Questão</button>
        `;
      } else {
        feedback.className = 'feedback error';
        feedback.innerHTML = `
          <h3>❌ Resposta incorreta</h3>
          <p>A alternativa correta era: <strong>${currentQuestion.alternatives[currentQuestion.correctAlternative]}</strong></p>
          <p>${currentQuestion.explanation}</p>
          <p><strong>-2 pontos</strong></p>
          <button class="next-btn" onclick="loadQuestion()">Próxima Questão</button>
        `;
      }
      
      feedback.style.display = 'block';
      
      // Desabilitar alternativas
      document.querySelectorAll('input[name="alternative"]').forEach(input => {
        input.disabled = true;
      });
      
      // Desabilitar botão
      document.querySelector('.submit-btn').disabled = true;
    }
  </script>
</body>
</html>
```

---

### Exemplo 3: Página de Ranking

```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Ranking - Bão de Prova</title>
  <style>
    * {
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }
    body {
      font-family: Arial, sans-serif;
      padding: 20px;
      background-color: #f5f5f5;
    }
    .container {
      max-width: 1000px;
      margin: 0 auto;
      padding: 30px;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    h1 {
      text-align: center;
      margin-bottom: 30px;
      color: #333;
    }
    .ranking-table {
      width: 100%;
      border-collapse: collapse;
    }
    .ranking-table th {
      background-color: #007bff;
      color: white;
      padding: 15px;
      text-align: left;
    }
    .ranking-table td {
      padding: 12px 15px;
      border-bottom: 1px solid #ddd;
    }
    .ranking-table tr:hover {
      background-color: #f5f5f5;
    }
    .top-1 {
      background-color: #ffd700 !important;
    }
    .top-2 {
      background-color: #c0c0c0 !important;
    }
    .top-3 {
      background-color: #cd7f32 !important;
    }
    .position {
      font-size: 24px;
      text-align: center;
    }
    .score {
      font-weight: bold;
      color: #007bff;
    }
  </style>
</head>
<body>
  <div class="container">
    <h1>🏆 Ranking Geral</h1>
    <div id="ranking-container">
      <p style="text-align: center;">Carregando ranking...</p>
    </div>
  </div>

  <script>
    const API_BASE_URL = 'http://localhost:5000/api';

    loadRanking();

    async function loadRanking() {
      try {
        const response = await fetch(`${API_BASE_URL}/userdata/ranking`);
        
        if (!response.ok) {
          throw new Error('Erro ao carregar ranking');
        }

        const ranking = await response.json();
        displayRanking(ranking);

      } catch (error) {
        document.getElementById('ranking-container').innerHTML = 
          `<p style="color: red; text-align: center;">Erro: ${error.message}</p>`;
      }
    }

    function displayRanking(ranking) {
      const container = document.getElementById('ranking-container');
      
      if (ranking.length === 0) {
        container.innerHTML = '<p style="text-align: center;">Nenhum dado disponível ainda.</p>';
        return;
      }

      const html = `
        <table class="ranking-table">
          <thead>
            <tr>
              <th style="width: 80px; text-align: center;">Posição</th>
              <th>Nome</th>
              <th style="text-align: center;">Questões</th>
              <th style="text-align: center;">Acertos</th>
              <th style="text-align: center;">Pontuação</th>
            </tr>
          </thead>
          <tbody>
            ${ranking.map((user, index) => `
              <tr class="${index < 3 ? 'top-' + (index + 1) : ''}">
                <td class="position">
                  ${index === 0 ? '🥇' : index === 1 ? '🥈' : index === 2 ? '🥉' : (index + 1)}
                </td>
                <td>${user.userName}</td>
                <td style="text-align: center;">${user.totalQuestions}</td>
                <td style="text-align: center;">${user.correctAnswers}</td>
                <td class="score" style="text-align: center;">${user.score} pts</td>
              </tr>
            `).join('')}
          </tbody>
        </table>
      `;
      
      container.innerHTML = html;
    }
  </script>
</body>
</html>
```

---

## 🎨 CSS Sugerido

```css
/* Estilos gerais recomendados */

.notification {
  position: fixed;
  top: 20px;
  right: 20px;
  padding: 15px 20px;
  border-radius: 4px;
  box-shadow: 0 2px 5px rgba(0,0,0,0.2);
  z-index: 1000;
  animation: slideIn 0.3s ease-out;
}

.notification.success {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.notification.error {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.notification.info {
  background-color: #d1ecf1;
  color: #0c5460;
  border: 1px solid #bee5eb;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

.loader {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #007bff;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin: 0 auto;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
```

---

## 🔗 Links Úteis

- [Repositório da API](https://github.com/Extensao-IFTM/bao-de-prova-api)
- [Documentação Completa](https://github.com/Extensao-IFTM/bao-de-prova-api#readme)

---

## 📞 Suporte

Se encontrar problemas ou tiver dúvidas sobre a integração, abra uma issue no repositório do GitHub.

---

## 📝 Changelog

### Versão 1.0.0 (Outubro 2025)
- ✅ Endpoints de questões
- ✅ Endpoints de usuários
- ✅ Sistema de pontuação
- ✅ Ranking e estatísticas

---

**Desenvolvido com ❤️ pela equipe Extensao-IFTM**

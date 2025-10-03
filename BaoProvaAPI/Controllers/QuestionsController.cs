using BaoProvaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        [HttpGet("api-life")]
        public IActionResult Verify()
        {
            return Ok("API is running");
        }

        [HttpGet()]
        public IActionResult GetQuestions()
        {
            List<Question> questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Statement = 
                    "O contribuinte que vende mais de R$ 20 mil de ações em " +
                    "Bolsa de Valores em um mês deverá pagar Imposto de Renda. " +
                    "O pagamento para a Receita Federal consistirá em 15% do lucro " +
                    "obtido com a venda das ações. Um contribuinte que vende por R$ 34 mil " +
                    "um lote de ações que custou R$ 26 mil terá de pagar de Imposto de Renda " +
                    "à Receita Federal o valor de",
                    Alternatives = new[] { "R$ 900,00.", "R$ 1 200,00.", "R$ 2 100,00.", "R$ 3 900,00.", "R$ 5 100,00." },
                    CorrectAlternative = 1,
                    Explanation = "Como o imposto deverá ser pago em cima do lucro, então precisamos descobrir qual foi esse valor. Para isso, basta diminuir o valor da venda pelo valor da compra, isto é:\r\n\r\nLucro = 34 000 - 26 000 = 8 000\r\n\r\nAgora temos que encontrar 15% deste valor, que será igual ao imposto que o contribuinte deverá pagar:\r\n\r\nImposto = 15% . 8 000=15/100.8000= 1200",
                    Category = "Matemática",
                    Year = 2013
                },
                new Question
                {
                    Id = 2,
                    Statement = "Um arquiteto está reformando uma casa. De modo a contribuir com o meio ambiente, decide reaproveitar tábuas de madeira retiradas da casa. Ele dispõe de 40 tábuas de 540 cm, 30 de 810 cm e 10 de 1 080 cm, todas de mesma largura e espessura. Ele pediu a um carpinteiro que cortasse as tábuas em pedaços de mesmo comprimento, sem deixar sobras, e de modo que as novas peças ficassem com o maior tamanho possível, mas de comprimento menor que 2 m.\r\n\r\nAtendendo o pedido do arquiteto, o carpinteiro deverá produzir",
                    Alternatives = new[] { "105 peças.", "120 peças.", "210 peças.", "243 peças.", "420 peças." },
                    CorrectAlternative = 4,
                    Explanation = "Bla BLa BLa RESPOSTA",
                    Category = "Matemática",
                    Year = 2015
                }
            };

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            List<Question> questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Statement =
                    "O contribuinte que vende mais de R$ 20 mil de ações em " +
                    "Bolsa de Valores em um mês deverá pagar Imposto de Renda. " +
                    "O pagamento para a Receita Federal consistirá em 15% do lucro " +
                    "obtido com a venda das ações. Um contribuinte que vende por R$ 34 mil " +
                    "um lote de ações que custou R$ 26 mil terá de pagar de Imposto de Renda " +
                    "à Receita Federal o valor de",
                    Alternatives = new[] { "R$ 900,00.", "R$ 1 200,00.", "R$ 2 100,00.", "R$ 3 900,00.", "R$ 5 100,00." },
                    CorrectAlternative = 1,
                    Explanation = "Como o imposto deverá ser pago em cima do lucro, então precisamos descobrir qual foi esse valor. Para isso, basta diminuir o valor da venda pelo valor da compra, isto é:\r\n\r\nLucro = 34 000 - 26 000 = 8 000\r\n\r\nAgora temos que encontrar 15% deste valor, que será igual ao imposto que o contribuinte deverá pagar:\r\n\r\nImposto = 15% . 8 000=15/100.8000= 1200",
                    Category = "Matemática",
                    Year = 2013
                },
                new Question
                {
                    Id = 2,
                    Statement = "Um arquiteto está reformando uma casa. De modo a contribuir com o meio ambiente, decide reaproveitar tábuas de madeira retiradas da casa. Ele dispõe de 40 tábuas de 540 cm, 30 de 810 cm e 10 de 1 080 cm, todas de mesma largura e espessura. Ele pediu a um carpinteiro que cortasse as tábuas em pedaços de mesmo comprimento, sem deixar sobras, e de modo que as novas peças ficassem com o maior tamanho possível, mas de comprimento menor que 2 m.\r\n\r\nAtendendo o pedido do arquiteto, o carpinteiro deverá produzir",
                    Alternatives = new[] { "105 peças.", "120 peças.", "210 peças.", "243 peças.", "420 peças." },
                    CorrectAlternative = 4,
                    Explanation = "Bla BLa BLa RESPOSTA",
                    Category = "Matemática",
                    Year = 2015
                }
            };
                
            Question? question = questions.FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return NotFound($"Questão com o id {id} não foi encontrada");
            }

            return Ok(question);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly AlunoRepository _repo;
    public AlunosController(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        _repo = new AlunoRepository(connectionString);
        _repo.GarantirEsquema();
    }
    [HttpGet]
    public ActionResult<List<Aluno>> GetAll()
    {
        return _repo.Listar();
    }
    [HttpGet("{propriedade}/{valor}")]
    public ActionResult<List<Aluno>> Buscar(string propriedade, string valor)
    {
        return _repo.Buscar(propriedade, valor);
    }
    [HttpPost]
    public ActionResult<int> Create([FromBody] Aluno aluno)
    {
        var id = _repo.Inserir(aluno.Nome, aluno.Idade, aluno.Email, aluno.DataNascimento);
        return CreatedAtAction(nameof(Buscar), new { propriedade = "Id", valor = id }, id);
    }
    [HttpPut("{id}")]
    public ActionResult Atualizar(int id, [FromBody] Aluno aluno)
    {
        var linhasAfetadas = _repo.Atualizar(id, aluno.Nome, aluno.Idade, aluno.Email, aluno.DataNascimento);
        if (linhasAfetadas == 0) return NotFound();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public ActionResult Excluir(int id)
    {
        var linhasAfetadas = _repo.Excluir(id);
        if (linhasAfetadas == 0) return NotFound();
        return NoContent();
    }
}
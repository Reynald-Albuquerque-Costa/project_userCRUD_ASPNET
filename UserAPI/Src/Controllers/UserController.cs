using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserAPI.Src.Model;
using UserAPI.Src.Repositories;


namespace UserAPI.Src.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        #region Attributes

        private readonly IUser _repository;


        #endregion


        #region Constructors
        public UserController(IUser repository)
        {
            _repository = repository;


        }

        #endregion

        #region Methods

        /// <summary>
        /// Criar novo usuário
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> NovoUsuarioAsync([FromBody] User user)
        {
            await _repository.NovoUsuarioAsync(user);

            return Created($"api/Usuarios", user);
        }

        /// <summary>
        /// Pegar usuario pelo Email
        /// </summary>
        [HttpGet("email/{emailUser}")]
        public async Task<ActionResult> PegarUsuarioPeloEmailAsync([FromRoute] string email)
        {
            try
            {
                return Ok(await _repository.PegarUsuarioPeloEmailAsync(email));
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Atulizar usuario
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> AtualizarUsuarioAsync([FromBody] User user)
        {
            try
            {
                await _repository.AtualizarUsuarioAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Deletar usuario pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarUsuario([FromRoute] int id)
        {
            try
            {
                await _repository.DeletarUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }

        #endregion
    }
}
